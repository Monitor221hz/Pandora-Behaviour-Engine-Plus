using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBlenderGeneratorInternalState Signatire: 0x84717488 size: 64 flags: FLAGS_NONE

    // childrenInternalStates class: hkbBlenderGeneratorChildInternalState Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // sortedChildren class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // endIntervalWeight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // numActiveChildren class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 52 flags: FLAGS_NONE enum: 
    // beginIntervalIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // endIntervalIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 58 flags: FLAGS_NONE enum: 
    // initSync class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // doSubtractiveBlend class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 61 flags: FLAGS_NONE enum: 
    public partial class hkbBlenderGeneratorInternalState : hkReferencedObject, IEquatable<hkbBlenderGeneratorInternalState?>
    {
        public IList<hkbBlenderGeneratorChildInternalState> childrenInternalStates { set; get; } = Array.Empty<hkbBlenderGeneratorChildInternalState>();
        public IList<short> sortedChildren { set; get; } = Array.Empty<short>();
        public float endIntervalWeight { set; get; }
        public int numActiveChildren { set; get; }
        public short beginIntervalIndex { set; get; }
        public short endIntervalIndex { set; get; }
        public bool initSync { set; get; }
        public bool doSubtractiveBlend { set; get; }

        public override uint Signature { set; get; } = 0x84717488;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            childrenInternalStates = des.ReadClassArray<hkbBlenderGeneratorChildInternalState>(br);
            sortedChildren = des.ReadInt16Array(br);
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
            s.WriteClassArray(bw, childrenInternalStates);
            s.WriteInt16Array(bw, sortedChildren);
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
            childrenInternalStates = xd.ReadClassArray<hkbBlenderGeneratorChildInternalState>(xe, nameof(childrenInternalStates));
            sortedChildren = xd.ReadInt16Array(xe, nameof(sortedChildren));
            endIntervalWeight = xd.ReadSingle(xe, nameof(endIntervalWeight));
            numActiveChildren = xd.ReadInt32(xe, nameof(numActiveChildren));
            beginIntervalIndex = xd.ReadInt16(xe, nameof(beginIntervalIndex));
            endIntervalIndex = xd.ReadInt16(xe, nameof(endIntervalIndex));
            initSync = xd.ReadBoolean(xe, nameof(initSync));
            doSubtractiveBlend = xd.ReadBoolean(xe, nameof(doSubtractiveBlend));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(childrenInternalStates), childrenInternalStates);
            xs.WriteNumberArray(xe, nameof(sortedChildren), sortedChildren);
            xs.WriteFloat(xe, nameof(endIntervalWeight), endIntervalWeight);
            xs.WriteNumber(xe, nameof(numActiveChildren), numActiveChildren);
            xs.WriteNumber(xe, nameof(beginIntervalIndex), beginIntervalIndex);
            xs.WriteNumber(xe, nameof(endIntervalIndex), endIntervalIndex);
            xs.WriteBoolean(xe, nameof(initSync), initSync);
            xs.WriteBoolean(xe, nameof(doSubtractiveBlend), doSubtractiveBlend);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBlenderGeneratorInternalState);
        }

        public bool Equals(hkbBlenderGeneratorInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   childrenInternalStates.SequenceEqual(other.childrenInternalStates) &&
                   sortedChildren.SequenceEqual(other.sortedChildren) &&
                   endIntervalWeight.Equals(other.endIntervalWeight) &&
                   numActiveChildren.Equals(other.numActiveChildren) &&
                   beginIntervalIndex.Equals(other.beginIntervalIndex) &&
                   endIntervalIndex.Equals(other.endIntervalIndex) &&
                   initSync.Equals(other.initSync) &&
                   doSubtractiveBlend.Equals(other.doSubtractiveBlend) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(childrenInternalStates.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(sortedChildren.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(endIntervalWeight);
            hashcode.Add(numActiveChildren);
            hashcode.Add(beginIntervalIndex);
            hashcode.Add(endIntervalIndex);
            hashcode.Add(initSync);
            hashcode.Add(doSubtractiveBlend);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

