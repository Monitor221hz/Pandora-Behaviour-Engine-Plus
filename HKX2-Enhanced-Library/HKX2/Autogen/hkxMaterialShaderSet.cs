using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxMaterialShaderSet Signatire: 0x154650f3 size: 32 flags: FLAGS_NONE

    // shaders class: hkxMaterialShader Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxMaterialShaderSet : hkReferencedObject, IEquatable<hkxMaterialShaderSet?>
    {
        public IList<hkxMaterialShader> shaders { set; get; } = Array.Empty<hkxMaterialShader>();

        public override uint Signature { set; get; } = 0x154650f3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            shaders = des.ReadClassPointerArray<hkxMaterialShader>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, shaders);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            shaders = xd.ReadClassPointerArray<hkxMaterialShader>(this, xe, nameof(shaders));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(shaders), shaders!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxMaterialShaderSet);
        }

        public bool Equals(hkxMaterialShaderSet? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   shaders.SequenceEqual(other.shaders) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(shaders.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

