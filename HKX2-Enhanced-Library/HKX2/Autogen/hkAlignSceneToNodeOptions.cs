using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkAlignSceneToNodeOptions Signatire: 0x207cb01 size: 40 flags: FLAGS_NONE

    // invert class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // transformPositionX class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 17 flags: FLAGS_NONE enum: 
    // transformPositionY class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 18 flags: FLAGS_NONE enum: 
    // transformPositionZ class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 19 flags: FLAGS_NONE enum: 
    // transformRotation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // transformScale class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 21 flags: FLAGS_NONE enum: 
    // transformSkew class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 22 flags: FLAGS_NONE enum: 
    // keyframe class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // nodeName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkAlignSceneToNodeOptions : hkReferencedObject, IEquatable<hkAlignSceneToNodeOptions?>
    {
        public bool invert { set; get; }
        public bool transformPositionX { set; get; }
        public bool transformPositionY { set; get; }
        public bool transformPositionZ { set; get; }
        public bool transformRotation { set; get; }
        public bool transformScale { set; get; }
        public bool transformSkew { set; get; }
        public int keyframe { set; get; }
        public string nodeName { set; get; } = "";

        public override uint Signature { set; get; } = 0x207cb01;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            invert = br.ReadBoolean();
            transformPositionX = br.ReadBoolean();
            transformPositionY = br.ReadBoolean();
            transformPositionZ = br.ReadBoolean();
            transformRotation = br.ReadBoolean();
            transformScale = br.ReadBoolean();
            transformSkew = br.ReadBoolean();
            br.Position += 1;
            keyframe = br.ReadInt32();
            br.Position += 4;
            nodeName = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(invert);
            bw.WriteBoolean(transformPositionX);
            bw.WriteBoolean(transformPositionY);
            bw.WriteBoolean(transformPositionZ);
            bw.WriteBoolean(transformRotation);
            bw.WriteBoolean(transformScale);
            bw.WriteBoolean(transformSkew);
            bw.Position += 1;
            bw.WriteInt32(keyframe);
            bw.Position += 4;
            s.WriteStringPointer(bw, nodeName);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            invert = xd.ReadBoolean(xe, nameof(invert));
            transformPositionX = xd.ReadBoolean(xe, nameof(transformPositionX));
            transformPositionY = xd.ReadBoolean(xe, nameof(transformPositionY));
            transformPositionZ = xd.ReadBoolean(xe, nameof(transformPositionZ));
            transformRotation = xd.ReadBoolean(xe, nameof(transformRotation));
            transformScale = xd.ReadBoolean(xe, nameof(transformScale));
            transformSkew = xd.ReadBoolean(xe, nameof(transformSkew));
            keyframe = xd.ReadInt32(xe, nameof(keyframe));
            nodeName = xd.ReadString(xe, nameof(nodeName));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(invert), invert);
            xs.WriteBoolean(xe, nameof(transformPositionX), transformPositionX);
            xs.WriteBoolean(xe, nameof(transformPositionY), transformPositionY);
            xs.WriteBoolean(xe, nameof(transformPositionZ), transformPositionZ);
            xs.WriteBoolean(xe, nameof(transformRotation), transformRotation);
            xs.WriteBoolean(xe, nameof(transformScale), transformScale);
            xs.WriteBoolean(xe, nameof(transformSkew), transformSkew);
            xs.WriteNumber(xe, nameof(keyframe), keyframe);
            xs.WriteString(xe, nameof(nodeName), nodeName);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkAlignSceneToNodeOptions);
        }

        public bool Equals(hkAlignSceneToNodeOptions? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   invert.Equals(other.invert) &&
                   transformPositionX.Equals(other.transformPositionX) &&
                   transformPositionY.Equals(other.transformPositionY) &&
                   transformPositionZ.Equals(other.transformPositionZ) &&
                   transformRotation.Equals(other.transformRotation) &&
                   transformScale.Equals(other.transformScale) &&
                   transformSkew.Equals(other.transformSkew) &&
                   keyframe.Equals(other.keyframe) &&
                   (nodeName is null && other.nodeName is null || nodeName == other.nodeName || nodeName is null && other.nodeName == "" || nodeName == "" && other.nodeName is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(invert);
            hashcode.Add(transformPositionX);
            hashcode.Add(transformPositionY);
            hashcode.Add(transformPositionZ);
            hashcode.Add(transformRotation);
            hashcode.Add(transformScale);
            hashcode.Add(transformSkew);
            hashcode.Add(keyframe);
            hashcode.Add(nodeName);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

