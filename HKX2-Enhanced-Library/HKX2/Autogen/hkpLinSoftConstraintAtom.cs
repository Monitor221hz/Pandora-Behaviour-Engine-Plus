using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpLinSoftConstraintAtom Signatire: 0x52b27d69 size: 12 flags: FLAGS_NONE

    // axisIndex class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // tau class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // damping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpLinSoftConstraintAtom : hkpConstraintAtom, IEquatable<hkpLinSoftConstraintAtom?>
    {
        public byte axisIndex { set; get; }
        public float tau { set; get; }
        public float damping { set; get; }

        public override uint Signature { set; get; } = 0x52b27d69;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            axisIndex = br.ReadByte();
            br.Position += 1;
            tau = br.ReadSingle();
            damping = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(axisIndex);
            bw.Position += 1;
            bw.WriteSingle(tau);
            bw.WriteSingle(damping);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            axisIndex = xd.ReadByte(xe, nameof(axisIndex));
            tau = xd.ReadSingle(xe, nameof(tau));
            damping = xd.ReadSingle(xe, nameof(damping));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(axisIndex), axisIndex);
            xs.WriteFloat(xe, nameof(tau), tau);
            xs.WriteFloat(xe, nameof(damping), damping);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpLinSoftConstraintAtom);
        }

        public bool Equals(hkpLinSoftConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   axisIndex.Equals(other.axisIndex) &&
                   tau.Equals(other.tau) &&
                   damping.Equals(other.damping) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(axisIndex);
            hashcode.Add(tau);
            hashcode.Add(damping);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

