using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBlenderGeneratorChild Signatire: 0xe2b384b0 size: 80 flags: FLAGS_NONE

    // generator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: ALIGN_16|FLAGS_NONE enum: 
    // boneWeights class: hkbBoneWeightArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // weight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // worldFromModelWeight class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    public partial class hkbBlenderGeneratorChild : hkbBindable, IEquatable<hkbBlenderGeneratorChild?>
    {
        public hkbGenerator? generator { set; get; }
        public hkbBoneWeightArray? boneWeights { set; get; }
        public float weight { set; get; }
        public float worldFromModelWeight { set; get; }

        public override uint Signature { set; get; } = 0xe2b384b0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            generator = des.ReadClassPointer<hkbGenerator>(br);
            boneWeights = des.ReadClassPointer<hkbBoneWeightArray>(br);
            weight = br.ReadSingle();
            worldFromModelWeight = br.ReadSingle();
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, generator);
            s.WriteClassPointer(bw, boneWeights);
            bw.WriteSingle(weight);
            bw.WriteSingle(worldFromModelWeight);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            generator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(generator));
            boneWeights = xd.ReadClassPointer<hkbBoneWeightArray>(this, xe, nameof(boneWeights));
            weight = xd.ReadSingle(xe, nameof(weight));
            worldFromModelWeight = xd.ReadSingle(xe, nameof(worldFromModelWeight));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(generator), generator);
            xs.WriteClassPointer(xe, nameof(boneWeights), boneWeights);
            xs.WriteFloat(xe, nameof(weight), weight);
            xs.WriteFloat(xe, nameof(worldFromModelWeight), worldFromModelWeight);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBlenderGeneratorChild);
        }

        public bool Equals(hkbBlenderGeneratorChild? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((generator is null && other.generator is null) || (generator is not null && other.generator is not null && generator.Equals((IHavokObject)other.generator))) &&
                   ((boneWeights is null && other.boneWeights is null) || (boneWeights is not null && other.boneWeights is not null && boneWeights.Equals((IHavokObject)other.boneWeights))) &&
                   weight.Equals(other.weight) &&
                   worldFromModelWeight.Equals(other.worldFromModelWeight) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(generator);
            hashcode.Add(boneWeights);
            hashcode.Add(weight);
            hashcode.Add(worldFromModelWeight);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

