using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxMeshUserChannelInfo Signatire: 0x270724a5 size: 48 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // className class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    public partial class hkxMeshUserChannelInfo : hkxAttributeHolder, IEquatable<hkxMeshUserChannelInfo?>
    {
        public string name { set; get; } = "";
        public string className { set; get; } = "";

        public override uint Signature { set; get; } = 0x270724a5;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            name = des.ReadStringPointer(br);
            className = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, name);
            s.WriteStringPointer(bw, className);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
            className = xd.ReadString(xe, nameof(className));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteString(xe, nameof(className), className);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxMeshUserChannelInfo);
        }

        public bool Equals(hkxMeshUserChannelInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   (className is null && other.className is null || className == other.className || className is null && other.className == "" || className == "" && other.className is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name);
            hashcode.Add(className);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

