using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSEventOnDeactivateModifier Signatire: 0x1062d993 size: 96 flags: FLAGS_NONE

    // @eventclass: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class BSEventOnDeactivateModifier : hkbModifier, IEquatable<BSEventOnDeactivateModifier?>
    {
        public hkbEventProperty @event{ set; get; } = new();

        public override uint Signature { set; get; } = 0x1062d993;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            @event.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            @event.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            @event= xd.ReadClass<hkbEventProperty>(xe, nameof(@event));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbEventProperty>(xe, nameof(@event), @event);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSEventOnDeactivateModifier);
        }

        public bool Equals(BSEventOnDeactivateModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((@event is null && other.@event is null) || (@event is not null && other.@event is not null && @event.Equals((IHavokObject)other.@event))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(@event);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

