// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Pandora.API.Patch;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using XmlCake.Linq;
using XmlCake.Linq.Expressions;

namespace Pandora.Models.Patch.Skyrim64.Format.Nemesis;

public class NemesisParser
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger(); //to do: move logger into inheritable base class

	private static readonly XSkipWrapExpression replacePattern = new(
		new XStep(XmlNodeType.Comment, "CLOSE"),
		new XStep(XmlNodeType.Comment, "OPEN"),
		new XStep(XmlNodeType.Comment, "ORIGINAL"),
		new XStep(XmlNodeType.Comment, "CLOSE")
	);
	private static readonly XSkipWrapExpression insertPattern = new(
		new XStep(XmlNodeType.Comment, "ORIGINAL"),
		new XStep(XmlNodeType.Comment, "OPEN"),
		new XStep(XmlNodeType.Comment, "CLOSE")
	);

	/// <summary>
	/// Parse and add a replace edit to the change owner.
	/// </summary>
	/// <param name="nodeName">The name of the node.</param>
	/// <param name="match">Assumes success</param>
	/// <param name="changeSet">The changeset to be added to.</param>
	/// <param name="lookup">The XPath lookup.</param>
	public static void ParseReplaceEdit(
		string nodeName,
		XMatch match,
		IPackFileChangeOwner changeSet,
		IXPathLookup lookup
	)
	{
		List<XNode> newNodes = [];
		int separatorIndex = match.Count;
		if (match[0].NodeType != XmlNodeType.Comment)
		{
			return;
		}
		XNode? previousNode = match[0].PreviousNode;

		for (int i = 1; i < separatorIndex; i++)
		{
			XNode node = match[i];

			if (node.NodeType == XmlNodeType.Comment)
			{
				separatorIndex = i;
				break;
			}
			newNodes.Add(node);
		}

		if (newNodes.Count > 0)
		{
			for (int i = separatorIndex + 1; i < match.Count - 1; i++)
			{
				XNode node = match[i];
				XNode newNode = newNodes[i - separatorIndex - 1];
				switch (node.NodeType)
				{
					case XmlNodeType.Text:
						previousNode = newNode.PreviousNode?.PreviousNode;
						int skipCharCount = 0;
						while (previousNode != null)
						{
							if (previousNode.NodeType == XmlNodeType.Text)
							{
								skipCharCount += ((XText)previousNode).Value.Length;
							}
							if (previousNode.NodeType == XmlNodeType.Element)
							{
								break;
							}
							previousNode = previousNode.PreviousNode;
						}
						string oldText = ((XText)node).Value;
						string newText = ((XText)newNode).Value;
						//packFile.Editor.QueueReplaceText(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value);

						changeSet.AddChange(
							new ReplaceTextChange(
								nodeName,
								lookup.LookupPath(node),
								skipCharCount,
								oldText,
								newText
							)
						);
						//lock (packFile.edits) packFile.edits.AddChange(new ReplaceTextChange(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value,modInfo));
						break;

					case XmlNodeType.Element:
						//packFile.Editor.QueueReplaceElement(lookup.LookupPath(node), (XElement)newNodes[i - separatorIndex - 1]);
						changeSet.AddChange(
							new ReplaceElementChange(
								nodeName,
								lookup.LookupPath(newNode),
								(XElement)newNode
							)
						);
						//lock (packFile.edits) packFile.edits.AddChange(new ReplaceElementChange(lookup.LookupPath(node), (XElement)newNodes[i - separatorIndex - 1],modInfo));
						break;
					default:
						break;
				}
			}
			return;
		}
		for (int i = separatorIndex + 1; i < match.Count - 1; i++)
		{
			XNode node = match[i];
			switch (node.NodeType)
			{
				case XmlNodeType.Text:
					int skipCharCount = 0;
					while (previousNode != null)
					{
						if (previousNode.NodeType == XmlNodeType.Text)
						{
							skipCharCount += ((XText)previousNode).Value.Length;
						}
						if (previousNode.NodeType == XmlNodeType.Element)
						{
							break;
						}
						previousNode = previousNode.PreviousNode;
					}
					var targetText = ((XText)node).Value.Trim();
					changeSet.AddChange(
						new RemoveTextChange(
							nodeName,
							lookup.LookupPath(node),
							targetText,
							skipCharCount
						)
					);
					break;
				case XmlNodeType.Element:
					//packFile.Editor.QueueRemoveElement(lookup.LookupPath(node));
					changeSet.AddChange(new RemoveElementChange(nodeName, lookup.LookupPath(node)));
					//lock (packFile.edits) packFile.edits.AddChange(new RemoveElementChange(lookup.LookupPath(node),modInfo));
					break;
				default:
					break;
			}
		}
	}

	public static void ParseInsertEdit(
		string nodeName,
		XMatch match,
		IPackFileChangeOwner changeSet,
		IXPathLookup lookup
	)
	{
		List<XNode> newNodes = match.nodes;
		XNode? previousNode;
		XNode? nextNode = newNodes.Last().NextNode;

		newNodes.RemoveAt(0);
		newNodes.RemoveAt(newNodes.Count - 1);
		bool isTextInsert = nextNode != null && nextNode.NodeType == XmlNodeType.Text;

		foreach (XNode node in newNodes)
		{
			string nodePath = lookup.LookupPath(node);
			switch (node.NodeType)
			{
				case XmlNodeType.Text:

					if (!isTextInsert)
					{
						changeSet.AddChange(
							new AppendTextChange(nodeName, nodePath, ((XText)node).Value)
						);
						break;
					}
					previousNode = node.PreviousNode?.PreviousNode;
					int skipCharCount = 0;
					while (previousNode != null)
					{
						if (previousNode.NodeType == XmlNodeType.Text)
						{
							skipCharCount += ((XText)previousNode).Value.Length;
						}
						if (previousNode.NodeType == XmlNodeType.Element)
						{
							break;
						}
						previousNode = previousNode.PreviousNode;
					}
					changeSet.AddChange(
						new InsertTextChange(nodeName, nodePath, skipCharCount, ((XText)node).Value)
					);
					break;
				case XmlNodeType.Element:

					changeSet.AddChange(
						new AppendElementChange(
							nodeName,
							nodePath.Substring(0, nodePath.LastIndexOf('/')),
							(XElement)node
						)
					);
					break;
				default:
					break;
			}
		}
	}

	public static bool MatchReplacePattern(
		string nodeName,
		IEnumerable<XNode> nodes,
		IPackFileChangeOwner changeSet,
		IXPathLookup lookup
	)
	{
		XMatchCollection matchCollection = replacePattern.Matches(nodes);
		if (!matchCollection.Success)
			return false;
		foreach (XMatch match in matchCollection)
		{
			if (!match.Success)
			{
				continue;
			}
			ParseReplaceEdit(nodeName, match, changeSet, lookup);
		}
		return true;
	}

	public static bool MatchInsertPattern(
		string nodeName,
		IEnumerable<XNode> nodes,
		PackFileChangeSet changeSet,
		IXPathLookup lookup
	)
	{
		XMatchCollection matchCollection = insertPattern.Matches(nodes);
		if (!matchCollection.Success)
			return false;
		foreach (XMatch match in matchCollection)
		{
			if (!match.Success)
			{
				continue;
			}
			ParseInsertEdit(nodeName, match, changeSet, lookup);
		}
		return true;
	}

	public static PackFileChangeSet ParsePackFileChanges(
		IPackFile packFile,
		IModInfo modInfo,
		DirectoryInfo folder
	)
	{
		FileInfo[] editFiles = folder.GetFiles("#*.txt");

		var changeSet = new PackFileChangeSet(modInfo);
		XPathLookup lookup = new();
		foreach (FileInfo editFile in editFiles)
		{
			IEnumerable<XNode> nodes;
			string nodeName = Path.GetFileNameWithoutExtension(editFile.Name);
			XElement element;
			try
			{
				element = XElement.Load(editFile.FullName);
			}
			catch (XmlException e)
			{
				Logger.Error(
					$"Nemesis Parser > {modInfo.Name} > File {editFile.FullName} > Load > FAILED > {e.Message}"
				);
				continue;
			}
			nodes = lookup.MapFromElement(element);

			lock (packFile)
			{
				if (!packFile.PopObjectAsXml(nodeName))
				{
					packFile.Dispatcher.TrackPotentialNode(packFile, nodeName, element);
				}
			}
			MatchInsertPattern(nodeName, nodes, changeSet, lookup);
			MatchReplacePattern(nodeName, nodes, changeSet, lookup);
		}
		return changeSet;
	}
}
