using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkSimpleLocalFrame Signatire: 0xe758f63c size: 128 flags: FLAGS_NONE

    // transform class:  Type.TYPE_TRANSFORM Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // children class: hkLocalFrame Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // parentFrame class: hkLocalFrame Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: NOT_OWNED|FLAGS_NONE enum: 
    // group class: hkLocalFrameGroup Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkSimpleLocalFrame : hkLocalFrame, IEquatable<hkSimpleLocalFrame?>
    {
        public Matrix4x4 transform { set; get; }
        public IList<hkLocalFrame> children { set; get; } = Array.Empty<hkLocalFrame>();
        public hkLocalFrame? parentFrame { set; get; }
        public hkLocalFrameGroup? group { set; get; }
        public string name { set; get; } = "";

        public override uint Signature { set; get; } = 0xe758f63c;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            transform = des.ReadTransform(br);
            children = des.ReadClassPointerArray<hkLocalFrame>(br);
            parentFrame = des.ReadClassPointer<hkLocalFrame>(br);
            group = des.ReadClassPointer<hkLocalFrameGroup>(br);
            name = des.ReadStringPointer(br);
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteTransform(bw, transform);
            s.WriteClassPointerArray(bw, children);
            s.WriteClassPointer(bw, parentFrame);
            s.WriteClassPointer(bw, group);
            s.WriteStringPointer(bw, name);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            transform = xd.ReadTransform(xe, nameof(transform));
            children = xd.ReadClassPointerArray<hkLocalFrame>(this, xe, nameof(children));
            parentFrame = xd.ReadClassPointer<hkLocalFrame>(this, xe, nameof(parentFrame));
            group = xd.ReadClassPointer<hkLocalFrameGroup>(this, xe, nameof(group));
            name = xd.ReadString(xe, nameof(name));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteTransform(xe, nameof(transform), transform);
            xs.WriteClassPointerArray(xe, nameof(children), children!);
            xs.WriteClassPointer(xe, nameof(parentFrame), parentFrame);
            xs.WriteClassPointer(xe, nameof(group), group);
            xs.WriteString(xe, nameof(name), name);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkSimpleLocalFrame);
        }

        public bool Equals(hkSimpleLocalFrame? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   transform.Equals(other.transform) &&
                   children.SequenceEqual(other.children) &&
                   ((parentFrame is null && other.parentFrame is null) || (parentFrame is not null && other.parentFrame is not null && parentFrame.Equals((IHavokObject)other.parentFrame))) &&
                   ((group is null && other.group is null) || (group is not null && other.group is not null && group.Equals((IHavokObject)other.group))) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(transform);
            hashcode.Add(children.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(parentFrame);
            hashcode.Add(group);
            hashcode.Add(name);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

