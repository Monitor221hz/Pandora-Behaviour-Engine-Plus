using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // BSOffsetAnimationGenerator Signatire: 0xb8571122 size: 176 flags: FLAGS_NONE

    // pDefaultGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: ALIGN_16|FLAGS_NONE enum: 
    // pOffsetClipGenerator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: ALIGN_16|FLAGS_NONE enum: 
    // fOffsetVariable class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // fOffsetRangeStart class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // fOffsetRangeEnd class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // BoneOffsetA class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // BoneIndexA class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 136 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // fCurrentPercentage class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 152 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // iCurrentFrame class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 156 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bZeroOffset class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 160 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bOffsetValid class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 161 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSOffsetAnimationGenerator : hkbGenerator, IEquatable<BSOffsetAnimationGenerator?>
    {
        public hkbGenerator? pDefaultGenerator { set; get; }
        public hkbGenerator? pOffsetClipGenerator { set; get; }
        public float fOffsetVariable { set; get; }
        public float fOffsetRangeStart { set; get; }
        public float fOffsetRangeEnd { set; get; }
        public IList<object> BoneOffsetA { set; get; } = Array.Empty<object>();
        public IList<object> BoneIndexA { set; get; } = Array.Empty<object>();
        private float fCurrentPercentage { set; get; }
        private uint iCurrentFrame { set; get; }
        private bool bZeroOffset { set; get; }
        private bool bOffsetValid { set; get; }

        public override uint Signature { set; get; } = 0xb8571122;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            pDefaultGenerator = des.ReadClassPointer<hkbGenerator>(br);
            br.Position += 8;
            pOffsetClipGenerator = des.ReadClassPointer<hkbGenerator>(br);
            fOffsetVariable = br.ReadSingle();
            fOffsetRangeStart = br.ReadSingle();
            fOffsetRangeEnd = br.ReadSingle();
            br.Position += 4;
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            fCurrentPercentage = br.ReadSingle();
            iCurrentFrame = br.ReadUInt32();
            bZeroOffset = br.ReadBoolean();
            bOffsetValid = br.ReadBoolean();
            br.Position += 14;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            s.WriteClassPointer(bw, pDefaultGenerator);
            bw.Position += 8;
            s.WriteClassPointer(bw, pOffsetClipGenerator);
            bw.WriteSingle(fOffsetVariable);
            bw.WriteSingle(fOffsetRangeStart);
            bw.WriteSingle(fOffsetRangeEnd);
            bw.Position += 4;
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            bw.WriteSingle(fCurrentPercentage);
            bw.WriteUInt32(iCurrentFrame);
            bw.WriteBoolean(bZeroOffset);
            bw.WriteBoolean(bOffsetValid);
            bw.Position += 14;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            pDefaultGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(pDefaultGenerator));
            pOffsetClipGenerator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(pOffsetClipGenerator));
            fOffsetVariable = xd.ReadSingle(xe, nameof(fOffsetVariable));
            fOffsetRangeStart = xd.ReadSingle(xe, nameof(fOffsetRangeStart));
            fOffsetRangeEnd = xd.ReadSingle(xe, nameof(fOffsetRangeEnd));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(pDefaultGenerator), pDefaultGenerator);
            xs.WriteClassPointer(xe, nameof(pOffsetClipGenerator), pOffsetClipGenerator);
            xs.WriteFloat(xe, nameof(fOffsetVariable), fOffsetVariable);
            xs.WriteFloat(xe, nameof(fOffsetRangeStart), fOffsetRangeStart);
            xs.WriteFloat(xe, nameof(fOffsetRangeEnd), fOffsetRangeEnd);
            xs.WriteSerializeIgnored(xe, nameof(BoneOffsetA));
            xs.WriteSerializeIgnored(xe, nameof(BoneIndexA));
            xs.WriteSerializeIgnored(xe, nameof(fCurrentPercentage));
            xs.WriteSerializeIgnored(xe, nameof(iCurrentFrame));
            xs.WriteSerializeIgnored(xe, nameof(bZeroOffset));
            xs.WriteSerializeIgnored(xe, nameof(bOffsetValid));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSOffsetAnimationGenerator);
        }

        public bool Equals(BSOffsetAnimationGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((pDefaultGenerator is null && other.pDefaultGenerator is null) || (pDefaultGenerator is not null && other.pDefaultGenerator is not null && pDefaultGenerator.Equals((IHavokObject)other.pDefaultGenerator))) &&
                   ((pOffsetClipGenerator is null && other.pOffsetClipGenerator is null) || (pOffsetClipGenerator is not null && other.pOffsetClipGenerator is not null && pOffsetClipGenerator.Equals((IHavokObject)other.pOffsetClipGenerator))) &&
                   fOffsetVariable.Equals(other.fOffsetVariable) &&
                   fOffsetRangeStart.Equals(other.fOffsetRangeStart) &&
                   fOffsetRangeEnd.Equals(other.fOffsetRangeEnd) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(pDefaultGenerator);
            hashcode.Add(pOffsetClipGenerator);
            hashcode.Add(fOffsetVariable);
            hashcode.Add(fOffsetRangeStart);
            hashcode.Add(fOffsetRangeEnd);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

