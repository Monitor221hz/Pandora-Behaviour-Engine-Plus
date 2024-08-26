using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxNodeAnnotationData Signatire: 0x433dee92 size: 16 flags: FLAGS_NONE

    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // description class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkxNodeAnnotationData : IHavokObject, IEquatable<hkxNodeAnnotationData?>
    {
        public float time { set; get; }
        public string description { set; get; } = "";

        public virtual uint Signature { set; get; } = 0x433dee92;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            time = br.ReadSingle();
            br.Position += 4;
            description = des.ReadStringPointer(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(time);
            bw.Position += 4;
            s.WriteStringPointer(bw, description);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            time = xd.ReadSingle(xe, nameof(time));
            description = xd.ReadString(xe, nameof(description));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(time), time);
            xs.WriteString(xe, nameof(description), description);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxNodeAnnotationData);
        }

        public bool Equals(hkxNodeAnnotationData? other)
        {
            return other is not null &&
                   time.Equals(other.time) &&
                   (description is null && other.description is null || description == other.description || description is null && other.description == "" || description == "" && other.description is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(time);
            hashcode.Add(description);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

