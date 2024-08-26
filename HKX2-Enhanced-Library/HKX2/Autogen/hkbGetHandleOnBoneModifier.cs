using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbGetHandleOnBoneModifier Signatire: 0x50c34a17 size: 104 flags: FLAGS_NONE

    // handleOut class: hkbHandle Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // localFrameName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // ragdollBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // animationBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 98 flags: FLAGS_NONE enum: 
    public partial class hkbGetHandleOnBoneModifier : hkbModifier, IEquatable<hkbGetHandleOnBoneModifier?>
    {
        public hkbHandle? handleOut { set; get; }
        public string localFrameName { set; get; } = "";
        public short ragdollBoneIndex { set; get; }
        public short animationBoneIndex { set; get; }

        public override uint Signature { set; get; } = 0x50c34a17;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            handleOut = des.ReadClassPointer<hkbHandle>(br);
            localFrameName = des.ReadStringPointer(br);
            ragdollBoneIndex = br.ReadInt16();
            animationBoneIndex = br.ReadInt16();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, handleOut);
            s.WriteStringPointer(bw, localFrameName);
            bw.WriteInt16(ragdollBoneIndex);
            bw.WriteInt16(animationBoneIndex);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            handleOut = xd.ReadClassPointer<hkbHandle>(this, xe, nameof(handleOut));
            localFrameName = xd.ReadString(xe, nameof(localFrameName));
            ragdollBoneIndex = xd.ReadInt16(xe, nameof(ragdollBoneIndex));
            animationBoneIndex = xd.ReadInt16(xe, nameof(animationBoneIndex));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(handleOut), handleOut);
            xs.WriteString(xe, nameof(localFrameName), localFrameName);
            xs.WriteNumber(xe, nameof(ragdollBoneIndex), ragdollBoneIndex);
            xs.WriteNumber(xe, nameof(animationBoneIndex), animationBoneIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbGetHandleOnBoneModifier);
        }

        public bool Equals(hkbGetHandleOnBoneModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((handleOut is null && other.handleOut is null) || (handleOut is not null && other.handleOut is not null && handleOut.Equals((IHavokObject)other.handleOut))) &&
                   (localFrameName is null && other.localFrameName is null || localFrameName == other.localFrameName || localFrameName is null && other.localFrameName == "" || localFrameName == "" && other.localFrameName is null) &&
                   ragdollBoneIndex.Equals(other.ragdollBoneIndex) &&
                   animationBoneIndex.Equals(other.animationBoneIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(handleOut);
            hashcode.Add(localFrameName);
            hashcode.Add(ragdollBoneIndex);
            hashcode.Add(animationBoneIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

