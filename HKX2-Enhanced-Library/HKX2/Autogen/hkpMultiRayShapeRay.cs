using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMultiRayShapeRay Signatire: 0xffdc0b65 size: 32 flags: FLAGS_NONE

    // start class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // end class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpMultiRayShapeRay : IHavokObject, IEquatable<hkpMultiRayShapeRay?>
    {
        public Vector4 start { set; get; }
        public Vector4 end { set; get; }

        public virtual uint Signature { set; get; } = 0xffdc0b65;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            start = br.ReadVector4();
            end = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(start);
            bw.WriteVector4(end);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            start = xd.ReadVector4(xe, nameof(start));
            end = xd.ReadVector4(xe, nameof(end));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(start), start);
            xs.WriteVector4(xe, nameof(end), end);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMultiRayShapeRay);
        }

        public bool Equals(hkpMultiRayShapeRay? other)
        {
            return other is not null &&
                   start.Equals(other.start) &&
                   end.Equals(other.end) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(start);
            hashcode.Add(end);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

