using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpShape Signatire: 0x666490a1 size: 32 flags: FLAGS_NONE

    // userData class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_UINT32 arrSize: 0 offset: 24 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpShape : hkReferencedObject, IEquatable<hkpShape?>
    {
        public ulong userData { set; get; }
        private uint type { set; get; }

        public override uint Signature { set; get; } = 0x666490a1;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            userData = br.ReadUInt64();
            type = br.ReadUInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(userData);
            bw.WriteUInt32(type);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            userData = xd.ReadUInt64(xe, nameof(userData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(userData), userData);
            xs.WriteSerializeIgnored(xe, nameof(type));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpShape);
        }

        public bool Equals(hkpShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   userData.Equals(other.userData) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(userData);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

