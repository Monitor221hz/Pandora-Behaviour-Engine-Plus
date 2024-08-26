using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxCamera Signatire: 0xe3597b02 size: 80 flags: FLAGS_NONE

    // from class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // focus class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // up class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // fov class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // far class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // near class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // leftHanded class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    public partial class hkxCamera : hkReferencedObject, IEquatable<hkxCamera?>
    {
        public Vector4 from { set; get; }
        public Vector4 focus { set; get; }
        public Vector4 up { set; get; }
        public float fov { set; get; }
        public float far { set; get; }
        public float near { set; get; }
        public bool leftHanded { set; get; }

        public override uint Signature { set; get; } = 0xe3597b02;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            from = br.ReadVector4();
            focus = br.ReadVector4();
            up = br.ReadVector4();
            fov = br.ReadSingle();
            far = br.ReadSingle();
            near = br.ReadSingle();
            leftHanded = br.ReadBoolean();
            br.Position += 3;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(from);
            bw.WriteVector4(focus);
            bw.WriteVector4(up);
            bw.WriteSingle(fov);
            bw.WriteSingle(far);
            bw.WriteSingle(near);
            bw.WriteBoolean(leftHanded);
            bw.Position += 3;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            from = xd.ReadVector4(xe, nameof(from));
            focus = xd.ReadVector4(xe, nameof(focus));
            up = xd.ReadVector4(xe, nameof(up));
            fov = xd.ReadSingle(xe, nameof(fov));
            far = xd.ReadSingle(xe, nameof(far));
            near = xd.ReadSingle(xe, nameof(near));
            leftHanded = xd.ReadBoolean(xe, nameof(leftHanded));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(from), from);
            xs.WriteVector4(xe, nameof(focus), focus);
            xs.WriteVector4(xe, nameof(up), up);
            xs.WriteFloat(xe, nameof(fov), fov);
            xs.WriteFloat(xe, nameof(far), far);
            xs.WriteFloat(xe, nameof(near), near);
            xs.WriteBoolean(xe, nameof(leftHanded), leftHanded);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxCamera);
        }

        public bool Equals(hkxCamera? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   from.Equals(other.from) &&
                   focus.Equals(other.focus) &&
                   up.Equals(other.up) &&
                   fov.Equals(other.fov) &&
                   far.Equals(other.far) &&
                   near.Equals(other.near) &&
                   leftHanded.Equals(other.leftHanded) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(from);
            hashcode.Add(focus);
            hashcode.Add(up);
            hashcode.Add(fov);
            hashcode.Add(far);
            hashcode.Add(near);
            hashcode.Add(leftHanded);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

