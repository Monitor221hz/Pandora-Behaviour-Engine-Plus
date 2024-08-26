using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCharacterRigidBodyCinfo Signatire: 0x892f441 size: 128 flags: FLAGS_NONE

    // collisionFilterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // shape class: hkpShape Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // position class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // rotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // mass class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // friction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // maxLinearVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // allowedPenetrationDepth class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    // up class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // maxSlope class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // maxForce class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // unweldingHeightOffsetFactor class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // maxSpeedForSimplexSolver class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // supportDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // hardSupportDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 116 flags: FLAGS_NONE enum: 
    // vdbColor class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    public partial class hkpCharacterRigidBodyCinfo : hkpCharacterControllerCinfo, IEquatable<hkpCharacterRigidBodyCinfo?>
    {
        public uint collisionFilterInfo { set; get; }
        public hkpShape? shape { set; get; }
        public Vector4 position { set; get; }
        public Quaternion rotation { set; get; }
        public float mass { set; get; }
        public float friction { set; get; }
        public float maxLinearVelocity { set; get; }
        public float allowedPenetrationDepth { set; get; }
        public Vector4 up { set; get; }
        public float maxSlope { set; get; }
        public float maxForce { set; get; }
        public float unweldingHeightOffsetFactor { set; get; }
        public float maxSpeedForSimplexSolver { set; get; }
        public float supportDistance { set; get; }
        public float hardSupportDistance { set; get; }
        public int vdbColor { set; get; }

        public override uint Signature { set; get; } = 0x892f441;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            collisionFilterInfo = br.ReadUInt32();
            br.Position += 4;
            shape = des.ReadClassPointer<hkpShape>(br);
            position = br.ReadVector4();
            rotation = des.ReadQuaternion(br);
            mass = br.ReadSingle();
            friction = br.ReadSingle();
            maxLinearVelocity = br.ReadSingle();
            allowedPenetrationDepth = br.ReadSingle();
            up = br.ReadVector4();
            maxSlope = br.ReadSingle();
            maxForce = br.ReadSingle();
            unweldingHeightOffsetFactor = br.ReadSingle();
            maxSpeedForSimplexSolver = br.ReadSingle();
            supportDistance = br.ReadSingle();
            hardSupportDistance = br.ReadSingle();
            vdbColor = br.ReadInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt32(collisionFilterInfo);
            bw.Position += 4;
            s.WriteClassPointer(bw, shape);
            bw.WriteVector4(position);
            s.WriteQuaternion(bw, rotation);
            bw.WriteSingle(mass);
            bw.WriteSingle(friction);
            bw.WriteSingle(maxLinearVelocity);
            bw.WriteSingle(allowedPenetrationDepth);
            bw.WriteVector4(up);
            bw.WriteSingle(maxSlope);
            bw.WriteSingle(maxForce);
            bw.WriteSingle(unweldingHeightOffsetFactor);
            bw.WriteSingle(maxSpeedForSimplexSolver);
            bw.WriteSingle(supportDistance);
            bw.WriteSingle(hardSupportDistance);
            bw.WriteInt32(vdbColor);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            collisionFilterInfo = xd.ReadUInt32(xe, nameof(collisionFilterInfo));
            shape = xd.ReadClassPointer<hkpShape>(this, xe, nameof(shape));
            position = xd.ReadVector4(xe, nameof(position));
            rotation = xd.ReadQuaternion(xe, nameof(rotation));
            mass = xd.ReadSingle(xe, nameof(mass));
            friction = xd.ReadSingle(xe, nameof(friction));
            maxLinearVelocity = xd.ReadSingle(xe, nameof(maxLinearVelocity));
            allowedPenetrationDepth = xd.ReadSingle(xe, nameof(allowedPenetrationDepth));
            up = xd.ReadVector4(xe, nameof(up));
            maxSlope = xd.ReadSingle(xe, nameof(maxSlope));
            maxForce = xd.ReadSingle(xe, nameof(maxForce));
            unweldingHeightOffsetFactor = xd.ReadSingle(xe, nameof(unweldingHeightOffsetFactor));
            maxSpeedForSimplexSolver = xd.ReadSingle(xe, nameof(maxSpeedForSimplexSolver));
            supportDistance = xd.ReadSingle(xe, nameof(supportDistance));
            hardSupportDistance = xd.ReadSingle(xe, nameof(hardSupportDistance));
            vdbColor = xd.ReadInt32(xe, nameof(vdbColor));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(collisionFilterInfo), collisionFilterInfo);
            xs.WriteClassPointer(xe, nameof(shape), shape);
            xs.WriteVector4(xe, nameof(position), position);
            xs.WriteQuaternion(xe, nameof(rotation), rotation);
            xs.WriteFloat(xe, nameof(mass), mass);
            xs.WriteFloat(xe, nameof(friction), friction);
            xs.WriteFloat(xe, nameof(maxLinearVelocity), maxLinearVelocity);
            xs.WriteFloat(xe, nameof(allowedPenetrationDepth), allowedPenetrationDepth);
            xs.WriteVector4(xe, nameof(up), up);
            xs.WriteFloat(xe, nameof(maxSlope), maxSlope);
            xs.WriteFloat(xe, nameof(maxForce), maxForce);
            xs.WriteFloat(xe, nameof(unweldingHeightOffsetFactor), unweldingHeightOffsetFactor);
            xs.WriteFloat(xe, nameof(maxSpeedForSimplexSolver), maxSpeedForSimplexSolver);
            xs.WriteFloat(xe, nameof(supportDistance), supportDistance);
            xs.WriteFloat(xe, nameof(hardSupportDistance), hardSupportDistance);
            xs.WriteNumber(xe, nameof(vdbColor), vdbColor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCharacterRigidBodyCinfo);
        }

        public bool Equals(hkpCharacterRigidBodyCinfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   collisionFilterInfo.Equals(other.collisionFilterInfo) &&
                   ((shape is null && other.shape is null) || (shape is not null && other.shape is not null && shape.Equals((IHavokObject)other.shape))) &&
                   position.Equals(other.position) &&
                   rotation.Equals(other.rotation) &&
                   mass.Equals(other.mass) &&
                   friction.Equals(other.friction) &&
                   maxLinearVelocity.Equals(other.maxLinearVelocity) &&
                   allowedPenetrationDepth.Equals(other.allowedPenetrationDepth) &&
                   up.Equals(other.up) &&
                   maxSlope.Equals(other.maxSlope) &&
                   maxForce.Equals(other.maxForce) &&
                   unweldingHeightOffsetFactor.Equals(other.unweldingHeightOffsetFactor) &&
                   maxSpeedForSimplexSolver.Equals(other.maxSpeedForSimplexSolver) &&
                   supportDistance.Equals(other.supportDistance) &&
                   hardSupportDistance.Equals(other.hardSupportDistance) &&
                   vdbColor.Equals(other.vdbColor) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(collisionFilterInfo);
            hashcode.Add(shape);
            hashcode.Add(position);
            hashcode.Add(rotation);
            hashcode.Add(mass);
            hashcode.Add(friction);
            hashcode.Add(maxLinearVelocity);
            hashcode.Add(allowedPenetrationDepth);
            hashcode.Add(up);
            hashcode.Add(maxSlope);
            hashcode.Add(maxForce);
            hashcode.Add(unweldingHeightOffsetFactor);
            hashcode.Add(maxSpeedForSimplexSolver);
            hashcode.Add(supportDistance);
            hashcode.Add(hardSupportDistance);
            hashcode.Add(vdbColor);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

