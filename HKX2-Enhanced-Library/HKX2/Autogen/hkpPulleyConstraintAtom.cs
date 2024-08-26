using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPulleyConstraintAtom Signatire: 0x94a08848 size: 64 flags: FLAGS_NONE

    // fixedPivotAinWorld class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // fixedPivotBinWorld class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // ropeLength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // leverageOnBodyB class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 52 flags: FLAGS_NONE enum: 
    public partial class hkpPulleyConstraintAtom : hkpConstraintAtom, IEquatable<hkpPulleyConstraintAtom?>
    {
        public Vector4 fixedPivotAinWorld { set; get; }
        public Vector4 fixedPivotBinWorld { set; get; }
        public float ropeLength { set; get; }
        public float leverageOnBodyB { set; get; }

        public override uint Signature { set; get; } = 0x94a08848;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 14;
            fixedPivotAinWorld = br.ReadVector4();
            fixedPivotBinWorld = br.ReadVector4();
            ropeLength = br.ReadSingle();
            leverageOnBodyB = br.ReadSingle();
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 14;
            bw.WriteVector4(fixedPivotAinWorld);
            bw.WriteVector4(fixedPivotBinWorld);
            bw.WriteSingle(ropeLength);
            bw.WriteSingle(leverageOnBodyB);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            fixedPivotAinWorld = xd.ReadVector4(xe, nameof(fixedPivotAinWorld));
            fixedPivotBinWorld = xd.ReadVector4(xe, nameof(fixedPivotBinWorld));
            ropeLength = xd.ReadSingle(xe, nameof(ropeLength));
            leverageOnBodyB = xd.ReadSingle(xe, nameof(leverageOnBodyB));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(fixedPivotAinWorld), fixedPivotAinWorld);
            xs.WriteVector4(xe, nameof(fixedPivotBinWorld), fixedPivotBinWorld);
            xs.WriteFloat(xe, nameof(ropeLength), ropeLength);
            xs.WriteFloat(xe, nameof(leverageOnBodyB), leverageOnBodyB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPulleyConstraintAtom);
        }

        public bool Equals(hkpPulleyConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   fixedPivotAinWorld.Equals(other.fixedPivotAinWorld) &&
                   fixedPivotBinWorld.Equals(other.fixedPivotBinWorld) &&
                   ropeLength.Equals(other.ropeLength) &&
                   leverageOnBodyB.Equals(other.leverageOnBodyB) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(fixedPivotAinWorld);
            hashcode.Add(fixedPivotBinWorld);
            hashcode.Add(ropeLength);
            hashcode.Add(leverageOnBodyB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

