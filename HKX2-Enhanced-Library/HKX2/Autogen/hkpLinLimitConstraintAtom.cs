using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpLinLimitConstraintAtom Signatire: 0xa44d1b07 size: 12 flags: FLAGS_NONE

    // axisIndex class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // min class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // max class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpLinLimitConstraintAtom : hkpConstraintAtom, IEquatable<hkpLinLimitConstraintAtom?>
    {
        public byte axisIndex { set; get; }
        public float min { set; get; }
        public float max { set; get; }

        public override uint Signature { set; get; } = 0xa44d1b07;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            axisIndex = br.ReadByte();
            br.Position += 1;
            min = br.ReadSingle();
            max = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(axisIndex);
            bw.Position += 1;
            bw.WriteSingle(min);
            bw.WriteSingle(max);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            axisIndex = xd.ReadByte(xe, nameof(axisIndex));
            min = xd.ReadSingle(xe, nameof(min));
            max = xd.ReadSingle(xe, nameof(max));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(axisIndex), axisIndex);
            xs.WriteFloat(xe, nameof(min), min);
            xs.WriteFloat(xe, nameof(max), max);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpLinLimitConstraintAtom);
        }

        public bool Equals(hkpLinLimitConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   axisIndex.Equals(other.axisIndex) &&
                   min.Equals(other.min) &&
                   max.Equals(other.max) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(axisIndex);
            hashcode.Add(min);
            hashcode.Add(max);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

