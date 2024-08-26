using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkRangeRealAttribute Signatire: 0x949db24f size: 16 flags: FLAGS_NONE

    // absmin class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // absmax class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // softmin class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // softmax class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkRangeRealAttribute : IHavokObject, IEquatable<hkRangeRealAttribute?>
    {
        public float absmin { set; get; }
        public float absmax { set; get; }
        public float softmin { set; get; }
        public float softmax { set; get; }

        public virtual uint Signature { set; get; } = 0x949db24f;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            absmin = br.ReadSingle();
            absmax = br.ReadSingle();
            softmin = br.ReadSingle();
            softmax = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(absmin);
            bw.WriteSingle(absmax);
            bw.WriteSingle(softmin);
            bw.WriteSingle(softmax);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            absmin = xd.ReadSingle(xe, nameof(absmin));
            absmax = xd.ReadSingle(xe, nameof(absmax));
            softmin = xd.ReadSingle(xe, nameof(softmin));
            softmax = xd.ReadSingle(xe, nameof(softmax));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(absmin), absmin);
            xs.WriteFloat(xe, nameof(absmax), absmax);
            xs.WriteFloat(xe, nameof(softmin), softmin);
            xs.WriteFloat(xe, nameof(softmax), softmax);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkRangeRealAttribute);
        }

        public bool Equals(hkRangeRealAttribute? other)
        {
            return other is not null &&
                   absmin.Equals(other.absmin) &&
                   absmax.Equals(other.absmax) &&
                   softmin.Equals(other.softmin) &&
                   softmax.Equals(other.softmax) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(absmin);
            hashcode.Add(absmax);
            hashcode.Add(softmin);
            hashcode.Add(softmax);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

