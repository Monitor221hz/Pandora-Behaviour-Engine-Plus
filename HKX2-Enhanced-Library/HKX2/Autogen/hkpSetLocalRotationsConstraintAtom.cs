using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSetLocalRotationsConstraintAtom Signatire: 0xf81db8e size: 112 flags: FLAGS_NONE

    // rotationA class:  Type.TYPE_ROTATION Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // rotationB class:  Type.TYPE_ROTATION Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpSetLocalRotationsConstraintAtom : hkpConstraintAtom, IEquatable<hkpSetLocalRotationsConstraintAtom?>
    {
        public Matrix4x4 rotationA { set; get; }
        public Matrix4x4 rotationB { set; get; }

        public override uint Signature { set; get; } = 0xf81db8e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 14;
            rotationA = des.ReadMatrix3(br); //TYPE_ROTATION
            rotationB = des.ReadMatrix3(br); //TYPE_ROTATION
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 14;
            s.WriteMatrix3(bw, rotationA);
            s.WriteMatrix3(bw, rotationB);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rotationA = xd.ReadRotation(xe, nameof(rotationA));
            rotationB = xd.ReadRotation(xe, nameof(rotationB));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteRotation(xe, nameof(rotationA), rotationA);
            xs.WriteRotation(xe, nameof(rotationB), rotationB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSetLocalRotationsConstraintAtom);
        }

        public bool Equals(hkpSetLocalRotationsConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rotationA.Equals(other.rotationA) &&
                   rotationB.Equals(other.rotationB) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rotationA);
            hashcode.Add(rotationB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

