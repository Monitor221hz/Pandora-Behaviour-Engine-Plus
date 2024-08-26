using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxAnimatedFloat Signatire: 0xce8b2fbd size: 40 flags: FLAGS_NONE

    // floats class:  Type.TYPE_ARRAY Type.TYPE_REAL arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // hint class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: Hint
    public partial class hkxAnimatedFloat : hkReferencedObject, IEquatable<hkxAnimatedFloat?>
    {
        public IList<float> floats { set; get; } = Array.Empty<float>();
        public byte hint { set; get; }

        public override uint Signature { set; get; } = 0xce8b2fbd;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            floats = des.ReadSingleArray(br);
            hint = br.ReadByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteSingleArray(bw, floats);
            bw.WriteByte(hint);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            floats = xd.ReadSingleArray(xe, nameof(floats));
            hint = xd.ReadFlag<Hint, byte>(xe, nameof(hint));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloatArray(xe, nameof(floats), floats);
            xs.WriteEnum<Hint, byte>(xe, nameof(hint), hint);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxAnimatedFloat);
        }

        public bool Equals(hkxAnimatedFloat? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   floats.SequenceEqual(other.floats) &&
                   hint.Equals(other.hint) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(floats.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(hint);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

