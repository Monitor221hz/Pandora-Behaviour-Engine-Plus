using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSpringAction Signatire: 0x88fc09fa size: 128 flags: FLAGS_NONE

    // lastForce class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // positionAinA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // positionBinB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // restLength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // strength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 116 flags: FLAGS_NONE enum: 
    // damping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // onCompression class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 124 flags: FLAGS_NONE enum: 
    // onExtension class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 125 flags: FLAGS_NONE enum: 
    public partial class hkpSpringAction : hkpBinaryAction, IEquatable<hkpSpringAction?>
    {
        public Vector4 lastForce { set; get; }
        public Vector4 positionAinA { set; get; }
        public Vector4 positionBinB { set; get; }
        public float restLength { set; get; }
        public float strength { set; get; }
        public float damping { set; get; }
        public bool onCompression { set; get; }
        public bool onExtension { set; get; }

        public override uint Signature { set; get; } = 0x88fc09fa;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            lastForce = br.ReadVector4();
            positionAinA = br.ReadVector4();
            positionBinB = br.ReadVector4();
            restLength = br.ReadSingle();
            strength = br.ReadSingle();
            damping = br.ReadSingle();
            onCompression = br.ReadBoolean();
            onExtension = br.ReadBoolean();
            br.Position += 2;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(lastForce);
            bw.WriteVector4(positionAinA);
            bw.WriteVector4(positionBinB);
            bw.WriteSingle(restLength);
            bw.WriteSingle(strength);
            bw.WriteSingle(damping);
            bw.WriteBoolean(onCompression);
            bw.WriteBoolean(onExtension);
            bw.Position += 2;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            lastForce = xd.ReadVector4(xe, nameof(lastForce));
            positionAinA = xd.ReadVector4(xe, nameof(positionAinA));
            positionBinB = xd.ReadVector4(xe, nameof(positionBinB));
            restLength = xd.ReadSingle(xe, nameof(restLength));
            strength = xd.ReadSingle(xe, nameof(strength));
            damping = xd.ReadSingle(xe, nameof(damping));
            onCompression = xd.ReadBoolean(xe, nameof(onCompression));
            onExtension = xd.ReadBoolean(xe, nameof(onExtension));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(lastForce), lastForce);
            xs.WriteVector4(xe, nameof(positionAinA), positionAinA);
            xs.WriteVector4(xe, nameof(positionBinB), positionBinB);
            xs.WriteFloat(xe, nameof(restLength), restLength);
            xs.WriteFloat(xe, nameof(strength), strength);
            xs.WriteFloat(xe, nameof(damping), damping);
            xs.WriteBoolean(xe, nameof(onCompression), onCompression);
            xs.WriteBoolean(xe, nameof(onExtension), onExtension);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSpringAction);
        }

        public bool Equals(hkpSpringAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   lastForce.Equals(other.lastForce) &&
                   positionAinA.Equals(other.positionAinA) &&
                   positionBinB.Equals(other.positionBinB) &&
                   restLength.Equals(other.restLength) &&
                   strength.Equals(other.strength) &&
                   damping.Equals(other.damping) &&
                   onCompression.Equals(other.onCompression) &&
                   onExtension.Equals(other.onExtension) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(lastForce);
            hashcode.Add(positionAinA);
            hashcode.Add(positionBinB);
            hashcode.Add(restLength);
            hashcode.Add(strength);
            hashcode.Add(damping);
            hashcode.Add(onCompression);
            hashcode.Add(onExtension);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

