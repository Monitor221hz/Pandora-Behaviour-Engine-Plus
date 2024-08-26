using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxSparselyAnimatedEnum Signatire: 0x68a47b64 size: 56 flags: FLAGS_NONE

    // @enum class: hkxEnum Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkxSparselyAnimatedEnum : hkxSparselyAnimatedInt, IEquatable<hkxSparselyAnimatedEnum?>
    {
        public hkxEnum? @enum { set; get; }

        public override uint Signature { set; get; } = 0x68a47b64;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            @enum = des.ReadClassPointer<hkxEnum>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, @enum);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            @enum = xd.ReadClassPointer<hkxEnum>(this, xe, nameof(@enum));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(@enum), @enum);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxSparselyAnimatedEnum);
        }

        public bool Equals(hkxSparselyAnimatedEnum? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((@enum is null && other.@enum is null) || (@enum is not null && other.@enum is not null && @enum.Equals((IHavokObject)other.@enum))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(@enum);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

