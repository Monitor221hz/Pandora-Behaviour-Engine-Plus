using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaAnimationBinding Signatire: 0x66eac971 size: 72 flags: FLAGS_NONE

    // originalSkeletonName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // animation class: hkaAnimation Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // transformTrackToBoneIndices class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // floatTrackToFloatSlotIndices class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // blendHint class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 64 flags: FLAGS_NONE enum: BlendHint
    public partial class hkaAnimationBinding : hkReferencedObject, IEquatable<hkaAnimationBinding?>
    {
        public string originalSkeletonName { set; get; } = "";
        public hkaAnimation? animation { set; get; }
        public IList<short> transformTrackToBoneIndices { set; get; } = Array.Empty<short>();
        public IList<short> floatTrackToFloatSlotIndices { set; get; } = Array.Empty<short>();
        public sbyte blendHint { set; get; }

        public override uint Signature { set; get; } = 0x66eac971;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            originalSkeletonName = des.ReadStringPointer(br);
            animation = des.ReadClassPointer<hkaAnimation>(br);
            transformTrackToBoneIndices = des.ReadInt16Array(br);
            floatTrackToFloatSlotIndices = des.ReadInt16Array(br);
            blendHint = br.ReadSByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, originalSkeletonName);
            s.WriteClassPointer(bw, animation);
            s.WriteInt16Array(bw, transformTrackToBoneIndices);
            s.WriteInt16Array(bw, floatTrackToFloatSlotIndices);
            bw.WriteSByte(blendHint);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            originalSkeletonName = xd.ReadString(xe, nameof(originalSkeletonName));
            animation = xd.ReadClassPointer<hkaAnimation>(this, xe, nameof(animation));
            transformTrackToBoneIndices = xd.ReadInt16Array(xe, nameof(transformTrackToBoneIndices));
            floatTrackToFloatSlotIndices = xd.ReadInt16Array(xe, nameof(floatTrackToFloatSlotIndices));
            blendHint = xd.ReadFlag<BlendHint, sbyte>(xe, nameof(blendHint));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(originalSkeletonName), originalSkeletonName);
            xs.WriteClassPointer(xe, nameof(animation), animation);
            xs.WriteNumberArray(xe, nameof(transformTrackToBoneIndices), transformTrackToBoneIndices);
            xs.WriteNumberArray(xe, nameof(floatTrackToFloatSlotIndices), floatTrackToFloatSlotIndices);
            xs.WriteEnum<BlendHint, sbyte>(xe, nameof(blendHint), blendHint);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaAnimationBinding);
        }

        public bool Equals(hkaAnimationBinding? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (originalSkeletonName is null && other.originalSkeletonName is null || originalSkeletonName == other.originalSkeletonName || originalSkeletonName is null && other.originalSkeletonName == "" || originalSkeletonName == "" && other.originalSkeletonName is null) &&
                   ((animation is null && other.animation is null) || (animation is not null && other.animation is not null && animation.Equals((IHavokObject)other.animation))) &&
                   transformTrackToBoneIndices.SequenceEqual(other.transformTrackToBoneIndices) &&
                   floatTrackToFloatSlotIndices.SequenceEqual(other.floatTrackToFloatSlotIndices) &&
                   blendHint.Equals(other.blendHint) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(originalSkeletonName);
            hashcode.Add(animation);
            hashcode.Add(transformTrackToBoneIndices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(floatTrackToFloatSlotIndices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(blendHint);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

