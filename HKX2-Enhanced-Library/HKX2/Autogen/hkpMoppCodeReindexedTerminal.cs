using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMoppCodeReindexedTerminal Signatire: 0x6ed8ac06 size: 8 flags: FLAGS_NONE

    // origShapeKey class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // reindexedShapeKey class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkpMoppCodeReindexedTerminal : IHavokObject, IEquatable<hkpMoppCodeReindexedTerminal?>
    {
        public uint origShapeKey { set; get; }
        public uint reindexedShapeKey { set; get; }

        public virtual uint Signature { set; get; } = 0x6ed8ac06;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            origShapeKey = br.ReadUInt32();
            reindexedShapeKey = br.ReadUInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(origShapeKey);
            bw.WriteUInt32(reindexedShapeKey);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            origShapeKey = xd.ReadUInt32(xe, nameof(origShapeKey));
            reindexedShapeKey = xd.ReadUInt32(xe, nameof(reindexedShapeKey));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(origShapeKey), origShapeKey);
            xs.WriteNumber(xe, nameof(reindexedShapeKey), reindexedShapeKey);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMoppCodeReindexedTerminal);
        }

        public bool Equals(hkpMoppCodeReindexedTerminal? other)
        {
            return other is not null &&
                   origShapeKey.Equals(other.origShapeKey) &&
                   reindexedShapeKey.Equals(other.reindexedShapeKey) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(origShapeKey);
            hashcode.Add(reindexedShapeKey);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

