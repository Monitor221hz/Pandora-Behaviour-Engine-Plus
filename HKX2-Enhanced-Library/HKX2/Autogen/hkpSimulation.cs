using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSimulation Signatire: 0x97aba922 size: 64 flags: FLAGS_NOT_SERIALIZABLE

    // determinismCheckFrameCounter class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // world class: hkpWorld Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // lastProcessingStep class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: LastProcessingStep
    // currentTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // currentPsiTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // physicsDeltaTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    // simulateUntilTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // frameMarkerPsiSnap class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 52 flags: FLAGS_NONE enum: 
    // previousStepResult class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    public partial class hkpSimulation : hkReferencedObject, IEquatable<hkpSimulation?>
    {
        public uint determinismCheckFrameCounter { set; get; }
        public hkpWorld? world { set; get; }
        public byte lastProcessingStep { set; get; }
        public float currentTime { set; get; }
        public float currentPsiTime { set; get; }
        public float physicsDeltaTime { set; get; }
        public float simulateUntilTime { set; get; }
        public float frameMarkerPsiSnap { set; get; }
        public uint previousStepResult { set; get; }

        public override uint Signature { set; get; } = 0x97aba922;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            determinismCheckFrameCounter = br.ReadUInt32();
            br.Position += 4;
            world = des.ReadClassPointer<hkpWorld>(br);
            lastProcessingStep = br.ReadByte();
            br.Position += 3;
            currentTime = br.ReadSingle();
            currentPsiTime = br.ReadSingle();
            physicsDeltaTime = br.ReadSingle();
            simulateUntilTime = br.ReadSingle();
            frameMarkerPsiSnap = br.ReadSingle();
            previousStepResult = br.ReadUInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt32(determinismCheckFrameCounter);
            bw.Position += 4;
            s.WriteClassPointer(bw, world);
            bw.WriteByte(lastProcessingStep);
            bw.Position += 3;
            bw.WriteSingle(currentTime);
            bw.WriteSingle(currentPsiTime);
            bw.WriteSingle(physicsDeltaTime);
            bw.WriteSingle(simulateUntilTime);
            bw.WriteSingle(frameMarkerPsiSnap);
            bw.WriteUInt32(previousStepResult);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            determinismCheckFrameCounter = xd.ReadUInt32(xe, nameof(determinismCheckFrameCounter));
            world = xd.ReadClassPointer<hkpWorld>(this, xe, nameof(world));
            lastProcessingStep = xd.ReadFlag<LastProcessingStep, byte>(xe, nameof(lastProcessingStep));
            currentTime = xd.ReadSingle(xe, nameof(currentTime));
            currentPsiTime = xd.ReadSingle(xe, nameof(currentPsiTime));
            physicsDeltaTime = xd.ReadSingle(xe, nameof(physicsDeltaTime));
            simulateUntilTime = xd.ReadSingle(xe, nameof(simulateUntilTime));
            frameMarkerPsiSnap = xd.ReadSingle(xe, nameof(frameMarkerPsiSnap));
            previousStepResult = xd.ReadUInt32(xe, nameof(previousStepResult));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(determinismCheckFrameCounter), determinismCheckFrameCounter);
            xs.WriteClassPointer(xe, nameof(world), world);
            xs.WriteEnum<LastProcessingStep, byte>(xe, nameof(lastProcessingStep), lastProcessingStep);
            xs.WriteFloat(xe, nameof(currentTime), currentTime);
            xs.WriteFloat(xe, nameof(currentPsiTime), currentPsiTime);
            xs.WriteFloat(xe, nameof(physicsDeltaTime), physicsDeltaTime);
            xs.WriteFloat(xe, nameof(simulateUntilTime), simulateUntilTime);
            xs.WriteFloat(xe, nameof(frameMarkerPsiSnap), frameMarkerPsiSnap);
            xs.WriteNumber(xe, nameof(previousStepResult), previousStepResult);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSimulation);
        }

        public bool Equals(hkpSimulation? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   determinismCheckFrameCounter.Equals(other.determinismCheckFrameCounter) &&
                   ((world is null && other.world is null) || (world is not null && other.world is not null && world.Equals((IHavokObject)other.world))) &&
                   lastProcessingStep.Equals(other.lastProcessingStep) &&
                   currentTime.Equals(other.currentTime) &&
                   currentPsiTime.Equals(other.currentPsiTime) &&
                   physicsDeltaTime.Equals(other.physicsDeltaTime) &&
                   simulateUntilTime.Equals(other.simulateUntilTime) &&
                   frameMarkerPsiSnap.Equals(other.frameMarkerPsiSnap) &&
                   previousStepResult.Equals(other.previousStepResult) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(determinismCheckFrameCounter);
            hashcode.Add(world);
            hashcode.Add(lastProcessingStep);
            hashcode.Add(currentTime);
            hashcode.Add(currentPsiTime);
            hashcode.Add(physicsDeltaTime);
            hashcode.Add(simulateUntilTime);
            hashcode.Add(frameMarkerPsiSnap);
            hashcode.Add(previousStepResult);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

