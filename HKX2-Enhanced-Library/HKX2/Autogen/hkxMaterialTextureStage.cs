using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxMaterialTextureStage Signatire: 0xfa6facb2 size: 16 flags: FLAGS_NONE

    // texture class: hkReferencedObject Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // usageHint class:  Type.TYPE_ENUM Type.TYPE_INT32 arrSize: 0 offset: 8 flags: FLAGS_NONE enum: TextureType
    // tcoordChannel class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkxMaterialTextureStage : IHavokObject, IEquatable<hkxMaterialTextureStage?>
    {
        public hkReferencedObject? texture { set; get; }
        public int usageHint { set; get; }
        public int tcoordChannel { set; get; }

        public virtual uint Signature { set; get; } = 0xfa6facb2;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            texture = des.ReadClassPointer<hkReferencedObject>(br);
            usageHint = br.ReadInt32();
            tcoordChannel = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer(bw, texture);
            bw.WriteInt32(usageHint);
            bw.WriteInt32(tcoordChannel);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            texture = xd.ReadClassPointer<hkReferencedObject>(this, xe, nameof(texture));
            usageHint = xd.ReadFlag<TextureType, int>(xe, nameof(usageHint));
            tcoordChannel = xd.ReadInt32(xe, nameof(tcoordChannel));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointer(xe, nameof(texture), texture);
            xs.WriteEnum<TextureType, int>(xe, nameof(usageHint), usageHint);
            xs.WriteNumber(xe, nameof(tcoordChannel), tcoordChannel);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxMaterialTextureStage);
        }

        public bool Equals(hkxMaterialTextureStage? other)
        {
            return other is not null &&
                   ((texture is null && other.texture is null) || (texture is not null && other.texture is not null && texture.Equals((IHavokObject)other.texture))) &&
                   usageHint.Equals(other.usageHint) &&
                   tcoordChannel.Equals(other.tcoordChannel) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(texture);
            hashcode.Add(usageHint);
            hashcode.Add(tcoordChannel);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

