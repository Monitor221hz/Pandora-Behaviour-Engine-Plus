using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbAttachmentSetup Signatire: 0x774632b size: 48 flags: FLAGS_NONE

    // blendInTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // moveAttacherFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // gain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // extrapolationTimeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // fixUpGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // maxLinearDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // maxAngularDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // attachmentType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 44 flags: FLAGS_NONE enum: AttachmentType
    public partial class hkbAttachmentSetup : hkReferencedObject, IEquatable<hkbAttachmentSetup?>
    {
        public float blendInTime { set; get; }
        public float moveAttacherFraction { set; get; }
        public float gain { set; get; }
        public float extrapolationTimeStep { set; get; }
        public float fixUpGain { set; get; }
        public float maxLinearDistance { set; get; }
        public float maxAngularDistance { set; get; }
        public sbyte attachmentType { set; get; }

        public override uint Signature { set; get; } = 0x774632b;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            blendInTime = br.ReadSingle();
            moveAttacherFraction = br.ReadSingle();
            gain = br.ReadSingle();
            extrapolationTimeStep = br.ReadSingle();
            fixUpGain = br.ReadSingle();
            maxLinearDistance = br.ReadSingle();
            maxAngularDistance = br.ReadSingle();
            attachmentType = br.ReadSByte();
            br.Position += 3;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(blendInTime);
            bw.WriteSingle(moveAttacherFraction);
            bw.WriteSingle(gain);
            bw.WriteSingle(extrapolationTimeStep);
            bw.WriteSingle(fixUpGain);
            bw.WriteSingle(maxLinearDistance);
            bw.WriteSingle(maxAngularDistance);
            bw.WriteSByte(attachmentType);
            bw.Position += 3;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            blendInTime = xd.ReadSingle(xe, nameof(blendInTime));
            moveAttacherFraction = xd.ReadSingle(xe, nameof(moveAttacherFraction));
            gain = xd.ReadSingle(xe, nameof(gain));
            extrapolationTimeStep = xd.ReadSingle(xe, nameof(extrapolationTimeStep));
            fixUpGain = xd.ReadSingle(xe, nameof(fixUpGain));
            maxLinearDistance = xd.ReadSingle(xe, nameof(maxLinearDistance));
            maxAngularDistance = xd.ReadSingle(xe, nameof(maxAngularDistance));
            attachmentType = xd.ReadFlag<AttachmentType, sbyte>(xe, nameof(attachmentType));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(blendInTime), blendInTime);
            xs.WriteFloat(xe, nameof(moveAttacherFraction), moveAttacherFraction);
            xs.WriteFloat(xe, nameof(gain), gain);
            xs.WriteFloat(xe, nameof(extrapolationTimeStep), extrapolationTimeStep);
            xs.WriteFloat(xe, nameof(fixUpGain), fixUpGain);
            xs.WriteFloat(xe, nameof(maxLinearDistance), maxLinearDistance);
            xs.WriteFloat(xe, nameof(maxAngularDistance), maxAngularDistance);
            xs.WriteEnum<AttachmentType, sbyte>(xe, nameof(attachmentType), attachmentType);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbAttachmentSetup);
        }

        public bool Equals(hkbAttachmentSetup? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   blendInTime.Equals(other.blendInTime) &&
                   moveAttacherFraction.Equals(other.moveAttacherFraction) &&
                   gain.Equals(other.gain) &&
                   extrapolationTimeStep.Equals(other.extrapolationTimeStep) &&
                   fixUpGain.Equals(other.fixUpGain) &&
                   maxLinearDistance.Equals(other.maxLinearDistance) &&
                   maxAngularDistance.Equals(other.maxAngularDistance) &&
                   attachmentType.Equals(other.attachmentType) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(blendInTime);
            hashcode.Add(moveAttacherFraction);
            hashcode.Add(gain);
            hashcode.Add(extrapolationTimeStep);
            hashcode.Add(fixUpGain);
            hashcode.Add(maxLinearDistance);
            hashcode.Add(maxAngularDistance);
            hashcode.Add(attachmentType);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

