using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkBitField Signatire: 0xda41bd9b size: 24 flags: FLAGS_NONE

    // words class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // numBits class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkBitField : IHavokObject, IEquatable<hkBitField?>
    {
        public IList<uint> words { set; get; } = Array.Empty<uint>();
        public int numBits { set; get; }

        public virtual uint Signature { set; get; } = 0xda41bd9b;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            words = des.ReadUInt32Array(br);
            numBits = br.ReadInt32();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteUInt32Array(bw, words);
            bw.WriteInt32(numBits);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            words = xd.ReadUInt32Array(xe, nameof(words));
            numBits = xd.ReadInt32(xe, nameof(numBits));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumberArray(xe, nameof(words), words);
            xs.WriteNumber(xe, nameof(numBits), numBits);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkBitField);
        }

        public bool Equals(hkBitField? other)
        {
            return other is not null &&
                   words.SequenceEqual(other.words) &&
                   numBits.Equals(other.numBits) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(words.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(numBits);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

