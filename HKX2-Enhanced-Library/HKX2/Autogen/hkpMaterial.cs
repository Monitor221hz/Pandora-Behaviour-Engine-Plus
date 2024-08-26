using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMaterial Signatire: 0x33be6570 size: 12 flags: FLAGS_NONE

    // responseType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: ResponseType
    // rollingFrictionMultiplier class:  Type.TYPE_HALF Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // friction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // restitution class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpMaterial : IHavokObject, IEquatable<hkpMaterial?>
    {
        public sbyte responseType { set; get; }
        public Half rollingFrictionMultiplier { set; get; }
        public float friction { set; get; }
        public float restitution { set; get; }

        public virtual uint Signature { set; get; } = 0x33be6570;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            responseType = br.ReadSByte();
            br.Position += 1;
            rollingFrictionMultiplier = br.ReadHalf();
            friction = br.ReadSingle();
            restitution = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSByte(responseType);
            bw.Position += 1;
            bw.WriteHalf(rollingFrictionMultiplier);
            bw.WriteSingle(friction);
            bw.WriteSingle(restitution);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            responseType = xd.ReadFlag<ResponseType, sbyte>(xe, nameof(responseType));
            rollingFrictionMultiplier = xd.ReadHalf(xe, nameof(rollingFrictionMultiplier));
            friction = xd.ReadSingle(xe, nameof(friction));
            restitution = xd.ReadSingle(xe, nameof(restitution));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteEnum<ResponseType, sbyte>(xe, nameof(responseType), responseType);
            xs.WriteFloat(xe, nameof(rollingFrictionMultiplier), rollingFrictionMultiplier);
            xs.WriteFloat(xe, nameof(friction), friction);
            xs.WriteFloat(xe, nameof(restitution), restitution);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMaterial);
        }

        public bool Equals(hkpMaterial? other)
        {
            return other is not null &&
                   responseType.Equals(other.responseType) &&
                   rollingFrictionMultiplier.Equals(other.rollingFrictionMultiplier) &&
                   friction.Equals(other.friction) &&
                   restitution.Equals(other.restitution) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(responseType);
            hashcode.Add(rollingFrictionMultiplier);
            hashcode.Add(friction);
            hashcode.Add(restitution);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

