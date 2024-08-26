using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaSplineCompressedAnimation Signatire: 0x792ee0bb size: 176 flags: FLAGS_NONE

    // numFrames class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // numBlocks class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // maxFramesPerBlock class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // maskAndQuantizationSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // blockDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // blockInverseDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    // frameDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // blockOffsets class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // floatBlockOffsets class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // transformOffsets class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // floatOffsets class:  Type.TYPE_ARRAY Type.TYPE_UINT32 arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // data class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // endian class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    public partial class hkaSplineCompressedAnimation : hkaAnimation, IEquatable<hkaSplineCompressedAnimation?>
    {
        public int numFrames { set; get; }
        public int numBlocks { set; get; }
        public int maxFramesPerBlock { set; get; }
        public int maskAndQuantizationSize { set; get; }
        public float blockDuration { set; get; }
        public float blockInverseDuration { set; get; }
        public float frameDuration { set; get; }
        public IList<uint> blockOffsets { set; get; } = Array.Empty<uint>();
        public IList<uint> floatBlockOffsets { set; get; } = Array.Empty<uint>();
        public IList<uint> transformOffsets { set; get; } = Array.Empty<uint>();
        public IList<uint> floatOffsets { set; get; } = Array.Empty<uint>();
        public IList<byte> data { set; get; } = Array.Empty<byte>();
        public int endian { set; get; }

        public override uint Signature { set; get; } = 0x792ee0bb;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            numFrames = br.ReadInt32();
            numBlocks = br.ReadInt32();
            maxFramesPerBlock = br.ReadInt32();
            maskAndQuantizationSize = br.ReadInt32();
            blockDuration = br.ReadSingle();
            blockInverseDuration = br.ReadSingle();
            frameDuration = br.ReadSingle();
            br.Position += 4;
            blockOffsets = des.ReadUInt32Array(br);
            floatBlockOffsets = des.ReadUInt32Array(br);
            transformOffsets = des.ReadUInt32Array(br);
            floatOffsets = des.ReadUInt32Array(br);
            data = des.ReadByteArray(br);
            endian = br.ReadInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(numFrames);
            bw.WriteInt32(numBlocks);
            bw.WriteInt32(maxFramesPerBlock);
            bw.WriteInt32(maskAndQuantizationSize);
            bw.WriteSingle(blockDuration);
            bw.WriteSingle(blockInverseDuration);
            bw.WriteSingle(frameDuration);
            bw.Position += 4;
            s.WriteUInt32Array(bw, blockOffsets);
            s.WriteUInt32Array(bw, floatBlockOffsets);
            s.WriteUInt32Array(bw, transformOffsets);
            s.WriteUInt32Array(bw, floatOffsets);
            s.WriteByteArray(bw, data);
            bw.WriteInt32(endian);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            numFrames = xd.ReadInt32(xe, nameof(numFrames));
            numBlocks = xd.ReadInt32(xe, nameof(numBlocks));
            maxFramesPerBlock = xd.ReadInt32(xe, nameof(maxFramesPerBlock));
            maskAndQuantizationSize = xd.ReadInt32(xe, nameof(maskAndQuantizationSize));
            blockDuration = xd.ReadSingle(xe, nameof(blockDuration));
            blockInverseDuration = xd.ReadSingle(xe, nameof(blockInverseDuration));
            frameDuration = xd.ReadSingle(xe, nameof(frameDuration));
            blockOffsets = xd.ReadUInt32Array(xe, nameof(blockOffsets));
            floatBlockOffsets = xd.ReadUInt32Array(xe, nameof(floatBlockOffsets));
            transformOffsets = xd.ReadUInt32Array(xe, nameof(transformOffsets));
            floatOffsets = xd.ReadUInt32Array(xe, nameof(floatOffsets));
            data = xd.ReadByteArray(xe, nameof(data));
            endian = xd.ReadInt32(xe, nameof(endian));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(numFrames), numFrames);
            xs.WriteNumber(xe, nameof(numBlocks), numBlocks);
            xs.WriteNumber(xe, nameof(maxFramesPerBlock), maxFramesPerBlock);
            xs.WriteNumber(xe, nameof(maskAndQuantizationSize), maskAndQuantizationSize);
            xs.WriteFloat(xe, nameof(blockDuration), blockDuration);
            xs.WriteFloat(xe, nameof(blockInverseDuration), blockInverseDuration);
            xs.WriteFloat(xe, nameof(frameDuration), frameDuration);
            xs.WriteNumberArray(xe, nameof(blockOffsets), blockOffsets);
            xs.WriteNumberArray(xe, nameof(floatBlockOffsets), floatBlockOffsets);
            xs.WriteNumberArray(xe, nameof(transformOffsets), transformOffsets);
            xs.WriteNumberArray(xe, nameof(floatOffsets), floatOffsets);
            xs.WriteNumberArray(xe, nameof(data), data);
            xs.WriteNumber(xe, nameof(endian), endian);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaSplineCompressedAnimation);
        }

        public bool Equals(hkaSplineCompressedAnimation? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   numFrames.Equals(other.numFrames) &&
                   numBlocks.Equals(other.numBlocks) &&
                   maxFramesPerBlock.Equals(other.maxFramesPerBlock) &&
                   maskAndQuantizationSize.Equals(other.maskAndQuantizationSize) &&
                   blockDuration.Equals(other.blockDuration) &&
                   blockInverseDuration.Equals(other.blockInverseDuration) &&
                   frameDuration.Equals(other.frameDuration) &&
                   blockOffsets.SequenceEqual(other.blockOffsets) &&
                   floatBlockOffsets.SequenceEqual(other.floatBlockOffsets) &&
                   transformOffsets.SequenceEqual(other.transformOffsets) &&
                   floatOffsets.SequenceEqual(other.floatOffsets) &&
                   data.SequenceEqual(other.data) &&
                   endian.Equals(other.endian) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(numFrames);
            hashcode.Add(numBlocks);
            hashcode.Add(maxFramesPerBlock);
            hashcode.Add(maskAndQuantizationSize);
            hashcode.Add(blockDuration);
            hashcode.Add(blockInverseDuration);
            hashcode.Add(frameDuration);
            hashcode.Add(blockOffsets.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(floatBlockOffsets.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(transformOffsets.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(floatOffsets.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(endian);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

