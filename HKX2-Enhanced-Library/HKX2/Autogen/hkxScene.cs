using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxScene Signatire: 0x5f673ddd size: 224 flags: FLAGS_NONE

    // modeller class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // asset class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // sceneLength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // rootNode class: hkxNode Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // selectionSets class: hkxNodeSelectionSet Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // cameras class: hkxCamera Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // lights class: hkxLight Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // meshes class: hkxMesh Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // materials class: hkxMaterial Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // inplaceTextures class: hkxTextureInplace Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // externalTextures class: hkxTextureFile Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // skinBindings class: hkxSkinBinding Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // appliedTransform class:  Type.TYPE_MATRIX3 Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    public partial class hkxScene : hkReferencedObject, IEquatable<hkxScene?>
    {
        public string modeller { set; get; } = "";
        public string asset { set; get; } = "";
        public float sceneLength { set; get; }
        public hkxNode? rootNode { set; get; }
        public IList<hkxNodeSelectionSet> selectionSets { set; get; } = Array.Empty<hkxNodeSelectionSet>();
        public IList<hkxCamera> cameras { set; get; } = Array.Empty<hkxCamera>();
        public IList<hkxLight> lights { set; get; } = Array.Empty<hkxLight>();
        public IList<hkxMesh> meshes { set; get; } = Array.Empty<hkxMesh>();
        public IList<hkxMaterial> materials { set; get; } = Array.Empty<hkxMaterial>();
        public IList<hkxTextureInplace> inplaceTextures { set; get; } = Array.Empty<hkxTextureInplace>();
        public IList<hkxTextureFile> externalTextures { set; get; } = Array.Empty<hkxTextureFile>();
        public IList<hkxSkinBinding> skinBindings { set; get; } = Array.Empty<hkxSkinBinding>();
        public Matrix4x4 appliedTransform { set; get; }

        public override uint Signature { set; get; } = 0x5f673ddd;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            modeller = des.ReadStringPointer(br);
            asset = des.ReadStringPointer(br);
            sceneLength = br.ReadSingle();
            br.Position += 4;
            rootNode = des.ReadClassPointer<hkxNode>(br);
            selectionSets = des.ReadClassPointerArray<hkxNodeSelectionSet>(br);
            cameras = des.ReadClassPointerArray<hkxCamera>(br);
            lights = des.ReadClassPointerArray<hkxLight>(br);
            meshes = des.ReadClassPointerArray<hkxMesh>(br);
            materials = des.ReadClassPointerArray<hkxMaterial>(br);
            inplaceTextures = des.ReadClassPointerArray<hkxTextureInplace>(br);
            externalTextures = des.ReadClassPointerArray<hkxTextureFile>(br);
            skinBindings = des.ReadClassPointerArray<hkxSkinBinding>(br);
            appliedTransform = des.ReadMatrix3(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, modeller);
            s.WriteStringPointer(bw, asset);
            bw.WriteSingle(sceneLength);
            bw.Position += 4;
            s.WriteClassPointer(bw, rootNode);
            s.WriteClassPointerArray(bw, selectionSets);
            s.WriteClassPointerArray(bw, cameras);
            s.WriteClassPointerArray(bw, lights);
            s.WriteClassPointerArray(bw, meshes);
            s.WriteClassPointerArray(bw, materials);
            s.WriteClassPointerArray(bw, inplaceTextures);
            s.WriteClassPointerArray(bw, externalTextures);
            s.WriteClassPointerArray(bw, skinBindings);
            s.WriteMatrix3(bw, appliedTransform);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            modeller = xd.ReadString(xe, nameof(modeller));
            asset = xd.ReadString(xe, nameof(asset));
            sceneLength = xd.ReadSingle(xe, nameof(sceneLength));
            rootNode = xd.ReadClassPointer<hkxNode>(this, xe, nameof(rootNode));
            selectionSets = xd.ReadClassPointerArray<hkxNodeSelectionSet>(this, xe, nameof(selectionSets));
            cameras = xd.ReadClassPointerArray<hkxCamera>(this, xe, nameof(cameras));
            lights = xd.ReadClassPointerArray<hkxLight>(this, xe, nameof(lights));
            meshes = xd.ReadClassPointerArray<hkxMesh>(this, xe, nameof(meshes));
            materials = xd.ReadClassPointerArray<hkxMaterial>(this, xe, nameof(materials));
            inplaceTextures = xd.ReadClassPointerArray<hkxTextureInplace>(this, xe, nameof(inplaceTextures));
            externalTextures = xd.ReadClassPointerArray<hkxTextureFile>(this, xe, nameof(externalTextures));
            skinBindings = xd.ReadClassPointerArray<hkxSkinBinding>(this, xe, nameof(skinBindings));
            appliedTransform = xd.ReadMatrix3(xe, nameof(appliedTransform));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(modeller), modeller);
            xs.WriteString(xe, nameof(asset), asset);
            xs.WriteFloat(xe, nameof(sceneLength), sceneLength);
            xs.WriteClassPointer(xe, nameof(rootNode), rootNode);
            xs.WriteClassPointerArray(xe, nameof(selectionSets), selectionSets!);
            xs.WriteClassPointerArray(xe, nameof(cameras), cameras!);
            xs.WriteClassPointerArray(xe, nameof(lights), lights!);
            xs.WriteClassPointerArray(xe, nameof(meshes), meshes!);
            xs.WriteClassPointerArray(xe, nameof(materials), materials!);
            xs.WriteClassPointerArray(xe, nameof(inplaceTextures), inplaceTextures!);
            xs.WriteClassPointerArray(xe, nameof(externalTextures), externalTextures!);
            xs.WriteClassPointerArray(xe, nameof(skinBindings), skinBindings!);
            xs.WriteMatrix3(xe, nameof(appliedTransform), appliedTransform);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxScene);
        }

        public bool Equals(hkxScene? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (modeller is null && other.modeller is null || modeller == other.modeller || modeller is null && other.modeller == "" || modeller == "" && other.modeller is null) &&
                   (asset is null && other.asset is null || asset == other.asset || asset is null && other.asset == "" || asset == "" && other.asset is null) &&
                   sceneLength.Equals(other.sceneLength) &&
                   ((rootNode is null && other.rootNode is null) || (rootNode is not null && other.rootNode is not null && rootNode.Equals((IHavokObject)other.rootNode))) &&
                   selectionSets.SequenceEqual(other.selectionSets) &&
                   cameras.SequenceEqual(other.cameras) &&
                   lights.SequenceEqual(other.lights) &&
                   meshes.SequenceEqual(other.meshes) &&
                   materials.SequenceEqual(other.materials) &&
                   inplaceTextures.SequenceEqual(other.inplaceTextures) &&
                   externalTextures.SequenceEqual(other.externalTextures) &&
                   skinBindings.SequenceEqual(other.skinBindings) &&
                   appliedTransform.Equals(other.appliedTransform) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(modeller);
            hashcode.Add(asset);
            hashcode.Add(sceneLength);
            hashcode.Add(rootNode);
            hashcode.Add(selectionSets.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(cameras.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(lights.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(meshes.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(materials.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(inplaceTextures.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(externalTextures.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(skinBindings.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(appliedTransform);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

