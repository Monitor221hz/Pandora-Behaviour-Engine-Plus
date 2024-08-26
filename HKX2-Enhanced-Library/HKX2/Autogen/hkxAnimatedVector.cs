using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxAnimatedVector Signatire: 0x34b1a197 size: 40 flags: FLAGS_NONE

    // vectors class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // hint class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: Hint
    public partial class hkxAnimatedVector : hkReferencedObject, IEquatable<hkxAnimatedVector?>
    {
        public IList<Vector4> vectors { set; get; } = Array.Empty<Vector4>();
        public byte hint { set; get; }

        public override uint Signature { set; get; } = 0x34b1a197;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            vectors = des.ReadVector4Array(br);
            hint = br.ReadByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVector4Array(bw, vectors);
            bw.WriteByte(hint);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            vectors = xd.ReadVector4Array(xe, nameof(vectors));
            hint = xd.ReadFlag<Hint, byte>(xe, nameof(hint));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4Array(xe, nameof(vectors), vectors);
            xs.WriteEnum<Hint, byte>(xe, nameof(hint), hint);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxAnimatedVector);
        }

        public bool Equals(hkxAnimatedVector? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   vectors.SequenceEqual(other.vectors) &&
                   hint.Equals(other.hint) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(vectors.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(hint);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

