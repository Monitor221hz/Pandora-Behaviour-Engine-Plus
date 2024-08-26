using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkMotionState Signatire: 0x5797386e size: 176 flags: FLAGS_NONE

    // transform class:  Type.TYPE_TRANSFORM Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // sweptTransform class: hkSweptTransform Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // deltaAngle class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // objectRadius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // linearDamping class:  Type.TYPE_HALF Type.TYPE_VOID arrSize: 0 offset: 164 flags: FLAGS_NONE enum: 
    // angularDamping class:  Type.TYPE_HALF Type.TYPE_VOID arrSize: 0 offset: 166 flags: FLAGS_NONE enum: 
    // timeFactor class:  Type.TYPE_HALF Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // maxLinearVelocity class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 170 flags: FLAGS_NONE enum: 
    // maxAngularVelocity class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 171 flags: FLAGS_NONE enum: 
    // deactivationClass class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 172 flags: FLAGS_NONE enum: 
    public partial class hkMotionState : IHavokObject, IEquatable<hkMotionState?>
    {
        public Matrix4x4 transform { set; get; }
        public hkSweptTransform sweptTransform { set; get; } = new();
        public Vector4 deltaAngle { set; get; }
        public float objectRadius { set; get; }
        public Half linearDamping { set; get; }
        public Half angularDamping { set; get; }
        public Half timeFactor { set; get; }
        public byte maxLinearVelocity { set; get; }
        public byte maxAngularVelocity { set; get; }
        public byte deactivationClass { set; get; }

        public virtual uint Signature { set; get; } = 0x5797386e;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            transform = des.ReadTransform(br);
            sweptTransform.Read(des, br);
            deltaAngle = br.ReadVector4();
            objectRadius = br.ReadSingle();
            linearDamping = br.ReadHalf();
            angularDamping = br.ReadHalf();
            timeFactor = br.ReadHalf();
            maxLinearVelocity = br.ReadByte();
            maxAngularVelocity = br.ReadByte();
            deactivationClass = br.ReadByte();
            br.Position += 3;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteTransform(bw, transform);
            sweptTransform.Write(s, bw);
            bw.WriteVector4(deltaAngle);
            bw.WriteSingle(objectRadius);
            bw.WriteHalf(linearDamping);
            bw.WriteHalf(angularDamping);
            bw.WriteHalf(timeFactor);
            bw.WriteByte(maxLinearVelocity);
            bw.WriteByte(maxAngularVelocity);
            bw.WriteByte(deactivationClass);
            bw.Position += 3;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transform = xd.ReadTransform(xe, nameof(transform));
            sweptTransform = xd.ReadClass<hkSweptTransform>(xe, nameof(sweptTransform));
            deltaAngle = xd.ReadVector4(xe, nameof(deltaAngle));
            objectRadius = xd.ReadSingle(xe, nameof(objectRadius));
            linearDamping = xd.ReadHalf(xe, nameof(linearDamping));
            angularDamping = xd.ReadHalf(xe, nameof(angularDamping));
            timeFactor = xd.ReadHalf(xe, nameof(timeFactor));
            maxLinearVelocity = xd.ReadByte(xe, nameof(maxLinearVelocity));
            maxAngularVelocity = xd.ReadByte(xe, nameof(maxAngularVelocity));
            deactivationClass = xd.ReadByte(xe, nameof(deactivationClass));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteTransform(xe, nameof(transform), transform);
            xs.WriteClass<hkSweptTransform>(xe, nameof(sweptTransform), sweptTransform);
            xs.WriteVector4(xe, nameof(deltaAngle), deltaAngle);
            xs.WriteFloat(xe, nameof(objectRadius), objectRadius);
            xs.WriteFloat(xe, nameof(linearDamping), linearDamping);
            xs.WriteFloat(xe, nameof(angularDamping), angularDamping);
            xs.WriteFloat(xe, nameof(timeFactor), timeFactor);
            xs.WriteNumber(xe, nameof(maxLinearVelocity), maxLinearVelocity);
            xs.WriteNumber(xe, nameof(maxAngularVelocity), maxAngularVelocity);
            xs.WriteNumber(xe, nameof(deactivationClass), deactivationClass);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkMotionState);
        }

        public bool Equals(hkMotionState? other)
        {
            return other is not null &&
                   transform.Equals(other.transform) &&
                   ((sweptTransform is null && other.sweptTransform is null) || (sweptTransform is not null && other.sweptTransform is not null && sweptTransform.Equals((IHavokObject)other.sweptTransform))) &&
                   deltaAngle.Equals(other.deltaAngle) &&
                   objectRadius.Equals(other.objectRadius) &&
                   linearDamping.Equals(other.linearDamping) &&
                   angularDamping.Equals(other.angularDamping) &&
                   timeFactor.Equals(other.timeFactor) &&
                   maxLinearVelocity.Equals(other.maxLinearVelocity) &&
                   maxAngularVelocity.Equals(other.maxAngularVelocity) &&
                   deactivationClass.Equals(other.deactivationClass) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transform);
            hashcode.Add(sweptTransform);
            hashcode.Add(deltaAngle);
            hashcode.Add(objectRadius);
            hashcode.Add(linearDamping);
            hashcode.Add(angularDamping);
            hashcode.Add(timeFactor);
            hashcode.Add(maxLinearVelocity);
            hashcode.Add(maxAngularVelocity);
            hashcode.Add(deactivationClass);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

