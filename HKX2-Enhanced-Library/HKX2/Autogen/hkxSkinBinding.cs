using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxSkinBinding Signatire: 0x5a93f338 size: 128 flags: FLAGS_NONE

    // mesh class: hkxMesh Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // nodeNames class:  Type.TYPE_ARRAY Type.TYPE_STRINGPTR arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // bindPose class:  Type.TYPE_ARRAY Type.TYPE_MATRIX4 arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // initSkinTransform class:  Type.TYPE_MATRIX4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkxSkinBinding : hkReferencedObject, IEquatable<hkxSkinBinding?>
    {
        public hkxMesh? mesh { set; get; }
        public IList<string> nodeNames { set; get; } = Array.Empty<string>();
        public IList<Matrix4x4> bindPose { set; get; } = Array.Empty<Matrix4x4>();
        public Matrix4x4 initSkinTransform { set; get; }

        public override uint Signature { set; get; } = 0x5a93f338;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            mesh = des.ReadClassPointer<hkxMesh>(br);
            nodeNames = des.ReadStringPointerArray(br);
            bindPose = des.ReadMatrix4Array(br);
            br.Position += 8;
            initSkinTransform = des.ReadMatrix4(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, mesh);
            s.WriteStringPointerArray(bw, nodeNames);
            s.WriteMatrix4Array(bw, bindPose);
            bw.Position += 8;
            s.WriteMatrix4(bw, initSkinTransform);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            mesh = xd.ReadClassPointer<hkxMesh>(this, xe, nameof(mesh));
            nodeNames = xd.ReadStringArray(xe, nameof(nodeNames));
            bindPose = xd.ReadMatrix4Array(xe, nameof(bindPose));
            initSkinTransform = xd.ReadMatrix4(xe, nameof(initSkinTransform));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(mesh), mesh);
            xs.WriteStringArray(xe, nameof(nodeNames), nodeNames);
            xs.WriteMatrix4Array(xe, nameof(bindPose), bindPose);
            xs.WriteMatrix4(xe, nameof(initSkinTransform), initSkinTransform);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxSkinBinding);
        }

        public bool Equals(hkxSkinBinding? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((mesh is null && other.mesh is null) || (mesh is not null && other.mesh is not null && mesh.Equals((IHavokObject)other.mesh))) &&
                   nodeNames.SequenceEqual(other.nodeNames) &&
                   bindPose.SequenceEqual(other.bindPose) &&
                   initSkinTransform.Equals(other.initSkinTransform) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(mesh);
            hashcode.Add(nodeNames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(bindPose.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(initSkinTransform);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

