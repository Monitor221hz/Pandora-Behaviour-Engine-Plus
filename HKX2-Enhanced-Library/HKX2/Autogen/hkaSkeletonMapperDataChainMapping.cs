using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaSkeletonMapperDataChainMapping Signatire: 0xa528f7cf size: 112 flags: FLAGS_NONE

    // startBoneA class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // endBoneA class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // startBoneB class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // endBoneB class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 6 flags: FLAGS_NONE enum: 
    // startAFromBTransform class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // endAFromBTransform class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkaSkeletonMapperDataChainMapping : IHavokObject, IEquatable<hkaSkeletonMapperDataChainMapping?>
    {
        public short startBoneA { set; get; }
        public short endBoneA { set; get; }
        public short startBoneB { set; get; }
        public short endBoneB { set; get; }
        public Matrix4x4 startAFromBTransform { set; get; }
        public Matrix4x4 endAFromBTransform { set; get; }

        public virtual uint Signature { set; get; } = 0xa528f7cf;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            startBoneA = br.ReadInt16();
            endBoneA = br.ReadInt16();
            startBoneB = br.ReadInt16();
            endBoneB = br.ReadInt16();
            br.Position += 8;
            startAFromBTransform = des.ReadQSTransform(br);
            endAFromBTransform = des.ReadQSTransform(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt16(startBoneA);
            bw.WriteInt16(endBoneA);
            bw.WriteInt16(startBoneB);
            bw.WriteInt16(endBoneB);
            bw.Position += 8;
            s.WriteQSTransform(bw, startAFromBTransform);
            s.WriteQSTransform(bw, endAFromBTransform);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            startBoneA = xd.ReadInt16(xe, nameof(startBoneA));
            endBoneA = xd.ReadInt16(xe, nameof(endBoneA));
            startBoneB = xd.ReadInt16(xe, nameof(startBoneB));
            endBoneB = xd.ReadInt16(xe, nameof(endBoneB));
            startAFromBTransform = xd.ReadQSTransform(xe, nameof(startAFromBTransform));
            endAFromBTransform = xd.ReadQSTransform(xe, nameof(endAFromBTransform));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(startBoneA), startBoneA);
            xs.WriteNumber(xe, nameof(endBoneA), endBoneA);
            xs.WriteNumber(xe, nameof(startBoneB), startBoneB);
            xs.WriteNumber(xe, nameof(endBoneB), endBoneB);
            xs.WriteQSTransform(xe, nameof(startAFromBTransform), startAFromBTransform);
            xs.WriteQSTransform(xe, nameof(endAFromBTransform), endAFromBTransform);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaSkeletonMapperDataChainMapping);
        }

        public bool Equals(hkaSkeletonMapperDataChainMapping? other)
        {
            return other is not null &&
                   startBoneA.Equals(other.startBoneA) &&
                   endBoneA.Equals(other.endBoneA) &&
                   startBoneB.Equals(other.startBoneB) &&
                   endBoneB.Equals(other.endBoneB) &&
                   startAFromBTransform.Equals(other.startAFromBTransform) &&
                   endAFromBTransform.Equals(other.endAFromBTransform) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(startBoneA);
            hashcode.Add(endBoneA);
            hashcode.Add(startBoneB);
            hashcode.Add(endBoneB);
            hashcode.Add(startAFromBTransform);
            hashcode.Add(endAFromBTransform);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

