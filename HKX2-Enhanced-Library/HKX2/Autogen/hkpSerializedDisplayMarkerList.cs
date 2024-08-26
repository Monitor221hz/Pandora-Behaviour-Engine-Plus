using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSerializedDisplayMarkerList Signatire: 0x54785c77 size: 32 flags: FLAGS_NONE

    // markers class: hkpSerializedDisplayMarker Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpSerializedDisplayMarkerList : hkReferencedObject, IEquatable<hkpSerializedDisplayMarkerList?>
    {
        public IList<hkpSerializedDisplayMarker> markers { set; get; } = Array.Empty<hkpSerializedDisplayMarker>();

        public override uint Signature { set; get; } = 0x54785c77;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            markers = des.ReadClassPointerArray<hkpSerializedDisplayMarker>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, markers);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            markers = xd.ReadClassPointerArray<hkpSerializedDisplayMarker>(this, xe, nameof(markers));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(markers), markers!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSerializedDisplayMarkerList);
        }

        public bool Equals(hkpSerializedDisplayMarkerList? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   markers.SequenceEqual(other.markers) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(markers.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

