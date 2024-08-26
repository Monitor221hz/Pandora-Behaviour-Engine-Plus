using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkMemoryResourceHandleExternalLink Signatire: 0x3144d17c size: 16 flags: FLAGS_NONE

    // memberName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // externalId class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkMemoryResourceHandleExternalLink : IHavokObject, IEquatable<hkMemoryResourceHandleExternalLink?>
    {
        public string memberName { set; get; } = "";
        public string externalId { set; get; } = "";

        public virtual uint Signature { set; get; } = 0x3144d17c;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            memberName = des.ReadStringPointer(br);
            externalId = des.ReadStringPointer(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteStringPointer(bw, memberName);
            s.WriteStringPointer(bw, externalId);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            memberName = xd.ReadString(xe, nameof(memberName));
            externalId = xd.ReadString(xe, nameof(externalId));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(memberName), memberName);
            xs.WriteString(xe, nameof(externalId), externalId);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkMemoryResourceHandleExternalLink);
        }

        public bool Equals(hkMemoryResourceHandleExternalLink? other)
        {
            return other is not null &&
                   (memberName is null && other.memberName is null || memberName == other.memberName || memberName is null && other.memberName == "" || memberName == "" && other.memberName is null) &&
                   (externalId is null && other.externalId is null || externalId == other.externalId || externalId is null && other.externalId == "" || externalId == "" && other.externalId is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(memberName);
            hashcode.Add(externalId);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

