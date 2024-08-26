using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpAngFrictionConstraintAtom Signatire: 0xf313aa80 size: 12 flags: FLAGS_NONE

    // isEnabled class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // firstFrictionAxis class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 3 flags: FLAGS_NONE enum: 
    // numFrictionAxes class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // maxFrictionTorque class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpAngFrictionConstraintAtom : hkpConstraintAtom, IEquatable<hkpAngFrictionConstraintAtom?>
    {
        public byte isEnabled { set; get; }
        public byte firstFrictionAxis { set; get; }
        public byte numFrictionAxes { set; get; }
        public float maxFrictionTorque { set; get; }

        public override uint Signature { set; get; } = 0xf313aa80;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isEnabled = br.ReadByte();
            firstFrictionAxis = br.ReadByte();
            numFrictionAxes = br.ReadByte();
            br.Position += 3;
            maxFrictionTorque = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(isEnabled);
            bw.WriteByte(firstFrictionAxis);
            bw.WriteByte(numFrictionAxes);
            bw.Position += 3;
            bw.WriteSingle(maxFrictionTorque);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isEnabled = xd.ReadByte(xe, nameof(isEnabled));
            firstFrictionAxis = xd.ReadByte(xe, nameof(firstFrictionAxis));
            numFrictionAxes = xd.ReadByte(xe, nameof(numFrictionAxes));
            maxFrictionTorque = xd.ReadSingle(xe, nameof(maxFrictionTorque));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(isEnabled), isEnabled);
            xs.WriteNumber(xe, nameof(firstFrictionAxis), firstFrictionAxis);
            xs.WriteNumber(xe, nameof(numFrictionAxes), numFrictionAxes);
            xs.WriteFloat(xe, nameof(maxFrictionTorque), maxFrictionTorque);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpAngFrictionConstraintAtom);
        }

        public bool Equals(hkpAngFrictionConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isEnabled.Equals(other.isEnabled) &&
                   firstFrictionAxis.Equals(other.firstFrictionAxis) &&
                   numFrictionAxes.Equals(other.numFrictionAxes) &&
                   maxFrictionTorque.Equals(other.maxFrictionTorque) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(isEnabled);
            hashcode.Add(firstFrictionAxis);
            hashcode.Add(numFrictionAxes);
            hashcode.Add(maxFrictionTorque);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

