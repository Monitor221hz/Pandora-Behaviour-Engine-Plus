using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaKeyFrameHierarchyUtilityControlData Signatire: 0xa3d0ac71 size: 48 flags: FLAGS_NONE

    // hierarchyGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // velocityDamping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // accelerationGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // velocityGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // positionGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // positionMaxLinearVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // positionMaxAngularVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // snapGain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 28 flags: FLAGS_NONE enum: 
    // snapMaxLinearVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // snapMaxAngularVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // snapMaxLinearDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // snapMaxAngularDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 44 flags: FLAGS_NONE enum: 
    public partial class hkaKeyFrameHierarchyUtilityControlData : IHavokObject, IEquatable<hkaKeyFrameHierarchyUtilityControlData?>
    {
        public float hierarchyGain { set; get; }
        public float velocityDamping { set; get; }
        public float accelerationGain { set; get; }
        public float velocityGain { set; get; }
        public float positionGain { set; get; }
        public float positionMaxLinearVelocity { set; get; }
        public float positionMaxAngularVelocity { set; get; }
        public float snapGain { set; get; }
        public float snapMaxLinearVelocity { set; get; }
        public float snapMaxAngularVelocity { set; get; }
        public float snapMaxLinearDistance { set; get; }
        public float snapMaxAngularDistance { set; get; }

        public virtual uint Signature { set; get; } = 0xa3d0ac71;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            hierarchyGain = br.ReadSingle();
            velocityDamping = br.ReadSingle();
            accelerationGain = br.ReadSingle();
            velocityGain = br.ReadSingle();
            positionGain = br.ReadSingle();
            positionMaxLinearVelocity = br.ReadSingle();
            positionMaxAngularVelocity = br.ReadSingle();
            snapGain = br.ReadSingle();
            snapMaxLinearVelocity = br.ReadSingle();
            snapMaxAngularVelocity = br.ReadSingle();
            snapMaxLinearDistance = br.ReadSingle();
            snapMaxAngularDistance = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(hierarchyGain);
            bw.WriteSingle(velocityDamping);
            bw.WriteSingle(accelerationGain);
            bw.WriteSingle(velocityGain);
            bw.WriteSingle(positionGain);
            bw.WriteSingle(positionMaxLinearVelocity);
            bw.WriteSingle(positionMaxAngularVelocity);
            bw.WriteSingle(snapGain);
            bw.WriteSingle(snapMaxLinearVelocity);
            bw.WriteSingle(snapMaxAngularVelocity);
            bw.WriteSingle(snapMaxLinearDistance);
            bw.WriteSingle(snapMaxAngularDistance);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            hierarchyGain = xd.ReadSingle(xe, nameof(hierarchyGain));
            velocityDamping = xd.ReadSingle(xe, nameof(velocityDamping));
            accelerationGain = xd.ReadSingle(xe, nameof(accelerationGain));
            velocityGain = xd.ReadSingle(xe, nameof(velocityGain));
            positionGain = xd.ReadSingle(xe, nameof(positionGain));
            positionMaxLinearVelocity = xd.ReadSingle(xe, nameof(positionMaxLinearVelocity));
            positionMaxAngularVelocity = xd.ReadSingle(xe, nameof(positionMaxAngularVelocity));
            snapGain = xd.ReadSingle(xe, nameof(snapGain));
            snapMaxLinearVelocity = xd.ReadSingle(xe, nameof(snapMaxLinearVelocity));
            snapMaxAngularVelocity = xd.ReadSingle(xe, nameof(snapMaxAngularVelocity));
            snapMaxLinearDistance = xd.ReadSingle(xe, nameof(snapMaxLinearDistance));
            snapMaxAngularDistance = xd.ReadSingle(xe, nameof(snapMaxAngularDistance));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(hierarchyGain), hierarchyGain);
            xs.WriteFloat(xe, nameof(velocityDamping), velocityDamping);
            xs.WriteFloat(xe, nameof(accelerationGain), accelerationGain);
            xs.WriteFloat(xe, nameof(velocityGain), velocityGain);
            xs.WriteFloat(xe, nameof(positionGain), positionGain);
            xs.WriteFloat(xe, nameof(positionMaxLinearVelocity), positionMaxLinearVelocity);
            xs.WriteFloat(xe, nameof(positionMaxAngularVelocity), positionMaxAngularVelocity);
            xs.WriteFloat(xe, nameof(snapGain), snapGain);
            xs.WriteFloat(xe, nameof(snapMaxLinearVelocity), snapMaxLinearVelocity);
            xs.WriteFloat(xe, nameof(snapMaxAngularVelocity), snapMaxAngularVelocity);
            xs.WriteFloat(xe, nameof(snapMaxLinearDistance), snapMaxLinearDistance);
            xs.WriteFloat(xe, nameof(snapMaxAngularDistance), snapMaxAngularDistance);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaKeyFrameHierarchyUtilityControlData);
        }

        public bool Equals(hkaKeyFrameHierarchyUtilityControlData? other)
        {
            return other is not null &&
                   hierarchyGain.Equals(other.hierarchyGain) &&
                   velocityDamping.Equals(other.velocityDamping) &&
                   accelerationGain.Equals(other.accelerationGain) &&
                   velocityGain.Equals(other.velocityGain) &&
                   positionGain.Equals(other.positionGain) &&
                   positionMaxLinearVelocity.Equals(other.positionMaxLinearVelocity) &&
                   positionMaxAngularVelocity.Equals(other.positionMaxAngularVelocity) &&
                   snapGain.Equals(other.snapGain) &&
                   snapMaxLinearVelocity.Equals(other.snapMaxLinearVelocity) &&
                   snapMaxAngularVelocity.Equals(other.snapMaxAngularVelocity) &&
                   snapMaxLinearDistance.Equals(other.snapMaxLinearDistance) &&
                   snapMaxAngularDistance.Equals(other.snapMaxAngularDistance) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(hierarchyGain);
            hashcode.Add(velocityDamping);
            hashcode.Add(accelerationGain);
            hashcode.Add(velocityGain);
            hashcode.Add(positionGain);
            hashcode.Add(positionMaxLinearVelocity);
            hashcode.Add(positionMaxAngularVelocity);
            hashcode.Add(snapGain);
            hashcode.Add(snapMaxLinearVelocity);
            hashcode.Add(snapMaxAngularVelocity);
            hashcode.Add(snapMaxLinearDistance);
            hashcode.Add(snapMaxAngularDistance);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

