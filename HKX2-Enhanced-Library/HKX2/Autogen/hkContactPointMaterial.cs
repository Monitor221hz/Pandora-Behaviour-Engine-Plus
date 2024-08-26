using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkContactPointMaterial Signatire: 0x4e32287c size: 16 flags: FLAGS_NONE

    // userData class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // friction class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // restitution class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 9 flags: FLAGS_NONE enum: 
    // maxImpulse class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 10 flags: FLAGS_NONE enum: 
    // flags class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 11 flags: FLAGS_NONE enum: 
    public partial class hkContactPointMaterial : IHavokObject, IEquatable<hkContactPointMaterial?>
    {
        public ulong userData { set; get; }
        public byte friction { set; get; }
        public byte restitution { set; get; }
        public byte maxImpulse { set; get; }
        public byte flags { set; get; }

        public virtual uint Signature { set; get; } = 0x4e32287c;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            userData = br.ReadUInt64();
            friction = br.ReadByte();
            restitution = br.ReadByte();
            maxImpulse = br.ReadByte();
            flags = br.ReadByte();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt64(userData);
            bw.WriteByte(friction);
            bw.WriteByte(restitution);
            bw.WriteByte(maxImpulse);
            bw.WriteByte(flags);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            userData = xd.ReadUInt64(xe, nameof(userData));
            friction = xd.ReadByte(xe, nameof(friction));
            restitution = xd.ReadByte(xe, nameof(restitution));
            maxImpulse = xd.ReadByte(xe, nameof(maxImpulse));
            flags = xd.ReadByte(xe, nameof(flags));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(userData), userData);
            xs.WriteNumber(xe, nameof(friction), friction);
            xs.WriteNumber(xe, nameof(restitution), restitution);
            xs.WriteNumber(xe, nameof(maxImpulse), maxImpulse);
            xs.WriteNumber(xe, nameof(flags), flags);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkContactPointMaterial);
        }

        public bool Equals(hkContactPointMaterial? other)
        {
            return other is not null &&
                   userData.Equals(other.userData) &&
                   friction.Equals(other.friction) &&
                   restitution.Equals(other.restitution) &&
                   maxImpulse.Equals(other.maxImpulse) &&
                   flags.Equals(other.flags) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(userData);
            hashcode.Add(friction);
            hashcode.Add(restitution);
            hashcode.Add(maxImpulse);
            hashcode.Add(flags);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

