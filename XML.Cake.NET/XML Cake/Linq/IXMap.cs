using System;
using System.Xml.Linq;

namespace XmlCake.Linq;
public interface IXMap
{
	string AppendElement(string path, XElement element);
	XElement CopyElement(string path);
	string DefaultGenerateKey(XElement element);
	string GetPath(XElement element);
	string InsertElement(string path, XElement newElement, bool remapParent);
	XElement Lookup(string path);
	void MapAll();
	void MapLayer(bool ignoreParents, params int[] pathIndices);
	void MapLayer(string rootPath);
	void MapLayer(string rootPath, bool useBlankPath);
	void MapLayer(string rootPath, bool useBlankPath, Action<string, XElement> mapFunc);
	void MapLayer(XElement workingRoot, bool useBlankPath);
	void MapLayer(XElement workingRoot, bool useBlankPath, Action<string, XElement> mapFunc);
	void MapSlice(string rootPath);
	void MapSlice(string rootPath, bool useBlankPath);
	void MapSlice(string rootPath, bool useBlankPath, Action<string, XElement> mapFunc);
	void MapSlice(string rootPath, int depth, bool useBlankPath);
	void MapSlice(string rootPath, int depth, bool useBlankPath, Action<string, XElement> mapFunc);
	void MapSlice(XElement rootElement);
	void MapSlice(XElement workingRoot, bool useBlankPath);
	void MapSlice(XElement workingRoot, bool useBlankPath, Action<string, XElement> mapFunc);
	void MapSlice(XElement workingRoot, int depth, bool useBlankPath);
	XElement NavigateTo(string path);
	XElement NavigateTo(string path, XElement workingRoot);
	XElement NavigateTo(string path, XElement workingRoot, Func<XElement, string> generateKey);
	bool PathExists(string path);
	string PushElement(string path, XElement element);
	XElement RemoveElement(string path);
	XElement ReplaceElement(string path, XElement newElement);
	bool TryLookup(string path, out XElement element);
}