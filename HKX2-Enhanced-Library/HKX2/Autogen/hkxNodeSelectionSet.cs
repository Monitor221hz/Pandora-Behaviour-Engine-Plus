using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxNodeSelectionSet Signatire: 0xd753fc4d size: 56 flags: FLAGS_NONE

    // selectedNodes class: hkxNode Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkxNodeSelectionSet : hkxAttributeHolder, IEquatable<hkxNodeSelectionSet?>
    {
        public IList<hkxNode> selectedNodes { set; get; } = Array.Empty<hkxNode>();
        public string name { set; get; } = "";

        public override uint Signature { set; get; } = 0xd753fc4d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            selectedNodes = des.ReadClassPointerArray<hkxNode>(br);
            name = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, selectedNodes);
            s.WriteStringPointer(bw, name);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            selectedNodes = xd.ReadClassPointerArray<hkxNode>(this, xe, nameof(selectedNodes));
            name = xd.ReadString(xe, nameof(name));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(selectedNodes), selectedNodes!);
            xs.WriteString(xe, nameof(name), name);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxNodeSelectionSet);
        }

        public bool Equals(hkxNodeSelectionSet? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   selectedNodes.SequenceEqual(other.selectedNodes) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(selectedNodes.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(name);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

