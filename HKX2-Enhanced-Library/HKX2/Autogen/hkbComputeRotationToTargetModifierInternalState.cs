using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbComputeRotationToTargetModifierInternalState Signatire: 0x71cd1eb0 size: 32 flags: FLAGS_NONE

    // rotationOut class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbComputeRotationToTargetModifierInternalState : hkReferencedObject, IEquatable<hkbComputeRotationToTargetModifierInternalState?>
    {
        public Quaternion rotationOut { set; get; }

        public override uint Signature { set; get; } = 0x71cd1eb0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rotationOut = des.ReadQuaternion(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQuaternion(bw, rotationOut);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rotationOut = xd.ReadQuaternion(xe, nameof(rotationOut));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQuaternion(xe, nameof(rotationOut), rotationOut);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbComputeRotationToTargetModifierInternalState);
        }

        public bool Equals(hkbComputeRotationToTargetModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rotationOut.Equals(other.rotationOut) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rotationOut);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

