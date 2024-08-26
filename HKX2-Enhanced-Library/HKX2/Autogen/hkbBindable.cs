using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBindable Signatire: 0x2c1432d7 size: 48 flags: FLAGS_NONE

    // variableBindingSet class: hkbVariableBindingSet Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // cachedBindables class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 24 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // areBindablesCached class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 40 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbBindable : hkReferencedObject, IEquatable<hkbBindable?>
    {
        public hkbVariableBindingSet? variableBindingSet { set; get; }
        public IList<object> cachedBindables { set; get; } = Array.Empty<object>();
        private bool areBindablesCached { set; get; }

        public override uint Signature { set; get; } = 0x2c1432d7;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            variableBindingSet = des.ReadClassPointer<hkbVariableBindingSet>(br);
            des.ReadEmptyArray(br);
            areBindablesCached = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, variableBindingSet);
            s.WriteVoidArray(bw);
            bw.WriteBoolean(areBindablesCached);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            variableBindingSet = xd.ReadClassPointer<hkbVariableBindingSet>(this, xe, nameof(variableBindingSet));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(variableBindingSet), variableBindingSet);
            xs.WriteSerializeIgnored(xe, nameof(cachedBindables));
            xs.WriteSerializeIgnored(xe, nameof(areBindablesCached));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBindable);
        }

        public bool Equals(hkbBindable? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((variableBindingSet is null && other.variableBindingSet is null) || (variableBindingSet is not null && other.variableBindingSet is not null && variableBindingSet.Equals((IHavokObject)other.variableBindingSet))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(variableBindingSet);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

