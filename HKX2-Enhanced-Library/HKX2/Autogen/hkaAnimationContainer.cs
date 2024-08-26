using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaAnimationContainer Signatire: 0x8dc20333 size: 96 flags: FLAGS_NONE

    // skeletons class: hkaSkeleton Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // animations class: hkaAnimation Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // bindings class: hkaAnimationBinding Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // attachments class: hkaBoneAttachment Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // skins class: hkaMeshBinding Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkaAnimationContainer : hkReferencedObject, IEquatable<hkaAnimationContainer?>
    {
        public IList<hkaSkeleton> skeletons { set; get; } = Array.Empty<hkaSkeleton>();
        public IList<hkaAnimation> animations { set; get; } = Array.Empty<hkaAnimation>();
        public IList<hkaAnimationBinding> bindings { set; get; } = Array.Empty<hkaAnimationBinding>();
        public IList<hkaBoneAttachment> attachments { set; get; } = Array.Empty<hkaBoneAttachment>();
        public IList<hkaMeshBinding> skins { set; get; } = Array.Empty<hkaMeshBinding>();

        public override uint Signature { set; get; } = 0x8dc20333;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            skeletons = des.ReadClassPointerArray<hkaSkeleton>(br);
            animations = des.ReadClassPointerArray<hkaAnimation>(br);
            bindings = des.ReadClassPointerArray<hkaAnimationBinding>(br);
            attachments = des.ReadClassPointerArray<hkaBoneAttachment>(br);
            skins = des.ReadClassPointerArray<hkaMeshBinding>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, skeletons);
            s.WriteClassPointerArray(bw, animations);
            s.WriteClassPointerArray(bw, bindings);
            s.WriteClassPointerArray(bw, attachments);
            s.WriteClassPointerArray(bw, skins);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            skeletons = xd.ReadClassPointerArray<hkaSkeleton>(this, xe, nameof(skeletons));
            animations = xd.ReadClassPointerArray<hkaAnimation>(this, xe, nameof(animations));
            bindings = xd.ReadClassPointerArray<hkaAnimationBinding>(this, xe, nameof(bindings));
            attachments = xd.ReadClassPointerArray<hkaBoneAttachment>(this, xe, nameof(attachments));
            skins = xd.ReadClassPointerArray<hkaMeshBinding>(this, xe, nameof(skins));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(skeletons), skeletons!);
            xs.WriteClassPointerArray(xe, nameof(animations), animations!);
            xs.WriteClassPointerArray(xe, nameof(bindings), bindings!);
            xs.WriteClassPointerArray(xe, nameof(attachments), attachments!);
            xs.WriteClassPointerArray(xe, nameof(skins), skins!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaAnimationContainer);
        }

        public bool Equals(hkaAnimationContainer? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   skeletons.SequenceEqual(other.skeletons) &&
                   animations.SequenceEqual(other.animations) &&
                   bindings.SequenceEqual(other.bindings) &&
                   attachments.SequenceEqual(other.attachments) &&
                   skins.SequenceEqual(other.skins) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(skeletons.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(animations.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(bindings.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(attachments.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(skins.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

