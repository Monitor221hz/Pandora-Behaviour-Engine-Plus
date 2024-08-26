using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpTriggerVolume Signatire: 0xa29a8d1a size: 88 flags: FLAGS_NONE

    // overlappingBodies class: hkpRigidBody Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // eventQueue class: hkpTriggerVolumeEventInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // triggerBody class: hkpRigidBody Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // sequenceNumber class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkpTriggerVolume : hkReferencedObject, IEquatable<hkpTriggerVolume?>
    {
        public IList<hkpRigidBody> overlappingBodies { set; get; } = Array.Empty<hkpRigidBody>();
        public IList<hkpTriggerVolumeEventInfo> eventQueue { set; get; } = Array.Empty<hkpTriggerVolumeEventInfo>();
        public hkpRigidBody? triggerBody { set; get; }
        public uint sequenceNumber { set; get; }

        public override uint Signature { set; get; } = 0xa29a8d1a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 24;
            overlappingBodies = des.ReadClassPointerArray<hkpRigidBody>(br);
            eventQueue = des.ReadClassArray<hkpTriggerVolumeEventInfo>(br);
            triggerBody = des.ReadClassPointer<hkpRigidBody>(br);
            sequenceNumber = br.ReadUInt32();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 24;
            s.WriteClassPointerArray(bw, overlappingBodies);
            s.WriteClassArray(bw, eventQueue);
            s.WriteClassPointer(bw, triggerBody);
            bw.WriteUInt32(sequenceNumber);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            overlappingBodies = xd.ReadClassPointerArray<hkpRigidBody>(this, xe, nameof(overlappingBodies));
            eventQueue = xd.ReadClassArray<hkpTriggerVolumeEventInfo>(xe, nameof(eventQueue));
            triggerBody = xd.ReadClassPointer<hkpRigidBody>(this, xe, nameof(triggerBody));
            sequenceNumber = xd.ReadUInt32(xe, nameof(sequenceNumber));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(overlappingBodies), overlappingBodies!);
            xs.WriteClassArray(xe, nameof(eventQueue), eventQueue);
            xs.WriteClassPointer(xe, nameof(triggerBody), triggerBody);
            xs.WriteNumber(xe, nameof(sequenceNumber), sequenceNumber);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpTriggerVolume);
        }

        public bool Equals(hkpTriggerVolume? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   overlappingBodies.SequenceEqual(other.overlappingBodies) &&
                   eventQueue.SequenceEqual(other.eventQueue) &&
                   ((triggerBody is null && other.triggerBody is null) || (triggerBody is not null && other.triggerBody is not null && triggerBody.Equals((IHavokObject)other.triggerBody))) &&
                   sequenceNumber.Equals(other.sequenceNumber) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(overlappingBodies.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(eventQueue.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(triggerBody);
            hashcode.Add(sequenceNumber);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

