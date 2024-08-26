using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMouseSpringAction Signatire: 0x6e087fd6 size: 144 flags: FLAGS_NONE

    // positionInRbLocal class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // mousePositionInWorld class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // springDamping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // springElasticity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // maxRelativeForce class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // objectDamping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // shapeKey class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // applyCallbacks class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpMouseSpringAction : hkpUnaryAction, IEquatable<hkpMouseSpringAction?>
    {
        public Vector4 positionInRbLocal { set; get; }
        public Vector4 mousePositionInWorld { set; get; }
        public float springDamping { set; get; }
        public float springElasticity { set; get; }
        public float maxRelativeForce { set; get; }
        public float objectDamping { set; get; }
        public uint shapeKey { set; get; }
        public IList<object> applyCallbacks { set; get; } = Array.Empty<object>();

        public override uint Signature { set; get; } = 0x6e087fd6;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            positionInRbLocal = br.ReadVector4();
            mousePositionInWorld = br.ReadVector4();
            springDamping = br.ReadSingle();
            springElasticity = br.ReadSingle();
            maxRelativeForce = br.ReadSingle();
            objectDamping = br.ReadSingle();
            shapeKey = br.ReadUInt32();
            br.Position += 4;
            des.ReadEmptyArray(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            bw.WriteVector4(positionInRbLocal);
            bw.WriteVector4(mousePositionInWorld);
            bw.WriteSingle(springDamping);
            bw.WriteSingle(springElasticity);
            bw.WriteSingle(maxRelativeForce);
            bw.WriteSingle(objectDamping);
            bw.WriteUInt32(shapeKey);
            bw.Position += 4;
            s.WriteVoidArray(bw);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            positionInRbLocal = xd.ReadVector4(xe, nameof(positionInRbLocal));
            mousePositionInWorld = xd.ReadVector4(xe, nameof(mousePositionInWorld));
            springDamping = xd.ReadSingle(xe, nameof(springDamping));
            springElasticity = xd.ReadSingle(xe, nameof(springElasticity));
            maxRelativeForce = xd.ReadSingle(xe, nameof(maxRelativeForce));
            objectDamping = xd.ReadSingle(xe, nameof(objectDamping));
            shapeKey = xd.ReadUInt32(xe, nameof(shapeKey));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(positionInRbLocal), positionInRbLocal);
            xs.WriteVector4(xe, nameof(mousePositionInWorld), mousePositionInWorld);
            xs.WriteFloat(xe, nameof(springDamping), springDamping);
            xs.WriteFloat(xe, nameof(springElasticity), springElasticity);
            xs.WriteFloat(xe, nameof(maxRelativeForce), maxRelativeForce);
            xs.WriteFloat(xe, nameof(objectDamping), objectDamping);
            xs.WriteNumber(xe, nameof(shapeKey), shapeKey);
            xs.WriteSerializeIgnored(xe, nameof(applyCallbacks));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMouseSpringAction);
        }

        public bool Equals(hkpMouseSpringAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   positionInRbLocal.Equals(other.positionInRbLocal) &&
                   mousePositionInWorld.Equals(other.mousePositionInWorld) &&
                   springDamping.Equals(other.springDamping) &&
                   springElasticity.Equals(other.springElasticity) &&
                   maxRelativeForce.Equals(other.maxRelativeForce) &&
                   objectDamping.Equals(other.objectDamping) &&
                   shapeKey.Equals(other.shapeKey) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(positionInRbLocal);
            hashcode.Add(mousePositionInWorld);
            hashcode.Add(springDamping);
            hashcode.Add(springElasticity);
            hashcode.Add(maxRelativeForce);
            hashcode.Add(objectDamping);
            hashcode.Add(shapeKey);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

