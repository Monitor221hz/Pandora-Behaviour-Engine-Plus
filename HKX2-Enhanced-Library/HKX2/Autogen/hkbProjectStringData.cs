using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbProjectStringData Signatire: 0x76ad60a size: 120 flags: FLAGS_NONE

    // animationFilenames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // behaviorFilenames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // characterFilenames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // eventNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // animationPath class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // behaviorPath class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // characterPath class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // fullPathToSource class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // rootPath class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbProjectStringData : hkReferencedObject, IEquatable<hkbProjectStringData?>
    {
        public IList<string> animationFilenames { set; get; } = Array.Empty<string>();
        public IList<string> behaviorFilenames { set; get; } = Array.Empty<string>();
        public IList<string> characterFilenames { set; get; } = Array.Empty<string>();
        public IList<string> eventNames { set; get; } = Array.Empty<string>();
        public string animationPath { set; get; } = "";
        public string behaviorPath { set; get; } = "";
        public string characterPath { set; get; } = "";
        public string fullPathToSource { set; get; } = "";
        public string rootPath { set; get; } = "";

        public override uint Signature { set; get; } = 0x76ad60a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            animationFilenames = des.ReadStringPointerArray(br);
            behaviorFilenames = des.ReadStringPointerArray(br);
            characterFilenames = des.ReadStringPointerArray(br);
            eventNames = des.ReadStringPointerArray(br);
            animationPath = des.ReadStringPointer(br);
            behaviorPath = des.ReadStringPointer(br);
            characterPath = des.ReadStringPointer(br);
            fullPathToSource = des.ReadStringPointer(br);
            rootPath = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointerArray(bw, animationFilenames);
            s.WriteStringPointerArray(bw, behaviorFilenames);
            s.WriteStringPointerArray(bw, characterFilenames);
            s.WriteStringPointerArray(bw, eventNames);
            s.WriteStringPointer(bw, animationPath);
            s.WriteStringPointer(bw, behaviorPath);
            s.WriteStringPointer(bw, characterPath);
            s.WriteStringPointer(bw, fullPathToSource);
            s.WriteStringPointer(bw, rootPath);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            animationFilenames = xd.ReadStringArray(xe, nameof(animationFilenames));
            behaviorFilenames = xd.ReadStringArray(xe, nameof(behaviorFilenames));
            characterFilenames = xd.ReadStringArray(xe, nameof(characterFilenames));
            eventNames = xd.ReadStringArray(xe, nameof(eventNames));
            animationPath = xd.ReadString(xe, nameof(animationPath));
            behaviorPath = xd.ReadString(xe, nameof(behaviorPath));
            characterPath = xd.ReadString(xe, nameof(characterPath));
            fullPathToSource = xd.ReadString(xe, nameof(fullPathToSource));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteStringArray(xe, nameof(animationFilenames), animationFilenames);
            xs.WriteStringArray(xe, nameof(behaviorFilenames), behaviorFilenames);
            xs.WriteStringArray(xe, nameof(characterFilenames), characterFilenames);
            xs.WriteStringArray(xe, nameof(eventNames), eventNames);
            xs.WriteString(xe, nameof(animationPath), animationPath);
            xs.WriteString(xe, nameof(behaviorPath), behaviorPath);
            xs.WriteString(xe, nameof(characterPath), characterPath);
            xs.WriteString(xe, nameof(fullPathToSource), fullPathToSource);
            xs.WriteSerializeIgnored(xe, nameof(rootPath));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbProjectStringData);
        }

        public bool Equals(hkbProjectStringData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   animationFilenames.SequenceEqual(other.animationFilenames) &&
                   behaviorFilenames.SequenceEqual(other.behaviorFilenames) &&
                   characterFilenames.SequenceEqual(other.characterFilenames) &&
                   eventNames.SequenceEqual(other.eventNames) &&
                   (animationPath is null && other.animationPath is null || animationPath == other.animationPath || animationPath is null && other.animationPath == "" || animationPath == "" && other.animationPath is null) &&
                   (behaviorPath is null && other.behaviorPath is null || behaviorPath == other.behaviorPath || behaviorPath is null && other.behaviorPath == "" || behaviorPath == "" && other.behaviorPath is null) &&
                   (characterPath is null && other.characterPath is null || characterPath == other.characterPath || characterPath is null && other.characterPath == "" || characterPath == "" && other.characterPath is null) &&
                   (fullPathToSource is null && other.fullPathToSource is null || fullPathToSource == other.fullPathToSource || fullPathToSource is null && other.fullPathToSource == "" || fullPathToSource == "" && other.fullPathToSource is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(animationFilenames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(behaviorFilenames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(characterFilenames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(eventNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(animationPath);
            hashcode.Add(behaviorPath);
            hashcode.Add(characterPath);
            hashcode.Add(fullPathToSource);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

