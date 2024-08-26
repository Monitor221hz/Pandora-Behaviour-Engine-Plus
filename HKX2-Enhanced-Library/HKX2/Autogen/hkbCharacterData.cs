using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterData Signatire: 0x300d6808 size: 176 flags: FLAGS_NONE

    // characterControllerInfo class: hkbCharacterDataCharacterControllerInfo Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // modelUpMS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // modelForwardMS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // modelRightMS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // characterPropertyInfos class: hkbVariableInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // numBonesPerLod class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // characterPropertyValues class: hkbVariableValueSet Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // footIkDriverInfo class: hkbFootIkDriverInfo Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // handIkDriverInfo class: hkbHandIkDriverInfo Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // stringData class: hkbCharacterStringData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // mirroredSkeletonInfo class: hkbMirroredSkeletonInfo Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // scale class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // numHands class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 172 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // numFloatSlots class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 174 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbCharacterData : hkReferencedObject, IEquatable<hkbCharacterData?>
    {
        public hkbCharacterDataCharacterControllerInfo characterControllerInfo { set; get; } = new();
        public Vector4 modelUpMS { set; get; }
        public Vector4 modelForwardMS { set; get; }
        public Vector4 modelRightMS { set; get; }
        public IList<hkbVariableInfo> characterPropertyInfos { set; get; } = Array.Empty<hkbVariableInfo>();
        public IList<int> numBonesPerLod { set; get; } = Array.Empty<int>();
        public hkbVariableValueSet? characterPropertyValues { set; get; }
        public hkbFootIkDriverInfo? footIkDriverInfo { set; get; }
        public hkbHandIkDriverInfo? handIkDriverInfo { set; get; }
        public hkbCharacterStringData? stringData { set; get; }
        public hkbMirroredSkeletonInfo? mirroredSkeletonInfo { set; get; }
        public float scale { set; get; }
        private short numHands { set; get; }
        private short numFloatSlots { set; get; }

        public override uint Signature { set; get; } = 0x300d6808;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            characterControllerInfo.Read(des, br);
            br.Position += 8;
            modelUpMS = br.ReadVector4();
            modelForwardMS = br.ReadVector4();
            modelRightMS = br.ReadVector4();
            characterPropertyInfos = des.ReadClassArray<hkbVariableInfo>(br);
            numBonesPerLod = des.ReadInt32Array(br);
            characterPropertyValues = des.ReadClassPointer<hkbVariableValueSet>(br);
            footIkDriverInfo = des.ReadClassPointer<hkbFootIkDriverInfo>(br);
            handIkDriverInfo = des.ReadClassPointer<hkbHandIkDriverInfo>(br);
            stringData = des.ReadClassPointer<hkbCharacterStringData>(br);
            mirroredSkeletonInfo = des.ReadClassPointer<hkbMirroredSkeletonInfo>(br);
            scale = br.ReadSingle();
            numHands = br.ReadInt16();
            numFloatSlots = br.ReadInt16();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            characterControllerInfo.Write(s, bw);
            bw.Position += 8;
            bw.WriteVector4(modelUpMS);
            bw.WriteVector4(modelForwardMS);
            bw.WriteVector4(modelRightMS);
            s.WriteClassArray(bw, characterPropertyInfos);
            s.WriteInt32Array(bw, numBonesPerLod);
            s.WriteClassPointer(bw, characterPropertyValues);
            s.WriteClassPointer(bw, footIkDriverInfo);
            s.WriteClassPointer(bw, handIkDriverInfo);
            s.WriteClassPointer(bw, stringData);
            s.WriteClassPointer(bw, mirroredSkeletonInfo);
            bw.WriteSingle(scale);
            bw.WriteInt16(numHands);
            bw.WriteInt16(numFloatSlots);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            characterControllerInfo = xd.ReadClass<hkbCharacterDataCharacterControllerInfo>(xe, nameof(characterControllerInfo));
            modelUpMS = xd.ReadVector4(xe, nameof(modelUpMS));
            modelForwardMS = xd.ReadVector4(xe, nameof(modelForwardMS));
            modelRightMS = xd.ReadVector4(xe, nameof(modelRightMS));
            characterPropertyInfos = xd.ReadClassArray<hkbVariableInfo>(xe, nameof(characterPropertyInfos));
            numBonesPerLod = xd.ReadInt32Array(xe, nameof(numBonesPerLod));
            characterPropertyValues = xd.ReadClassPointer<hkbVariableValueSet>(this, xe, nameof(characterPropertyValues));
            footIkDriverInfo = xd.ReadClassPointer<hkbFootIkDriverInfo>(this, xe, nameof(footIkDriverInfo));
            handIkDriverInfo = xd.ReadClassPointer<hkbHandIkDriverInfo>(this, xe, nameof(handIkDriverInfo));
            stringData = xd.ReadClassPointer<hkbCharacterStringData>(this, xe, nameof(stringData));
            mirroredSkeletonInfo = xd.ReadClassPointer<hkbMirroredSkeletonInfo>(this, xe, nameof(mirroredSkeletonInfo));
            scale = xd.ReadSingle(xe, nameof(scale));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbCharacterDataCharacterControllerInfo>(xe, nameof(characterControllerInfo), characterControllerInfo);
            xs.WriteVector4(xe, nameof(modelUpMS), modelUpMS);
            xs.WriteVector4(xe, nameof(modelForwardMS), modelForwardMS);
            xs.WriteVector4(xe, nameof(modelRightMS), modelRightMS);
            xs.WriteClassArray(xe, nameof(characterPropertyInfos), characterPropertyInfos);
            xs.WriteNumberArray(xe, nameof(numBonesPerLod), numBonesPerLod);
            xs.WriteClassPointer(xe, nameof(characterPropertyValues), characterPropertyValues);
            xs.WriteClassPointer(xe, nameof(footIkDriverInfo), footIkDriverInfo);
            xs.WriteClassPointer(xe, nameof(handIkDriverInfo), handIkDriverInfo);
            xs.WriteClassPointer(xe, nameof(stringData), stringData);
            xs.WriteClassPointer(xe, nameof(mirroredSkeletonInfo), mirroredSkeletonInfo);
            xs.WriteFloat(xe, nameof(scale), scale);
            xs.WriteSerializeIgnored(xe, nameof(numHands));
            xs.WriteSerializeIgnored(xe, nameof(numFloatSlots));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterData);
        }

        public bool Equals(hkbCharacterData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((characterControllerInfo is null && other.characterControllerInfo is null) || (characterControllerInfo is not null && other.characterControllerInfo is not null && characterControllerInfo.Equals((IHavokObject)other.characterControllerInfo))) &&
                   modelUpMS.Equals(other.modelUpMS) &&
                   modelForwardMS.Equals(other.modelForwardMS) &&
                   modelRightMS.Equals(other.modelRightMS) &&
                   characterPropertyInfos.SequenceEqual(other.characterPropertyInfos) &&
                   numBonesPerLod.SequenceEqual(other.numBonesPerLod) &&
                   ((characterPropertyValues is null && other.characterPropertyValues is null) || (characterPropertyValues is not null && other.characterPropertyValues is not null && characterPropertyValues.Equals((IHavokObject)other.characterPropertyValues))) &&
                   ((footIkDriverInfo is null && other.footIkDriverInfo is null) || (footIkDriverInfo is not null && other.footIkDriverInfo is not null && footIkDriverInfo.Equals((IHavokObject)other.footIkDriverInfo))) &&
                   ((handIkDriverInfo is null && other.handIkDriverInfo is null) || (handIkDriverInfo is not null && other.handIkDriverInfo is not null && handIkDriverInfo.Equals((IHavokObject)other.handIkDriverInfo))) &&
                   ((stringData is null && other.stringData is null) || (stringData is not null && other.stringData is not null && stringData.Equals((IHavokObject)other.stringData))) &&
                   ((mirroredSkeletonInfo is null && other.mirroredSkeletonInfo is null) || (mirroredSkeletonInfo is not null && other.mirroredSkeletonInfo is not null && mirroredSkeletonInfo.Equals((IHavokObject)other.mirroredSkeletonInfo))) &&
                   scale.Equals(other.scale) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(characterControllerInfo);
            hashcode.Add(modelUpMS);
            hashcode.Add(modelForwardMS);
            hashcode.Add(modelRightMS);
            hashcode.Add(characterPropertyInfos.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(numBonesPerLod.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(characterPropertyValues);
            hashcode.Add(footIkDriverInfo);
            hashcode.Add(handIkDriverInfo);
            hashcode.Add(stringData);
            hashcode.Add(mirroredSkeletonInfo);
            hashcode.Add(scale);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

