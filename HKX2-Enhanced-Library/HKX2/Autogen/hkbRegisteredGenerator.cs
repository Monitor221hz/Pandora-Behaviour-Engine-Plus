using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbRegisteredGenerator Signatire: 0x58b1d082 size: 96 flags: FLAGS_NONE

    // generator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // relativePosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // relativeDirection class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkbRegisteredGenerator : hkbBindable, IEquatable<hkbRegisteredGenerator?>
    {
        public hkbGenerator? generator { set; get; }
        public Vector4 relativePosition { set; get; }
        public Vector4 relativeDirection { set; get; }

        public override uint Signature { set; get; } = 0x58b1d082;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            generator = des.ReadClassPointer<hkbGenerator>(br);
            br.Position += 8;
            relativePosition = br.ReadVector4();
            relativeDirection = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, generator);
            bw.Position += 8;
            bw.WriteVector4(relativePosition);
            bw.WriteVector4(relativeDirection);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            generator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(generator));
            relativePosition = xd.ReadVector4(xe, nameof(relativePosition));
            relativeDirection = xd.ReadVector4(xe, nameof(relativeDirection));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(generator), generator);
            xs.WriteVector4(xe, nameof(relativePosition), relativePosition);
            xs.WriteVector4(xe, nameof(relativeDirection), relativeDirection);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbRegisteredGenerator);
        }

        public bool Equals(hkbRegisteredGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((generator is null && other.generator is null) || (generator is not null && other.generator is not null && generator.Equals((IHavokObject)other.generator))) &&
                   relativePosition.Equals(other.relativePosition) &&
                   relativeDirection.Equals(other.relativeDirection) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(generator);
            hashcode.Add(relativePosition);
            hashcode.Add(relativeDirection);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

