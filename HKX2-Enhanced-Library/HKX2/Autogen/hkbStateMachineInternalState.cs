using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineInternalState Signatire: 0xbd1a7502 size: 104 flags: FLAGS_NONE

    // activeTransitions class: hkbStateMachineActiveTransitionInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // transitionFlags class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // wildcardTransitionFlags class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // delayedTransitions class: hkbStateMachineDelayedTransitionInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // timeInState class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // lastLocalTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // currentStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // previousStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // nextStartStateIndexOverride class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // stateOrTransitionChanged class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // echoNextUpdate class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 101 flags: FLAGS_NONE enum: 
    public partial class hkbStateMachineInternalState : hkReferencedObject, IEquatable<hkbStateMachineInternalState?>
    {
        public IList<hkbStateMachineActiveTransitionInfo> activeTransitions { set; get; } = Array.Empty<hkbStateMachineActiveTransitionInfo>();
        public IList<byte> transitionFlags { set; get; } = Array.Empty<byte>();
        public IList<byte> wildcardTransitionFlags { set; get; } = Array.Empty<byte>();
        public IList<hkbStateMachineDelayedTransitionInfo> delayedTransitions { set; get; } = Array.Empty<hkbStateMachineDelayedTransitionInfo>();
        public float timeInState { set; get; }
        public float lastLocalTime { set; get; }
        public int currentStateId { set; get; }
        public int previousStateId { set; get; }
        public int nextStartStateIndexOverride { set; get; }
        public bool stateOrTransitionChanged { set; get; }
        public bool echoNextUpdate { set; get; }

        public override uint Signature { set; get; } = 0xbd1a7502;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            activeTransitions = des.ReadClassArray<hkbStateMachineActiveTransitionInfo>(br);
            transitionFlags = des.ReadByteArray(br);
            wildcardTransitionFlags = des.ReadByteArray(br);
            delayedTransitions = des.ReadClassArray<hkbStateMachineDelayedTransitionInfo>(br);
            timeInState = br.ReadSingle();
            lastLocalTime = br.ReadSingle();
            currentStateId = br.ReadInt32();
            previousStateId = br.ReadInt32();
            nextStartStateIndexOverride = br.ReadInt32();
            stateOrTransitionChanged = br.ReadBoolean();
            echoNextUpdate = br.ReadBoolean();
            br.Position += 2;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, activeTransitions);
            s.WriteByteArray(bw, transitionFlags);
            s.WriteByteArray(bw, wildcardTransitionFlags);
            s.WriteClassArray(bw, delayedTransitions);
            bw.WriteSingle(timeInState);
            bw.WriteSingle(lastLocalTime);
            bw.WriteInt32(currentStateId);
            bw.WriteInt32(previousStateId);
            bw.WriteInt32(nextStartStateIndexOverride);
            bw.WriteBoolean(stateOrTransitionChanged);
            bw.WriteBoolean(echoNextUpdate);
            bw.Position += 2;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            activeTransitions = xd.ReadClassArray<hkbStateMachineActiveTransitionInfo>(xe, nameof(activeTransitions));
            transitionFlags = xd.ReadByteArray(xe, nameof(transitionFlags));
            wildcardTransitionFlags = xd.ReadByteArray(xe, nameof(wildcardTransitionFlags));
            delayedTransitions = xd.ReadClassArray<hkbStateMachineDelayedTransitionInfo>(xe, nameof(delayedTransitions));
            timeInState = xd.ReadSingle(xe, nameof(timeInState));
            lastLocalTime = xd.ReadSingle(xe, nameof(lastLocalTime));
            currentStateId = xd.ReadInt32(xe, nameof(currentStateId));
            previousStateId = xd.ReadInt32(xe, nameof(previousStateId));
            nextStartStateIndexOverride = xd.ReadInt32(xe, nameof(nextStartStateIndexOverride));
            stateOrTransitionChanged = xd.ReadBoolean(xe, nameof(stateOrTransitionChanged));
            echoNextUpdate = xd.ReadBoolean(xe, nameof(echoNextUpdate));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(activeTransitions), activeTransitions);
            xs.WriteNumberArray(xe, nameof(transitionFlags), transitionFlags);
            xs.WriteNumberArray(xe, nameof(wildcardTransitionFlags), wildcardTransitionFlags);
            xs.WriteClassArray(xe, nameof(delayedTransitions), delayedTransitions);
            xs.WriteFloat(xe, nameof(timeInState), timeInState);
            xs.WriteFloat(xe, nameof(lastLocalTime), lastLocalTime);
            xs.WriteNumber(xe, nameof(currentStateId), currentStateId);
            xs.WriteNumber(xe, nameof(previousStateId), previousStateId);
            xs.WriteNumber(xe, nameof(nextStartStateIndexOverride), nextStartStateIndexOverride);
            xs.WriteBoolean(xe, nameof(stateOrTransitionChanged), stateOrTransitionChanged);
            xs.WriteBoolean(xe, nameof(echoNextUpdate), echoNextUpdate);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineInternalState);
        }

        public bool Equals(hkbStateMachineInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   activeTransitions.SequenceEqual(other.activeTransitions) &&
                   transitionFlags.SequenceEqual(other.transitionFlags) &&
                   wildcardTransitionFlags.SequenceEqual(other.wildcardTransitionFlags) &&
                   delayedTransitions.SequenceEqual(other.delayedTransitions) &&
                   timeInState.Equals(other.timeInState) &&
                   lastLocalTime.Equals(other.lastLocalTime) &&
                   currentStateId.Equals(other.currentStateId) &&
                   previousStateId.Equals(other.previousStateId) &&
                   nextStartStateIndexOverride.Equals(other.nextStartStateIndexOverride) &&
                   stateOrTransitionChanged.Equals(other.stateOrTransitionChanged) &&
                   echoNextUpdate.Equals(other.echoNextUpdate) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(activeTransitions.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(transitionFlags.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(wildcardTransitionFlags.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(delayedTransitions.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(timeInState);
            hashcode.Add(lastLocalTime);
            hashcode.Add(currentStateId);
            hashcode.Add(previousStateId);
            hashcode.Add(nextStartStateIndexOverride);
            hashcode.Add(stateOrTransitionChanged);
            hashcode.Add(echoNextUpdate);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

