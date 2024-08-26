using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxMaterial Signatire: 0x2954537a size: 176 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // stages class: hkxMaterialTextureStage Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // diffuseColor class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // ambientColor class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // specularColor class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // emissiveColor class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // subMaterials class: hkxMaterial Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // extraData class: hkReferencedObject Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // properties class: hkxMaterialProperty Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    public partial class hkxMaterial : hkxAttributeHolder, IEquatable<hkxMaterial?>
    {
        public string name { set; get; } = "";
        public IList<hkxMaterialTextureStage> stages { set; get; } = Array.Empty<hkxMaterialTextureStage>();
        public Vector4 diffuseColor { set; get; }
        public Vector4 ambientColor { set; get; }
        public Vector4 specularColor { set; get; }
        public Vector4 emissiveColor { set; get; }
        public IList<hkxMaterial> subMaterials { set; get; } = Array.Empty<hkxMaterial>();
        public hkReferencedObject? extraData { set; get; }
        public IList<hkxMaterialProperty> properties { set; get; } = Array.Empty<hkxMaterialProperty>();

        public override uint Signature { set; get; } = 0x2954537a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            name = des.ReadStringPointer(br);
            stages = des.ReadClassArray<hkxMaterialTextureStage>(br);
            br.Position += 8;
            diffuseColor = br.ReadVector4();
            ambientColor = br.ReadVector4();
            specularColor = br.ReadVector4();
            emissiveColor = br.ReadVector4();
            subMaterials = des.ReadClassPointerArray<hkxMaterial>(br);
            extraData = des.ReadClassPointer<hkReferencedObject>(br);
            properties = des.ReadClassArray<hkxMaterialProperty>(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, name);
            s.WriteClassArray(bw, stages);
            bw.Position += 8;
            bw.WriteVector4(diffuseColor);
            bw.WriteVector4(ambientColor);
            bw.WriteVector4(specularColor);
            bw.WriteVector4(emissiveColor);
            s.WriteClassPointerArray(bw, subMaterials);
            s.WriteClassPointer(bw, extraData);
            s.WriteClassArray(bw, properties);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
            stages = xd.ReadClassArray<hkxMaterialTextureStage>(xe, nameof(stages));
            diffuseColor = xd.ReadVector4(xe, nameof(diffuseColor));
            ambientColor = xd.ReadVector4(xe, nameof(ambientColor));
            specularColor = xd.ReadVector4(xe, nameof(specularColor));
            emissiveColor = xd.ReadVector4(xe, nameof(emissiveColor));
            subMaterials = xd.ReadClassPointerArray<hkxMaterial>(this, xe, nameof(subMaterials));
            extraData = xd.ReadClassPointer<hkReferencedObject>(this, xe, nameof(extraData));
            properties = xd.ReadClassArray<hkxMaterialProperty>(xe, nameof(properties));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteClassArray(xe, nameof(stages), stages);
            xs.WriteVector4(xe, nameof(diffuseColor), diffuseColor);
            xs.WriteVector4(xe, nameof(ambientColor), ambientColor);
            xs.WriteVector4(xe, nameof(specularColor), specularColor);
            xs.WriteVector4(xe, nameof(emissiveColor), emissiveColor);
            xs.WriteClassPointerArray(xe, nameof(subMaterials), subMaterials!);
            xs.WriteClassPointer(xe, nameof(extraData), extraData);
            xs.WriteClassArray(xe, nameof(properties), properties);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxMaterial);
        }

        public bool Equals(hkxMaterial? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   stages.SequenceEqual(other.stages) &&
                   diffuseColor.Equals(other.diffuseColor) &&
                   ambientColor.Equals(other.ambientColor) &&
                   specularColor.Equals(other.specularColor) &&
                   emissiveColor.Equals(other.emissiveColor) &&
                   subMaterials.SequenceEqual(other.subMaterials) &&
                   ((extraData is null && other.extraData is null) || (extraData is not null && other.extraData is not null && extraData.Equals((IHavokObject)other.extraData))) &&
                   properties.SequenceEqual(other.properties) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name);
            hashcode.Add(stages.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(diffuseColor);
            hashcode.Add(ambientColor);
            hashcode.Add(specularColor);
            hashcode.Add(emissiveColor);
            hashcode.Add(subMaterials.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(extraData);
            hashcode.Add(properties.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

