using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBallGun Signatire: 0x57b06d35 size: 112 flags: FLAGS_NONE

    // bulletRadius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // bulletVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // bulletMass class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // damageMultiplier class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // maxBulletsInWorld class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // bulletOffsetFromCenter class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // addedBodies class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpBallGun : hkpFirstPersonGun, IEquatable<hkpBallGun?>
    {
        public float bulletRadius { set; get; }
        public float bulletVelocity { set; get; }
        public float bulletMass { set; get; }
        public float damageMultiplier { set; get; }
        public int maxBulletsInWorld { set; get; }
        public Vector4 bulletOffsetFromCenter { set; get; }
        private object? addedBodies { set; get; }

        public override uint Signature { set; get; } = 0x57b06d35;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            bulletRadius = br.ReadSingle();
            bulletVelocity = br.ReadSingle();
            bulletMass = br.ReadSingle();
            damageMultiplier = br.ReadSingle();
            maxBulletsInWorld = br.ReadInt32();
            br.Position += 4;
            bulletOffsetFromCenter = br.ReadVector4();
            des.ReadEmptyPointer(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(bulletRadius);
            bw.WriteSingle(bulletVelocity);
            bw.WriteSingle(bulletMass);
            bw.WriteSingle(damageMultiplier);
            bw.WriteInt32(maxBulletsInWorld);
            bw.Position += 4;
            bw.WriteVector4(bulletOffsetFromCenter);
            s.WriteVoidPointer(bw);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            bulletRadius = xd.ReadSingle(xe, nameof(bulletRadius));
            bulletVelocity = xd.ReadSingle(xe, nameof(bulletVelocity));
            bulletMass = xd.ReadSingle(xe, nameof(bulletMass));
            damageMultiplier = xd.ReadSingle(xe, nameof(damageMultiplier));
            maxBulletsInWorld = xd.ReadInt32(xe, nameof(maxBulletsInWorld));
            bulletOffsetFromCenter = xd.ReadVector4(xe, nameof(bulletOffsetFromCenter));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(bulletRadius), bulletRadius);
            xs.WriteFloat(xe, nameof(bulletVelocity), bulletVelocity);
            xs.WriteFloat(xe, nameof(bulletMass), bulletMass);
            xs.WriteFloat(xe, nameof(damageMultiplier), damageMultiplier);
            xs.WriteNumber(xe, nameof(maxBulletsInWorld), maxBulletsInWorld);
            xs.WriteVector4(xe, nameof(bulletOffsetFromCenter), bulletOffsetFromCenter);
            xs.WriteSerializeIgnored(xe, nameof(addedBodies));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBallGun);
        }

        public bool Equals(hkpBallGun? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   bulletRadius.Equals(other.bulletRadius) &&
                   bulletVelocity.Equals(other.bulletVelocity) &&
                   bulletMass.Equals(other.bulletMass) &&
                   damageMultiplier.Equals(other.damageMultiplier) &&
                   maxBulletsInWorld.Equals(other.maxBulletsInWorld) &&
                   bulletOffsetFromCenter.Equals(other.bulletOffsetFromCenter) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(bulletRadius);
            hashcode.Add(bulletVelocity);
            hashcode.Add(bulletMass);
            hashcode.Add(damageMultiplier);
            hashcode.Add(maxBulletsInWorld);
            hashcode.Add(bulletOffsetFromCenter);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

