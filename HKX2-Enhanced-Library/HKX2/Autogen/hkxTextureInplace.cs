using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxTextureInplace Signatire: 0xd45841d6 size: 56 flags: FLAGS_NONE

    // fileType class:  Type.TYPE_CHAR Type.TYPE_VOID arrSize: 4 offset: 16 flags: FLAGS_NONE enum: 
    // data class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // originalFilename class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkxTextureInplace : hkReferencedObject, IEquatable<hkxTextureInplace?>
    {
        public string fileType { set; get; } = "";
        public IList<byte> data { set; get; } = Array.Empty<byte>();
        public string name { set; get; } = "";
        public string originalFilename { set; get; } = "";

        public override uint Signature { set; get; } = 0xd45841d6;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            fileType = br.ReadASCII(4);
            br.Position += 4;
            data = des.ReadByteArray(br);
            name = des.ReadStringPointer(br);
            originalFilename = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteASCII(fileType);
            bw.Position += 4;
            s.WriteByteArray(bw, data);
            s.WriteStringPointer(bw, name);
            s.WriteStringPointer(bw, originalFilename);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            fileType = xd.ReadString(xe, nameof(fileType));
            data = xd.ReadByteArray(xe, nameof(data));
            name = xd.ReadString(xe, nameof(name));
            originalFilename = xd.ReadString(xe, nameof(originalFilename));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(fileType), fileType);
            xs.WriteNumberArray(xe, nameof(data), data);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteString(xe, nameof(originalFilename), originalFilename);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxTextureInplace);
        }

        public bool Equals(hkxTextureInplace? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   fileType.SequenceEqual(other.fileType) &&
                   data.SequenceEqual(other.data) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   (originalFilename is null && other.originalFilename is null || originalFilename == other.originalFilename || originalFilename is null && other.originalFilename == "" || originalFilename == "" && other.originalFilename is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(fileType.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(name);
            hashcode.Add(originalFilename);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

