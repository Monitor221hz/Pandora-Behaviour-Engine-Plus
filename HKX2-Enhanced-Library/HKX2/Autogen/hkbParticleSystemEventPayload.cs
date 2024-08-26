using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbParticleSystemEventPayload Signatire: 0x9df46cd6 size: 80 flags: FLAGS_NONE

    // type class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: SystemType
    // emitBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 18 flags: FLAGS_NONE enum: 
    // offset class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // direction class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // numParticles class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // speed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    public partial class hkbParticleSystemEventPayload : hkbEventPayload, IEquatable<hkbParticleSystemEventPayload?>
    {
        public byte type { set; get; }
        public short emitBoneIndex { set; get; }
        public Vector4 offset { set; get; }
        public Vector4 direction { set; get; }
        public int numParticles { set; get; }
        public float speed { set; get; }

        public override uint Signature { set; get; } = 0x9df46cd6;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            type = br.ReadByte();
            br.Position += 1;
            emitBoneIndex = br.ReadInt16();
            br.Position += 12;
            offset = br.ReadVector4();
            direction = br.ReadVector4();
            numParticles = br.ReadInt32();
            speed = br.ReadSingle();
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(type);
            bw.Position += 1;
            bw.WriteInt16(emitBoneIndex);
            bw.Position += 12;
            bw.WriteVector4(offset);
            bw.WriteVector4(direction);
            bw.WriteInt32(numParticles);
            bw.WriteSingle(speed);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            type = xd.ReadFlag<SystemType, byte>(xe, nameof(type));
            emitBoneIndex = xd.ReadInt16(xe, nameof(emitBoneIndex));
            offset = xd.ReadVector4(xe, nameof(offset));
            direction = xd.ReadVector4(xe, nameof(direction));
            numParticles = xd.ReadInt32(xe, nameof(numParticles));
            speed = xd.ReadSingle(xe, nameof(speed));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<SystemType, byte>(xe, nameof(type), type);
            xs.WriteNumber(xe, nameof(emitBoneIndex), emitBoneIndex);
            xs.WriteVector4(xe, nameof(offset), offset);
            xs.WriteVector4(xe, nameof(direction), direction);
            xs.WriteNumber(xe, nameof(numParticles), numParticles);
            xs.WriteFloat(xe, nameof(speed), speed);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbParticleSystemEventPayload);
        }

        public bool Equals(hkbParticleSystemEventPayload? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   type.Equals(other.type) &&
                   emitBoneIndex.Equals(other.emitBoneIndex) &&
                   offset.Equals(other.offset) &&
                   direction.Equals(other.direction) &&
                   numParticles.Equals(other.numParticles) &&
                   speed.Equals(other.speed) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(type);
            hashcode.Add(emitBoneIndex);
            hashcode.Add(offset);
            hashcode.Add(direction);
            hashcode.Add(numParticles);
            hashcode.Add(speed);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

