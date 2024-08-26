using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSIStateManagerModifierBSiStateData Signatire: 0x6b8a15fc size: 16 flags: FLAGS_NONE

    // pStateMachine class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // StateID class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // iStateToSetAs class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class BSIStateManagerModifierBSiStateData : IHavokObject, IEquatable<BSIStateManagerModifierBSiStateData?>
    {
        public hkbGenerator? pStateMachine { set; get; }
        public int StateID { set; get; }
        public int iStateToSetAs { set; get; }

        public virtual uint Signature { set; get; } = 0x6b8a15fc;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            pStateMachine = des.ReadClassPointer<hkbGenerator>(br);
            StateID = br.ReadInt32();
            iStateToSetAs = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer(bw, pStateMachine);
            bw.WriteInt32(StateID);
            bw.WriteInt32(iStateToSetAs);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            pStateMachine = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(pStateMachine));
            StateID = xd.ReadInt32(xe, nameof(StateID));
            iStateToSetAs = xd.ReadInt32(xe, nameof(iStateToSetAs));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointer(xe, nameof(pStateMachine), pStateMachine);
            xs.WriteNumber(xe, nameof(StateID), StateID);
            xs.WriteNumber(xe, nameof(iStateToSetAs), iStateToSetAs);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSIStateManagerModifierBSiStateData);
        }

        public bool Equals(BSIStateManagerModifierBSiStateData? other)
        {
            return other is not null &&
                   ((pStateMachine is null && other.pStateMachine is null) || (pStateMachine is not null && other.pStateMachine is not null && pStateMachine.Equals((IHavokObject)other.pStateMachine))) &&
                   StateID.Equals(other.StateID) &&
                   iStateToSetAs.Equals(other.iStateToSetAs) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(pStateMachine);
            hashcode.Add(StateID);
            hashcode.Add(iStateToSetAs);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

