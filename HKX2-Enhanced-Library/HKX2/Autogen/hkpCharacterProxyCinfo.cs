using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCharacterProxyCinfo Signatire: 0x586d97b2 size: 144 flags: FLAGS_NONE

    // position class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // velocity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // dynamicFriction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // staticFriction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 52 flags: FLAGS_NONE enum: 
    // keepContactTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // up class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // extraUpStaticFriction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // extraDownStaticFriction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // shapePhantom class: hkpShapePhantom Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // keepDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // contactAngleSensitivity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // userPlanes class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // maxCharacterSpeedForSolver class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // characterStrength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // characterMass class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 116 flags: FLAGS_NONE enum: 
    // maxSlope class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // penetrationRecoverySpeed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 124 flags: FLAGS_NONE enum: 
    // maxCastIterations class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // refreshManifoldInCheckSupport class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 132 flags: FLAGS_NONE enum: 
    public partial class hkpCharacterProxyCinfo : hkpCharacterControllerCinfo, IEquatable<hkpCharacterProxyCinfo?>
    {
        public Vector4 position { set; get; }
        public Vector4 velocity { set; get; }
        public float dynamicFriction { set; get; }
        public float staticFriction { set; get; }
        public float keepContactTolerance { set; get; }
        public Vector4 up { set; get; }
        public float extraUpStaticFriction { set; get; }
        public float extraDownStaticFriction { set; get; }
        public hkpShapePhantom? shapePhantom { set; get; }
        public float keepDistance { set; get; }
        public float contactAngleSensitivity { set; get; }
        public uint userPlanes { set; get; }
        public float maxCharacterSpeedForSolver { set; get; }
        public float characterStrength { set; get; }
        public float characterMass { set; get; }
        public float maxSlope { set; get; }
        public float penetrationRecoverySpeed { set; get; }
        public int maxCastIterations { set; get; }
        public bool refreshManifoldInCheckSupport { set; get; }

        public override uint Signature { set; get; } = 0x586d97b2;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            position = br.ReadVector4();
            velocity = br.ReadVector4();
            dynamicFriction = br.ReadSingle();
            staticFriction = br.ReadSingle();
            keepContactTolerance = br.ReadSingle();
            br.Position += 4;
            up = br.ReadVector4();
            extraUpStaticFriction = br.ReadSingle();
            extraDownStaticFriction = br.ReadSingle();
            shapePhantom = des.ReadClassPointer<hkpShapePhantom>(br);
            keepDistance = br.ReadSingle();
            contactAngleSensitivity = br.ReadSingle();
            userPlanes = br.ReadUInt32();
            maxCharacterSpeedForSolver = br.ReadSingle();
            characterStrength = br.ReadSingle();
            characterMass = br.ReadSingle();
            maxSlope = br.ReadSingle();
            penetrationRecoverySpeed = br.ReadSingle();
            maxCastIterations = br.ReadInt32();
            refreshManifoldInCheckSupport = br.ReadBoolean();
            br.Position += 11;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(position);
            bw.WriteVector4(velocity);
            bw.WriteSingle(dynamicFriction);
            bw.WriteSingle(staticFriction);
            bw.WriteSingle(keepContactTolerance);
            bw.Position += 4;
            bw.WriteVector4(up);
            bw.WriteSingle(extraUpStaticFriction);
            bw.WriteSingle(extraDownStaticFriction);
            s.WriteClassPointer(bw, shapePhantom);
            bw.WriteSingle(keepDistance);
            bw.WriteSingle(contactAngleSensitivity);
            bw.WriteUInt32(userPlanes);
            bw.WriteSingle(maxCharacterSpeedForSolver);
            bw.WriteSingle(characterStrength);
            bw.WriteSingle(characterMass);
            bw.WriteSingle(maxSlope);
            bw.WriteSingle(penetrationRecoverySpeed);
            bw.WriteInt32(maxCastIterations);
            bw.WriteBoolean(refreshManifoldInCheckSupport);
            bw.Position += 11;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            position = xd.ReadVector4(xe, nameof(position));
            velocity = xd.ReadVector4(xe, nameof(velocity));
            dynamicFriction = xd.ReadSingle(xe, nameof(dynamicFriction));
            staticFriction = xd.ReadSingle(xe, nameof(staticFriction));
            keepContactTolerance = xd.ReadSingle(xe, nameof(keepContactTolerance));
            up = xd.ReadVector4(xe, nameof(up));
            extraUpStaticFriction = xd.ReadSingle(xe, nameof(extraUpStaticFriction));
            extraDownStaticFriction = xd.ReadSingle(xe, nameof(extraDownStaticFriction));
            shapePhantom = xd.ReadClassPointer<hkpShapePhantom>(this, xe, nameof(shapePhantom));
            keepDistance = xd.ReadSingle(xe, nameof(keepDistance));
            contactAngleSensitivity = xd.ReadSingle(xe, nameof(contactAngleSensitivity));
            userPlanes = xd.ReadUInt32(xe, nameof(userPlanes));
            maxCharacterSpeedForSolver = xd.ReadSingle(xe, nameof(maxCharacterSpeedForSolver));
            characterStrength = xd.ReadSingle(xe, nameof(characterStrength));
            characterMass = xd.ReadSingle(xe, nameof(characterMass));
            maxSlope = xd.ReadSingle(xe, nameof(maxSlope));
            penetrationRecoverySpeed = xd.ReadSingle(xe, nameof(penetrationRecoverySpeed));
            maxCastIterations = xd.ReadInt32(xe, nameof(maxCastIterations));
            refreshManifoldInCheckSupport = xd.ReadBoolean(xe, nameof(refreshManifoldInCheckSupport));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(position), position);
            xs.WriteVector4(xe, nameof(velocity), velocity);
            xs.WriteFloat(xe, nameof(dynamicFriction), dynamicFriction);
            xs.WriteFloat(xe, nameof(staticFriction), staticFriction);
            xs.WriteFloat(xe, nameof(keepContactTolerance), keepContactTolerance);
            xs.WriteVector4(xe, nameof(up), up);
            xs.WriteFloat(xe, nameof(extraUpStaticFriction), extraUpStaticFriction);
            xs.WriteFloat(xe, nameof(extraDownStaticFriction), extraDownStaticFriction);
            xs.WriteClassPointer(xe, nameof(shapePhantom), shapePhantom);
            xs.WriteFloat(xe, nameof(keepDistance), keepDistance);
            xs.WriteFloat(xe, nameof(contactAngleSensitivity), contactAngleSensitivity);
            xs.WriteNumber(xe, nameof(userPlanes), userPlanes);
            xs.WriteFloat(xe, nameof(maxCharacterSpeedForSolver), maxCharacterSpeedForSolver);
            xs.WriteFloat(xe, nameof(characterStrength), characterStrength);
            xs.WriteFloat(xe, nameof(characterMass), characterMass);
            xs.WriteFloat(xe, nameof(maxSlope), maxSlope);
            xs.WriteFloat(xe, nameof(penetrationRecoverySpeed), penetrationRecoverySpeed);
            xs.WriteNumber(xe, nameof(maxCastIterations), maxCastIterations);
            xs.WriteBoolean(xe, nameof(refreshManifoldInCheckSupport), refreshManifoldInCheckSupport);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCharacterProxyCinfo);
        }

        public bool Equals(hkpCharacterProxyCinfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   position.Equals(other.position) &&
                   velocity.Equals(other.velocity) &&
                   dynamicFriction.Equals(other.dynamicFriction) &&
                   staticFriction.Equals(other.staticFriction) &&
                   keepContactTolerance.Equals(other.keepContactTolerance) &&
                   up.Equals(other.up) &&
                   extraUpStaticFriction.Equals(other.extraUpStaticFriction) &&
                   extraDownStaticFriction.Equals(other.extraDownStaticFriction) &&
                   ((shapePhantom is null && other.shapePhantom is null) || (shapePhantom is not null && other.shapePhantom is not null && shapePhantom.Equals((IHavokObject)other.shapePhantom))) &&
                   keepDistance.Equals(other.keepDistance) &&
                   contactAngleSensitivity.Equals(other.contactAngleSensitivity) &&
                   userPlanes.Equals(other.userPlanes) &&
                   maxCharacterSpeedForSolver.Equals(other.maxCharacterSpeedForSolver) &&
                   characterStrength.Equals(other.characterStrength) &&
                   characterMass.Equals(other.characterMass) &&
                   maxSlope.Equals(other.maxSlope) &&
                   penetrationRecoverySpeed.Equals(other.penetrationRecoverySpeed) &&
                   maxCastIterations.Equals(other.maxCastIterations) &&
                   refreshManifoldInCheckSupport.Equals(other.refreshManifoldInCheckSupport) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(position);
            hashcode.Add(velocity);
            hashcode.Add(dynamicFriction);
            hashcode.Add(staticFriction);
            hashcode.Add(keepContactTolerance);
            hashcode.Add(up);
            hashcode.Add(extraUpStaticFriction);
            hashcode.Add(extraDownStaticFriction);
            hashcode.Add(shapePhantom);
            hashcode.Add(keepDistance);
            hashcode.Add(contactAngleSensitivity);
            hashcode.Add(userPlanes);
            hashcode.Add(maxCharacterSpeedForSolver);
            hashcode.Add(characterStrength);
            hashcode.Add(characterMass);
            hashcode.Add(maxSlope);
            hashcode.Add(penetrationRecoverySpeed);
            hashcode.Add(maxCastIterations);
            hashcode.Add(refreshManifoldInCheckSupport);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

