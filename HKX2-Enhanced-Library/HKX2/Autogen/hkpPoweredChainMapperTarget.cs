using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPoweredChainMapperTarget Signatire: 0xf651c74d size: 16 flags: FLAGS_NONE

    // chain class: hkpPoweredChainData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // infoIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpPoweredChainMapperTarget : IHavokObject, IEquatable<hkpPoweredChainMapperTarget?>
    {
        public hkpPoweredChainData? chain { set; get; }
        public int infoIndex { set; get; }

        public virtual uint Signature { set; get; } = 0xf651c74d;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            chain = des.ReadClassPointer<hkpPoweredChainData>(br);
            infoIndex = br.ReadInt32();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer(bw, chain);
            bw.WriteInt32(infoIndex);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            chain = xd.ReadClassPointer<hkpPoweredChainData>(this, xe, nameof(chain));
            infoIndex = xd.ReadInt32(xe, nameof(infoIndex));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointer(xe, nameof(chain), chain);
            xs.WriteNumber(xe, nameof(infoIndex), infoIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPoweredChainMapperTarget);
        }

        public bool Equals(hkpPoweredChainMapperTarget? other)
        {
            return other is not null &&
                   ((chain is null && other.chain is null) || (chain is not null && other.chain is not null && chain.Equals((IHavokObject)other.chain))) &&
                   infoIndex.Equals(other.infoIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(chain);
            hashcode.Add(infoIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

