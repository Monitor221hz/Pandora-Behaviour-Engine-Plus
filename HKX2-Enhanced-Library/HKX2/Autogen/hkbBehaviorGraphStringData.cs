using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBehaviorGraphStringData Signatire: 0xc713064e size: 80 flags: FLAGS_NONE

    // eventNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // attributeNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // variableNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // characterPropertyNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkbBehaviorGraphStringData : hkReferencedObject, IEquatable<hkbBehaviorGraphStringData?>
    {
        public IList<string> eventNames { set; get; } = Array.Empty<string>();
        public IList<string> attributeNames { set; get; } = Array.Empty<string>();
        public IList<string> variableNames { set; get; } = Array.Empty<string>();
        public IList<string> characterPropertyNames { set; get; } = Array.Empty<string>();

        public override uint Signature { set; get; } = 0xc713064e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            eventNames = des.ReadStringPointerArray(br);
            attributeNames = des.ReadStringPointerArray(br);
            variableNames = des.ReadStringPointerArray(br);
            characterPropertyNames = des.ReadStringPointerArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointerArray(bw, eventNames);
            s.WriteStringPointerArray(bw, attributeNames);
            s.WriteStringPointerArray(bw, variableNames);
            s.WriteStringPointerArray(bw, characterPropertyNames);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            eventNames = xd.ReadStringArray(xe, nameof(eventNames));
            attributeNames = xd.ReadStringArray(xe, nameof(attributeNames));
            variableNames = xd.ReadStringArray(xe, nameof(variableNames));
            characterPropertyNames = xd.ReadStringArray(xe, nameof(characterPropertyNames));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteStringArray(xe, nameof(eventNames), eventNames);
            xs.WriteStringArray(xe, nameof(attributeNames), attributeNames);
            xs.WriteStringArray(xe, nameof(variableNames), variableNames);
            xs.WriteStringArray(xe, nameof(characterPropertyNames), characterPropertyNames);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBehaviorGraphStringData);
        }

        public bool Equals(hkbBehaviorGraphStringData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   eventNames.SequenceEqual(other.eventNames) &&
                   attributeNames.SequenceEqual(other.attributeNames) &&
                   variableNames.SequenceEqual(other.variableNames) &&
                   characterPropertyNames.SequenceEqual(other.characterPropertyNames) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(eventNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(attributeNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(variableNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(characterPropertyNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

