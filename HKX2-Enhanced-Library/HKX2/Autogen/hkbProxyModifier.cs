using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbProxyModifier Signatire: 0x8a41554f size: 288 flags: FLAGS_NONE

    // proxyInfo class: hkbProxyModifierProxyInfo Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // linearVelocity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // horizontalGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // verticalGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 180 flags: FLAGS_NONE enum: 
    // maxHorizontalSeparation class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // maxVerticalSeparation class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 188 flags: FLAGS_NONE enum: 
    // verticalDisplacementError class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 192 flags: FLAGS_NONE enum: 
    // verticalDisplacementErrorGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 196 flags: FLAGS_NONE enum: 
    // maxVerticalDisplacement class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 200 flags: FLAGS_NONE enum: 
    // minVerticalDisplacement class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 204 flags: FLAGS_NONE enum: 
    // capsuleHeight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 208 flags: FLAGS_NONE enum: 
    // capsuleRadius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 212 flags: FLAGS_NONE enum: 
    // maxSlopeForRotation class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 216 flags: FLAGS_NONE enum: 
    // collisionFilterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 220 flags: FLAGS_NONE enum: 
    // phantomType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 224 flags: FLAGS_NONE enum: PhantomType
    // linearVelocityMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 225 flags: FLAGS_NONE enum: LinearVelocityMode
    // ignoreIncomingRotation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 226 flags: FLAGS_NONE enum: 
    // ignoreCollisionDuringRotation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 227 flags: FLAGS_NONE enum: 
    // ignoreIncomingTranslation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 228 flags: FLAGS_NONE enum: 
    // includeDownwardMomentum class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 229 flags: FLAGS_NONE enum: 
    // followWorldFromModel class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 230 flags: FLAGS_NONE enum: 
    // isTouchingGround class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 231 flags: FLAGS_NONE enum: 
    // characterProxy class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 232 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // phantom class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 240 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // phantomShape class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 248 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // horizontalDisplacement class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 256 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // verticalDisplacement class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 272 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timestep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 276 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // previousFrameFollowWorldFromModel class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 280 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbProxyModifier : hkbModifier, IEquatable<hkbProxyModifier?>
    {
        public hkbProxyModifierProxyInfo proxyInfo { set; get; } = new();
        public Vector4 linearVelocity { set; get; }
        public float horizontalGain { set; get; }
        public float verticalGain { set; get; }
        public float maxHorizontalSeparation { set; get; }
        public float maxVerticalSeparation { set; get; }
        public float verticalDisplacementError { set; get; }
        public float verticalDisplacementErrorGain { set; get; }
        public float maxVerticalDisplacement { set; get; }
        public float minVerticalDisplacement { set; get; }
        public float capsuleHeight { set; get; }
        public float capsuleRadius { set; get; }
        public float maxSlopeForRotation { set; get; }
        public uint collisionFilterInfo { set; get; }
        public sbyte phantomType { set; get; }
        public sbyte linearVelocityMode { set; get; }
        public bool ignoreIncomingRotation { set; get; }
        public bool ignoreCollisionDuringRotation { set; get; }
        public bool ignoreIncomingTranslation { set; get; }
        public bool includeDownwardMomentum { set; get; }
        public bool followWorldFromModel { set; get; }
        public bool isTouchingGround { set; get; }
        private object? characterProxy { set; get; }
        private object? phantom { set; get; }
        private object? phantomShape { set; get; }
        private Vector4 horizontalDisplacement { set; get; }
        private float verticalDisplacement { set; get; }
        private float timestep { set; get; }
        private bool previousFrameFollowWorldFromModel { set; get; }

        public override uint Signature { set; get; } = 0x8a41554f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            proxyInfo.Read(des, br);
            linearVelocity = br.ReadVector4();
            horizontalGain = br.ReadSingle();
            verticalGain = br.ReadSingle();
            maxHorizontalSeparation = br.ReadSingle();
            maxVerticalSeparation = br.ReadSingle();
            verticalDisplacementError = br.ReadSingle();
            verticalDisplacementErrorGain = br.ReadSingle();
            maxVerticalDisplacement = br.ReadSingle();
            minVerticalDisplacement = br.ReadSingle();
            capsuleHeight = br.ReadSingle();
            capsuleRadius = br.ReadSingle();
            maxSlopeForRotation = br.ReadSingle();
            collisionFilterInfo = br.ReadUInt32();
            phantomType = br.ReadSByte();
            linearVelocityMode = br.ReadSByte();
            ignoreIncomingRotation = br.ReadBoolean();
            ignoreCollisionDuringRotation = br.ReadBoolean();
            ignoreIncomingTranslation = br.ReadBoolean();
            includeDownwardMomentum = br.ReadBoolean();
            followWorldFromModel = br.ReadBoolean();
            isTouchingGround = br.ReadBoolean();
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            horizontalDisplacement = br.ReadVector4();
            verticalDisplacement = br.ReadSingle();
            timestep = br.ReadSingle();
            previousFrameFollowWorldFromModel = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            proxyInfo.Write(s, bw);
            bw.WriteVector4(linearVelocity);
            bw.WriteSingle(horizontalGain);
            bw.WriteSingle(verticalGain);
            bw.WriteSingle(maxHorizontalSeparation);
            bw.WriteSingle(maxVerticalSeparation);
            bw.WriteSingle(verticalDisplacementError);
            bw.WriteSingle(verticalDisplacementErrorGain);
            bw.WriteSingle(maxVerticalDisplacement);
            bw.WriteSingle(minVerticalDisplacement);
            bw.WriteSingle(capsuleHeight);
            bw.WriteSingle(capsuleRadius);
            bw.WriteSingle(maxSlopeForRotation);
            bw.WriteUInt32(collisionFilterInfo);
            bw.WriteSByte(phantomType);
            bw.WriteSByte(linearVelocityMode);
            bw.WriteBoolean(ignoreIncomingRotation);
            bw.WriteBoolean(ignoreCollisionDuringRotation);
            bw.WriteBoolean(ignoreIncomingTranslation);
            bw.WriteBoolean(includeDownwardMomentum);
            bw.WriteBoolean(followWorldFromModel);
            bw.WriteBoolean(isTouchingGround);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.WriteVector4(horizontalDisplacement);
            bw.WriteSingle(verticalDisplacement);
            bw.WriteSingle(timestep);
            bw.WriteBoolean(previousFrameFollowWorldFromModel);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            proxyInfo = xd.ReadClass<hkbProxyModifierProxyInfo>(xe, nameof(proxyInfo));
            linearVelocity = xd.ReadVector4(xe, nameof(linearVelocity));
            horizontalGain = xd.ReadSingle(xe, nameof(horizontalGain));
            verticalGain = xd.ReadSingle(xe, nameof(verticalGain));
            maxHorizontalSeparation = xd.ReadSingle(xe, nameof(maxHorizontalSeparation));
            maxVerticalSeparation = xd.ReadSingle(xe, nameof(maxVerticalSeparation));
            verticalDisplacementError = xd.ReadSingle(xe, nameof(verticalDisplacementError));
            verticalDisplacementErrorGain = xd.ReadSingle(xe, nameof(verticalDisplacementErrorGain));
            maxVerticalDisplacement = xd.ReadSingle(xe, nameof(maxVerticalDisplacement));
            minVerticalDisplacement = xd.ReadSingle(xe, nameof(minVerticalDisplacement));
            capsuleHeight = xd.ReadSingle(xe, nameof(capsuleHeight));
            capsuleRadius = xd.ReadSingle(xe, nameof(capsuleRadius));
            maxSlopeForRotation = xd.ReadSingle(xe, nameof(maxSlopeForRotation));
            collisionFilterInfo = xd.ReadUInt32(xe, nameof(collisionFilterInfo));
            phantomType = xd.ReadFlag<PhantomType, sbyte>(xe, nameof(phantomType));
            linearVelocityMode = xd.ReadFlag<LinearVelocityMode, sbyte>(xe, nameof(linearVelocityMode));
            ignoreIncomingRotation = xd.ReadBoolean(xe, nameof(ignoreIncomingRotation));
            ignoreCollisionDuringRotation = xd.ReadBoolean(xe, nameof(ignoreCollisionDuringRotation));
            ignoreIncomingTranslation = xd.ReadBoolean(xe, nameof(ignoreIncomingTranslation));
            includeDownwardMomentum = xd.ReadBoolean(xe, nameof(includeDownwardMomentum));
            followWorldFromModel = xd.ReadBoolean(xe, nameof(followWorldFromModel));
            isTouchingGround = xd.ReadBoolean(xe, nameof(isTouchingGround));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbProxyModifierProxyInfo>(xe, nameof(proxyInfo), proxyInfo);
            xs.WriteVector4(xe, nameof(linearVelocity), linearVelocity);
            xs.WriteFloat(xe, nameof(horizontalGain), horizontalGain);
            xs.WriteFloat(xe, nameof(verticalGain), verticalGain);
            xs.WriteFloat(xe, nameof(maxHorizontalSeparation), maxHorizontalSeparation);
            xs.WriteFloat(xe, nameof(maxVerticalSeparation), maxVerticalSeparation);
            xs.WriteFloat(xe, nameof(verticalDisplacementError), verticalDisplacementError);
            xs.WriteFloat(xe, nameof(verticalDisplacementErrorGain), verticalDisplacementErrorGain);
            xs.WriteFloat(xe, nameof(maxVerticalDisplacement), maxVerticalDisplacement);
            xs.WriteFloat(xe, nameof(minVerticalDisplacement), minVerticalDisplacement);
            xs.WriteFloat(xe, nameof(capsuleHeight), capsuleHeight);
            xs.WriteFloat(xe, nameof(capsuleRadius), capsuleRadius);
            xs.WriteFloat(xe, nameof(maxSlopeForRotation), maxSlopeForRotation);
            xs.WriteNumber(xe, nameof(collisionFilterInfo), collisionFilterInfo);
            xs.WriteEnum<PhantomType, sbyte>(xe, nameof(phantomType), phantomType);
            xs.WriteEnum<LinearVelocityMode, sbyte>(xe, nameof(linearVelocityMode), linearVelocityMode);
            xs.WriteBoolean(xe, nameof(ignoreIncomingRotation), ignoreIncomingRotation);
            xs.WriteBoolean(xe, nameof(ignoreCollisionDuringRotation), ignoreCollisionDuringRotation);
            xs.WriteBoolean(xe, nameof(ignoreIncomingTranslation), ignoreIncomingTranslation);
            xs.WriteBoolean(xe, nameof(includeDownwardMomentum), includeDownwardMomentum);
            xs.WriteBoolean(xe, nameof(followWorldFromModel), followWorldFromModel);
            xs.WriteBoolean(xe, nameof(isTouchingGround), isTouchingGround);
            xs.WriteSerializeIgnored(xe, nameof(characterProxy));
            xs.WriteSerializeIgnored(xe, nameof(phantom));
            xs.WriteSerializeIgnored(xe, nameof(phantomShape));
            xs.WriteSerializeIgnored(xe, nameof(horizontalDisplacement));
            xs.WriteSerializeIgnored(xe, nameof(verticalDisplacement));
            xs.WriteSerializeIgnored(xe, nameof(timestep));
            xs.WriteSerializeIgnored(xe, nameof(previousFrameFollowWorldFromModel));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbProxyModifier);
        }

        public bool Equals(hkbProxyModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((proxyInfo is null && other.proxyInfo is null) || (proxyInfo is not null && other.proxyInfo is not null && proxyInfo.Equals((IHavokObject)other.proxyInfo))) &&
                   linearVelocity.Equals(other.linearVelocity) &&
                   horizontalGain.Equals(other.horizontalGain) &&
                   verticalGain.Equals(other.verticalGain) &&
                   maxHorizontalSeparation.Equals(other.maxHorizontalSeparation) &&
                   maxVerticalSeparation.Equals(other.maxVerticalSeparation) &&
                   verticalDisplacementError.Equals(other.verticalDisplacementError) &&
                   verticalDisplacementErrorGain.Equals(other.verticalDisplacementErrorGain) &&
                   maxVerticalDisplacement.Equals(other.maxVerticalDisplacement) &&
                   minVerticalDisplacement.Equals(other.minVerticalDisplacement) &&
                   capsuleHeight.Equals(other.capsuleHeight) &&
                   capsuleRadius.Equals(other.capsuleRadius) &&
                   maxSlopeForRotation.Equals(other.maxSlopeForRotation) &&
                   collisionFilterInfo.Equals(other.collisionFilterInfo) &&
                   phantomType.Equals(other.phantomType) &&
                   linearVelocityMode.Equals(other.linearVelocityMode) &&
                   ignoreIncomingRotation.Equals(other.ignoreIncomingRotation) &&
                   ignoreCollisionDuringRotation.Equals(other.ignoreCollisionDuringRotation) &&
                   ignoreIncomingTranslation.Equals(other.ignoreIncomingTranslation) &&
                   includeDownwardMomentum.Equals(other.includeDownwardMomentum) &&
                   followWorldFromModel.Equals(other.followWorldFromModel) &&
                   isTouchingGround.Equals(other.isTouchingGround) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(proxyInfo);
            hashcode.Add(linearVelocity);
            hashcode.Add(horizontalGain);
            hashcode.Add(verticalGain);
            hashcode.Add(maxHorizontalSeparation);
            hashcode.Add(maxVerticalSeparation);
            hashcode.Add(verticalDisplacementError);
            hashcode.Add(verticalDisplacementErrorGain);
            hashcode.Add(maxVerticalDisplacement);
            hashcode.Add(minVerticalDisplacement);
            hashcode.Add(capsuleHeight);
            hashcode.Add(capsuleRadius);
            hashcode.Add(maxSlopeForRotation);
            hashcode.Add(collisionFilterInfo);
            hashcode.Add(phantomType);
            hashcode.Add(linearVelocityMode);
            hashcode.Add(ignoreIncomingRotation);
            hashcode.Add(ignoreCollisionDuringRotation);
            hashcode.Add(ignoreIncomingTranslation);
            hashcode.Add(includeDownwardMomentum);
            hashcode.Add(followWorldFromModel);
            hashcode.Add(isTouchingGround);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

