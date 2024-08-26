using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBehaviorReferenceGenerator Signatire: 0xfcb5423 size: 88 flags: FLAGS_NONE

    // behaviorName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // behavior class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 80 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbBehaviorReferenceGenerator : hkbGenerator, IEquatable<hkbBehaviorReferenceGenerator?>
    {
        public string behaviorName { set; get; } = "";
        private object? behavior { set; get; }

        public override uint Signature { set; get; } = 0xfcb5423;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            behaviorName = des.ReadStringPointer(br);
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, behaviorName);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            behaviorName = xd.ReadString(xe, nameof(behaviorName));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(behaviorName), behaviorName);
            xs.WriteSerializeIgnored(xe, nameof(behavior));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBehaviorReferenceGenerator);
        }

        public bool Equals(hkbBehaviorReferenceGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (behaviorName is null && other.behaviorName is null || behaviorName == other.behaviorName || behaviorName is null && other.behaviorName == "" || behaviorName == "" && other.behaviorName is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(behaviorName);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

