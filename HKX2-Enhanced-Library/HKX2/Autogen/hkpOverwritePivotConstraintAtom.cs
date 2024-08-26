using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpOverwritePivotConstraintAtom Signatire: 0x1f11b467 size: 4 flags: FLAGS_NONE

    // copyToPivotBFromPivotA class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    public partial class hkpOverwritePivotConstraintAtom : hkpConstraintAtom, IEquatable<hkpOverwritePivotConstraintAtom?>
    {
        public byte copyToPivotBFromPivotA { set; get; }

        public override uint Signature { set; get; } = 0x1f11b467;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            copyToPivotBFromPivotA = br.ReadByte();
            br.Position += 1;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(copyToPivotBFromPivotA);
            bw.Position += 1;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            copyToPivotBFromPivotA = xd.ReadByte(xe, nameof(copyToPivotBFromPivotA));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(copyToPivotBFromPivotA), copyToPivotBFromPivotA);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpOverwritePivotConstraintAtom);
        }

        public bool Equals(hkpOverwritePivotConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   copyToPivotBFromPivotA.Equals(other.copyToPivotBFromPivotA) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(copyToPivotBFromPivotA);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

