using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineActiveTransitionInfo Signatire: 0xbb90d54f size: 40 flags: FLAGS_NONE

    // transitionEffect class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // transitionEffectInternalStateInfo class: hkbNodeInternalStateInfo Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // transitionInfoReference class: hkbStateMachineTransitionInfoReference Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // transitionInfoReferenceForTE class: hkbStateMachineTransitionInfoReference Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 22 flags: FLAGS_NONE enum: 
    // fromStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // toStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // isReturnToPreviousState class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    public partial class hkbStateMachineActiveTransitionInfo : IHavokObject, IEquatable<hkbStateMachineActiveTransitionInfo?>
    {
        private object? transitionEffect { set; get; }
        public hkbNodeInternalStateInfo? transitionEffectInternalStateInfo { set; get; }
        public hkbStateMachineTransitionInfoReference transitionInfoReference { set; get; } = new();
        public hkbStateMachineTransitionInfoReference transitionInfoReferenceForTE { set; get; } = new();
        public int fromStateId { set; get; }
        public int toStateId { set; get; }
        public bool isReturnToPreviousState { set; get; }

        public virtual uint Signature { set; get; } = 0xbb90d54f;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            des.ReadEmptyPointer(br);
            transitionEffectInternalStateInfo = des.ReadClassPointer<hkbNodeInternalStateInfo>(br);
            transitionInfoReference.Read(des, br);
            transitionInfoReferenceForTE.Read(des, br);
            fromStateId = br.ReadInt32();
            toStateId = br.ReadInt32();
            isReturnToPreviousState = br.ReadBoolean();
            br.Position += 3;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVoidPointer(bw);
            s.WriteClassPointer(bw, transitionEffectInternalStateInfo);
            transitionInfoReference.Write(s, bw);
            transitionInfoReferenceForTE.Write(s, bw);
            bw.WriteInt32(fromStateId);
            bw.WriteInt32(toStateId);
            bw.WriteBoolean(isReturnToPreviousState);
            bw.Position += 3;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transitionEffectInternalStateInfo = xd.ReadClassPointer<hkbNodeInternalStateInfo>(this, xe, nameof(transitionEffectInternalStateInfo));
            transitionInfoReference = xd.ReadClass<hkbStateMachineTransitionInfoReference>(xe, nameof(transitionInfoReference));
            transitionInfoReferenceForTE = xd.ReadClass<hkbStateMachineTransitionInfoReference>(xe, nameof(transitionInfoReferenceForTE));
            fromStateId = xd.ReadInt32(xe, nameof(fromStateId));
            toStateId = xd.ReadInt32(xe, nameof(toStateId));
            isReturnToPreviousState = xd.ReadBoolean(xe, nameof(isReturnToPreviousState));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(transitionEffect));
            xs.WriteClassPointer(xe, nameof(transitionEffectInternalStateInfo), transitionEffectInternalStateInfo);
            xs.WriteClass<hkbStateMachineTransitionInfoReference>(xe, nameof(transitionInfoReference), transitionInfoReference);
            xs.WriteClass<hkbStateMachineTransitionInfoReference>(xe, nameof(transitionInfoReferenceForTE), transitionInfoReferenceForTE);
            xs.WriteNumber(xe, nameof(fromStateId), fromStateId);
            xs.WriteNumber(xe, nameof(toStateId), toStateId);
            xs.WriteBoolean(xe, nameof(isReturnToPreviousState), isReturnToPreviousState);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineActiveTransitionInfo);
        }

        public bool Equals(hkbStateMachineActiveTransitionInfo? other)
        {
            return other is not null &&
                   ((transitionEffectInternalStateInfo is null && other.transitionEffectInternalStateInfo is null) || (transitionEffectInternalStateInfo is not null && other.transitionEffectInternalStateInfo is not null && transitionEffectInternalStateInfo.Equals((IHavokObject)other.transitionEffectInternalStateInfo))) &&
                   ((transitionInfoReference is null && other.transitionInfoReference is null) || (transitionInfoReference is not null && other.transitionInfoReference is not null && transitionInfoReference.Equals((IHavokObject)other.transitionInfoReference))) &&
                   ((transitionInfoReferenceForTE is null && other.transitionInfoReferenceForTE is null) || (transitionInfoReferenceForTE is not null && other.transitionInfoReferenceForTE is not null && transitionInfoReferenceForTE.Equals((IHavokObject)other.transitionInfoReferenceForTE))) &&
                   fromStateId.Equals(other.fromStateId) &&
                   toStateId.Equals(other.toStateId) &&
                   isReturnToPreviousState.Equals(other.isReturnToPreviousState) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transitionEffectInternalStateInfo);
            hashcode.Add(transitionInfoReference);
            hashcode.Add(transitionInfoReferenceForTE);
            hashcode.Add(fromStateId);
            hashcode.Add(toStateId);
            hashcode.Add(isReturnToPreviousState);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

