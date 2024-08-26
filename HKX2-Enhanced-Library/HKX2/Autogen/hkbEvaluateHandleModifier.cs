using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEvaluateHandleModifier Signatire: 0x79757102 size: 240 flags: FLAGS_NONE

    // handle class: hkbHandle Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // handlePositionOut class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // handleRotationOut class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // isValidOut class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // extrapolationTimeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 132 flags: FLAGS_NONE enum: 
    // handleChangeSpeed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // handleChangeMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 140 flags: FLAGS_NONE enum: HandleChangeMode
    // oldHandle class: hkbHandle Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // oldHandlePosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 192 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // oldHandleRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 208 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timeSinceLastModify class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // smoothlyChangingHandles class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 228 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbEvaluateHandleModifier : hkbModifier, IEquatable<hkbEvaluateHandleModifier?>
    {
        public hkbHandle? handle { set; get; }
        public Vector4 handlePositionOut { set; get; }
        public Quaternion handleRotationOut { set; get; }
        public bool isValidOut { set; get; }
        public float extrapolationTimeStep { set; get; }
        public float handleChangeSpeed { set; get; }
        public sbyte handleChangeMode { set; get; }
        public hkbHandle oldHandle { set; get; } = new();
        private Vector4 oldHandlePosition { set; get; }
        private Quaternion oldHandleRotation { set; get; }
        private float timeSinceLastModify { set; get; }
        private bool smoothlyChangingHandles { set; get; }

        public override uint Signature { set; get; } = 0x79757102;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            handle = des.ReadClassPointer<hkbHandle>(br);
            br.Position += 8;
            handlePositionOut = br.ReadVector4();
            handleRotationOut = des.ReadQuaternion(br);
            isValidOut = br.ReadBoolean();
            br.Position += 3;
            extrapolationTimeStep = br.ReadSingle();
            handleChangeSpeed = br.ReadSingle();
            handleChangeMode = br.ReadSByte();
            br.Position += 3;
            oldHandle.Read(des, br);
            oldHandlePosition = br.ReadVector4();
            oldHandleRotation = des.ReadQuaternion(br);
            timeSinceLastModify = br.ReadSingle();
            smoothlyChangingHandles = br.ReadBoolean();
            br.Position += 11;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, handle);
            bw.Position += 8;
            bw.WriteVector4(handlePositionOut);
            s.WriteQuaternion(bw, handleRotationOut);
            bw.WriteBoolean(isValidOut);
            bw.Position += 3;
            bw.WriteSingle(extrapolationTimeStep);
            bw.WriteSingle(handleChangeSpeed);
            bw.WriteSByte(handleChangeMode);
            bw.Position += 3;
            oldHandle.Write(s, bw);
            bw.WriteVector4(oldHandlePosition);
            s.WriteQuaternion(bw, oldHandleRotation);
            bw.WriteSingle(timeSinceLastModify);
            bw.WriteBoolean(smoothlyChangingHandles);
            bw.Position += 11;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            handle = xd.ReadClassPointer<hkbHandle>(this, xe, nameof(handle));
            handlePositionOut = xd.ReadVector4(xe, nameof(handlePositionOut));
            handleRotationOut = xd.ReadQuaternion(xe, nameof(handleRotationOut));
            isValidOut = xd.ReadBoolean(xe, nameof(isValidOut));
            extrapolationTimeStep = xd.ReadSingle(xe, nameof(extrapolationTimeStep));
            handleChangeSpeed = xd.ReadSingle(xe, nameof(handleChangeSpeed));
            handleChangeMode = xd.ReadFlag<HandleChangeMode, sbyte>(xe, nameof(handleChangeMode));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(handle), handle);
            xs.WriteVector4(xe, nameof(handlePositionOut), handlePositionOut);
            xs.WriteQuaternion(xe, nameof(handleRotationOut), handleRotationOut);
            xs.WriteBoolean(xe, nameof(isValidOut), isValidOut);
            xs.WriteFloat(xe, nameof(extrapolationTimeStep), extrapolationTimeStep);
            xs.WriteFloat(xe, nameof(handleChangeSpeed), handleChangeSpeed);
            xs.WriteEnum<HandleChangeMode, sbyte>(xe, nameof(handleChangeMode), handleChangeMode);
            xs.WriteSerializeIgnored(xe, nameof(oldHandle));
            xs.WriteSerializeIgnored(xe, nameof(oldHandlePosition));
            xs.WriteSerializeIgnored(xe, nameof(oldHandleRotation));
            xs.WriteSerializeIgnored(xe, nameof(timeSinceLastModify));
            xs.WriteSerializeIgnored(xe, nameof(smoothlyChangingHandles));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEvaluateHandleModifier);
        }

        public bool Equals(hkbEvaluateHandleModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((handle is null && other.handle is null) || (handle is not null && other.handle is not null && handle.Equals((IHavokObject)other.handle))) &&
                   handlePositionOut.Equals(other.handlePositionOut) &&
                   handleRotationOut.Equals(other.handleRotationOut) &&
                   isValidOut.Equals(other.isValidOut) &&
                   extrapolationTimeStep.Equals(other.extrapolationTimeStep) &&
                   handleChangeSpeed.Equals(other.handleChangeSpeed) &&
                   handleChangeMode.Equals(other.handleChangeMode) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(handle);
            hashcode.Add(handlePositionOut);
            hashcode.Add(handleRotationOut);
            hashcode.Add(isValidOut);
            hashcode.Add(extrapolationTimeStep);
            hashcode.Add(handleChangeSpeed);
            hashcode.Add(handleChangeMode);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

