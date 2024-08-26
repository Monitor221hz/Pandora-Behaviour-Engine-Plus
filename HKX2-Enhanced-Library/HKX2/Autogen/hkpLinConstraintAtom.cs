using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpLinConstraintAtom Signatire: 0x7b6b0210 size: 4 flags: FLAGS_NONE

    // axisIndex class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    public partial class hkpLinConstraintAtom : hkpConstraintAtom, IEquatable<hkpLinConstraintAtom?>
    {
        public byte axisIndex { set; get; }

        public override uint Signature { set; get; } = 0x7b6b0210;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            axisIndex = br.ReadByte();
            br.Position += 1;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(axisIndex);
            bw.Position += 1;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            axisIndex = xd.ReadByte(xe, nameof(axisIndex));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(axisIndex), axisIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpLinConstraintAtom);
        }

        public bool Equals(hkpLinConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   axisIndex.Equals(other.axisIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(axisIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

