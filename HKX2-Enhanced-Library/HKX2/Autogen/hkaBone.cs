using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaBone Signatire: 0x35912f8a size: 16 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // lockTranslation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkaBone : IHavokObject, IEquatable<hkaBone?>
    {
        public string name { set; get; } = "";
        public bool lockTranslation { set; get; }

        public virtual uint Signature { set; get; } = 0x35912f8a;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            name = des.ReadStringPointer(br);
            lockTranslation = br.ReadBoolean();
            br.Position += 7;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteStringPointer(bw, name);
            bw.WriteBoolean(lockTranslation);
            bw.Position += 7;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            name = xd.ReadString(xe, nameof(name));
            lockTranslation = xd.ReadBoolean(xe, nameof(lockTranslation));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(name), name);
            xs.WriteBoolean(xe, nameof(lockTranslation), lockTranslation);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaBone);
        }

        public bool Equals(hkaBone? other)
        {
            return other is not null &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   lockTranslation.Equals(other.lockTranslation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(name);
            hashcode.Add(lockTranslation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

