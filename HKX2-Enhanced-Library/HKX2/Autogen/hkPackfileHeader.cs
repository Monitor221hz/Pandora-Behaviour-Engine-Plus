using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkPackfileHeader Signatire: 0x79f9ffda size: 64 flags: FLAGS_NONE

    // magic class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 2 offset: 0 flags: FLAGS_NONE enum: 
    // userTag class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // fileVersion class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // layoutRules class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 4 offset: 16 flags: FLAGS_NONE enum: 
    // numSections class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // contentsSectionIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // contentsSectionOffset class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // contentsClassNameSectionIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // contentsClassNameSectionOffset class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // contentsVersion class:  Type.TYPE_CHAR Type.TYPE_VOID arrSize: 16 offset: 40 flags: FLAGS_NONE enum: 
    // flags class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // pad class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 1 offset: 60 flags: FLAGS_NONE enum: 
    public partial class hkPackfileHeader : IHavokObject, IEquatable<hkPackfileHeader?>
    {
        public int[] magic = new int[2];
        public int userTag { set; get; }
        public int fileVersion { set; get; }
        public byte[] layoutRules = new byte[4];
        public int numSections { set; get; }
        public int contentsSectionIndex { set; get; }
        public int contentsSectionOffset { set; get; }
        public int contentsClassNameSectionIndex { set; get; }
        public int contentsClassNameSectionOffset { set; get; }
        public string contentsVersion { set; get; } = "";
        public int flags { set; get; }
        public int[] pad = new int[1];

        public virtual uint Signature { set; get; } = 0x79f9ffda;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            magic = des.ReadInt32CStyleArray(br, 2);
            userTag = br.ReadInt32();
            fileVersion = br.ReadInt32();
            layoutRules = des.ReadByteCStyleArray(br, 4);
            numSections = br.ReadInt32();
            contentsSectionIndex = br.ReadInt32();
            contentsSectionOffset = br.ReadInt32();
            contentsClassNameSectionIndex = br.ReadInt32();
            contentsClassNameSectionOffset = br.ReadInt32();
            contentsVersion = br.ReadASCII(16);
            flags = br.ReadInt32();
            pad = des.ReadInt32CStyleArray(br, 1);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteInt32CStyleArray(bw, magic);
            bw.WriteInt32(userTag);
            bw.WriteInt32(fileVersion);
            s.WriteByteCStyleArray(bw, layoutRules);
            bw.WriteInt32(numSections);
            bw.WriteInt32(contentsSectionIndex);
            bw.WriteInt32(contentsSectionOffset);
            bw.WriteInt32(contentsClassNameSectionIndex);
            bw.WriteInt32(contentsClassNameSectionOffset);
            bw.WriteASCII(contentsVersion);
            bw.WriteInt32(flags);
            s.WriteInt32CStyleArray(bw, pad);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            magic = xd.ReadInt32CStyleArray(xe, nameof(magic), 2);
            userTag = xd.ReadInt32(xe, nameof(userTag));
            fileVersion = xd.ReadInt32(xe, nameof(fileVersion));
            layoutRules = xd.ReadByteCStyleArray(xe, nameof(layoutRules), 4);
            numSections = xd.ReadInt32(xe, nameof(numSections));
            contentsSectionIndex = xd.ReadInt32(xe, nameof(contentsSectionIndex));
            contentsSectionOffset = xd.ReadInt32(xe, nameof(contentsSectionOffset));
            contentsClassNameSectionIndex = xd.ReadInt32(xe, nameof(contentsClassNameSectionIndex));
            contentsClassNameSectionOffset = xd.ReadInt32(xe, nameof(contentsClassNameSectionOffset));
            contentsVersion = xd.ReadString(xe, nameof(contentsVersion));
            flags = xd.ReadInt32(xe, nameof(flags));
            pad = xd.ReadInt32CStyleArray(xe, nameof(pad), 1);
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumberArray(xe, nameof(magic), magic);
            xs.WriteNumber(xe, nameof(userTag), userTag);
            xs.WriteNumber(xe, nameof(fileVersion), fileVersion);
            xs.WriteNumberArray(xe, nameof(layoutRules), layoutRules);
            xs.WriteNumber(xe, nameof(numSections), numSections);
            xs.WriteNumber(xe, nameof(contentsSectionIndex), contentsSectionIndex);
            xs.WriteNumber(xe, nameof(contentsSectionOffset), contentsSectionOffset);
            xs.WriteNumber(xe, nameof(contentsClassNameSectionIndex), contentsClassNameSectionIndex);
            xs.WriteNumber(xe, nameof(contentsClassNameSectionOffset), contentsClassNameSectionOffset);
            xs.WriteString(xe, nameof(contentsVersion), contentsVersion);
            xs.WriteNumber(xe, nameof(flags), flags);
            xs.WriteNumberArray(xe, nameof(pad), pad);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkPackfileHeader);
        }

        public bool Equals(hkPackfileHeader? other)
        {
            return other is not null &&
                   magic.SequenceEqual(other.magic) &&
                   userTag.Equals(other.userTag) &&
                   fileVersion.Equals(other.fileVersion) &&
                   layoutRules.SequenceEqual(other.layoutRules) &&
                   numSections.Equals(other.numSections) &&
                   contentsSectionIndex.Equals(other.contentsSectionIndex) &&
                   contentsSectionOffset.Equals(other.contentsSectionOffset) &&
                   contentsClassNameSectionIndex.Equals(other.contentsClassNameSectionIndex) &&
                   contentsClassNameSectionOffset.Equals(other.contentsClassNameSectionOffset) &&
                   contentsVersion.SequenceEqual(other.contentsVersion) &&
                   flags.Equals(other.flags) &&
                   pad.SequenceEqual(other.pad) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(magic.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(userTag);
            hashcode.Add(fileVersion);
            hashcode.Add(layoutRules.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(numSections);
            hashcode.Add(contentsSectionIndex);
            hashcode.Add(contentsSectionOffset);
            hashcode.Add(contentsClassNameSectionIndex);
            hashcode.Add(contentsClassNameSectionOffset);
            hashcode.Add(contentsVersion.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(flags);
            hashcode.Add(pad.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

