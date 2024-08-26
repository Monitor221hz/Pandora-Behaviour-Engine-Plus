using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBlendingTransitionEffect Signatire: 0xfd8584fe size: 144 flags: FLAGS_NONE

    // duration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // toGeneratorStartTimeFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // flags class:  Type.TYPE_FLAGS Type.TYPE_UINT16 arrSize: 0 offset: 88 flags: FLAGS_NONE enum: FlagBits
    // endMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 90 flags: FLAGS_NONE enum: EndMode
    // blendCurve class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 91 flags: FLAGS_NONE enum: BlendCurve
    // fromGenerator class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // toGenerator class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // characterPoseAtBeginningOfTransition class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timeRemaining class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 128 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timeInTransition class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 132 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // applySelfTransition class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 136 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // initializeCharacterPose class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 137 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbBlendingTransitionEffect : hkbTransitionEffect, IEquatable<hkbBlendingTransitionEffect?>
    {
        public static hkbBlendingTransitionEffect GetDefault() => new()
        {
            selfTransitionMode = 0,
            eventMode = 0, 
            flags = 1,
            endMode = 0,
            blendCurve = 0
        };
        public void SetDefault()
        {
            selfTransitionMode = 0;
            eventMode = 0;
            flags = 2;
            endMode = 0;
            blendCurve = 0;
		}
        public float duration { set; get; }
        public float toGeneratorStartTimeFraction { set; get; }
        public ushort flags { set; get; }
        public sbyte endMode { set; get; }
        public sbyte blendCurve { set; get; }
        private object? fromGenerator { set; get; }
        private object? toGenerator { set; get; }
        public IList<object> characterPoseAtBeginningOfTransition { set; get; } = Array.Empty<object>();
        private float timeRemaining { set; get; }
        private float timeInTransition { set; get; }
        private bool applySelfTransition { set; get; }
        private bool initializeCharacterPose { set; get; }

        public override uint Signature { set; get; } = 0xfd8584fe;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            duration = br.ReadSingle();
            toGeneratorStartTimeFraction = br.ReadSingle();
            flags = br.ReadUInt16();
            endMode = br.ReadSByte();
            blendCurve = br.ReadSByte();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyArray(br);
            timeRemaining = br.ReadSingle();
            timeInTransition = br.ReadSingle();
            applySelfTransition = br.ReadBoolean();
            initializeCharacterPose = br.ReadBoolean();
            br.Position += 6;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(duration);
            bw.WriteSingle(toGeneratorStartTimeFraction);
            bw.WriteUInt16(flags);
            bw.WriteSByte(endMode);
            bw.WriteSByte(blendCurve);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidArray(bw);
            bw.WriteSingle(timeRemaining);
            bw.WriteSingle(timeInTransition);
            bw.WriteBoolean(applySelfTransition);
            bw.WriteBoolean(initializeCharacterPose);
            bw.Position += 6;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            duration = xd.ReadSingle(xe, nameof(duration));
            toGeneratorStartTimeFraction = xd.ReadSingle(xe, nameof(toGeneratorStartTimeFraction));
            flags = xd.ReadFlag<FlagBits, ushort>(xe, nameof(flags));
            endMode = xd.ReadFlag<EndMode, sbyte>(xe, nameof(endMode));
            blendCurve = xd.ReadFlag<BlendCurve, sbyte>(xe, nameof(blendCurve));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(duration), duration);
            xs.WriteFloat(xe, nameof(toGeneratorStartTimeFraction), toGeneratorStartTimeFraction);
            xs.WriteFlag<FlagBits, ushort>(xe, nameof(flags), flags);
            xs.WriteEnum<EndMode, sbyte>(xe, nameof(endMode), endMode);
            xs.WriteEnum<BlendCurve, sbyte>(xe, nameof(blendCurve), blendCurve);
            xs.WriteSerializeIgnored(xe, nameof(fromGenerator));
            xs.WriteSerializeIgnored(xe, nameof(toGenerator));
            xs.WriteSerializeIgnored(xe, nameof(characterPoseAtBeginningOfTransition));
            xs.WriteSerializeIgnored(xe, nameof(timeRemaining));
            xs.WriteSerializeIgnored(xe, nameof(timeInTransition));
            xs.WriteSerializeIgnored(xe, nameof(applySelfTransition));
            xs.WriteSerializeIgnored(xe, nameof(initializeCharacterPose));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBlendingTransitionEffect);
        }

        public bool Equals(hkbBlendingTransitionEffect? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   duration.Equals(other.duration) &&
                   toGeneratorStartTimeFraction.Equals(other.toGeneratorStartTimeFraction) &&
                   flags.Equals(other.flags) &&
                   endMode.Equals(other.endMode) &&
                   blendCurve.Equals(other.blendCurve) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(duration);
            hashcode.Add(toGeneratorStartTimeFraction);
            hashcode.Add(flags);
            hashcode.Add(endMode);
            hashcode.Add(blendCurve);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

