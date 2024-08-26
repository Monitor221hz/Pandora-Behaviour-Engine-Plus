using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // BSIStateManagerModifier Signatire: 0x6cb24f2e size: 128 flags: FLAGS_NONE

    // iStateVar class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // stateData class: BSIStateManagerModifierBSiStateData Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // myStateListener class: BSIStateManagerModifierBSIStateManagerStateListener Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSIStateManagerModifier : hkbModifier, IEquatable<BSIStateManagerModifier?>
    {
        public int iStateVar { set; get; }
        public IList<BSIStateManagerModifierBSiStateData> stateData { set; get; } = Array.Empty<BSIStateManagerModifierBSiStateData>();
        public BSIStateManagerModifierBSIStateManagerStateListener myStateListener { set; get; } = new();

        public override uint Signature { set; get; } = 0x6cb24f2e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            iStateVar = br.ReadInt32();
            br.Position += 4;
            stateData = des.ReadClassArray<BSIStateManagerModifierBSiStateData>(br);
            myStateListener.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(iStateVar);
            bw.Position += 4;
            s.WriteClassArray(bw, stateData);
            myStateListener.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            iStateVar = xd.ReadInt32(xe, nameof(iStateVar));
            stateData = xd.ReadClassArray<BSIStateManagerModifierBSiStateData>(xe, nameof(stateData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(iStateVar), iStateVar);
            xs.WriteClassArray(xe, nameof(stateData), stateData);
            xs.WriteSerializeIgnored(xe, nameof(myStateListener));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSIStateManagerModifier);
        }

        public bool Equals(BSIStateManagerModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   iStateVar.Equals(other.iStateVar) &&
                   stateData.SequenceEqual(other.stateData) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(iStateVar);
            hashcode.Add(stateData.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

