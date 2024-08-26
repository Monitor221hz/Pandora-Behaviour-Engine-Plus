using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachine Signatire: 0x816c1dcb size: 264 flags: FLAGS_NONE

    // eventToSendWhenStateOrTransitionChanges class: hkbEvent Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // startStateChooser class: hkbStateChooser Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // startStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // returnToPreviousStateEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // randomTransitionEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // transitionToNextHigherStateEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 116 flags: FLAGS_NONE enum: 
    // transitionToNextLowerStateEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // syncVariableIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 124 flags: FLAGS_NONE enum: 
    // currentStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 128 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // wrapAroundStateId class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 132 flags: FLAGS_NONE enum: 
    // maxSimultaneousTransitions class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 133 flags: FLAGS_NONE enum: 
    // startStateMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 134 flags: FLAGS_NONE enum: StartStateMode
    // selfTransitionMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 135 flags: FLAGS_NONE enum: StateMachineSelfTransitionMode
    // isActive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 136 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // states class: hkbStateMachineStateInfo Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // wildcardTransitions class: hkbStateMachineTransitionInfoArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // stateIdToIndexMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 168 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // activeTransitions class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 176 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // transitionFlags class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 192 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // wildcardTransitionFlags class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 208 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // delayedTransitions class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timeInState class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 240 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // lastLocalTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 244 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // previousStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 248 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nextStartStateIndexOverride class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 252 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // stateOrTransitionChanged class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 256 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // echoNextUpdate class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 257 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // sCurrentStateIndexAndEntered class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 258 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbStateMachine : hkbGenerator, IEquatable<hkbStateMachine?>
    {
        public static hkbStateMachine GetDefault() => new()
        {
            startStateId = 0,
            eventToSendWhenStateOrTransitionChanges = hkbEvent.GetDefault(),
            maxSimultaneousTransitions = 32,
            returnToPreviousStateEventId = -1, 
            randomTransitionEventId = -1, 
            transitionToNextLowerStateEventId = -1,
            transitionToNextHigherStateEventId = -1,
            syncVariableIndex = -1,
            wrapAroundStateId = false, 
            startStateMode = 0, 
            selfTransitionMode = 0,
            states = new List<hkbStateMachineStateInfo>()  
        };
        public void SetDefault()
        {
            startStateId = 0;
            eventToSendWhenStateOrTransitionChanges = hkbEvent.GetDefault();
            maxSimultaneousTransitions = 32;
            returnToPreviousStateEventId = -1;
            randomTransitionEventId = -1;
            transitionToNextLowerStateEventId = -1;
            transitionToNextHigherStateEventId = -1;
            syncVariableIndex = -1;
            wrapAroundStateId = false;
            startStateMode = 0;
            selfTransitionMode = 0;
            states = new List<hkbStateMachineStateInfo>();
		}
        public int AddDefaultStateInfo(hkbStateMachineStateInfo state)
        {
            int stateId;
			lock (states)
            {
                stateId = states.Count;
				states.Add(state);
			}
			state.stateId = stateId;
			return stateId;
		}
        public hkbEvent eventToSendWhenStateOrTransitionChanges { set; get; } = new();
        public hkbStateChooser? startStateChooser { set; get; }
        public int startStateId { set; get; }
        public int returnToPreviousStateEventId { set; get; }
        public int randomTransitionEventId { set; get; }
        public int transitionToNextHigherStateEventId { set; get; }
        public int transitionToNextLowerStateEventId { set; get; }
        public int syncVariableIndex { set; get; }
        private int currentStateId { set; get; }
        public bool wrapAroundStateId { set; get; }
        public sbyte maxSimultaneousTransitions { set; get; }
        public sbyte startStateMode { set; get; }
        public sbyte selfTransitionMode { set; get; }
        private bool isActive { set; get; }
        public IList<hkbStateMachineStateInfo> states { set; get; } = Array.Empty<hkbStateMachineStateInfo>();
        public hkbStateMachineTransitionInfoArray? wildcardTransitions { set; get; }
        private object? stateIdToIndexMap { set; get; }
        public IList<object> activeTransitions { set; get; } = Array.Empty<object>();
        public IList<object> transitionFlags { set; get; } = Array.Empty<object>();
        public IList<object> wildcardTransitionFlags { set; get; } = Array.Empty<object>();
        public IList<object> delayedTransitions { set; get; } = Array.Empty<object>();
        private float timeInState { set; get; }
        private float lastLocalTime { set; get; }
        private int previousStateId { set; get; }
        private int nextStartStateIndexOverride { set; get; }
        private bool stateOrTransitionChanged { set; get; }
        private bool echoNextUpdate { set; get; }
        private ushort sCurrentStateIndexAndEntered { set; get; }

        public override uint Signature { set; get; } = 0x816c1dcb;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            eventToSendWhenStateOrTransitionChanges.Read(des, br);
            startStateChooser = des.ReadClassPointer<hkbStateChooser>(br);
            startStateId = br.ReadInt32();
            returnToPreviousStateEventId = br.ReadInt32();
            randomTransitionEventId = br.ReadInt32();
            transitionToNextHigherStateEventId = br.ReadInt32();
            transitionToNextLowerStateEventId = br.ReadInt32();
            syncVariableIndex = br.ReadInt32();
            currentStateId = br.ReadInt32();
            wrapAroundStateId = br.ReadBoolean();
            maxSimultaneousTransitions = br.ReadSByte();
            startStateMode = br.ReadSByte();
            selfTransitionMode = br.ReadSByte();
            isActive = br.ReadBoolean();
            br.Position += 7;
            states = des.ReadClassPointerArray<hkbStateMachineStateInfo>(br);
            wildcardTransitions = des.ReadClassPointer<hkbStateMachineTransitionInfoArray>(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            timeInState = br.ReadSingle();
            lastLocalTime = br.ReadSingle();
            previousStateId = br.ReadInt32();
            nextStartStateIndexOverride = br.ReadInt32();
            stateOrTransitionChanged = br.ReadBoolean();
            echoNextUpdate = br.ReadBoolean();
            sCurrentStateIndexAndEntered = br.ReadUInt16();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            eventToSendWhenStateOrTransitionChanges.Write(s, bw);
            s.WriteClassPointer(bw, startStateChooser);
            bw.WriteInt32(startStateId);
            bw.WriteInt32(returnToPreviousStateEventId);
            bw.WriteInt32(randomTransitionEventId);
            bw.WriteInt32(transitionToNextHigherStateEventId);
            bw.WriteInt32(transitionToNextLowerStateEventId);
            bw.WriteInt32(syncVariableIndex);
            bw.WriteInt32(currentStateId);
            bw.WriteBoolean(wrapAroundStateId);
            bw.WriteSByte(maxSimultaneousTransitions);
            bw.WriteSByte(startStateMode);
            bw.WriteSByte(selfTransitionMode);
            bw.WriteBoolean(isActive);
            bw.Position += 7;
            s.WriteClassPointerArray(bw, states);
            s.WriteClassPointer(bw, wildcardTransitions);
            s.WriteVoidPointer(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            bw.WriteSingle(timeInState);
            bw.WriteSingle(lastLocalTime);
            bw.WriteInt32(previousStateId);
            bw.WriteInt32(nextStartStateIndexOverride);
            bw.WriteBoolean(stateOrTransitionChanged);
            bw.WriteBoolean(echoNextUpdate);
            bw.WriteUInt16(sCurrentStateIndexAndEntered);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            eventToSendWhenStateOrTransitionChanges = xd.ReadClass<hkbEvent>(xe, nameof(eventToSendWhenStateOrTransitionChanges));
            startStateChooser = xd.ReadClassPointer<hkbStateChooser>(this, xe, nameof(startStateChooser));
            startStateId = xd.ReadInt32(xe, nameof(startStateId));
            returnToPreviousStateEventId = xd.ReadInt32(xe, nameof(returnToPreviousStateEventId));
            randomTransitionEventId = xd.ReadInt32(xe, nameof(randomTransitionEventId));
            transitionToNextHigherStateEventId = xd.ReadInt32(xe, nameof(transitionToNextHigherStateEventId));
            transitionToNextLowerStateEventId = xd.ReadInt32(xe, nameof(transitionToNextLowerStateEventId));
            syncVariableIndex = xd.ReadInt32(xe, nameof(syncVariableIndex));
            wrapAroundStateId = xd.ReadBoolean(xe, nameof(wrapAroundStateId));
            maxSimultaneousTransitions = xd.ReadSByte(xe, nameof(maxSimultaneousTransitions));
            startStateMode = xd.ReadFlag<StartStateMode, sbyte>(xe, nameof(startStateMode));
            selfTransitionMode = xd.ReadFlag<StateMachineSelfTransitionMode, sbyte>(xe, nameof(selfTransitionMode));
            states = xd.ReadClassPointerArray<hkbStateMachineStateInfo>(this, xe, nameof(states));
            wildcardTransitions = xd.ReadClassPointer<hkbStateMachineTransitionInfoArray>(this, xe, nameof(wildcardTransitions));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbEvent>(xe, nameof(eventToSendWhenStateOrTransitionChanges), eventToSendWhenStateOrTransitionChanges);
            xs.WriteClassPointer(xe, nameof(startStateChooser), startStateChooser);
            xs.WriteNumber(xe, nameof(startStateId), startStateId);
            xs.WriteNumber(xe, nameof(returnToPreviousStateEventId), returnToPreviousStateEventId);
            xs.WriteNumber(xe, nameof(randomTransitionEventId), randomTransitionEventId);
            xs.WriteNumber(xe, nameof(transitionToNextHigherStateEventId), transitionToNextHigherStateEventId);
            xs.WriteNumber(xe, nameof(transitionToNextLowerStateEventId), transitionToNextLowerStateEventId);
            xs.WriteNumber(xe, nameof(syncVariableIndex), syncVariableIndex);
            xs.WriteSerializeIgnored(xe, nameof(currentStateId));
            xs.WriteBoolean(xe, nameof(wrapAroundStateId), wrapAroundStateId);
            xs.WriteNumber(xe, nameof(maxSimultaneousTransitions), maxSimultaneousTransitions);
            xs.WriteEnum<StartStateMode, sbyte>(xe, nameof(startStateMode), startStateMode);
            xs.WriteEnum<StateMachineSelfTransitionMode, sbyte>(xe, nameof(selfTransitionMode), selfTransitionMode);
            xs.WriteSerializeIgnored(xe, nameof(isActive));
            xs.WriteClassPointerArray(xe, nameof(states), states!);
            xs.WriteClassPointer(xe, nameof(wildcardTransitions), wildcardTransitions);
            xs.WriteSerializeIgnored(xe, nameof(stateIdToIndexMap));
            xs.WriteSerializeIgnored(xe, nameof(activeTransitions));
            xs.WriteSerializeIgnored(xe, nameof(transitionFlags));
            xs.WriteSerializeIgnored(xe, nameof(wildcardTransitionFlags));
            xs.WriteSerializeIgnored(xe, nameof(delayedTransitions));
            xs.WriteSerializeIgnored(xe, nameof(timeInState));
            xs.WriteSerializeIgnored(xe, nameof(lastLocalTime));
            xs.WriteSerializeIgnored(xe, nameof(previousStateId));
            xs.WriteSerializeIgnored(xe, nameof(nextStartStateIndexOverride));
            xs.WriteSerializeIgnored(xe, nameof(stateOrTransitionChanged));
            xs.WriteSerializeIgnored(xe, nameof(echoNextUpdate));
            xs.WriteSerializeIgnored(xe, nameof(sCurrentStateIndexAndEntered));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachine);
        }

        public bool Equals(hkbStateMachine? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((eventToSendWhenStateOrTransitionChanges is null && other.eventToSendWhenStateOrTransitionChanges is null) || (eventToSendWhenStateOrTransitionChanges is not null && other.eventToSendWhenStateOrTransitionChanges is not null && eventToSendWhenStateOrTransitionChanges.Equals((IHavokObject)other.eventToSendWhenStateOrTransitionChanges))) &&
                   ((startStateChooser is null && other.startStateChooser is null) || (startStateChooser is not null && other.startStateChooser is not null && startStateChooser.Equals((IHavokObject)other.startStateChooser))) &&
                   startStateId.Equals(other.startStateId) &&
                   returnToPreviousStateEventId.Equals(other.returnToPreviousStateEventId) &&
                   randomTransitionEventId.Equals(other.randomTransitionEventId) &&
                   transitionToNextHigherStateEventId.Equals(other.transitionToNextHigherStateEventId) &&
                   transitionToNextLowerStateEventId.Equals(other.transitionToNextLowerStateEventId) &&
                   syncVariableIndex.Equals(other.syncVariableIndex) &&
                   wrapAroundStateId.Equals(other.wrapAroundStateId) &&
                   maxSimultaneousTransitions.Equals(other.maxSimultaneousTransitions) &&
                   startStateMode.Equals(other.startStateMode) &&
                   selfTransitionMode.Equals(other.selfTransitionMode) &&
                   states.SequenceEqual(other.states) &&
                   ((wildcardTransitions is null && other.wildcardTransitions is null) || (wildcardTransitions is not null && other.wildcardTransitions is not null && wildcardTransitions.Equals((IHavokObject)other.wildcardTransitions))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(eventToSendWhenStateOrTransitionChanges);
            hashcode.Add(startStateChooser);
            hashcode.Add(startStateId);
            hashcode.Add(returnToPreviousStateEventId);
            hashcode.Add(randomTransitionEventId);
            hashcode.Add(transitionToNextHigherStateEventId);
            hashcode.Add(transitionToNextLowerStateEventId);
            hashcode.Add(syncVariableIndex);
            hashcode.Add(wrapAroundStateId);
            hashcode.Add(maxSimultaneousTransitions);
            hashcode.Add(startStateMode);
            hashcode.Add(selfTransitionMode);
            hashcode.Add(states.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(wildcardTransitions);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

