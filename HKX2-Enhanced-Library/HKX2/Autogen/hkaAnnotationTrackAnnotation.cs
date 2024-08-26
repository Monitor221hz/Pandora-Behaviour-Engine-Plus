using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaAnnotationTrackAnnotation Signatire: 0x623bf34f size: 16 flags: FLAGS_NONE

    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // text class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkaAnnotationTrackAnnotation : IHavokObject, IEquatable<hkaAnnotationTrackAnnotation?>
    {
        public float time { set; get; }
        public string text { set; get; } = "";

        public virtual uint Signature { set; get; } = 0x623bf34f;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            time = br.ReadSingle();
            br.Position += 4;
            text = des.ReadStringPointer(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(time);
            bw.Position += 4;
            s.WriteStringPointer(bw, text);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            time = xd.ReadSingle(xe, nameof(time));
            text = xd.ReadString(xe, nameof(text));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(time), time);
            xs.WriteString(xe, nameof(text), text);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaAnnotationTrackAnnotation);
        }

        public bool Equals(hkaAnnotationTrackAnnotation? other)
        {
            return other is not null &&
                   time.Equals(other.time) &&
                   (text is null && other.text is null || text == other.text || text is null && other.text == "" || text == "" && other.text is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(time);
            hashcode.Add(text);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

