using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbRealVariableSequencedDataSample Signatire: 0xbb708bbd size: 8 flags: FLAGS_NONE

    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // value class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkbRealVariableSequencedDataSample : IHavokObject, IEquatable<hkbRealVariableSequencedDataSample?>
    {
        public float time { set; get; }
        public float value { set; get; }

        public virtual uint Signature { set; get; } = 0xbb708bbd;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            time = br.ReadSingle();
            value = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(time);
            bw.WriteSingle(value);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            time = xd.ReadSingle(xe, nameof(time));
            value = xd.ReadSingle(xe, nameof(value));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(time), time);
            xs.WriteFloat(xe, nameof(value), value);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbRealVariableSequencedDataSample);
        }

        public bool Equals(hkbRealVariableSequencedDataSample? other)
        {
            return other is not null &&
                   time.Equals(other.time) &&
                   value.Equals(other.value) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(time);
            hashcode.Add(value);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

