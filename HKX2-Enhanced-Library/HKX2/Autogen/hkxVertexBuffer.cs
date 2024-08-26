using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxVertexBuffer Signatire: 0x4ab10615 size: 136 flags: FLAGS_NONE

    // data class: hkxVertexBufferVertexData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // desc class: hkxVertexDescription Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    public partial class hkxVertexBuffer : hkReferencedObject, IEquatable<hkxVertexBuffer?>
    {
        public hkxVertexBufferVertexData data { set; get; } = new();
        public hkxVertexDescription desc { set; get; } = new();

        public override uint Signature { set; get; } = 0x4ab10615;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            data.Read(des, br);
            desc.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            data.Write(s, bw);
            desc.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            data = xd.ReadClass<hkxVertexBufferVertexData>(xe, nameof(data));
            desc = xd.ReadClass<hkxVertexDescription>(xe, nameof(desc));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkxVertexBufferVertexData>(xe, nameof(data), data);
            xs.WriteClass<hkxVertexDescription>(xe, nameof(desc), desc);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxVertexBuffer);
        }

        public bool Equals(hkxVertexBuffer? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((data is null && other.data is null) || (data is not null && other.data is not null && data.Equals((IHavokObject)other.data))) &&
                   ((desc is null && other.desc is null) || (desc is not null && other.desc is not null && desc.Equals((IHavokObject)other.desc))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(data);
            hashcode.Add(desc);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

