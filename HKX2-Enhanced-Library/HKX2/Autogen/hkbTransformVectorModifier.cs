using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbTransformVectorModifier Signatire: 0xf93e0e24 size: 160 flags: FLAGS_NONE

    // rotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // translation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // vectorIn class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // vectorOut class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // rotateOnly class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // inverse class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 145 flags: FLAGS_NONE enum: 
    // computeOnActivate class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 146 flags: FLAGS_NONE enum: 
    // computeOnModify class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 147 flags: FLAGS_NONE enum: 
    public partial class hkbTransformVectorModifier : hkbModifier, IEquatable<hkbTransformVectorModifier?>
    {
        public Quaternion rotation { set; get; }
        public Vector4 translation { set; get; }
        public Vector4 vectorIn { set; get; }
        public Vector4 vectorOut { set; get; }
        public bool rotateOnly { set; get; }
        public bool inverse { set; get; }
        public bool computeOnActivate { set; get; }
        public bool computeOnModify { set; get; }

        public override uint Signature { set; get; } = 0xf93e0e24;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rotation = des.ReadQuaternion(br);
            translation = br.ReadVector4();
            vectorIn = br.ReadVector4();
            vectorOut = br.ReadVector4();
            rotateOnly = br.ReadBoolean();
            inverse = br.ReadBoolean();
            computeOnActivate = br.ReadBoolean();
            computeOnModify = br.ReadBoolean();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQuaternion(bw, rotation);
            bw.WriteVector4(translation);
            bw.WriteVector4(vectorIn);
            bw.WriteVector4(vectorOut);
            bw.WriteBoolean(rotateOnly);
            bw.WriteBoolean(inverse);
            bw.WriteBoolean(computeOnActivate);
            bw.WriteBoolean(computeOnModify);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rotation = xd.ReadQuaternion(xe, nameof(rotation));
            translation = xd.ReadVector4(xe, nameof(translation));
            vectorIn = xd.ReadVector4(xe, nameof(vectorIn));
            vectorOut = xd.ReadVector4(xe, nameof(vectorOut));
            rotateOnly = xd.ReadBoolean(xe, nameof(rotateOnly));
            inverse = xd.ReadBoolean(xe, nameof(inverse));
            computeOnActivate = xd.ReadBoolean(xe, nameof(computeOnActivate));
            computeOnModify = xd.ReadBoolean(xe, nameof(computeOnModify));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQuaternion(xe, nameof(rotation), rotation);
            xs.WriteVector4(xe, nameof(translation), translation);
            xs.WriteVector4(xe, nameof(vectorIn), vectorIn);
            xs.WriteVector4(xe, nameof(vectorOut), vectorOut);
            xs.WriteBoolean(xe, nameof(rotateOnly), rotateOnly);
            xs.WriteBoolean(xe, nameof(inverse), inverse);
            xs.WriteBoolean(xe, nameof(computeOnActivate), computeOnActivate);
            xs.WriteBoolean(xe, nameof(computeOnModify), computeOnModify);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbTransformVectorModifier);
        }

        public bool Equals(hkbTransformVectorModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rotation.Equals(other.rotation) &&
                   translation.Equals(other.translation) &&
                   vectorIn.Equals(other.vectorIn) &&
                   vectorOut.Equals(other.vectorOut) &&
                   rotateOnly.Equals(other.rotateOnly) &&
                   inverse.Equals(other.inverse) &&
                   computeOnActivate.Equals(other.computeOnActivate) &&
                   computeOnModify.Equals(other.computeOnModify) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rotation);
            hashcode.Add(translation);
            hashcode.Add(vectorIn);
            hashcode.Add(vectorOut);
            hashcode.Add(rotateOnly);
            hashcode.Add(inverse);
            hashcode.Add(computeOnActivate);
            hashcode.Add(computeOnModify);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

