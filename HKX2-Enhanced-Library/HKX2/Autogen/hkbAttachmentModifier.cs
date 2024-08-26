using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbAttachmentModifier Signatire: 0xcc0aab32 size: 200 flags: FLAGS_NONE

    // sendToAttacherOnAttach class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // sendToAttacheeOnAttach class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // sendToAttacherOnDetach class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // sendToAttacheeOnDetach class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // attachmentSetup class: hkbAttachmentSetup Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // attacherHandle class: hkbHandle Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // attacheeHandle class: hkbHandle Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // attacheeLayer class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // attacheeRB class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 176 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // oldMotionType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 184 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // oldFilterInfo class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 188 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // attachment class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 192 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbAttachmentModifier : hkbModifier, IEquatable<hkbAttachmentModifier?>
    {
        public hkbEventProperty sendToAttacherOnAttach { set; get; } = new();
        public hkbEventProperty sendToAttacheeOnAttach { set; get; } = new();
        public hkbEventProperty sendToAttacherOnDetach { set; get; } = new();
        public hkbEventProperty sendToAttacheeOnDetach { set; get; } = new();
        public hkbAttachmentSetup? attachmentSetup { set; get; }
        public hkbHandle? attacherHandle { set; get; }
        public hkbHandle? attacheeHandle { set; get; }
        public int attacheeLayer { set; get; }
        private object? attacheeRB { set; get; }
        private byte oldMotionType { set; get; }
        private int oldFilterInfo { set; get; }
        private object? attachment { set; get; }

        public override uint Signature { set; get; } = 0xcc0aab32;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            sendToAttacherOnAttach.Read(des, br);
            sendToAttacheeOnAttach.Read(des, br);
            sendToAttacherOnDetach.Read(des, br);
            sendToAttacheeOnDetach.Read(des, br);
            attachmentSetup = des.ReadClassPointer<hkbAttachmentSetup>(br);
            attacherHandle = des.ReadClassPointer<hkbHandle>(br);
            attacheeHandle = des.ReadClassPointer<hkbHandle>(br);
            attacheeLayer = br.ReadInt32();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            oldMotionType = br.ReadByte();
            br.Position += 3;
            oldFilterInfo = br.ReadInt32();
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            sendToAttacherOnAttach.Write(s, bw);
            sendToAttacheeOnAttach.Write(s, bw);
            sendToAttacherOnDetach.Write(s, bw);
            sendToAttacheeOnDetach.Write(s, bw);
            s.WriteClassPointer(bw, attachmentSetup);
            s.WriteClassPointer(bw, attacherHandle);
            s.WriteClassPointer(bw, attacheeHandle);
            bw.WriteInt32(attacheeLayer);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            bw.WriteByte(oldMotionType);
            bw.Position += 3;
            bw.WriteInt32(oldFilterInfo);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            sendToAttacherOnAttach = xd.ReadClass<hkbEventProperty>(xe, nameof(sendToAttacherOnAttach));
            sendToAttacheeOnAttach = xd.ReadClass<hkbEventProperty>(xe, nameof(sendToAttacheeOnAttach));
            sendToAttacherOnDetach = xd.ReadClass<hkbEventProperty>(xe, nameof(sendToAttacherOnDetach));
            sendToAttacheeOnDetach = xd.ReadClass<hkbEventProperty>(xe, nameof(sendToAttacheeOnDetach));
            attachmentSetup = xd.ReadClassPointer<hkbAttachmentSetup>(this, xe, nameof(attachmentSetup));
            attacherHandle = xd.ReadClassPointer<hkbHandle>(this, xe, nameof(attacherHandle));
            attacheeHandle = xd.ReadClassPointer<hkbHandle>(this, xe, nameof(attacheeHandle));
            attacheeLayer = xd.ReadInt32(xe, nameof(attacheeLayer));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbEventProperty>(xe, nameof(sendToAttacherOnAttach), sendToAttacherOnAttach);
            xs.WriteClass<hkbEventProperty>(xe, nameof(sendToAttacheeOnAttach), sendToAttacheeOnAttach);
            xs.WriteClass<hkbEventProperty>(xe, nameof(sendToAttacherOnDetach), sendToAttacherOnDetach);
            xs.WriteClass<hkbEventProperty>(xe, nameof(sendToAttacheeOnDetach), sendToAttacheeOnDetach);
            xs.WriteClassPointer(xe, nameof(attachmentSetup), attachmentSetup);
            xs.WriteClassPointer(xe, nameof(attacherHandle), attacherHandle);
            xs.WriteClassPointer(xe, nameof(attacheeHandle), attacheeHandle);
            xs.WriteNumber(xe, nameof(attacheeLayer), attacheeLayer);
            xs.WriteSerializeIgnored(xe, nameof(attacheeRB));
            xs.WriteSerializeIgnored(xe, nameof(oldMotionType));
            xs.WriteSerializeIgnored(xe, nameof(oldFilterInfo));
            xs.WriteSerializeIgnored(xe, nameof(attachment));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbAttachmentModifier);
        }

        public bool Equals(hkbAttachmentModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((sendToAttacherOnAttach is null && other.sendToAttacherOnAttach is null) || (sendToAttacherOnAttach is not null && other.sendToAttacherOnAttach is not null && sendToAttacherOnAttach.Equals((IHavokObject)other.sendToAttacherOnAttach))) &&
                   ((sendToAttacheeOnAttach is null && other.sendToAttacheeOnAttach is null) || (sendToAttacheeOnAttach is not null && other.sendToAttacheeOnAttach is not null && sendToAttacheeOnAttach.Equals((IHavokObject)other.sendToAttacheeOnAttach))) &&
                   ((sendToAttacherOnDetach is null && other.sendToAttacherOnDetach is null) || (sendToAttacherOnDetach is not null && other.sendToAttacherOnDetach is not null && sendToAttacherOnDetach.Equals((IHavokObject)other.sendToAttacherOnDetach))) &&
                   ((sendToAttacheeOnDetach is null && other.sendToAttacheeOnDetach is null) || (sendToAttacheeOnDetach is not null && other.sendToAttacheeOnDetach is not null && sendToAttacheeOnDetach.Equals((IHavokObject)other.sendToAttacheeOnDetach))) &&
                   ((attachmentSetup is null && other.attachmentSetup is null) || (attachmentSetup is not null && other.attachmentSetup is not null && attachmentSetup.Equals((IHavokObject)other.attachmentSetup))) &&
                   ((attacherHandle is null && other.attacherHandle is null) || (attacherHandle is not null && other.attacherHandle is not null && attacherHandle.Equals((IHavokObject)other.attacherHandle))) &&
                   ((attacheeHandle is null && other.attacheeHandle is null) || (attacheeHandle is not null && other.attacheeHandle is not null && attacheeHandle.Equals((IHavokObject)other.attacheeHandle))) &&
                   attacheeLayer.Equals(other.attacheeLayer) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(sendToAttacherOnAttach);
            hashcode.Add(sendToAttacheeOnAttach);
            hashcode.Add(sendToAttacherOnDetach);
            hashcode.Add(sendToAttacheeOnDetach);
            hashcode.Add(attachmentSetup);
            hashcode.Add(attacherHandle);
            hashcode.Add(attacheeHandle);
            hashcode.Add(attacheeLayer);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

