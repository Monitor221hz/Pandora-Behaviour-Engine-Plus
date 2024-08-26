using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBehaviorInfoIdToNamePair Signatire: 0x35a0439a size: 24 flags: FLAGS_NONE

    // behaviorName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // nodeName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // toolType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: ToolNodeType
    // id class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 18 flags: FLAGS_NONE enum: 
    public partial class hkbBehaviorInfoIdToNamePair : IHavokObject, IEquatable<hkbBehaviorInfoIdToNamePair?>
    {
        public string behaviorName { set; get; } = "";
        public string nodeName { set; get; } = "";
        public byte toolType { set; get; }
        public short id { set; get; }

        public virtual uint Signature { set; get; } = 0x35a0439a;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            behaviorName = des.ReadStringPointer(br);
            nodeName = des.ReadStringPointer(br);
            toolType = br.ReadByte();
            br.Position += 1;
            id = br.ReadInt16();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteStringPointer(bw, behaviorName);
            s.WriteStringPointer(bw, nodeName);
            bw.WriteByte(toolType);
            bw.Position += 1;
            bw.WriteInt16(id);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            behaviorName = xd.ReadString(xe, nameof(behaviorName));
            nodeName = xd.ReadString(xe, nameof(nodeName));
            toolType = xd.ReadFlag<ToolNodeType, byte>(xe, nameof(toolType));
            id = xd.ReadInt16(xe, nameof(id));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(behaviorName), behaviorName);
            xs.WriteString(xe, nameof(nodeName), nodeName);
            xs.WriteEnum<ToolNodeType, byte>(xe, nameof(toolType), toolType);
            xs.WriteNumber(xe, nameof(id), id);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBehaviorInfoIdToNamePair);
        }

        public bool Equals(hkbBehaviorInfoIdToNamePair? other)
        {
            return other is not null &&
                   (behaviorName is null && other.behaviorName is null || behaviorName == other.behaviorName || behaviorName is null && other.behaviorName == "" || behaviorName == "" && other.behaviorName is null) &&
                   (nodeName is null && other.nodeName is null || nodeName == other.nodeName || nodeName is null && other.nodeName == "" || nodeName == "" && other.nodeName is null) &&
                   toolType.Equals(other.toolType) &&
                   id.Equals(other.id) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(behaviorName);
            hashcode.Add(nodeName);
            hashcode.Add(toolType);
            hashcode.Add(id);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

