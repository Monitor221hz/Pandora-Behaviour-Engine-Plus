using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaSkeletonMapperDataSimpleMapping Signatire: 0x3405deca size: 64 flags: FLAGS_NONE

    // boneA class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // boneB class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // aFromBTransform class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkaSkeletonMapperDataSimpleMapping : IHavokObject, IEquatable<hkaSkeletonMapperDataSimpleMapping?>
    {
        public short boneA { set; get; }
        public short boneB { set; get; }
        public Matrix4x4 aFromBTransform { set; get; }

        public virtual uint Signature { set; get; } = 0x3405deca;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            boneA = br.ReadInt16();
            boneB = br.ReadInt16();
            br.Position += 12;
            aFromBTransform = des.ReadQSTransform(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt16(boneA);
            bw.WriteInt16(boneB);
            bw.Position += 12;
            s.WriteQSTransform(bw, aFromBTransform);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            boneA = xd.ReadInt16(xe, nameof(boneA));
            boneB = xd.ReadInt16(xe, nameof(boneB));
            aFromBTransform = xd.ReadQSTransform(xe, nameof(aFromBTransform));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(boneA), boneA);
            xs.WriteNumber(xe, nameof(boneB), boneB);
            xs.WriteQSTransform(xe, nameof(aFromBTransform), aFromBTransform);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaSkeletonMapperDataSimpleMapping);
        }

        public bool Equals(hkaSkeletonMapperDataSimpleMapping? other)
        {
            return other is not null &&
                   boneA.Equals(other.boneA) &&
                   boneB.Equals(other.boneB) &&
                   aFromBTransform.Equals(other.aFromBTransform) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(boneA);
            hashcode.Add(boneB);
            hashcode.Add(aFromBTransform);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

