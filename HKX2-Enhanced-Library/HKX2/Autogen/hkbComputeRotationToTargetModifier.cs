using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbComputeRotationToTargetModifier Signatire: 0x47665f1c size: 192 flags: FLAGS_NONE

    // rotationOut class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // targetPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // currentPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // currentRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // localAxisOfRotation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // localFacingDirection class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // resultIsDelta class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    public partial class hkbComputeRotationToTargetModifier : hkbModifier, IEquatable<hkbComputeRotationToTargetModifier?>
    {
        public Quaternion rotationOut { set; get; }
        public Vector4 targetPosition { set; get; }
        public Vector4 currentPosition { set; get; }
        public Quaternion currentRotation { set; get; }
        public Vector4 localAxisOfRotation { set; get; }
        public Vector4 localFacingDirection { set; get; }
        public bool resultIsDelta { set; get; }

        public override uint Signature { set; get; } = 0x47665f1c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rotationOut = des.ReadQuaternion(br);
            targetPosition = br.ReadVector4();
            currentPosition = br.ReadVector4();
            currentRotation = des.ReadQuaternion(br);
            localAxisOfRotation = br.ReadVector4();
            localFacingDirection = br.ReadVector4();
            resultIsDelta = br.ReadBoolean();
            br.Position += 15;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQuaternion(bw, rotationOut);
            bw.WriteVector4(targetPosition);
            bw.WriteVector4(currentPosition);
            s.WriteQuaternion(bw, currentRotation);
            bw.WriteVector4(localAxisOfRotation);
            bw.WriteVector4(localFacingDirection);
            bw.WriteBoolean(resultIsDelta);
            bw.Position += 15;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rotationOut = xd.ReadQuaternion(xe, nameof(rotationOut));
            targetPosition = xd.ReadVector4(xe, nameof(targetPosition));
            currentPosition = xd.ReadVector4(xe, nameof(currentPosition));
            currentRotation = xd.ReadQuaternion(xe, nameof(currentRotation));
            localAxisOfRotation = xd.ReadVector4(xe, nameof(localAxisOfRotation));
            localFacingDirection = xd.ReadVector4(xe, nameof(localFacingDirection));
            resultIsDelta = xd.ReadBoolean(xe, nameof(resultIsDelta));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQuaternion(xe, nameof(rotationOut), rotationOut);
            xs.WriteVector4(xe, nameof(targetPosition), targetPosition);
            xs.WriteVector4(xe, nameof(currentPosition), currentPosition);
            xs.WriteQuaternion(xe, nameof(currentRotation), currentRotation);
            xs.WriteVector4(xe, nameof(localAxisOfRotation), localAxisOfRotation);
            xs.WriteVector4(xe, nameof(localFacingDirection), localFacingDirection);
            xs.WriteBoolean(xe, nameof(resultIsDelta), resultIsDelta);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbComputeRotationToTargetModifier);
        }

        public bool Equals(hkbComputeRotationToTargetModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rotationOut.Equals(other.rotationOut) &&
                   targetPosition.Equals(other.targetPosition) &&
                   currentPosition.Equals(other.currentPosition) &&
                   currentRotation.Equals(other.currentRotation) &&
                   localAxisOfRotation.Equals(other.localAxisOfRotation) &&
                   localFacingDirection.Equals(other.localFacingDirection) &&
                   resultIsDelta.Equals(other.resultIsDelta) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rotationOut);
            hashcode.Add(targetPosition);
            hashcode.Add(currentPosition);
            hashcode.Add(currentRotation);
            hashcode.Add(localAxisOfRotation);
            hashcode.Add(localFacingDirection);
            hashcode.Add(resultIsDelta);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

