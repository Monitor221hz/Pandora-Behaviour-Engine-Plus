using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxMaterialEffect Signatire: 0x1d39f925 size: 48 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: EffectType
    // data class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkxMaterialEffect : hkReferencedObject, IEquatable<hkxMaterialEffect?>
    {
        public string name { set; get; } = "";
        public byte type { set; get; }
        public IList<byte> data { set; get; } = Array.Empty<byte>();

        public override uint Signature { set; get; } = 0x1d39f925;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            name = des.ReadStringPointer(br);
            type = br.ReadByte();
            br.Position += 7;
            data = des.ReadByteArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, name);
            bw.WriteByte(type);
            bw.Position += 7;
            s.WriteByteArray(bw, data);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
            type = xd.ReadFlag<EffectType, byte>(xe, nameof(type));
            data = xd.ReadByteArray(xe, nameof(data));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteEnum<EffectType, byte>(xe, nameof(type), type);
            xs.WriteNumberArray(xe, nameof(data), data);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxMaterialEffect);
        }

        public bool Equals(hkxMaterialEffect? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   type.Equals(other.type) &&
                   data.SequenceEqual(other.data) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name);
            hashcode.Add(type);
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

