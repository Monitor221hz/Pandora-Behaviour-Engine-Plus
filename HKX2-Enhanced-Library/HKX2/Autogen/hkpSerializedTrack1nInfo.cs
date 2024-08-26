using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSerializedTrack1nInfo Signatire: 0xf12d48d9 size: 32 flags: FLAGS_NONE

    // sectors class: hkpAgent1nSector Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // subTracks class: hkpSerializedSubTrack1nInfo Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpSerializedTrack1nInfo : IHavokObject, IEquatable<hkpSerializedTrack1nInfo?>
    {
        public IList<hkpAgent1nSector> sectors { set; get; } = Array.Empty<hkpAgent1nSector>();
        public IList<hkpSerializedSubTrack1nInfo> subTracks { set; get; } = Array.Empty<hkpSerializedSubTrack1nInfo>();

        public virtual uint Signature { set; get; } = 0xf12d48d9;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            sectors = des.ReadClassPointerArray<hkpAgent1nSector>(br);
            subTracks = des.ReadClassPointerArray<hkpSerializedSubTrack1nInfo>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointerArray(bw, sectors);
            s.WriteClassPointerArray(bw, subTracks);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            sectors = xd.ReadClassPointerArray<hkpAgent1nSector>(this, xe, nameof(sectors));
            subTracks = xd.ReadClassPointerArray<hkpSerializedSubTrack1nInfo>(this, xe, nameof(subTracks));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointerArray(xe, nameof(sectors), sectors!);
            xs.WriteClassPointerArray(xe, nameof(subTracks), subTracks!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSerializedTrack1nInfo);
        }

        public bool Equals(hkpSerializedTrack1nInfo? other)
        {
            return other is not null &&
                   sectors.SequenceEqual(other.sectors) &&
                   subTracks.SequenceEqual(other.subTracks) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(sectors.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(subTracks.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

