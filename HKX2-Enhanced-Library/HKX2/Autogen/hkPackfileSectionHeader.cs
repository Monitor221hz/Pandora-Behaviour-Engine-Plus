using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkPackfileSectionHeader Signatire: 0xf2a92154 size: 48 flags: FLAGS_NONE

    // sectionTag class:  Type.TYPE_CHAR Type.TYPE_VOID arrSize: 19 offset: 0 flags: FLAGS_NONE enum: 
    // nullByte class:  Type.TYPE_CHAR Type.TYPE_VOID arrSize: 0 offset: 19 flags: FLAGS_NONE enum: 
    // absoluteDataStart class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // localFixupsOffset class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // globalFixupsOffset class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // virtualFixupsOffset class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // exportsOffset class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // importsOffset class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // endOffset class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    public partial class hkPackfileSectionHeader : IHavokObject, IEquatable<hkPackfileSectionHeader?>
    {
        public string sectionTag { set; get; } = "";
        public string nullByte { set; get; } = "";
        public int absoluteDataStart { set; get; }
        public int localFixupsOffset { set; get; }
        public int globalFixupsOffset { set; get; }
        public int virtualFixupsOffset { set; get; }
        public int exportsOffset { set; get; }
        public int importsOffset { set; get; }
        public int endOffset { set; get; }

        public virtual uint Signature { set; get; } = 0xf2a92154;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            sectionTag = br.ReadASCII(19);
            nullByte = br.ReadASCII();
            absoluteDataStart = br.ReadInt32();
            localFixupsOffset = br.ReadInt32();
            globalFixupsOffset = br.ReadInt32();
            virtualFixupsOffset = br.ReadInt32();
            exportsOffset = br.ReadInt32();
            importsOffset = br.ReadInt32();
            endOffset = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteASCII(sectionTag);
            bw.WriteASCII(nullByte);
            bw.WriteInt32(absoluteDataStart);
            bw.WriteInt32(localFixupsOffset);
            bw.WriteInt32(globalFixupsOffset);
            bw.WriteInt32(virtualFixupsOffset);
            bw.WriteInt32(exportsOffset);
            bw.WriteInt32(importsOffset);
            bw.WriteInt32(endOffset);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            sectionTag = xd.ReadString(xe, nameof(sectionTag));
            nullByte = xd.ReadString(xe, nameof(nullByte));
            absoluteDataStart = xd.ReadInt32(xe, nameof(absoluteDataStart));
            localFixupsOffset = xd.ReadInt32(xe, nameof(localFixupsOffset));
            globalFixupsOffset = xd.ReadInt32(xe, nameof(globalFixupsOffset));
            virtualFixupsOffset = xd.ReadInt32(xe, nameof(virtualFixupsOffset));
            exportsOffset = xd.ReadInt32(xe, nameof(exportsOffset));
            importsOffset = xd.ReadInt32(xe, nameof(importsOffset));
            endOffset = xd.ReadInt32(xe, nameof(endOffset));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(sectionTag), sectionTag);
            xs.WriteString(xe, nameof(nullByte), nullByte);
            xs.WriteNumber(xe, nameof(absoluteDataStart), absoluteDataStart);
            xs.WriteNumber(xe, nameof(localFixupsOffset), localFixupsOffset);
            xs.WriteNumber(xe, nameof(globalFixupsOffset), globalFixupsOffset);
            xs.WriteNumber(xe, nameof(virtualFixupsOffset), virtualFixupsOffset);
            xs.WriteNumber(xe, nameof(exportsOffset), exportsOffset);
            xs.WriteNumber(xe, nameof(importsOffset), importsOffset);
            xs.WriteNumber(xe, nameof(endOffset), endOffset);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkPackfileSectionHeader);
        }

        public bool Equals(hkPackfileSectionHeader? other)
        {
            return other is not null &&
                   sectionTag.SequenceEqual(other.sectionTag) &&
                   nullByte.Equals(other.nullByte) &&
                   absoluteDataStart.Equals(other.absoluteDataStart) &&
                   localFixupsOffset.Equals(other.localFixupsOffset) &&
                   globalFixupsOffset.Equals(other.globalFixupsOffset) &&
                   virtualFixupsOffset.Equals(other.virtualFixupsOffset) &&
                   exportsOffset.Equals(other.exportsOffset) &&
                   importsOffset.Equals(other.importsOffset) &&
                   endOffset.Equals(other.endOffset) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(sectionTag.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(nullByte);
            hashcode.Add(absoluteDataStart);
            hashcode.Add(localFixupsOffset);
            hashcode.Add(globalFixupsOffset);
            hashcode.Add(virtualFixupsOffset);
            hashcode.Add(exportsOffset);
            hashcode.Add(importsOffset);
            hashcode.Add(endOffset);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

