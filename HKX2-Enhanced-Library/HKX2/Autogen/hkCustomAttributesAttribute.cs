using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkCustomAttributesAttribute Signatire: 0x1388d601 size: 24 flags: FLAGS_NONE

    // name class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // value class:  Type.TYPE_VARIANT Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkCustomAttributesAttribute : IHavokObject, IEquatable<hkCustomAttributesAttribute?>
    {
        public string name { set; get; } = "";
        public object? value { set; get; }

        public virtual uint Signature { set; get; } = 0x1388d601;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            name = des.ReadCString(br);
            throw new NotImplementedException("TPYE_VARIANT");
            //br.Position += 8;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteCString(bw, name);
            throw new NotImplementedException("TPYE_VARIANT");
            //bw.Position += 8;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            name = xd.ReadString(xe, nameof(name));
            throw new NotImplementedException("TPYE_VARIANT");
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(name), name);
            throw new NotImplementedException("TPYE_VARIANT");
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkCustomAttributesAttribute);
        }

        public bool Equals(hkCustomAttributesAttribute? other)
        {
            return other is not null &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   value!.Equals(other.value) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(name);
            hashcode.Add(value);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

