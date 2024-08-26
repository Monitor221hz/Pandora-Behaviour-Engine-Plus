using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxTextureFile Signatire: 0x1e289259 size: 40 flags: FLAGS_NONE

    // filename class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // originalFilename class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkxTextureFile : hkReferencedObject, IEquatable<hkxTextureFile?>
    {
        public string filename { set; get; } = "";
        public string name { set; get; } = "";
        public string originalFilename { set; get; } = "";

        public override uint Signature { set; get; } = 0x1e289259;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            filename = des.ReadStringPointer(br);
            name = des.ReadStringPointer(br);
            originalFilename = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, filename);
            s.WriteStringPointer(bw, name);
            s.WriteStringPointer(bw, originalFilename);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            filename = xd.ReadString(xe, nameof(filename));
            name = xd.ReadString(xe, nameof(name));
            originalFilename = xd.ReadString(xe, nameof(originalFilename));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(filename), filename);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteString(xe, nameof(originalFilename), originalFilename);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxTextureFile);
        }

        public bool Equals(hkxTextureFile? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (filename is null && other.filename is null || filename == other.filename || filename is null && other.filename == "" || filename == "" && other.filename is null) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   (originalFilename is null && other.originalFilename is null || originalFilename == other.originalFilename || originalFilename is null && other.originalFilename == "" || originalFilename == "" && other.originalFilename is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(filename);
            hashcode.Add(name);
            hashcode.Add(originalFilename);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

