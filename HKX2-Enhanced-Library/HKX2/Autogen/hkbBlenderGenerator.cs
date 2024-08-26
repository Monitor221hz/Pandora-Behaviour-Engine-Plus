using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBlenderGenerator Signatire: 0x22df7147 size: 160 flags: FLAGS_NONE

    // referencePoseWeightThreshold class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // blendParameter class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    // minCyclicBlendParameter class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // maxCyclicBlendParameter class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // indexOfSyncMasterChild class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // flags class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 90 flags: FLAGS_NONE enum: 
    // subtractLastChild class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // children class: hkbBlenderGeneratorChild Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // childrenInternalStates class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // sortedChildren class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 128 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // endIntervalWeight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // numActiveChildren class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 148 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // beginIntervalIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 152 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // endIntervalIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 154 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // initSync class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 156 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // doSubtractiveBlend class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 157 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbBlenderGenerator : hkbGenerator, IEquatable<hkbBlenderGenerator?>
    {
        public float referencePoseWeightThreshold { set; get; }
        public float blendParameter { set; get; }
        public float minCyclicBlendParameter { set; get; }
        public float maxCyclicBlendParameter { set; get; }
        public short indexOfSyncMasterChild { set; get; }
        public short flags { set; get; }
        public bool subtractLastChild { set; get; }
        public IList<hkbBlenderGeneratorChild> children { set; get; } = Array.Empty<hkbBlenderGeneratorChild>();
        public IList<object> childrenInternalStates { set; get; } = Array.Empty<object>();
        public IList<object> sortedChildren { set; get; } = Array.Empty<object>();
        private float endIntervalWeight { set; get; }
        private int numActiveChildren { set; get; }
        private short beginIntervalIndex { set; get; }
        private short endIntervalIndex { set; get; }
        private bool initSync { set; get; }
        private bool doSubtractiveBlend { set; get; }

        public override uint Signature { set; get; } = 0x22df7147;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            referencePoseWeightThreshold = br.ReadSingle();
            blendParameter = br.ReadSingle();
            minCyclicBlendParameter = br.ReadSingle();
            maxCyclicBlendParameter = br.ReadSingle();
            indexOfSyncMasterChild = br.ReadInt16();
            flags = br.ReadInt16();
            subtractLastChild = br.ReadBoolean();
            br.Position += 3;
            children = des.ReadClassPointerArray<hkbBlenderGeneratorChild>(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            endIntervalWeight = br.ReadSingle();
            numActiveChildren = br.ReadInt32();
            beginIntervalIndex = br.ReadInt16();
            endIntervalIndex = br.ReadInt16();
            initSync = br.ReadBoolean();
            doSubtractiveBlend = br.ReadBoolean();
            br.Position += 2;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(referencePoseWeightThreshold);
            bw.WriteSingle(blendParameter);
            bw.WriteSingle(minCyclicBlendParameter);
            bw.WriteSingle(maxCyclicBlendParameter);
            bw.WriteInt16(indexOfSyncMasterChild);
            bw.WriteInt16(flags);
            bw.WriteBoolean(subtractLastChild);
            bw.Position += 3;
            s.WriteClassPointerArray(bw, children);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            bw.WriteSingle(endIntervalWeight);
            bw.WriteInt32(numActiveChildren);
            bw.WriteInt16(beginIntervalIndex);
            bw.WriteInt16(endIntervalIndex);
            bw.WriteBoolean(initSync);
            bw.WriteBoolean(doSubtractiveBlend);
            bw.Position += 2;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            referencePoseWeightThreshold = xd.ReadSingle(xe, nameof(referencePoseWeightThreshold));
            blendParameter = xd.ReadSingle(xe, nameof(blendParameter));
            minCyclicBlendParameter = xd.ReadSingle(xe, nameof(minCyclicBlendParameter));
            maxCyclicBlendParameter = xd.ReadSingle(xe, nameof(maxCyclicBlendParameter));
            indexOfSyncMasterChild = xd.ReadInt16(xe, nameof(indexOfSyncMasterChild));
            flags = xd.ReadInt16(xe, nameof(flags));
            subtractLastChild = xd.ReadBoolean(xe, nameof(subtractLastChild));
            children = xd.ReadClassPointerArray<hkbBlenderGeneratorChild>(this, xe, nameof(children));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(referencePoseWeightThreshold), referencePoseWeightThreshold);
            xs.WriteFloat(xe, nameof(blendParameter), blendParameter);
            xs.WriteFloat(xe, nameof(minCyclicBlendParameter), minCyclicBlendParameter);
            xs.WriteFloat(xe, nameof(maxCyclicBlendParameter), maxCyclicBlendParameter);
            xs.WriteNumber(xe, nameof(indexOfSyncMasterChild), indexOfSyncMasterChild);
            xs.WriteNumber(xe, nameof(flags), flags);
            xs.WriteBoolean(xe, nameof(subtractLastChild), subtractLastChild);
            xs.WriteClassPointerArray(xe, nameof(children), children!);
            xs.WriteSerializeIgnored(xe, nameof(childrenInternalStates));
            xs.WriteSerializeIgnored(xe, nameof(sortedChildren));
            xs.WriteSerializeIgnored(xe, nameof(endIntervalWeight));
            xs.WriteSerializeIgnored(xe, nameof(numActiveChildren));
            xs.WriteSerializeIgnored(xe, nameof(beginIntervalIndex));
            xs.WriteSerializeIgnored(xe, nameof(endIntervalIndex));
            xs.WriteSerializeIgnored(xe, nameof(initSync));
            xs.WriteSerializeIgnored(xe, nameof(doSubtractiveBlend));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBlenderGenerator);
        }

        public bool Equals(hkbBlenderGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   referencePoseWeightThreshold.Equals(other.referencePoseWeightThreshold) &&
                   blendParameter.Equals(other.blendParameter) &&
                   minCyclicBlendParameter.Equals(other.minCyclicBlendParameter) &&
                   maxCyclicBlendParameter.Equals(other.maxCyclicBlendParameter) &&
                   indexOfSyncMasterChild.Equals(other.indexOfSyncMasterChild) &&
                   flags.Equals(other.flags) &&
                   subtractLastChild.Equals(other.subtractLastChild) &&
                   children.SequenceEqual(other.children) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(referencePoseWeightThreshold);
            hashcode.Add(blendParameter);
            hashcode.Add(minCyclicBlendParameter);
            hashcode.Add(maxCyclicBlendParameter);
            hashcode.Add(indexOfSyncMasterChild);
            hashcode.Add(flags);
            hashcode.Add(subtractLastChild);
            hashcode.Add(children.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

