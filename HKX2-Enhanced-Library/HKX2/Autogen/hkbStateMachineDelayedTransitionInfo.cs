using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineDelayedTransitionInfo Signatire: 0x26d5499 size: 24 flags: FLAGS_NONE

    // delayedTransition class: hkbStateMachineProspectiveTransitionInfo Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // timeDelayed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // isDelayedTransitionReturnToPreviousState class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // wasInAbutRangeLastFrame class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 21 flags: FLAGS_NONE enum: 
    public partial class hkbStateMachineDelayedTransitionInfo : IHavokObject, IEquatable<hkbStateMachineDelayedTransitionInfo?>
    {
        public hkbStateMachineProspectiveTransitionInfo delayedTransition { set; get; } = new();
        public float timeDelayed { set; get; }
        public bool isDelayedTransitionReturnToPreviousState { set; get; }
        public bool wasInAbutRangeLastFrame { set; get; }

        public virtual uint Signature { set; get; } = 0x26d5499;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            delayedTransition.Read(des, br);
            timeDelayed = br.ReadSingle();
            isDelayedTransitionReturnToPreviousState = br.ReadBoolean();
            wasInAbutRangeLastFrame = br.ReadBoolean();
            br.Position += 2;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            delayedTransition.Write(s, bw);
            bw.WriteSingle(timeDelayed);
            bw.WriteBoolean(isDelayedTransitionReturnToPreviousState);
            bw.WriteBoolean(wasInAbutRangeLastFrame);
            bw.Position += 2;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            delayedTransition = xd.ReadClass<hkbStateMachineProspectiveTransitionInfo>(xe, nameof(delayedTransition));
            timeDelayed = xd.ReadSingle(xe, nameof(timeDelayed));
            isDelayedTransitionReturnToPreviousState = xd.ReadBoolean(xe, nameof(isDelayedTransitionReturnToPreviousState));
            wasInAbutRangeLastFrame = xd.ReadBoolean(xe, nameof(wasInAbutRangeLastFrame));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkbStateMachineProspectiveTransitionInfo>(xe, nameof(delayedTransition), delayedTransition);
            xs.WriteFloat(xe, nameof(timeDelayed), timeDelayed);
            xs.WriteBoolean(xe, nameof(isDelayedTransitionReturnToPreviousState), isDelayedTransitionReturnToPreviousState);
            xs.WriteBoolean(xe, nameof(wasInAbutRangeLastFrame), wasInAbutRangeLastFrame);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineDelayedTransitionInfo);
        }

        public bool Equals(hkbStateMachineDelayedTransitionInfo? other)
        {
            return other is not null &&
                   ((delayedTransition is null && other.delayedTransition is null) || (delayedTransition is not null && other.delayedTransition is not null && delayedTransition.Equals((IHavokObject)other.delayedTransition))) &&
                   timeDelayed.Equals(other.timeDelayed) &&
                   isDelayedTransitionReturnToPreviousState.Equals(other.isDelayedTransitionReturnToPreviousState) &&
                   wasInAbutRangeLastFrame.Equals(other.wasInAbutRangeLastFrame) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(delayedTransition);
            hashcode.Add(timeDelayed);
            hashcode.Add(isDelayedTransitionReturnToPreviousState);
            hashcode.Add(wasInAbutRangeLastFrame);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

