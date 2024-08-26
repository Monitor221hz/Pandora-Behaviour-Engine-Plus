using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSetNodePropertyCommand Signatire: 0xc5160b64 size: 48 flags: FLAGS_NONE

    // characterId class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // nodeName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // propertyName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // propertyValue class: hkbVariableValue Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // padding class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    public partial class hkbSetNodePropertyCommand : hkReferencedObject, IEquatable<hkbSetNodePropertyCommand?>
    {
        public ulong characterId { set; get; }
        public string nodeName { set; get; } = "";
        public string propertyName { set; get; } = "";
        public hkbVariableValue propertyValue { set; get; } = new();
        public int padding { set; get; }

        public override uint Signature { set; get; } = 0xc5160b64;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterId = br.ReadUInt64();
            nodeName = des.ReadStringPointer(br);
            propertyName = des.ReadStringPointer(br);
            propertyValue.Read(des, br);
            padding = br.ReadInt32();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(characterId);
            s.WriteStringPointer(bw, nodeName);
            s.WriteStringPointer(bw, propertyName);
            propertyValue.Write(s, bw);
            bw.WriteInt32(padding);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterId = xd.ReadUInt64(xe, nameof(characterId));
            nodeName = xd.ReadString(xe, nameof(nodeName));
            propertyName = xd.ReadString(xe, nameof(propertyName));
            propertyValue = xd.ReadClass<hkbVariableValue>(xe, nameof(propertyValue));
            padding = xd.ReadInt32(xe, nameof(padding));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(characterId), characterId);
            xs.WriteString(xe, nameof(nodeName), nodeName);
            xs.WriteString(xe, nameof(propertyName), propertyName);
            xs.WriteClass<hkbVariableValue>(xe, nameof(propertyValue), propertyValue);
            xs.WriteNumber(xe, nameof(padding), padding);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSetNodePropertyCommand);
        }

        public bool Equals(hkbSetNodePropertyCommand? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   characterId.Equals(other.characterId) &&
                   (nodeName is null && other.nodeName is null || nodeName == other.nodeName || nodeName is null && other.nodeName == "" || nodeName == "" && other.nodeName is null) &&
                   (propertyName is null && other.propertyName is null || propertyName == other.propertyName || propertyName is null && other.propertyName == "" || propertyName == "" && other.propertyName is null) &&
                   ((propertyValue is null && other.propertyValue is null) || (propertyValue is not null && other.propertyValue is not null && propertyValue.Equals((IHavokObject)other.propertyValue))) &&
                   padding.Equals(other.padding) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterId);
            hashcode.Add(nodeName);
            hashcode.Add(propertyName);
            hashcode.Add(propertyValue);
            hashcode.Add(padding);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

