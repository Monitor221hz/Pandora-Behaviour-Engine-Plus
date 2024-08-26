using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxMesh Signatire: 0xf2edcc5f size: 48 flags: FLAGS_NONE

    // sections class: hkxMeshSection Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // userChannelInfos class: hkxMeshUserChannelInfo Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkxMesh : hkReferencedObject, IEquatable<hkxMesh?>
    {
        public IList<hkxMeshSection> sections { set; get; } = Array.Empty<hkxMeshSection>();
        public IList<hkxMeshUserChannelInfo> userChannelInfos { set; get; } = Array.Empty<hkxMeshUserChannelInfo>();

        public override uint Signature { set; get; } = 0xf2edcc5f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            sections = des.ReadClassPointerArray<hkxMeshSection>(br);
            userChannelInfos = des.ReadClassPointerArray<hkxMeshUserChannelInfo>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, sections);
            s.WriteClassPointerArray(bw, userChannelInfos);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            sections = xd.ReadClassPointerArray<hkxMeshSection>(this, xe, nameof(sections));
            userChannelInfos = xd.ReadClassPointerArray<hkxMeshUserChannelInfo>(this, xe, nameof(userChannelInfos));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(sections), sections!);
            xs.WriteClassPointerArray(xe, nameof(userChannelInfos), userChannelInfos!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxMesh);
        }

        public bool Equals(hkxMesh? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   sections.SequenceEqual(other.sections) &&
                   userChannelInfos.SequenceEqual(other.userChannelInfos) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(sections.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(userChannelInfos.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

