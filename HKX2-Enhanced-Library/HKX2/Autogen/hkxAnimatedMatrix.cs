using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxAnimatedMatrix Signatire: 0x5838e337 size: 40 flags: FLAGS_NONE

    // matrices class:  Type.TYPE_ARRAY Type.TYPE_MATRIX4 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // hint class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: Hint
    public partial class hkxAnimatedMatrix : hkReferencedObject, IEquatable<hkxAnimatedMatrix?>
    {
        public IList<Matrix4x4> matrices { set; get; } = Array.Empty<Matrix4x4>();
        public byte hint { set; get; }

        public override uint Signature { set; get; } = 0x5838e337;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            matrices = des.ReadMatrix4Array(br);
            hint = br.ReadByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteMatrix4Array(bw, matrices);
            bw.WriteByte(hint);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            matrices = xd.ReadMatrix4Array(xe, nameof(matrices));
            hint = xd.ReadFlag<Hint, byte>(xe, nameof(hint));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteMatrix4Array(xe, nameof(matrices), matrices);
            xs.WriteEnum<Hint, byte>(xe, nameof(hint), hint);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxAnimatedMatrix);
        }

        public bool Equals(hkxAnimatedMatrix? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   matrices.SequenceEqual(other.matrices) &&
                   hint.Equals(other.hint) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(matrices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(hint);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

