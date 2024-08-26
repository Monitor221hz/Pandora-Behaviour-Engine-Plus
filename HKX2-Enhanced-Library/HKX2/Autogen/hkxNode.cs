using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxNode Signatire: 0x5a218502 size: 112 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // object class: hkReferencedObject Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // keyFrames class:  Type.TYPE_ARRAY Type.TYPE_MATRIX4 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // children class: hkxNode Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // annotations class: hkxNodeAnnotationData Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // userProperties class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // selected class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    public partial class hkxNode : hkxAttributeHolder, IEquatable<hkxNode?>
    {
        public string name { set; get; } = "";
        public hkReferencedObject? @object { set; get; }
        public IList<Matrix4x4> keyFrames { set; get; } = Array.Empty<Matrix4x4>();
        public IList<hkxNode> children { set; get; } = Array.Empty<hkxNode>();
        public IList<hkxNodeAnnotationData> annotations { set; get; } = Array.Empty<hkxNodeAnnotationData>();
        public string userProperties { set; get; } = "";
        public bool selected { set; get; }

        public override uint Signature { set; get; } = 0x5a218502;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            name = des.ReadStringPointer(br);
            @object = des.ReadClassPointer<hkReferencedObject>(br);
            keyFrames = des.ReadMatrix4Array(br);
            children = des.ReadClassPointerArray<hkxNode>(br);
            annotations = des.ReadClassArray<hkxNodeAnnotationData>(br);
            userProperties = des.ReadStringPointer(br);
            selected = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, name);
            s.WriteClassPointer(bw, @object);
            s.WriteMatrix4Array(bw, keyFrames);
            s.WriteClassPointerArray(bw, children);
            s.WriteClassArray(bw, annotations);
            s.WriteStringPointer(bw, userProperties);
            bw.WriteBoolean(selected);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
            @object = xd.ReadClassPointer<hkReferencedObject>(this, xe, nameof(@object));
            keyFrames = xd.ReadMatrix4Array(xe, nameof(keyFrames));
            children = xd.ReadClassPointerArray<hkxNode>(this, xe, nameof(children));
            annotations = xd.ReadClassArray<hkxNodeAnnotationData>(xe, nameof(annotations));
            userProperties = xd.ReadString(xe, nameof(userProperties));
            selected = xd.ReadBoolean(xe, nameof(selected));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteClassPointer(xe, nameof(@object), @object);
            xs.WriteMatrix4Array(xe, nameof(keyFrames), keyFrames);
            xs.WriteClassPointerArray(xe, nameof(children), children!);
            xs.WriteClassArray(xe, nameof(annotations), annotations);
            xs.WriteString(xe, nameof(userProperties), userProperties);
            xs.WriteBoolean(xe, nameof(selected), selected);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxNode);
        }

        public bool Equals(hkxNode? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   ((@object is null && other.@object is null) || (@object is not null && other.@object is not null && @object.Equals((IHavokObject)other.@object))) &&
                   keyFrames.SequenceEqual(other.keyFrames) &&
                   children.SequenceEqual(other.children) &&
                   annotations.SequenceEqual(other.annotations) &&
                   (userProperties is null && other.userProperties is null || userProperties == other.userProperties || userProperties is null && other.userProperties == "" || userProperties == "" && other.userProperties is null) &&
                   selected.Equals(other.selected) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name);
            hashcode.Add(@object);
            hashcode.Add(keyFrames.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(children.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(annotations.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(userProperties);
            hashcode.Add(selected);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

