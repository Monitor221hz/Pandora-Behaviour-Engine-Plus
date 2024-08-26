using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPairCollisionFilterMapPairFilterKeyOverrideType Signatire: 0x36195969 size: 16 flags: FLAGS_NONE

    // elem class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // numElems class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // hashMod class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkpPairCollisionFilterMapPairFilterKeyOverrideType : IHavokObject, IEquatable<hkpPairCollisionFilterMapPairFilterKeyOverrideType?>
    {
        private object? elem { set; get; }
        public int numElems { set; get; }
        public int hashMod { set; get; }

        public virtual uint Signature { set; get; } = 0x36195969;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            des.ReadEmptyPointer(br);
            numElems = br.ReadInt32();
            hashMod = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVoidPointer(bw);
            bw.WriteInt32(numElems);
            bw.WriteInt32(hashMod);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            numElems = xd.ReadInt32(xe, nameof(numElems));
            hashMod = xd.ReadInt32(xe, nameof(hashMod));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(elem));
            xs.WriteNumber(xe, nameof(numElems), numElems);
            xs.WriteNumber(xe, nameof(hashMod), hashMod);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPairCollisionFilterMapPairFilterKeyOverrideType);
        }

        public bool Equals(hkpPairCollisionFilterMapPairFilterKeyOverrideType? other)
        {
            return other is not null &&
                   numElems.Equals(other.numElems) &&
                   hashMod.Equals(other.hashMod) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(numElems);
            hashcode.Add(hashMod);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

