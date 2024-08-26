using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSTimerModifier Signatire: 0x531f3292 size: 112 flags: FLAGS_NONE

    // alarmTimeSeconds class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // alarmEvent class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // resetAlarm class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // secondsElapsed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSTimerModifier : hkbModifier, IEquatable<BSTimerModifier?>
    {
        public float alarmTimeSeconds { set; get; }
        public hkbEventProperty alarmEvent { set; get; } = new();
        public bool resetAlarm { set; get; }
        private float secondsElapsed { set; get; }

        public override uint Signature { set; get; } = 0x531f3292;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            alarmTimeSeconds = br.ReadSingle();
            br.Position += 4;
            alarmEvent.Read(des, br);
            resetAlarm = br.ReadBoolean();
            br.Position += 3;
            secondsElapsed = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(alarmTimeSeconds);
            bw.Position += 4;
            alarmEvent.Write(s, bw);
            bw.WriteBoolean(resetAlarm);
            bw.Position += 3;
            bw.WriteSingle(secondsElapsed);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            alarmTimeSeconds = xd.ReadSingle(xe, nameof(alarmTimeSeconds));
            alarmEvent = xd.ReadClass<hkbEventProperty>(xe, nameof(alarmEvent));
            resetAlarm = xd.ReadBoolean(xe, nameof(resetAlarm));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(alarmTimeSeconds), alarmTimeSeconds);
            xs.WriteClass<hkbEventProperty>(xe, nameof(alarmEvent), alarmEvent);
            xs.WriteBoolean(xe, nameof(resetAlarm), resetAlarm);
            xs.WriteSerializeIgnored(xe, nameof(secondsElapsed));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSTimerModifier);
        }

        public bool Equals(BSTimerModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   alarmTimeSeconds.Equals(other.alarmTimeSeconds) &&
                   ((alarmEvent is null && other.alarmEvent is null) || (alarmEvent is not null && other.alarmEvent is not null && alarmEvent.Equals((IHavokObject)other.alarmEvent))) &&
                   resetAlarm.Equals(other.resetAlarm) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(alarmTimeSeconds);
            hashcode.Add(alarmEvent);
            hashcode.Add(resetAlarm);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

