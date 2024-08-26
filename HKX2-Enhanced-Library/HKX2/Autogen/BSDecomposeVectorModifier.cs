using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // BSDecomposeVectorModifier Signatire: 0x31f6b8b6 size: 112 flags: FLAGS_NONE

    // vector class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // x class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // y class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // z class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // w class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    public partial class BSDecomposeVectorModifier : hkbModifier, IEquatable<BSDecomposeVectorModifier?>
    {
        public Vector4 vector { set; get; }
        public float x { set; get; }
        public float y { set; get; }
        public float z { set; get; }
        public float w { set; get; }

        public override uint Signature { set; get; } = 0x31f6b8b6;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            vector = br.ReadVector4();
            x = br.ReadSingle();
            y = br.ReadSingle();
            z = br.ReadSingle();
            w = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(vector);
            bw.WriteSingle(x);
            bw.WriteSingle(y);
            bw.WriteSingle(z);
            bw.WriteSingle(w);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            vector = xd.ReadVector4(xe, nameof(vector));
            x = xd.ReadSingle(xe, nameof(x));
            y = xd.ReadSingle(xe, nameof(y));
            z = xd.ReadSingle(xe, nameof(z));
            w = xd.ReadSingle(xe, nameof(w));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(vector), vector);
            xs.WriteFloat(xe, nameof(x), x);
            xs.WriteFloat(xe, nameof(y), y);
            xs.WriteFloat(xe, nameof(z), z);
            xs.WriteFloat(xe, nameof(w), w);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSDecomposeVectorModifier);
        }

        public bool Equals(BSDecomposeVectorModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   vector.Equals(other.vector) &&
                   x.Equals(other.x) &&
                   y.Equals(other.y) &&
                   z.Equals(other.z) &&
                   w.Equals(other.w) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(vector);
            hashcode.Add(x);
            hashcode.Add(y);
            hashcode.Add(z);
            hashcode.Add(w);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

