using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbFootIkModifierInternalLegData Signatire: 0xe5ca3677 size: 32 flags: FLAGS_NONE

    // groundPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // footIkSolver class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbFootIkModifierInternalLegData : IHavokObject, IEquatable<hkbFootIkModifierInternalLegData?>
    {
        public Vector4 groundPosition { set; get; }
        private object? footIkSolver { set; get; }

        public virtual uint Signature { set; get; } = 0xe5ca3677;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            groundPosition = br.ReadVector4();
            des.ReadEmptyPointer(br);
            br.Position += 8;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(groundPosition);
            s.WriteVoidPointer(bw);
            bw.Position += 8;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            groundPosition = xd.ReadVector4(xe, nameof(groundPosition));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(groundPosition), groundPosition);
            xs.WriteSerializeIgnored(xe, nameof(footIkSolver));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbFootIkModifierInternalLegData);
        }

        public bool Equals(hkbFootIkModifierInternalLegData? other)
        {
            return other is not null &&
                   groundPosition.Equals(other.groundPosition) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(groundPosition);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

