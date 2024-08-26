using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpProjectileGun Signatire: 0xb4f30148 size: 104 flags: FLAGS_NONE

    // maxProjectiles class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // reloadTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // reload class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // projectiles class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 72 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // world class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 88 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // destructionWorld class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpProjectileGun : hkpFirstPersonGun, IEquatable<hkpProjectileGun?>
    {
        public int maxProjectiles { set; get; }
        public float reloadTime { set; get; }
        private float reload { set; get; }
        public IList<object> projectiles { set; get; } = Array.Empty<object>();
        private object? world { set; get; }
        private object? destructionWorld { set; get; }

        public override uint Signature { set; get; } = 0xb4f30148;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            maxProjectiles = br.ReadInt32();
            reloadTime = br.ReadSingle();
            reload = br.ReadSingle();
            br.Position += 4;
            des.ReadEmptyArray(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(maxProjectiles);
            bw.WriteSingle(reloadTime);
            bw.WriteSingle(reload);
            bw.Position += 4;
            s.WriteVoidArray(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            maxProjectiles = xd.ReadInt32(xe, nameof(maxProjectiles));
            reloadTime = xd.ReadSingle(xe, nameof(reloadTime));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(maxProjectiles), maxProjectiles);
            xs.WriteFloat(xe, nameof(reloadTime), reloadTime);
            xs.WriteSerializeIgnored(xe, nameof(reload));
            xs.WriteSerializeIgnored(xe, nameof(projectiles));
            xs.WriteSerializeIgnored(xe, nameof(world));
            xs.WriteSerializeIgnored(xe, nameof(destructionWorld));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpProjectileGun);
        }

        public bool Equals(hkpProjectileGun? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   maxProjectiles.Equals(other.maxProjectiles) &&
                   reloadTime.Equals(other.reloadTime) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(maxProjectiles);
            hashcode.Add(reloadTime);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

