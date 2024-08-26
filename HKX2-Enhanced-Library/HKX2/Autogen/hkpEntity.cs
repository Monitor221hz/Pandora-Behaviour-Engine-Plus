using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpEntity Signatire: 0xa03c774b size: 720 flags: FLAGS_NONE

    // material class: hkpMaterial Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 208 flags: FLAGS_NONE enum: 
    // limitContactImpulseUtilAndFlag class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // damageMultiplier class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 232 flags: FLAGS_NONE enum: 
    // breakableBody class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 240 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // solverData class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 248 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // storageIndex class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 252 flags: FLAGS_NONE enum: 
    // contactPointCallbackDelay class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 254 flags: FLAGS_NONE enum: 
    // constraintsMaster class: hkpEntitySmallArraySerializeOverrideType Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 256 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // constraintsSlave class: hkpConstraintInstance Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 272 flags: SERIALIZE_IGNORED|NOT_OWNED|FLAGS_NONE enum: 
    // constraintRuntime class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 288 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // simulationIsland class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 304 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // autoRemoveLevel class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 312 flags: FLAGS_NONE enum: 
    // numShapeKeysInContactPointProperties class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 313 flags: FLAGS_NONE enum: 
    // responseModifierFlags class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 314 flags: FLAGS_NONE enum: 
    // uid class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 316 flags: FLAGS_NONE enum: 
    // spuCollisionCallback class: hkpEntitySpuCollisionCallback Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 320 flags: FLAGS_NONE enum: 
    // motion class: hkpMaxSizeMotion Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 336 flags: FLAGS_NONE enum: 
    // contactListeners class: hkpEntitySmallArraySerializeOverrideType Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 656 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // actions class: hkpEntitySmallArraySerializeOverrideType Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 672 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // localFrame class: hkLocalFrame Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 688 flags: FLAGS_NONE enum: 
    // extendedListeners class: hkpEntityExtendedListeners Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 696 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // npData class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 704 flags: FLAGS_NONE enum: 
    public partial class hkpEntity : hkpWorldObject, IEquatable<hkpEntity?>
    {
        public hkpMaterial material { set; get; } = new();
        private object? limitContactImpulseUtilAndFlag { set; get; }
        public float damageMultiplier { set; get; }
        private object? breakableBody { set; get; }
        private uint solverData { set; get; }
        public ushort storageIndex { set; get; }
        public ushort contactPointCallbackDelay { set; get; }
        public hkpEntitySmallArraySerializeOverrideType constraintsMaster { set; get; } = new();
        public IList<hkpConstraintInstance> constraintsSlave { set; get; } = new List<hkpConstraintInstance>();
        public IList<byte> constraintRuntime { set; get; } = Array.Empty<byte>();
        private object? simulationIsland { set; get; }
        public sbyte autoRemoveLevel { set; get; }
        public byte numShapeKeysInContactPointProperties { set; get; }
        public byte responseModifierFlags { set; get; }
        public uint uid { set; get; }
        public hkpEntitySpuCollisionCallback spuCollisionCallback { set; get; } = new();
        public hkpMaxSizeMotion motion { set; get; } = new();
        public hkpEntitySmallArraySerializeOverrideType contactListeners { set; get; } = new();
        public hkpEntitySmallArraySerializeOverrideType actions { set; get; } = new();
        public hkLocalFrame? localFrame { set; get; }
        private hkpEntityExtendedListeners? extendedListeners { set; get; }
        public uint npData { set; get; }

        public override uint Signature { set; get; } = 0xa03c774b;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            material.Read(des, br);
            br.Position += 4;
            des.ReadEmptyPointer(br);
            damageMultiplier = br.ReadSingle();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            solverData = br.ReadUInt32();
            storageIndex = br.ReadUInt16();
            contactPointCallbackDelay = br.ReadUInt16();
            constraintsMaster.Read(des, br);
            des.ReadEmptyArray(br);
            constraintRuntime = des.ReadByteArray(br);
            des.ReadEmptyPointer(br);
            autoRemoveLevel = br.ReadSByte();
            numShapeKeysInContactPointProperties = br.ReadByte();
            responseModifierFlags = br.ReadByte();
            br.Position += 1;
            uid = br.ReadUInt32();
            spuCollisionCallback.Read(des, br);
            motion.Read(des, br);
            contactListeners.Read(des, br);
            actions.Read(des, br);
            localFrame = des.ReadClassPointer<hkLocalFrame>(br);
            extendedListeners = des.ReadClassPointer<hkpEntityExtendedListeners>(br);
            npData = br.ReadUInt32();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            material.Write(s, bw);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            bw.WriteSingle(damageMultiplier);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            bw.WriteUInt32(solverData);
            bw.WriteUInt16(storageIndex);
            bw.WriteUInt16(contactPointCallbackDelay);
            constraintsMaster.Write(s, bw);
            s.WriteVoidArray(bw);
            s.WriteByteArray(bw, constraintRuntime);
            s.WriteVoidPointer(bw);
            bw.WriteSByte(autoRemoveLevel);
            bw.WriteByte(numShapeKeysInContactPointProperties);
            bw.WriteByte(responseModifierFlags);
            bw.Position += 1;
            bw.WriteUInt32(uid);
            spuCollisionCallback.Write(s, bw);
            motion.Write(s, bw);
            contactListeners.Write(s, bw);
            actions.Write(s, bw);
            s.WriteClassPointer(bw, localFrame);
            s.WriteClassPointer(bw, extendedListeners);
            bw.WriteUInt32(npData);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            material = xd.ReadClass<hkpMaterial>(xe, nameof(material));
            damageMultiplier = xd.ReadSingle(xe, nameof(damageMultiplier));
            storageIndex = xd.ReadUInt16(xe, nameof(storageIndex));
            contactPointCallbackDelay = xd.ReadUInt16(xe, nameof(contactPointCallbackDelay));
            autoRemoveLevel = xd.ReadSByte(xe, nameof(autoRemoveLevel));
            numShapeKeysInContactPointProperties = xd.ReadByte(xe, nameof(numShapeKeysInContactPointProperties));
            responseModifierFlags = xd.ReadByte(xe, nameof(responseModifierFlags));
            uid = xd.ReadUInt32(xe, nameof(uid));
            spuCollisionCallback = xd.ReadClass<hkpEntitySpuCollisionCallback>(xe, nameof(spuCollisionCallback));
            motion = xd.ReadClass<hkpMaxSizeMotion>(xe, nameof(motion));
            localFrame = xd.ReadClassPointer<hkLocalFrame>(this, xe, nameof(localFrame));
            npData = xd.ReadUInt32(xe, nameof(npData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpMaterial>(xe, nameof(material), material);
            xs.WriteSerializeIgnored(xe, nameof(limitContactImpulseUtilAndFlag));
            xs.WriteFloat(xe, nameof(damageMultiplier), damageMultiplier);
            xs.WriteSerializeIgnored(xe, nameof(breakableBody));
            xs.WriteSerializeIgnored(xe, nameof(solverData));
            xs.WriteNumber(xe, nameof(storageIndex), storageIndex);
            xs.WriteNumber(xe, nameof(contactPointCallbackDelay), contactPointCallbackDelay);
            xs.WriteSerializeIgnored(xe, nameof(constraintsMaster));
            xs.WriteSerializeIgnored(xe, nameof(constraintsSlave));
            xs.WriteSerializeIgnored(xe, nameof(constraintRuntime));
            xs.WriteSerializeIgnored(xe, nameof(simulationIsland));
            xs.WriteNumber(xe, nameof(autoRemoveLevel), autoRemoveLevel);
            xs.WriteNumber(xe, nameof(numShapeKeysInContactPointProperties), numShapeKeysInContactPointProperties);
            xs.WriteNumber(xe, nameof(responseModifierFlags), responseModifierFlags);
            xs.WriteNumber(xe, nameof(uid), uid);
            xs.WriteClass<hkpEntitySpuCollisionCallback>(xe, nameof(spuCollisionCallback), spuCollisionCallback);
            xs.WriteClass<hkpMaxSizeMotion>(xe, nameof(motion), motion);
            xs.WriteSerializeIgnored(xe, nameof(contactListeners));
            xs.WriteSerializeIgnored(xe, nameof(actions));
            xs.WriteClassPointer(xe, nameof(localFrame), localFrame);
            xs.WriteSerializeIgnored(xe, nameof(extendedListeners));
            xs.WriteNumber(xe, nameof(npData), npData);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpEntity);
        }

        public bool Equals(hkpEntity? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((material is null && other.material is null) || (material is not null && other.material is not null && material.Equals((IHavokObject)other.material))) &&
                   damageMultiplier.Equals(other.damageMultiplier) &&
                   storageIndex.Equals(other.storageIndex) &&
                   contactPointCallbackDelay.Equals(other.contactPointCallbackDelay) &&
                   autoRemoveLevel.Equals(other.autoRemoveLevel) &&
                   numShapeKeysInContactPointProperties.Equals(other.numShapeKeysInContactPointProperties) &&
                   responseModifierFlags.Equals(other.responseModifierFlags) &&
                   uid.Equals(other.uid) &&
                   ((spuCollisionCallback is null && other.spuCollisionCallback is null) || (spuCollisionCallback is not null && other.spuCollisionCallback is not null && spuCollisionCallback.Equals((IHavokObject)other.spuCollisionCallback))) &&
                   ((motion is null && other.motion is null) || (motion is not null && other.motion is not null && motion.Equals((IHavokObject)other.motion))) &&
                   ((localFrame is null && other.localFrame is null) || (localFrame is not null && other.localFrame is not null && localFrame.Equals((IHavokObject)other.localFrame))) &&
                   npData.Equals(other.npData) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(material);
            hashcode.Add(damageMultiplier);
            hashcode.Add(storageIndex);
            hashcode.Add(contactPointCallbackDelay);
            hashcode.Add(autoRemoveLevel);
            hashcode.Add(numShapeKeysInContactPointProperties);
            hashcode.Add(responseModifierFlags);
            hashcode.Add(uid);
            hashcode.Add(spuCollisionCallback);
            hashcode.Add(motion);
            hashcode.Add(localFrame);
            hashcode.Add(npData);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

