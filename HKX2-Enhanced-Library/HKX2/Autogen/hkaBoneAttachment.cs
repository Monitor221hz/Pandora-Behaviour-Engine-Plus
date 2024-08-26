using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaBoneAttachment Signatire: 0xa8ccd5cf size: 128 flags: FLAGS_NONE

    // originalSkeletonName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // boneFromAttachment class:  Type.TYPE_MATRIX4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // attachment class: hkReferencedObject Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // boneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkaBoneAttachment : hkReferencedObject, IEquatable<hkaBoneAttachment?>
    {
        public string originalSkeletonName { set; get; } = "";
        public Matrix4x4 boneFromAttachment { set; get; }
        public hkReferencedObject? attachment { set; get; }
        public string name { set; get; } = "";
        public short boneIndex { set; get; }

        public override uint Signature { set; get; } = 0xa8ccd5cf;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            originalSkeletonName = des.ReadStringPointer(br);
            br.Position += 8;
            boneFromAttachment = des.ReadMatrix4(br);
            attachment = des.ReadClassPointer<hkReferencedObject>(br);
            name = des.ReadStringPointer(br);
            boneIndex = br.ReadInt16();
            br.Position += 14;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, originalSkeletonName);
            bw.Position += 8;
            s.WriteMatrix4(bw, boneFromAttachment);
            s.WriteClassPointer(bw, attachment);
            s.WriteStringPointer(bw, name);
            bw.WriteInt16(boneIndex);
            bw.Position += 14;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            originalSkeletonName = xd.ReadString(xe, nameof(originalSkeletonName));
            boneFromAttachment = xd.ReadMatrix4(xe, nameof(boneFromAttachment));
            attachment = xd.ReadClassPointer<hkReferencedObject>(this, xe, nameof(attachment));
            name = xd.ReadString(xe, nameof(name));
            boneIndex = xd.ReadInt16(xe, nameof(boneIndex));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(originalSkeletonName), originalSkeletonName);
            xs.WriteMatrix4(xe, nameof(boneFromAttachment), boneFromAttachment);
            xs.WriteClassPointer(xe, nameof(attachment), attachment);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteNumber(xe, nameof(boneIndex), boneIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaBoneAttachment);
        }

        public bool Equals(hkaBoneAttachment? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (originalSkeletonName is null && other.originalSkeletonName is null || originalSkeletonName == other.originalSkeletonName || originalSkeletonName is null && other.originalSkeletonName == "" || originalSkeletonName == "" && other.originalSkeletonName is null) &&
                   boneFromAttachment.Equals(other.boneFromAttachment) &&
                   ((attachment is null && other.attachment is null) || (attachment is not null && other.attachment is not null && attachment.Equals((IHavokObject)other.attachment))) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   boneIndex.Equals(other.boneIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(originalSkeletonName);
            hashcode.Add(boneFromAttachment);
            hashcode.Add(attachment);
            hashcode.Add(name);
            hashcode.Add(boneIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

