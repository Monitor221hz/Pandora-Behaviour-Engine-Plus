using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkRootLevelContainerNamedVariant Signatire: 0xb103a2cd size: 24 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // className class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // variant class: hkReferencedObject Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 

    public partial class hkRootLevelContainerNamedVariant : IHavokObject, IEquatable<hkRootLevelContainerNamedVariant?>
    {

        public string name { set; get; } = "";
        public string className { set; get; } = "";
        public hkReferencedObject? variant { set; get; } = default;

        public uint Signature { set; get; } = 0xb103a2cd;

        public void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            name = des.ReadStringPointer(br);
            className = des.ReadStringPointer(br);
            variant = des.ReadClassPointer<hkReferencedObject>(br);
        }

        public void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteStringPointer(bw, name);
            s.WriteStringPointer(bw, className);
            s.WriteClassPointer(bw, variant);
        }

        public void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            name = xd.ReadString(xe, nameof(name));
            className = xd.ReadString(xe, nameof(className));
            variant = xd.ReadClassPointer<hkReferencedObject>(this, xe, nameof(variant));
        }

        public void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(name), name);
            xs.WriteString(xe, nameof(className), className);
            xs.WriteClassPointer(xe, nameof(variant), variant);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkRootLevelContainerNamedVariant);
        }

        public bool Equals(hkRootLevelContainerNamedVariant? other)
        {
            return other is not null &&
                   name == other.name &&
                   className == other.className &&
                   ((variant is null && other.variant is null) || (variant is not null && variant.Equals((IHavokObject)other.variant!))) &&
                   Signature == other.Signature;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(name);
            hashcode.Add(className);
            hashcode.Add(variant);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

