using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacterStringData Signatire: 0x655b42bc size: 192 flags: FLAGS_NONE

    // deformableSkinNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // rigidSkinNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // animationNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // animationFilenames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // characterPropertyNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // retargetingSkeletonMapperFilenames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // lodNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // mirroredSyncPointSubstringsA class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // mirroredSyncPointSubstringsB class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // rigName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // ragdollName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // behaviorFilename class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    public partial class hkbCharacterStringData : hkReferencedObject, IEquatable<hkbCharacterStringData?>
    {
        public IList<string> deformableSkinNames { set; get; } = Array.Empty<string>();
        public IList<string> rigidSkinNames { set; get; } = Array.Empty<string>();
        public IList<string> animationNames { set; get; } = Array.Empty<string>();
        public IList<string> animationFilenames { set; get; } = Array.Empty<string>();
        public IList<string> characterPropertyNames { set; get; } = Array.Empty<string>();
        public IList<string> retargetingSkeletonMapperFilenames { set; get; } = Array.Empty<string>();
        public IList<string> lodNames { set; get; } = Array.Empty<string>();
        public IList<string> mirroredSyncPointSubstringsA { set; get; } = Array.Empty<string>();
        public IList<string> mirroredSyncPointSubstringsB { set; get; } = Array.Empty<string>();
        public string name { set; get; } = "";
        public string rigName { set; get; } = "";
        public string ragdollName { set; get; } = "";
        public string behaviorFilename { set; get; } = "";

        public override uint Signature { set; get; } = 0x655b42bc;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            deformableSkinNames = des.ReadStringPointerArray(br);
            rigidSkinNames = des.ReadStringPointerArray(br);
            animationNames = des.ReadStringPointerArray(br);
            animationFilenames = des.ReadStringPointerArray(br);
            characterPropertyNames = des.ReadStringPointerArray(br);
            retargetingSkeletonMapperFilenames = des.ReadStringPointerArray(br);
            lodNames = des.ReadStringPointerArray(br);
            mirroredSyncPointSubstringsA = des.ReadStringPointerArray(br);
            mirroredSyncPointSubstringsB = des.ReadStringPointerArray(br);
            name = des.ReadStringPointer(br);
            rigName = des.ReadStringPointer(br);
            ragdollName = des.ReadStringPointer(br);
            behaviorFilename = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointerArray(bw, deformableSkinNames);
            s.WriteStringPointerArray(bw, rigidSkinNames);
            s.WriteStringPointerArray(bw, animationNames);
            s.WriteStringPointerArray(bw, animationFilenames);
            s.WriteStringPointerArray(bw, characterPropertyNames);
            s.WriteStringPointerArray(bw, retargetingSkeletonMapperFilenames);
            s.WriteStringPointerArray(bw, lodNames);
            s.WriteStringPointerArray(bw, mirroredSyncPointSubstringsA);
            s.WriteStringPointerArray(bw, mirroredSyncPointSubstringsB);
            s.WriteStringPointer(bw, name);
            s.WriteStringPointer(bw, rigName);
            s.WriteStringPointer(bw, ragdollName);
            s.WriteStringPointer(bw, behaviorFilename);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            deformableSkinNames = xd.ReadStringArray(xe, nameof(deformableSkinNames));
            rigidSkinNames = xd.ReadStringArray(xe, nameof(rigidSkinNames));
            animationNames = xd.ReadStringArray(xe, nameof(animationNames));
            animationFilenames = xd.ReadStringArray(xe, nameof(animationFilenames));
            characterPropertyNames = xd.ReadStringArray(xe, nameof(characterPropertyNames));
            retargetingSkeletonMapperFilenames = xd.ReadStringArray(xe, nameof(retargetingSkeletonMapperFilenames));
            lodNames = xd.ReadStringArray(xe, nameof(lodNames));
            mirroredSyncPointSubstringsA = xd.ReadStringArray(xe, nameof(mirroredSyncPointSubstringsA));
            mirroredSyncPointSubstringsB = xd.ReadStringArray(xe, nameof(mirroredSyncPointSubstringsB));
            name = xd.ReadString(xe, nameof(name));
            rigName = xd.ReadString(xe, nameof(rigName));
            ragdollName = xd.ReadString(xe, nameof(ragdollName));
            behaviorFilename = xd.ReadString(xe, nameof(behaviorFilename));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteStringArray(xe, nameof(deformableSkinNames), deformableSkinNames);
            xs.WriteStringArray(xe, nameof(rigidSkinNames), rigidSkinNames);
            xs.WriteStringArray(xe, nameof(animationNames), animationNames);
            xs.WriteStringArray(xe, nameof(animationFilenames), animationFilenames);
            xs.WriteStringArray(xe, nameof(characterPropertyNames), characterPropertyNames);
            xs.WriteStringArray(xe, nameof(retargetingSkeletonMapperFilenames), retargetingSkeletonMapperFilenames);
            xs.WriteStringArray(xe, nameof(lodNames), lodNames);
            xs.WriteStringArray(xe, nameof(mirroredSyncPointSubstringsA), mirroredSyncPointSubstringsA);
            xs.WriteStringArray(xe, nameof(mirroredSyncPointSubstringsB), mirroredSyncPointSubstringsB);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteString(xe, nameof(rigName), rigName);
            xs.WriteString(xe, nameof(ragdollName), ragdollName);
            xs.WriteString(xe, nameof(behaviorFilename), behaviorFilename);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacterStringData);
        }

        public bool Equals(hkbCharacterStringData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   deformableSkinNames.SequenceEqual(other.deformableSkinNames) &&
                   rigidSkinNames.SequenceEqual(other.rigidSkinNames) &&
                   animationNames.SequenceEqual(other.animationNames) &&
                   animationFilenames.SequenceEqual(other.animationFilenames) &&
                   characterPropertyNames.SequenceEqual(other.characterPropertyNames) &&
                   retargetingSkeletonMapperFilenames.SequenceEqual(other.retargetingSkeletonMapperFilenames) &&
                   lodNames.SequenceEqual(other.lodNames) &&
                   mirroredSyncPointSubstringsA.SequenceEqual(other.mirroredSyncPointSubstringsA) &&
                   mirroredSyncPointSubstringsB.SequenceEqual(other.mirroredSyncPointSubstringsB) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   (rigName is null && other.rigName is null || rigName == other.rigName || rigName is null && other.rigName == "" || rigName == "" && other.rigName is null) &&
                   (ragdollName is null && other.ragdollName is null || ragdollName == other.ragdollName || ragdollName is null && other.ragdollName == "" || ragdollName == "" && other.ragdollName is null) &&
                   (behaviorFilename is null && other.behaviorFilename is null || behaviorFilename == other.behaviorFilename || behaviorFilename is null && other.behaviorFilename == "" || behaviorFilename == "" && other.behaviorFilename is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(deformableSkinNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(rigidSkinNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(animationNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(animationFilenames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(characterPropertyNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(retargetingSkeletonMapperFilenames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(lodNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(mirroredSyncPointSubstringsA.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(mirroredSyncPointSubstringsB.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(name);
            hashcode.Add(rigName);
            hashcode.Add(ragdollName);
            hashcode.Add(behaviorFilename);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

