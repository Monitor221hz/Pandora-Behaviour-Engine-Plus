using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbIntVariableSequencedDataSample Signatire: 0xbe7ac63c size: 8 flags: FLAGS_NONE

    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // value class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkbIntVariableSequencedDataSample : IHavokObject, IEquatable<hkbIntVariableSequencedDataSample?>
    {
        public float time { set; get; }
        public int value { set; get; }

        public virtual uint Signature { set; get; } = 0xbe7ac63c;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            time = br.ReadSingle();
            value = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(time);
            bw.WriteInt32(value);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            time = xd.ReadSingle(xe, nameof(time));
            value = xd.ReadInt32(xe, nameof(value));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(time), time);
            xs.WriteNumber(xe, nameof(value), value);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbIntVariableSequencedDataSample);
        }

        public bool Equals(hkbIntVariableSequencedDataSample? other)
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

