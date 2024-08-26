using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpStorageMeshShape Signatire: 0xbefd8b39 size: 144 flags: FLAGS_NONE

    // storage class: hkpStorageMeshShapeSubpartStorage Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    public partial class hkpStorageMeshShape : hkpMeshShape, IEquatable<hkpStorageMeshShape?>
    {
        public IList<hkpStorageMeshShapeSubpartStorage> storage { set; get; } = Array.Empty<hkpStorageMeshShapeSubpartStorage>();

        public override uint Signature { set; get; } = 0xbefd8b39;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            storage = des.ReadClassPointerArray<hkpStorageMeshShapeSubpartStorage>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, storage);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            storage = xd.ReadClassPointerArray<hkpStorageMeshShapeSubpartStorage>(this, xe, nameof(storage));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(storage), storage!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpStorageMeshShape);
        }

        public bool Equals(hkpStorageMeshShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   storage.SequenceEqual(other.storage) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(storage.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

