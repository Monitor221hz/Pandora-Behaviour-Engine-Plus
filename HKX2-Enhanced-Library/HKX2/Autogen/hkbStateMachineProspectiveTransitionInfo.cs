using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineProspectiveTransitionInfo Signatire: 0x3ab09a2e size: 16 flags: FLAGS_NONE

    // transitionInfoReference class: hkbStateMachineTransitionInfoReference Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // transitionInfoReferenceForTE class: hkbStateMachineTransitionInfoReference Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 6 flags: FLAGS_NONE enum: 
    // toStateId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkbStateMachineProspectiveTransitionInfo : IHavokObject, IEquatable<hkbStateMachineProspectiveTransitionInfo?>
    {
        public hkbStateMachineTransitionInfoReference transitionInfoReference { set; get; } = new();
        public hkbStateMachineTransitionInfoReference transitionInfoReferenceForTE { set; get; } = new();
        public int toStateId { set; get; }

        public virtual uint Signature { set; get; } = 0x3ab09a2e;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            transitionInfoReference.Read(des, br);
            transitionInfoReferenceForTE.Read(des, br);
            toStateId = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            transitionInfoReference.Write(s, bw);
            transitionInfoReferenceForTE.Write(s, bw);
            bw.WriteInt32(toStateId);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transitionInfoReference = xd.ReadClass<hkbStateMachineTransitionInfoReference>(xe, nameof(transitionInfoReference));
            transitionInfoReferenceForTE = xd.ReadClass<hkbStateMachineTransitionInfoReference>(xe, nameof(transitionInfoReferenceForTE));
            toStateId = xd.ReadInt32(xe, nameof(toStateId));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkbStateMachineTransitionInfoReference>(xe, nameof(transitionInfoReference), transitionInfoReference);
            xs.WriteClass<hkbStateMachineTransitionInfoReference>(xe, nameof(transitionInfoReferenceForTE), transitionInfoReferenceForTE);
            xs.WriteNumber(xe, nameof(toStateId), toStateId);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineProspectiveTransitionInfo);
        }

        public bool Equals(hkbStateMachineProspectiveTransitionInfo? other)
        {
            return other is not null &&
                   ((transitionInfoReference is null && other.transitionInfoReference is null) || (transitionInfoReference is not null && other.transitionInfoReference is not null && transitionInfoReference.Equals((IHavokObject)other.transitionInfoReference))) &&
                   ((transitionInfoReferenceForTE is null && other.transitionInfoReferenceForTE is null) || (transitionInfoReferenceForTE is not null && other.transitionInfoReferenceForTE is not null && transitionInfoReferenceForTE.Equals((IHavokObject)other.transitionInfoReferenceForTE))) &&
                   toStateId.Equals(other.toStateId) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transitionInfoReference);
            hashcode.Add(transitionInfoReferenceForTE);
            hashcode.Add(toStateId);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

