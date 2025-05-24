using Pandora.API.Patch;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using XmlCake.Linq;
using XmlCake.Linq.Expressions;

namespace Pandora.Models.Patch.Skyrim64.Format.Nemesis;

public class NemesisParser
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger(); //to do: move logger into inheritable base class

	private static readonly XSkipWrapExpression replacePattern = new(new XStep(XmlNodeType.Comment, "CLOSE"), new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "CLOSE"));
	private static readonly XSkipWrapExpression insertPattern = new(new XStep(XmlNodeType.Comment, "ORIGINAL"), new XStep(XmlNodeType.Comment, "OPEN"), new XStep(XmlNodeType.Comment, "CLOSE"));
	public static void ParseReplaceEdit(PackFile packFile, string nodeName, XMatch match, PackFileChangeSet changeSet, XPathLookup lookup)
	{
		List<XNode> newNodes = [];
		int separatorIndex = match.Count;

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

						StringBuilder previousTextBuilder = new();
						StringBuilder bufferTextBuilder = new();
						bool skipText = false;
						previousNode = newNode.PreviousNode?.PreviousNode;
						while (previousNode != null)
						{
							if (previousNode.NodeType == XmlNodeType.Comment)
							{
								var comment = (XComment)previousNode;
								if (comment.Value.Contains("close", StringComparison.OrdinalIgnoreCase))
								{
									skipText = true;
								}
								else if (comment.Value.Contains("open", StringComparison.OrdinalIgnoreCase))
								{
									skipText = false;
									previousTextBuilder.Insert(0, bufferTextBuilder);
									bufferTextBuilder = bufferTextBuilder.Clear();
								}
								else if (comment.Value.Contains("original", StringComparison.OrdinalIgnoreCase))
								{
									skipText = false;
									bufferTextBuilder = bufferTextBuilder.Clear();
								}
								previousNode = previousNode.PreviousNode;
								continue;
							}
							if (skipText)
							{
								bufferTextBuilder.Insert(0, '\n');
								bufferTextBuilder.Insert(0, previousNode.ToString());
								previousNode = previousNode.PreviousNode;
								continue;
							}
							previousTextBuilder.Insert(0, '\n');
							previousTextBuilder.Insert(0, previousNode.ToString());
							previousNode = previousNode.PreviousNode;
						}

						string preText = previousTextBuilder.ToString();
						string oldText = ((XText)node).Value;
						string newText = ((XText)newNode).Value;
						//packFile.Editor.QueueReplaceText(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value);

						changeSet.AddChange(new ReplaceTextChange(nodeName, lookup.LookupPath(node), preText, oldText, newText));
						//lock (packFile.edits) packFile.edits.AddChange(new ReplaceTextChange(lookup.LookupPath(node), ((XText)node).Value, ((XText)newNodes[i - separatorIndex - 1]).Value,modInfo));
						break;


					case XmlNodeType.Element:
						//packFile.Editor.QueueReplaceElement(lookup.LookupPath(node), (XElement)newNodes[i - separatorIndex - 1]);
						changeSet.AddChange(new ReplaceElementChange(nodeName, lookup.LookupPath(newNode), (XElement)newNode));
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

	public static void ParseInsertEdit(PackFile packFile, string nodeName, XMatch match, PackFileChangeSet changeSet, XPathLookup lookup)
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
			//if (node.Parent != null) node.Remove();
			switch (node.NodeType)
			{
				case XmlNodeType.Text:



					//packFile.Editor.QueueInsertText(lookup.LookupPath(node), ((XText)node).Value);
					if (!isTextInsert)
					{
						changeSet.AddChange(new AppendTextChange(nodeName, nodePath, ((XText)node).Value));
						break;
					}

					StringBuilder previousTextBuilder = new();
					StringBuilder bufferTextBuilder = new();
					bool skipText = false;
					previousNode = node.PreviousNode?.PreviousNode;
					while (previousNode != null)
					{
						if (previousNode.NodeType == XmlNodeType.Comment)
						{
							var comment = (XComment)previousNode;
							if (comment.Value.Contains("close", StringComparison.OrdinalIgnoreCase))
							{
								skipText = true;
							}
							else if (comment.Value.Contains("open", StringComparison.OrdinalIgnoreCase))
							{
								skipText = false;
								previousTextBuilder.Insert(0, bufferTextBuilder);
								bufferTextBuilder = bufferTextBuilder.Clear();
							}
							else if (comment.Value.Contains("original", StringComparison.OrdinalIgnoreCase))
							{
								skipText = false;
								bufferTextBuilder = bufferTextBuilder.Clear();
							}
							previousNode = previousNode.PreviousNode;
							continue;
						}
						if (skipText)
						{
							bufferTextBuilder.Insert(0, '\n');
							bufferTextBuilder.Insert(0, previousNode.ToString());
							previousNode = previousNode.PreviousNode;
							continue;
						}
						previousTextBuilder.Insert(0, '\n');
						previousTextBuilder.Insert(0, previousNode.ToString());
						previousNode = previousNode.PreviousNode;
					}

					string preText = previousTextBuilder.ToString();
					changeSet.AddChange(new InsertTextChange(nodeName, nodePath, preText, ((XText)node).Value));

					//lock (packFile.edits) packFile.edits.AddChange(new InsertTextChange(nodePath, ((XText)node).Value, modInfo));
					break;
				case XmlNodeType.Element:
					//packFile.Editor.QueueInsertElement(lookup.LookupPath(node), (XElement)node);
					lock (packFile.Dispatcher)
					{
						if (packFile.Map.PathExists(nodePath))
						{
							changeSet.AddChange(new InsertElementChange(nodeName, nodePath, (XElement)node));
							//packFile.edits.AddChange(new InsertElementChange(nodePath, (XElement)node, modInfo));
						}
						else
						{
							changeSet.AddChange(new AppendElementChange(nodeName, nodePath.Substring(0, nodePath.LastIndexOf('/')), (XElement)node));
							//packFile.edits.AddChange(new AppendElementChange(nodePath.Substring(0, nodePath.LastIndexOf('/')), (XElement)node, modInfo));
						}
					}
					break;
				default:
					break;
			}
		}
	}

	public static bool MatchReplacePattern(PackFile packFile, string nodeName, IEnumerable<XNode> nodes, PackFileChangeSet changeSet, XPathLookup lookup)
	{
		XMatchCollection matchCollection = replacePattern.Matches(nodes);
		if (!matchCollection.Success) return false;
		foreach (XMatch match in matchCollection)
		{
			ParseReplaceEdit(packFile, nodeName, match, changeSet, lookup);
		}
		return true;
	}

	public static bool MatchInsertPattern(PackFile packFile, string nodeName, IEnumerable<XNode> nodes, PackFileChangeSet changeSet, XPathLookup lookup)
	{
		XMatchCollection matchCollection = insertPattern.Matches(nodes);
		if (!matchCollection.Success) return false;
		foreach (XMatch match in matchCollection)
		{
			ParseInsertEdit(packFile, nodeName, match, changeSet, lookup);
		}
		return true;
	}
	public static PackFileChangeSet ParsePackFileChanges(PackFile packFile, IModInfo modInfo, DirectoryInfo folder)
	{
		FileInfo[] editFiles = folder.GetFiles("#*.txt");

		var changeSet = new PackFileChangeSet(modInfo);
		var modName = modInfo.Name;
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
				Logger.Error($"Nemesis Parser > File {editFile.FullName} > Load > FAILED > {e.Message}");
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
			MatchInsertPattern(packFile, nodeName, nodes, changeSet, lookup);
			MatchReplacePattern(packFile, nodeName, nodes, changeSet, lookup);
		}
		return changeSet;
	}
}
