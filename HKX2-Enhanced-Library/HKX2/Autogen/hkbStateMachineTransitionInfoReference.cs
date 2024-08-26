using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStateMachineTransitionInfoReference Signatire: 0x9810c2d0 size: 6 flags: FLAGS_NONE

    // fromStateIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // transitionIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // stateMachineId class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkbStateMachineTransitionInfoReference : IHavokObject, IEquatable<hkbStateMachineTransitionInfoReference?>
    {
        public short fromStateIndex { set; get; }
        public short transitionIndex { set; get; }
        public short stateMachineId { set; get; }

        public virtual uint Signature { set; get; } = 0x9810c2d0;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            fromStateIndex = br.ReadInt16();
            transitionIndex = br.ReadInt16();
            stateMachineId = br.ReadInt16();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt16(fromStateIndex);
            bw.WriteInt16(transitionIndex);
            bw.WriteInt16(stateMachineId);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            fromStateIndex = xd.ReadInt16(xe, nameof(fromStateIndex));
            transitionIndex = xd.ReadInt16(xe, nameof(transitionIndex));
            stateMachineId = xd.ReadInt16(xe, nameof(stateMachineId));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(fromStateIndex), fromStateIndex);
            xs.WriteNumber(xe, nameof(transitionIndex), transitionIndex);
            xs.WriteNumber(xe, nameof(stateMachineId), stateMachineId);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStateMachineTransitionInfoReference);
        }

        public bool Equals(hkbStateMachineTransitionInfoReference? other)
        {
            return other is not null &&
                   fromStateIndex.Equals(other.fromStateIndex) &&
                   transitionIndex.Equals(other.transitionIndex) &&
                   stateMachineId.Equals(other.stateMachineId) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(fromStateIndex);
            hashcode.Add(transitionIndex);
            hashcode.Add(stateMachineId);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

