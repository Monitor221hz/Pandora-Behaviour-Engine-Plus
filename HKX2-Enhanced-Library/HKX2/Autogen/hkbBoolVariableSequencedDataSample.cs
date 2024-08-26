using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBoolVariableSequencedDataSample Signatire: 0x514763dc size: 8 flags: FLAGS_NONE

    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // value class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkbBoolVariableSequencedDataSample : IHavokObject, IEquatable<hkbBoolVariableSequencedDataSample?>
    {
        public float time { set; get; }
        public bool value { set; get; }

        public virtual uint Signature { set; get; } = 0x514763dc;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            time = br.ReadSingle();
            value = br.ReadBoolean();
            br.Position += 3;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(time);
            bw.WriteBoolean(value);
            bw.Position += 3;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            time = xd.ReadSingle(xe, nameof(time));
            value = xd.ReadBoolean(xe, nameof(value));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(time), time);
            xs.WriteBoolean(xe, nameof(value), value);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBoolVariableSequencedDataSample);
        }

        public bool Equals(hkbBoolVariableSequencedDataSample? other)
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

