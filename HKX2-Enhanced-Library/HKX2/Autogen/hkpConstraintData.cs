using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConstraintData Signatire: 0x80559a4e size: 24 flags: FLAGS_NONE

    // userData class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpConstraintData : hkReferencedObject, IEquatable<hkpConstraintData?>
    {
        public ulong userData { set; get; }

        public override uint Signature { set; get; } = 0x80559a4e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            userData = br.ReadUInt64();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt64(userData);
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
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConstraintData);
        }

        public bool Equals(hkpConstraintData? other)
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

