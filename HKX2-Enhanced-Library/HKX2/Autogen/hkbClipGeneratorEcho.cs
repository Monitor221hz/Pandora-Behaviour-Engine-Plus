using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbClipGeneratorEcho Signatire: 0x750edf40 size: 16 flags: FLAGS_NONE

    // offsetLocalTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: ALIGN_16|FLAGS_NONE enum: 
    // weight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // dwdt class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkbClipGeneratorEcho : IHavokObject, IEquatable<hkbClipGeneratorEcho?>
    {
        public float offsetLocalTime { set; get; }
        public float weight { set; get; }
        public float dwdt { set; get; }

        public virtual uint Signature { set; get; } = 0x750edf40;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            offsetLocalTime = br.ReadSingle();
            weight = br.ReadSingle();
            dwdt = br.ReadSingle();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(offsetLocalTime);
            bw.WriteSingle(weight);
            bw.WriteSingle(dwdt);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            offsetLocalTime = xd.ReadSingle(xe, nameof(offsetLocalTime));
            weight = xd.ReadSingle(xe, nameof(weight));
            dwdt = xd.ReadSingle(xe, nameof(dwdt));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(offsetLocalTime), offsetLocalTime);
            xs.WriteFloat(xe, nameof(weight), weight);
            xs.WriteFloat(xe, nameof(dwdt), dwdt);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbClipGeneratorEcho);
        }

        public bool Equals(hkbClipGeneratorEcho? other)
        {
            return other is not null &&
                   offsetLocalTime.Equals(other.offsetLocalTime) &&
                   weight.Equals(other.weight) &&
                   dwdt.Equals(other.dwdt) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(offsetLocalTime);
            hashcode.Add(weight);
            hashcode.Add(dwdt);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

