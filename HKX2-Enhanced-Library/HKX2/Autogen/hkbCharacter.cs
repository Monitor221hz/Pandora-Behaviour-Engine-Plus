using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCharacter Signatire: 0x3088a5c5 size: 160 flags: FLAGS_NONE

    // nearbyCharacters class: hkbCharacter Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // currentLod class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // numTracksInLod class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 34 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // ragdollDriver class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // characterControllerDriver class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // footIkDriver class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 64 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // handIkDriver class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 72 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // setup class: hkbCharacterSetup Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // behaviorGraph class: hkbBehaviorGraph Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // projectData class: hkbProjectData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // animationBindingSet class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // raycastInterface class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // world class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|NOT_OWNED|FLAGS_NONE enum: 
    // eventQueue class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 128 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // worldFromModel class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 136 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // poseLocal class:  Type.TYPE_SIMPLEARRAY Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // deleteWorldFromModel class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 156 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // deletePoseLocal class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 157 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbCharacter : hkReferencedObject, IEquatable<hkbCharacter?>
    {
        public IList<hkbCharacter> nearbyCharacters { set; get; } = Array.Empty<hkbCharacter>();
        public short currentLod { set; get; }
        private short numTracksInLod { set; get; }
        public string name { set; get; } = "";
        private object? ragdollDriver { set; get; }
        private object? characterControllerDriver { set; get; }
        private object? footIkDriver { set; get; }
        private object? handIkDriver { set; get; }
        public hkbCharacterSetup? setup { set; get; }
        public hkbBehaviorGraph? behaviorGraph { set; get; }
        public hkbProjectData? projectData { set; get; }
        private object? animationBindingSet { set; get; }
        private object? raycastInterface { set; get; }
        private object? world { set; get; }
        private object? eventQueue { set; get; }
        private object? worldFromModel { set; get; }
        private object? poseLocal { set; get; }
        private bool deleteWorldFromModel { set; get; }
        private bool deletePoseLocal { set; get; }

        public override uint Signature { set; get; } = 0x3088a5c5;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            nearbyCharacters = des.ReadClassPointerArray<hkbCharacter>(br);
            currentLod = br.ReadInt16();
            numTracksInLod = br.ReadInt16();
            br.Position += 4;
            name = des.ReadStringPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            setup = des.ReadClassPointer<hkbCharacterSetup>(br);
            behaviorGraph = des.ReadClassPointer<hkbBehaviorGraph>(br);
            projectData = des.ReadClassPointer<hkbProjectData>(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            //deleteWorldFromModel = br.ReadBoolean();
            //deletePoseLocal = br.ReadBoolean();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, nearbyCharacters);
            bw.WriteInt16(currentLod);
            bw.WriteInt16(numTracksInLod);
            bw.Position += 4;
            s.WriteStringPointer(bw, name);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteClassPointer(bw, setup);
            s.WriteClassPointer(bw, behaviorGraph);
            s.WriteClassPointer(bw, projectData);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
            //bw.WriteBoolean(deleteWorldFromModel);
            //bw.WriteBoolean(deletePoseLocal);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            nearbyCharacters = xd.ReadClassPointerArray<hkbCharacter>(this, xe, nameof(nearbyCharacters));
            currentLod = xd.ReadInt16(xe, nameof(currentLod));
            name = xd.ReadString(xe, nameof(name));
            setup = xd.ReadClassPointer<hkbCharacterSetup>(this, xe, nameof(setup));
            behaviorGraph = xd.ReadClassPointer<hkbBehaviorGraph>(this, xe, nameof(behaviorGraph));
            projectData = xd.ReadClassPointer<hkbProjectData>(this, xe, nameof(projectData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(nearbyCharacters), nearbyCharacters!);
            xs.WriteNumber(xe, nameof(currentLod), currentLod);
            xs.WriteSerializeIgnored(xe, nameof(numTracksInLod));
            xs.WriteString(xe, nameof(name), name);
            xs.WriteSerializeIgnored(xe, nameof(ragdollDriver));
            xs.WriteSerializeIgnored(xe, nameof(characterControllerDriver));
            xs.WriteSerializeIgnored(xe, nameof(footIkDriver));
            xs.WriteSerializeIgnored(xe, nameof(handIkDriver));
            xs.WriteClassPointer(xe, nameof(setup), setup);
            xs.WriteClassPointer(xe, nameof(behaviorGraph), behaviorGraph);
            xs.WriteClassPointer(xe, nameof(projectData), projectData);
            xs.WriteSerializeIgnored(xe, nameof(animationBindingSet));
            xs.WriteSerializeIgnored(xe, nameof(raycastInterface));
            xs.WriteSerializeIgnored(xe, nameof(world));
            xs.WriteSerializeIgnored(xe, nameof(eventQueue));
            xs.WriteSerializeIgnored(xe, nameof(worldFromModel));
            xs.WriteSerializeIgnored(xe, nameof(poseLocal));
            xs.WriteSerializeIgnored(xe, nameof(deleteWorldFromModel));
            xs.WriteSerializeIgnored(xe, nameof(deletePoseLocal));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCharacter);
        }

        public bool Equals(hkbCharacter? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   nearbyCharacters.SequenceEqual(other.nearbyCharacters) &&
                   currentLod.Equals(other.currentLod) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   ((setup is null && other.setup is null) || (setup is not null && other.setup is not null && setup.Equals((IHavokObject)other.setup))) &&
                   ((behaviorGraph is null && other.behaviorGraph is null) || (behaviorGraph is not null && other.behaviorGraph is not null && behaviorGraph.Equals((IHavokObject)other.behaviorGraph))) &&
                   ((projectData is null && other.projectData is null) || (projectData is not null && other.projectData is not null && projectData.Equals((IHavokObject)other.projectData))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(nearbyCharacters.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(currentLod);
            hashcode.Add(name);
            hashcode.Add(setup);
            hashcode.Add(behaviorGraph);
            hashcode.Add(projectData);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

