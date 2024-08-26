using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbHandIkControlData Signatire: 0xd72b8d17 size: 96 flags: FLAGS_NONE

    // targetPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // targetRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // targetNormal class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // targetHandle class: hkbHandle Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // transformOnFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // normalOnFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // fadeInDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // fadeOutDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // extrapolationTimeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // handleChangeSpeed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    // handleChangeMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 80 flags: FLAGS_NONE enum: HandleChangeMode
    // fixUp class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 81 flags: FLAGS_NONE enum: 
    public partial class hkbHandIkControlData : IHavokObject, IEquatable<hkbHandIkControlData?>
    {
        public Vector4 targetPosition { set; get; }
        public Quaternion targetRotation { set; get; }
        public Vector4 targetNormal { set; get; }
        public hkbHandle? targetHandle { set; get; }
        public float transformOnFraction { set; get; }
        public float normalOnFraction { set; get; }
        public float fadeInDuration { set; get; }
        public float fadeOutDuration { set; get; }
        public float extrapolationTimeStep { set; get; }
        public float handleChangeSpeed { set; get; }
        public sbyte handleChangeMode { set; get; }
        public bool fixUp { set; get; }

        public virtual uint Signature { set; get; } = 0xd72b8d17;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            targetPosition = br.ReadVector4();
            targetRotation = des.ReadQuaternion(br);
            targetNormal = br.ReadVector4();
            targetHandle = des.ReadClassPointer<hkbHandle>(br);
            transformOnFraction = br.ReadSingle();
            normalOnFraction = br.ReadSingle();
            fadeInDuration = br.ReadSingle();
            fadeOutDuration = br.ReadSingle();
            extrapolationTimeStep = br.ReadSingle();
            handleChangeSpeed = br.ReadSingle();
            handleChangeMode = br.ReadSByte();
            fixUp = br.ReadBoolean();
            br.Position += 14;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(targetPosition);
            s.WriteQuaternion(bw, targetRotation);
            bw.WriteVector4(targetNormal);
            s.WriteClassPointer(bw, targetHandle);
            bw.WriteSingle(transformOnFraction);
            bw.WriteSingle(normalOnFraction);
            bw.WriteSingle(fadeInDuration);
            bw.WriteSingle(fadeOutDuration);
            bw.WriteSingle(extrapolationTimeStep);
            bw.WriteSingle(handleChangeSpeed);
            bw.WriteSByte(handleChangeMode);
            bw.WriteBoolean(fixUp);
            bw.Position += 14;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            targetPosition = xd.ReadVector4(xe, nameof(targetPosition));
            targetRotation = xd.ReadQuaternion(xe, nameof(targetRotation));
            targetNormal = xd.ReadVector4(xe, nameof(targetNormal));
            targetHandle = xd.ReadClassPointer<hkbHandle>(this, xe, nameof(targetHandle));
            transformOnFraction = xd.ReadSingle(xe, nameof(transformOnFraction));
            normalOnFraction = xd.ReadSingle(xe, nameof(normalOnFraction));
            fadeInDuration = xd.ReadSingle(xe, nameof(fadeInDuration));
            fadeOutDuration = xd.ReadSingle(xe, nameof(fadeOutDuration));
            extrapolationTimeStep = xd.ReadSingle(xe, nameof(extrapolationTimeStep));
            handleChangeSpeed = xd.ReadSingle(xe, nameof(handleChangeSpeed));
            handleChangeMode = xd.ReadFlag<HandleChangeMode, sbyte>(xe, nameof(handleChangeMode));
            fixUp = xd.ReadBoolean(xe, nameof(fixUp));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(targetPosition), targetPosition);
            xs.WriteQuaternion(xe, nameof(targetRotation), targetRotation);
            xs.WriteVector4(xe, nameof(targetNormal), targetNormal);
            xs.WriteClassPointer(xe, nameof(targetHandle), targetHandle);
            xs.WriteFloat(xe, nameof(transformOnFraction), transformOnFraction);
            xs.WriteFloat(xe, nameof(normalOnFraction), normalOnFraction);
            xs.WriteFloat(xe, nameof(fadeInDuration), fadeInDuration);
            xs.WriteFloat(xe, nameof(fadeOutDuration), fadeOutDuration);
            xs.WriteFloat(xe, nameof(extrapolationTimeStep), extrapolationTimeStep);
            xs.WriteFloat(xe, nameof(handleChangeSpeed), handleChangeSpeed);
            xs.WriteEnum<HandleChangeMode, sbyte>(xe, nameof(handleChangeMode), handleChangeMode);
            xs.WriteBoolean(xe, nameof(fixUp), fixUp);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbHandIkControlData);
        }

        public bool Equals(hkbHandIkControlData? other)
        {
            return other is not null &&
                   targetPosition.Equals(other.targetPosition) &&
                   targetRotation.Equals(other.targetRotation) &&
                   targetNormal.Equals(other.targetNormal) &&
                   ((targetHandle is null && other.targetHandle is null) || (targetHandle is not null && other.targetHandle is not null && targetHandle.Equals((IHavokObject)other.targetHandle))) &&
                   transformOnFraction.Equals(other.transformOnFraction) &&
                   normalOnFraction.Equals(other.normalOnFraction) &&
                   fadeInDuration.Equals(other.fadeInDuration) &&
                   fadeOutDuration.Equals(other.fadeOutDuration) &&
                   extrapolationTimeStep.Equals(other.extrapolationTimeStep) &&
                   handleChangeSpeed.Equals(other.handleChangeSpeed) &&
                   handleChangeMode.Equals(other.handleChangeMode) &&
                   fixUp.Equals(other.fixUp) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(targetPosition);
            hashcode.Add(targetRotation);
            hashcode.Add(targetNormal);
            hashcode.Add(targetHandle);
            hashcode.Add(transformOnFraction);
            hashcode.Add(normalOnFraction);
            hashcode.Add(fadeInDuration);
            hashcode.Add(fadeOutDuration);
            hashcode.Add(extrapolationTimeStep);
            hashcode.Add(handleChangeSpeed);
            hashcode.Add(handleChangeMode);
            hashcode.Add(fixUp);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

