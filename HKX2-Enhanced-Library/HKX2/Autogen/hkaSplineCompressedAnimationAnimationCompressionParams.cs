using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaSplineCompressedAnimationAnimationCompressionParams Signatire: 0xde830789 size: 4 flags: FLAGS_NONE

    // maxFramesPerBlock class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // enableSampleSingleTracks class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    public partial class hkaSplineCompressedAnimationAnimationCompressionParams : IHavokObject, IEquatable<hkaSplineCompressedAnimationAnimationCompressionParams?>
    {
        public ushort maxFramesPerBlock { set; get; }
        public bool enableSampleSingleTracks { set; get; }

        public virtual uint Signature { set; get; } = 0xde830789;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            maxFramesPerBlock = br.ReadUInt16();
            enableSampleSingleTracks = br.ReadBoolean();
            br.Position += 1;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt16(maxFramesPerBlock);
            bw.WriteBoolean(enableSampleSingleTracks);
            bw.Position += 1;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            maxFramesPerBlock = xd.ReadUInt16(xe, nameof(maxFramesPerBlock));
            enableSampleSingleTracks = xd.ReadBoolean(xe, nameof(enableSampleSingleTracks));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(maxFramesPerBlock), maxFramesPerBlock);
            xs.WriteBoolean(xe, nameof(enableSampleSingleTracks), enableSampleSingleTracks);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaSplineCompressedAnimationAnimationCompressionParams);
        }

        public bool Equals(hkaSplineCompressedAnimationAnimationCompressionParams? other)
        {
            return other is not null &&
                   maxFramesPerBlock.Equals(other.maxFramesPerBlock) &&
                   enableSampleSingleTracks.Equals(other.enableSampleSingleTracks) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(maxFramesPerBlock);
            hashcode.Add(enableSampleSingleTracks);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

