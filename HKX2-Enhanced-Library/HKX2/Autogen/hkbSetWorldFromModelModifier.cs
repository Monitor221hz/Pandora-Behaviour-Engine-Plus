using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSetWorldFromModelModifier Signatire: 0xafcfa211 size: 128 flags: FLAGS_NONE

    // translation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // rotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // setTranslation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // setRotation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 113 flags: FLAGS_NONE enum: 
    public partial class hkbSetWorldFromModelModifier : hkbModifier, IEquatable<hkbSetWorldFromModelModifier?>
    {
        public Vector4 translation { set; get; }
        public Quaternion rotation { set; get; }
        public bool setTranslation { set; get; }
        public bool setRotation { set; get; }

        public override uint Signature { set; get; } = 0xafcfa211;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            translation = br.ReadVector4();
            rotation = des.ReadQuaternion(br);
            setTranslation = br.ReadBoolean();
            setRotation = br.ReadBoolean();
            br.Position += 14;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(translation);
            s.WriteQuaternion(bw, rotation);
            bw.WriteBoolean(setTranslation);
            bw.WriteBoolean(setRotation);
            bw.Position += 14;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            translation = xd.ReadVector4(xe, nameof(translation));
            rotation = xd.ReadQuaternion(xe, nameof(rotation));
            setTranslation = xd.ReadBoolean(xe, nameof(setTranslation));
            setRotation = xd.ReadBoolean(xe, nameof(setRotation));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(translation), translation);
            xs.WriteQuaternion(xe, nameof(rotation), rotation);
            xs.WriteBoolean(xe, nameof(setTranslation), setTranslation);
            xs.WriteBoolean(xe, nameof(setRotation), setRotation);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSetWorldFromModelModifier);
        }

        public bool Equals(hkbSetWorldFromModelModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   translation.Equals(other.translation) &&
                   rotation.Equals(other.rotation) &&
                   setTranslation.Equals(other.setTranslation) &&
                   setRotation.Equals(other.setRotation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(translation);
            hashcode.Add(rotation);
            hashcode.Add(setTranslation);
            hashcode.Add(setRotation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

