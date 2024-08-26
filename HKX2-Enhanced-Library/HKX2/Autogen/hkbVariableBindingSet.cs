using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbVariableBindingSet Signatire: 0x338ad4ff size: 40 flags: FLAGS_NONE

    // bindings class: hkbVariableBindingSetBinding Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // indexOfBindingToEnable class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // hasOutputBinding class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 36 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbVariableBindingSet : hkReferencedObject, IEquatable<hkbVariableBindingSet?>
    {
        public IList<hkbVariableBindingSetBinding> bindings { set; get; } = Array.Empty<hkbVariableBindingSetBinding>();
        public int indexOfBindingToEnable { set; get; }
        private bool hasOutputBinding { set; get; }

        public override uint Signature { set; get; } = 0x338ad4ff;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            bindings = des.ReadClassArray<hkbVariableBindingSetBinding>(br);
            indexOfBindingToEnable = br.ReadInt32();
            hasOutputBinding = br.ReadBoolean();
            br.Position += 3;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, bindings);
            bw.WriteInt32(indexOfBindingToEnable);
            bw.WriteBoolean(hasOutputBinding);
            bw.Position += 3;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            bindings = xd.ReadClassArray<hkbVariableBindingSetBinding>(xe, nameof(bindings));
            indexOfBindingToEnable = xd.ReadInt32(xe, nameof(indexOfBindingToEnable));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(bindings), bindings);
            xs.WriteNumber(xe, nameof(indexOfBindingToEnable), indexOfBindingToEnable);
            xs.WriteSerializeIgnored(xe, nameof(hasOutputBinding));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbVariableBindingSet);
        }

        public bool Equals(hkbVariableBindingSet? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   bindings.SequenceEqual(other.bindings) &&
                   indexOfBindingToEnable.Equals(other.indexOfBindingToEnable) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(bindings.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(indexOfBindingToEnable);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

