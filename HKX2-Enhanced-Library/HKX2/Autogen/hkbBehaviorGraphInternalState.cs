using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBehaviorGraphInternalState Signatire: 0x8699b6eb size: 40 flags: FLAGS_NONE

    // nodeInternalStateInfos class: hkbNodeInternalStateInfo Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // variableValueSet class: hkbVariableValueSet Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkbBehaviorGraphInternalState : hkReferencedObject, IEquatable<hkbBehaviorGraphInternalState?>
    {
        public IList<hkbNodeInternalStateInfo> nodeInternalStateInfos { set; get; } = Array.Empty<hkbNodeInternalStateInfo>();
        public hkbVariableValueSet? variableValueSet { set; get; }

        public override uint Signature { set; get; } = 0x8699b6eb;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            nodeInternalStateInfos = des.ReadClassPointerArray<hkbNodeInternalStateInfo>(br);
            variableValueSet = des.ReadClassPointer<hkbVariableValueSet>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, nodeInternalStateInfos);
            s.WriteClassPointer(bw, variableValueSet);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            nodeInternalStateInfos = xd.ReadClassPointerArray<hkbNodeInternalStateInfo>(this, xe, nameof(nodeInternalStateInfos));
            variableValueSet = xd.ReadClassPointer<hkbVariableValueSet>(this, xe, nameof(variableValueSet));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(nodeInternalStateInfos), nodeInternalStateInfos!);
            xs.WriteClassPointer(xe, nameof(variableValueSet), variableValueSet);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBehaviorGraphInternalState);
        }

        public bool Equals(hkbBehaviorGraphInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   nodeInternalStateInfos.SequenceEqual(other.nodeInternalStateInfos) &&
                   ((variableValueSet is null && other.variableValueSet is null) || (variableValueSet is not null && other.variableValueSet is not null && variableValueSet.Equals((IHavokObject)other.variableValueSet))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(nodeInternalStateInfos.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(variableValueSet);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

