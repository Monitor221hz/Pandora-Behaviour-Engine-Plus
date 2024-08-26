using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkRangeInt32Attribute Signatire: 0x4846be29 size: 16 flags: FLAGS_NONE

    // absmin class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // absmax class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // softmin class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // softmax class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkRangeInt32Attribute : IHavokObject, IEquatable<hkRangeInt32Attribute?>
    {
        public int absmin { set; get; }
        public int absmax { set; get; }
        public int softmin { set; get; }
        public int softmax { set; get; }

        public virtual uint Signature { set; get; } = 0x4846be29;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            absmin = br.ReadInt32();
            absmax = br.ReadInt32();
            softmin = br.ReadInt32();
            softmax = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(absmin);
            bw.WriteInt32(absmax);
            bw.WriteInt32(softmin);
            bw.WriteInt32(softmax);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            absmin = xd.ReadInt32(xe, nameof(absmin));
            absmax = xd.ReadInt32(xe, nameof(absmax));
            softmin = xd.ReadInt32(xe, nameof(softmin));
            softmax = xd.ReadInt32(xe, nameof(softmax));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(absmin), absmin);
            xs.WriteNumber(xe, nameof(absmax), absmax);
            xs.WriteNumber(xe, nameof(softmin), softmin);
            xs.WriteNumber(xe, nameof(softmax), softmax);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkRangeInt32Attribute);
        }

        public bool Equals(hkRangeInt32Attribute? other)
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

