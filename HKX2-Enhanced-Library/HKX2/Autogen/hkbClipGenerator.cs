using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbClipGenerator Signatire: 0x333b85b9 size: 272 flags: FLAGS_NONE

    // animationName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // triggers class: hkbClipTriggerArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // cropStartAmountLocalTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // cropEndAmountLocalTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // startTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // playbackSpeed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // enforcedDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // userControlledTimeFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // animationBindingIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // mode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 114 flags: FLAGS_NONE enum: PlaybackMode
    // flags class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 115 flags: FLAGS_NONE enum: 
    // animDatas class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // animationControl class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 136 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // originalTriggers class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // mapperData class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 152 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // binding class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 160 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // mirroredAnimation class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 168 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // extractedMotion class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 176 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // echos class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // localTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 240 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 244 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // previousUserControlledTimeFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 248 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bufferSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 252 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // echoBufferSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 256 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // atEnd class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 260 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // ignoreStartTime class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 261 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pingPongBackward class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 262 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbClipGenerator : hkbGenerator, IEquatable<hkbClipGenerator?>
    {
        public static hkbClipGenerator GetDefault() => new()
        {
			cropStartAmountLocalTime = 0.0f,
			cropEndAmountLocalTime = 0.0f,
			startTime = 0.0f,
			playbackSpeed = 1.0f,
			enforcedDuration = 0.0f,
			userControlledTimeFraction = 0.0f,
			animationBindingIndex = -1,
			flags = 0,
			userData = 0
		};
        public void SetDefault()
        {
			cropStartAmountLocalTime = 0.0f;
			cropEndAmountLocalTime = 0.0f;
			startTime = 0.0f;
            playbackSpeed = 1.0f;
			enforcedDuration = 0.0f;
			userControlledTimeFraction = 0.0f;
			animationBindingIndex = -1;
			flags = 0;
			userData = 0;
		}
        public string animationName { set; get; } = "";
        public hkbClipTriggerArray? triggers { set; get; }
        public float cropStartAmountLocalTime { set; get; }
        public float cropEndAmountLocalTime { set; get; }
        public float startTime { set; get; }
        public float playbackSpeed { set; get; }
        public float enforcedDuration { set; get; }
        public float userControlledTimeFraction { set; get; }
        public short animationBindingIndex { set; get; }
        public sbyte mode { set; get; }
        public sbyte flags { set; get; }
        public IList<object> animDatas { set; get; } = Array.Empty<object>();
        private object? animationControl { set; get; }
        private object? originalTriggers { set; get; }
        private object? mapperData { set; get; }
        private object? binding { set; get; }
        private object? mirroredAnimation { set; get; }
        private Matrix4x4 extractedMotion { set; get; }
        public IList<object> echos { set; get; } = Array.Empty<object>();
        private float localTime { set; get; }
        private float time { set; get; }
        private float previousUserControlledTimeFraction { set; get; }
        private int bufferSize { set; get; }
        private int echoBufferSize { set; get; }
        private bool atEnd { set; get; }
        private bool ignoreStartTime { set; get; }
        private bool pingPongBackward { set; get; }

        public override uint Signature { set; get; } = 0x333b85b9;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            animationName = des.ReadStringPointer(br);
            triggers = des.ReadClassPointer<hkbClipTriggerArray>(br);
            cropStartAmountLocalTime = br.ReadSingle();
            cropEndAmountLocalTime = br.ReadSingle();
            startTime = br.ReadSingle();
            playbackSpeed = br.ReadSingle();
            enforcedDuration = br.ReadSingle();
            userControlledTimeFraction = br.ReadSingle();
            animationBindingIndex = br.ReadInt16();
            mode = br.ReadSByte();
            flags = br.ReadSByte();
            br.Position += 4;
            des.ReadEmptyArray(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            extractedMotion = des.ReadQSTransform(br);
            des.ReadEmptyArray(br);
            localTime = br.ReadSingle();
            time = br.ReadSingle();
            previousUserControlledTimeFraction = br.ReadSingle();
            bufferSize = br.ReadInt32();
            echoBufferSize = br.ReadInt32();
            atEnd = br.ReadBoolean();
            ignoreStartTime = br.ReadBoolean();
            pingPongBackward = br.ReadBoolean();
            br.Position += 9;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, animationName);
            s.WriteClassPointer(bw, triggers);
            bw.WriteSingle(cropStartAmountLocalTime);
            bw.WriteSingle(cropEndAmountLocalTime);
            bw.WriteSingle(startTime);
            bw.WriteSingle(playbackSpeed);
            bw.WriteSingle(enforcedDuration);
            bw.WriteSingle(userControlledTimeFraction);
            bw.WriteInt16(animationBindingIndex);
            bw.WriteSByte(mode);
            bw.WriteSByte(flags);
            bw.Position += 4;
            s.WriteVoidArray(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteQSTransform(bw, extractedMotion);
            s.WriteVoidArray(bw);
            bw.WriteSingle(localTime);
            bw.WriteSingle(time);
            bw.WriteSingle(previousUserControlledTimeFraction);
            bw.WriteInt32(bufferSize);
            bw.WriteInt32(echoBufferSize);
            bw.WriteBoolean(atEnd);
            bw.WriteBoolean(ignoreStartTime);
            bw.WriteBoolean(pingPongBackward);
            bw.Position += 9;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            animationName = xd.ReadString(xe, nameof(animationName));
            triggers = xd.ReadClassPointer<hkbClipTriggerArray>(this, xe, nameof(triggers));
            cropStartAmountLocalTime = xd.ReadSingle(xe, nameof(cropStartAmountLocalTime));
            cropEndAmountLocalTime = xd.ReadSingle(xe, nameof(cropEndAmountLocalTime));
            startTime = xd.ReadSingle(xe, nameof(startTime));
            playbackSpeed = xd.ReadSingle(xe, nameof(playbackSpeed));
            enforcedDuration = xd.ReadSingle(xe, nameof(enforcedDuration));
            userControlledTimeFraction = xd.ReadSingle(xe, nameof(userControlledTimeFraction));
            animationBindingIndex = xd.ReadInt16(xe, nameof(animationBindingIndex));
            mode = xd.ReadFlag<PlaybackMode, sbyte>(xe, nameof(mode));
            flags = xd.ReadSByte(xe, nameof(flags));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(animationName), animationName);
            xs.WriteClassPointer(xe, nameof(triggers), triggers);
            xs.WriteFloat(xe, nameof(cropStartAmountLocalTime), cropStartAmountLocalTime);
            xs.WriteFloat(xe, nameof(cropEndAmountLocalTime), cropEndAmountLocalTime);
            xs.WriteFloat(xe, nameof(startTime), startTime);
            xs.WriteFloat(xe, nameof(playbackSpeed), playbackSpeed);
            xs.WriteFloat(xe, nameof(enforcedDuration), enforcedDuration);
            xs.WriteFloat(xe, nameof(userControlledTimeFraction), userControlledTimeFraction);
            xs.WriteNumber(xe, nameof(animationBindingIndex), animationBindingIndex);
            xs.WriteEnum<PlaybackMode, sbyte>(xe, nameof(mode), mode);
            xs.WriteNumber(xe, nameof(flags), flags);
            xs.WriteSerializeIgnored(xe, nameof(animDatas));
            xs.WriteSerializeIgnored(xe, nameof(animationControl));
            xs.WriteSerializeIgnored(xe, nameof(originalTriggers));
            xs.WriteSerializeIgnored(xe, nameof(mapperData));
            xs.WriteSerializeIgnored(xe, nameof(binding));
            xs.WriteSerializeIgnored(xe, nameof(mirroredAnimation));
            xs.WriteSerializeIgnored(xe, nameof(extractedMotion));
            xs.WriteSerializeIgnored(xe, nameof(echos));
            xs.WriteSerializeIgnored(xe, nameof(localTime));
            xs.WriteSerializeIgnored(xe, nameof(time));
            xs.WriteSerializeIgnored(xe, nameof(previousUserControlledTimeFraction));
            xs.WriteSerializeIgnored(xe, nameof(bufferSize));
            xs.WriteSerializeIgnored(xe, nameof(echoBufferSize));
            xs.WriteSerializeIgnored(xe, nameof(atEnd));
            xs.WriteSerializeIgnored(xe, nameof(ignoreStartTime));
            xs.WriteSerializeIgnored(xe, nameof(pingPongBackward));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbClipGenerator);
        }

        public bool Equals(hkbClipGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (animationName is null && other.animationName is null || animationName == other.animationName || animationName is null && other.animationName == "" || animationName == "" && other.animationName is null) &&
                   ((triggers is null && other.triggers is null) || (triggers is not null && other.triggers is not null && triggers.Equals((IHavokObject)other.triggers))) &&
                   cropStartAmountLocalTime.Equals(other.cropStartAmountLocalTime) &&
                   cropEndAmountLocalTime.Equals(other.cropEndAmountLocalTime) &&
                   startTime.Equals(other.startTime) &&
                   playbackSpeed.Equals(other.playbackSpeed) &&
                   enforcedDuration.Equals(other.enforcedDuration) &&
                   userControlledTimeFraction.Equals(other.userControlledTimeFraction) &&
                   animationBindingIndex.Equals(other.animationBindingIndex) &&
                   mode.Equals(other.mode) &&
                   flags.Equals(other.flags) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(animationName);
            hashcode.Add(triggers);
            hashcode.Add(cropStartAmountLocalTime);
            hashcode.Add(cropEndAmountLocalTime);
            hashcode.Add(startTime);
            hashcode.Add(playbackSpeed);
            hashcode.Add(enforcedDuration);
            hashcode.Add(userControlledTimeFraction);
            hashcode.Add(animationBindingIndex);
            hashcode.Add(mode);
            hashcode.Add(flags);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

