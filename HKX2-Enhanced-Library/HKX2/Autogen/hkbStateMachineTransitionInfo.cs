using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineTransitionInfo Signatire: 0xcdec8025 size: 72 flags: FLAGS_NONE

    // triggerInterval class: hkbStateMachineTimeInterval Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // initiateInterval class: hkbStateMachineTimeInterval Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // transition class: hkbTransitionEffect Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // condition class: hkbCondition Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // eventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // toStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 52 flags: FLAGS_NONE enum: 
    // fromNestedStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // toNestedStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // priority class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // flags class:  Type.TYPE_FLAGS Type.TYPE_INT16 arrSize: 0 offset: 66 flags: FLAGS_NONE enum: TransitionFlags
    public partial class hkbStateMachineTransitionInfo : IHavokObject, IEquatable<hkbStateMachineTransitionInfo?>
    {
        public static hkbStateMachineTransitionInfo GetDefault() => new()
        {
            triggerInterval = hkbStateMachineTimeInterval.GetDefault(),
            initiateInterval = hkbStateMachineTimeInterval.GetDefault(),
            fromNestedStateId = 0, 
            toNestedStateId = 0, 
            priority = 0,
            flags = 256, // TransitionFlags.FLAG_DISABLE_CONDITION
		};
        public void SetDefault()
        {
            triggerInterval = hkbStateMachineTimeInterval.GetDefault();
            initiateInterval = hkbStateMachineTimeInterval.GetDefault();
            fromNestedStateId = 0; 
            toNestedStateId = 0; 
            priority = 0;
            flags = condition == null ? (short)256 : (short)0;
        }
        public hkbStateMachineTimeInterval triggerInterval { set; get; } = new();
        public hkbStateMachineTimeInterval initiateInterval { set; get; } = new();
        public hkbTransitionEffect? transition { set; get; }
        public hkbCondition? condition { set; get; }
        public int eventId { set; get; }
        public int toStateId { set; get; }
        public int fromNestedStateId { set; get; }
        public int toNestedStateId { set; get; }
        public short priority { set; get; }
        public short flags { set; get; }

        public virtual uint Signature { set; get; } = 0xcdec8025;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            triggerInterval.Read(des, br);
            initiateInterval.Read(des, br);
            transition = des.ReadClassPointer<hkbTransitionEffect>(br);
            condition = des.ReadClassPointer<hkbCondition>(br);
            eventId = br.ReadInt32();
            toStateId = br.ReadInt32();
            fromNestedStateId = br.ReadInt32();
            toNestedStateId = br.ReadInt32();
            priority = br.ReadInt16();
            flags = br.ReadInt16();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            triggerInterval.Write(s, bw);
            initiateInterval.Write(s, bw);
            s.WriteClassPointer(bw, transition);
            s.WriteClassPointer(bw, condition);
            bw.WriteInt32(eventId);
            bw.WriteInt32(toStateId);
            bw.WriteInt32(fromNestedStateId);
            bw.WriteInt32(toNestedStateId);
            bw.WriteInt16(priority);
            bw.WriteInt16(flags);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            triggerInterval = xd.ReadClass<hkbStateMachineTimeInterval>(xe, nameof(triggerInterval));
            initiateInterval = xd.ReadClass<hkbStateMachineTimeInterval>(xe, nameof(initiateInterval));
            transition = xd.ReadClassPointer<hkbTransitionEffect>(this, xe, nameof(transition));
            condition = xd.ReadClassPointer<hkbCondition>(this, xe, nameof(condition));
            eventId = xd.ReadInt32(xe, nameof(eventId));
            toStateId = xd.ReadInt32(xe, nameof(toStateId));
            fromNestedStateId = xd.ReadInt32(xe, nameof(fromNestedStateId));
            toNestedStateId = xd.ReadInt32(xe, nameof(toNestedStateId));
            priority = xd.ReadInt16(xe, nameof(priority));
            flags = xd.ReadFlag<TransitionFlags, short>(xe, nameof(flags));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkbStateMachineTimeInterval>(xe, nameof(triggerInterval), triggerInterval);
            xs.WriteClass<hkbStateMachineTimeInterval>(xe, nameof(initiateInterval), initiateInterval);
            xs.WriteClassPointer(xe, nameof(transition), transition);
            xs.WriteClassPointer(xe, nameof(condition), condition);
            xs.WriteNumber(xe, nameof(eventId), eventId);
            xs.WriteNumber(xe, nameof(toStateId), toStateId);
            xs.WriteNumber(xe, nameof(fromNestedStateId), fromNestedStateId);
            xs.WriteNumber(xe, nameof(toNestedStateId), toNestedStateId);
            xs.WriteNumber(xe, nameof(priority), priority);
            xs.WriteFlag<TransitionFlags, short>(xe, nameof(flags), flags);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineTransitionInfo);
        }

        public bool Equals(hkbStateMachineTransitionInfo? other)
        {
            return other is not null &&
                   ((triggerInterval is null && other.triggerInterval is null) || (triggerInterval is not null && other.triggerInterval is not null && triggerInterval.Equals((IHavokObject)other.triggerInterval))) &&
                   ((initiateInterval is null && other.initiateInterval is null) || (initiateInterval is not null && other.initiateInterval is not null && initiateInterval.Equals((IHavokObject)other.initiateInterval))) &&
                   ((transition is null && other.transition is null) || (transition is not null && other.transition is not null && transition.Equals((IHavokObject)other.transition))) &&
                   ((condition is null && other.condition is null) || (condition is not null && other.condition is not null && condition.Equals((IHavokObject)other.condition))) &&
                   eventId.Equals(other.eventId) &&
                   toStateId.Equals(other.toStateId) &&
                   fromNestedStateId.Equals(other.fromNestedStateId) &&
                   toNestedStateId.Equals(other.toNestedStateId) &&
                   priority.Equals(other.priority) &&
                   flags.Equals(other.flags) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(triggerInterval);
            hashcode.Add(initiateInterval);
            hashcode.Add(transition);
            hashcode.Add(condition);
            hashcode.Add(eventId);
            hashcode.Add(toStateId);
            hashcode.Add(fromNestedStateId);
            hashcode.Add(toNestedStateId);
            hashcode.Add(priority);
            hashcode.Add(flags);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

