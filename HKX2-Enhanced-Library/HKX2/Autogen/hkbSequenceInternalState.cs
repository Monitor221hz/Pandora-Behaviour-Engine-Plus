using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSequenceInternalState Signatire: 0x419b9a05 size: 88 flags: FLAGS_NONE

    // nextSampleEvents class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // nextSampleReals class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // nextSampleBools class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // nextSampleInts class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // isEnabled class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    public partial class hkbSequenceInternalState : hkReferencedObject, IEquatable<hkbSequenceInternalState?>
    {
        public IList<int> nextSampleEvents { set; get; } = Array.Empty<int>();
        public IList<int> nextSampleReals { set; get; } = Array.Empty<int>();
        public IList<int> nextSampleBools { set; get; } = Array.Empty<int>();
        public IList<int> nextSampleInts { set; get; } = Array.Empty<int>();
        public float time { set; get; }
        public bool isEnabled { set; get; }

        public override uint Signature { set; get; } = 0x419b9a05;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            nextSampleEvents = des.ReadInt32Array(br);
            nextSampleReals = des.ReadInt32Array(br);
            nextSampleBools = des.ReadInt32Array(br);
            nextSampleInts = des.ReadInt32Array(br);
            time = br.ReadSingle();
            isEnabled = br.ReadBoolean();
            br.Position += 3;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteInt32Array(bw, nextSampleEvents);
            s.WriteInt32Array(bw, nextSampleReals);
            s.WriteInt32Array(bw, nextSampleBools);
            s.WriteInt32Array(bw, nextSampleInts);
            bw.WriteSingle(time);
            bw.WriteBoolean(isEnabled);
            bw.Position += 3;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            nextSampleEvents = xd.ReadInt32Array(xe, nameof(nextSampleEvents));
            nextSampleReals = xd.ReadInt32Array(xe, nameof(nextSampleReals));
            nextSampleBools = xd.ReadInt32Array(xe, nameof(nextSampleBools));
            nextSampleInts = xd.ReadInt32Array(xe, nameof(nextSampleInts));
            time = xd.ReadSingle(xe, nameof(time));
            isEnabled = xd.ReadBoolean(xe, nameof(isEnabled));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(nextSampleEvents), nextSampleEvents);
            xs.WriteNumberArray(xe, nameof(nextSampleReals), nextSampleReals);
            xs.WriteNumberArray(xe, nameof(nextSampleBools), nextSampleBools);
            xs.WriteNumberArray(xe, nameof(nextSampleInts), nextSampleInts);
            xs.WriteFloat(xe, nameof(time), time);
            xs.WriteBoolean(xe, nameof(isEnabled), isEnabled);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSequenceInternalState);
        }

        public bool Equals(hkbSequenceInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   nextSampleEvents.SequenceEqual(other.nextSampleEvents) &&
                   nextSampleReals.SequenceEqual(other.nextSampleReals) &&
                   nextSampleBools.SequenceEqual(other.nextSampleBools) &&
                   nextSampleInts.SequenceEqual(other.nextSampleInts) &&
                   time.Equals(other.time) &&
                   isEnabled.Equals(other.isEnabled) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(nextSampleEvents.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(nextSampleReals.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(nextSampleBools.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(nextSampleInts.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(time);
            hashcode.Add(isEnabled);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

