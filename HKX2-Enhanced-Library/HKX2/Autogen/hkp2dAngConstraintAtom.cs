using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkp_2dAngConstraintAtom Signatire: 0xdcdb8b8b size: 4 flags: FLAGS_NONE

    // freeRotationAxis class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    public partial class hkp_2dAngConstraintAtom : hkpConstraintAtom, IEquatable<hkp_2dAngConstraintAtom?>
    {
        public byte freeRotationAxis { set; get; }

        public override uint Signature { set; get; } = 0xdcdb8b8b;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            freeRotationAxis = br.ReadByte();
            br.Position += 1;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(freeRotationAxis);
            bw.Position += 1;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            freeRotationAxis = xd.ReadByte(xe, nameof(freeRotationAxis));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(freeRotationAxis), freeRotationAxis);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkp_2dAngConstraintAtom);
        }

        public bool Equals(hkp_2dAngConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   freeRotationAxis.Equals(other.freeRotationAxis) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(freeRotationAxis);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

