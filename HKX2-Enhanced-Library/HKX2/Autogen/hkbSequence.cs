using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSequence Signatire: 0x43182ca3 size: 248 flags: FLAGS_NONE

    // eventSequencedData class: hkbEventSequencedData Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // realVariableSequencedData class: hkbRealVariableSequencedData Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // boolVariableSequencedData class: hkbBoolVariableSequencedData Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // intVariableSequencedData class: hkbIntVariableSequencedData Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // enableEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // disableEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 148 flags: FLAGS_NONE enum: 
    // stringData class: hkbSequenceStringData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // variableIdMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 160 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // eventIdMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 168 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nextSampleEvents class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 176 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nextSampleReals class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 192 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nextSampleBools class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 208 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nextSampleInts class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 240 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // isEnabled class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 244 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbSequence : hkbModifier, IEquatable<hkbSequence?>
    {
        public IList<hkbEventSequencedData> eventSequencedData { set; get; } = Array.Empty<hkbEventSequencedData>();
        public IList<hkbRealVariableSequencedData> realVariableSequencedData { set; get; } = Array.Empty<hkbRealVariableSequencedData>();
        public IList<hkbBoolVariableSequencedData> boolVariableSequencedData { set; get; } = Array.Empty<hkbBoolVariableSequencedData>();
        public IList<hkbIntVariableSequencedData> intVariableSequencedData { set; get; } = Array.Empty<hkbIntVariableSequencedData>();
        public int enableEventId { set; get; }
        public int disableEventId { set; get; }
        public hkbSequenceStringData? stringData { set; get; }
        private object? variableIdMap { set; get; }
        private object? eventIdMap { set; get; }
        public IList<object> nextSampleEvents { set; get; } = Array.Empty<object>();
        public IList<object> nextSampleReals { set; get; } = Array.Empty<object>();
        public IList<object> nextSampleBools { set; get; } = Array.Empty<object>();
        public IList<object> nextSampleInts { set; get; } = Array.Empty<object>();
        private float time { set; get; }
        private bool isEnabled { set; get; }

        public override uint Signature { set; get; } = 0x43182ca3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            eventSequencedData = des.ReadClassPointerArray<hkbEventSequencedData>(br);
            realVariableSequencedData = des.ReadClassPointerArray<hkbRealVariableSequencedData>(br);
            boolVariableSequencedData = des.ReadClassPointerArray<hkbBoolVariableSequencedData>(br);
            intVariableSequencedData = des.ReadClassPointerArray<hkbIntVariableSequencedData>(br);
            enableEventId = br.ReadInt32();
            disableEventId = br.ReadInt32();
            stringData = des.ReadClassPointer<hkbSequenceStringData>(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            time = br.ReadSingle();
            isEnabled = br.ReadBoolean();
            br.Position += 3;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, eventSequencedData);
            s.WriteClassPointerArray(bw, realVariableSequencedData);
            s.WriteClassPointerArray(bw, boolVariableSequencedData);
            s.WriteClassPointerArray(bw, intVariableSequencedData);
            bw.WriteInt32(enableEventId);
            bw.WriteInt32(disableEventId);
            s.WriteClassPointer(bw, stringData);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            bw.WriteSingle(time);
            bw.WriteBoolean(isEnabled);
            bw.Position += 3;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            eventSequencedData = xd.ReadClassPointerArray<hkbEventSequencedData>(this, xe, nameof(eventSequencedData));
            realVariableSequencedData = xd.ReadClassPointerArray<hkbRealVariableSequencedData>(this, xe, nameof(realVariableSequencedData));
            boolVariableSequencedData = xd.ReadClassPointerArray<hkbBoolVariableSequencedData>(this, xe, nameof(boolVariableSequencedData));
            intVariableSequencedData = xd.ReadClassPointerArray<hkbIntVariableSequencedData>(this, xe, nameof(intVariableSequencedData));
            enableEventId = xd.ReadInt32(xe, nameof(enableEventId));
            disableEventId = xd.ReadInt32(xe, nameof(disableEventId));
            stringData = xd.ReadClassPointer<hkbSequenceStringData>(this, xe, nameof(stringData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(eventSequencedData), eventSequencedData!);
            xs.WriteClassPointerArray(xe, nameof(realVariableSequencedData), realVariableSequencedData!);
            xs.WriteClassPointerArray(xe, nameof(boolVariableSequencedData), boolVariableSequencedData!);
            xs.WriteClassPointerArray(xe, nameof(intVariableSequencedData), intVariableSequencedData!);
            xs.WriteNumber(xe, nameof(enableEventId), enableEventId);
            xs.WriteNumber(xe, nameof(disableEventId), disableEventId);
            xs.WriteClassPointer(xe, nameof(stringData), stringData);
            xs.WriteSerializeIgnored(xe, nameof(variableIdMap));
            xs.WriteSerializeIgnored(xe, nameof(eventIdMap));
            xs.WriteSerializeIgnored(xe, nameof(nextSampleEvents));
            xs.WriteSerializeIgnored(xe, nameof(nextSampleReals));
            xs.WriteSerializeIgnored(xe, nameof(nextSampleBools));
            xs.WriteSerializeIgnored(xe, nameof(nextSampleInts));
            xs.WriteSerializeIgnored(xe, nameof(time));
            xs.WriteSerializeIgnored(xe, nameof(isEnabled));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSequence);
        }

        public bool Equals(hkbSequence? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   eventSequencedData.SequenceEqual(other.eventSequencedData) &&
                   realVariableSequencedData.SequenceEqual(other.realVariableSequencedData) &&
                   boolVariableSequencedData.SequenceEqual(other.boolVariableSequencedData) &&
                   intVariableSequencedData.SequenceEqual(other.intVariableSequencedData) &&
                   enableEventId.Equals(other.enableEventId) &&
                   disableEventId.Equals(other.disableEventId) &&
                   ((stringData is null && other.stringData is null) || (stringData is not null && other.stringData is not null && stringData.Equals((IHavokObject)other.stringData))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(eventSequencedData.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(realVariableSequencedData.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(boolVariableSequencedData.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(intVariableSequencedData.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(enableEventId);
            hashcode.Add(disableEventId);
            hashcode.Add(stringData);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

