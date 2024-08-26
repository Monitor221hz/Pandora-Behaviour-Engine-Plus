using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpStiffSpringChainData Signatire: 0xf170356b size: 80 flags: FLAGS_NONE

    // atoms class: hkpBridgeAtoms Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // infos class: hkpStiffSpringChainDataConstraintInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // tau class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // damping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // cfm class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    public partial class hkpStiffSpringChainData : hkpConstraintChainData, IEquatable<hkpStiffSpringChainData?>
    {
        public hkpBridgeAtoms atoms { set; get; } = new();
        public IList<hkpStiffSpringChainDataConstraintInfo> infos { set; get; } = Array.Empty<hkpStiffSpringChainDataConstraintInfo>();
        public float tau { set; get; }
        public float damping { set; get; }
        public float cfm { set; get; }

        public override uint Signature { set; get; } = 0xf170356b;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            atoms.Read(des, br);
            infos = des.ReadClassArray<hkpStiffSpringChainDataConstraintInfo>(br);
            tau = br.ReadSingle();
            damping = br.ReadSingle();
            cfm = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            atoms.Write(s, bw);
            s.WriteClassArray(bw, infos);
            bw.WriteSingle(tau);
            bw.WriteSingle(damping);
            bw.WriteSingle(cfm);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            atoms = xd.ReadClass<hkpBridgeAtoms>(xe, nameof(atoms));
            infos = xd.ReadClassArray<hkpStiffSpringChainDataConstraintInfo>(xe, nameof(infos));
            tau = xd.ReadSingle(xe, nameof(tau));
            damping = xd.ReadSingle(xe, nameof(damping));
            cfm = xd.ReadSingle(xe, nameof(cfm));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpBridgeAtoms>(xe, nameof(atoms), atoms);
            xs.WriteClassArray(xe, nameof(infos), infos);
            xs.WriteFloat(xe, nameof(tau), tau);
            xs.WriteFloat(xe, nameof(damping), damping);
            xs.WriteFloat(xe, nameof(cfm), cfm);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpStiffSpringChainData);
        }

        public bool Equals(hkpStiffSpringChainData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((atoms is null && other.atoms is null) || (atoms is not null && other.atoms is not null && atoms.Equals((IHavokObject)other.atoms))) &&
                   infos.SequenceEqual(other.infos) &&
                   tau.Equals(other.tau) &&
                   damping.Equals(other.damping) &&
                   cfm.Equals(other.cfm) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(atoms);
            hashcode.Add(infos.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(tau);
            hashcode.Add(damping);
            hashcode.Add(cfm);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

