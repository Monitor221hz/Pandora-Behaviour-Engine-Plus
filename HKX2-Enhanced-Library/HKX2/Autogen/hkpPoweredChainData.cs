using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPoweredChainData Signatire: 0x38aeafc3 size: 96 flags: FLAGS_NONE

    // atoms class: hkpBridgeAtoms Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // infos class: hkpPoweredChainDataConstraintInfo Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // tau class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // damping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // cfmLinAdd class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // cfmLinMul class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 76 flags: FLAGS_NONE enum: 
    // cfmAngAdd class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // cfmAngMul class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // maxErrorDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    public partial class hkpPoweredChainData : hkpConstraintChainData, IEquatable<hkpPoweredChainData?>
    {
        public hkpBridgeAtoms atoms { set; get; } = new();
        public IList<hkpPoweredChainDataConstraintInfo> infos { set; get; } = Array.Empty<hkpPoweredChainDataConstraintInfo>();
        public float tau { set; get; }
        public float damping { set; get; }
        public float cfmLinAdd { set; get; }
        public float cfmLinMul { set; get; }
        public float cfmAngAdd { set; get; }
        public float cfmAngMul { set; get; }
        public float maxErrorDistance { set; get; }

        public override uint Signature { set; get; } = 0x38aeafc3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            atoms.Read(des, br);
            infos = des.ReadClassArray<hkpPoweredChainDataConstraintInfo>(br);
            tau = br.ReadSingle();
            damping = br.ReadSingle();
            cfmLinAdd = br.ReadSingle();
            cfmLinMul = br.ReadSingle();
            cfmAngAdd = br.ReadSingle();
            cfmAngMul = br.ReadSingle();
            maxErrorDistance = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            atoms.Write(s, bw);
            s.WriteClassArray(bw, infos);
            bw.WriteSingle(tau);
            bw.WriteSingle(damping);
            bw.WriteSingle(cfmLinAdd);
            bw.WriteSingle(cfmLinMul);
            bw.WriteSingle(cfmAngAdd);
            bw.WriteSingle(cfmAngMul);
            bw.WriteSingle(maxErrorDistance);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            atoms = xd.ReadClass<hkpBridgeAtoms>(xe, nameof(atoms));
            infos = xd.ReadClassArray<hkpPoweredChainDataConstraintInfo>(xe, nameof(infos));
            tau = xd.ReadSingle(xe, nameof(tau));
            damping = xd.ReadSingle(xe, nameof(damping));
            cfmLinAdd = xd.ReadSingle(xe, nameof(cfmLinAdd));
            cfmLinMul = xd.ReadSingle(xe, nameof(cfmLinMul));
            cfmAngAdd = xd.ReadSingle(xe, nameof(cfmAngAdd));
            cfmAngMul = xd.ReadSingle(xe, nameof(cfmAngMul));
            maxErrorDistance = xd.ReadSingle(xe, nameof(maxErrorDistance));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpBridgeAtoms>(xe, nameof(atoms), atoms);
            xs.WriteClassArray(xe, nameof(infos), infos);
            xs.WriteFloat(xe, nameof(tau), tau);
            xs.WriteFloat(xe, nameof(damping), damping);
            xs.WriteFloat(xe, nameof(cfmLinAdd), cfmLinAdd);
            xs.WriteFloat(xe, nameof(cfmLinMul), cfmLinMul);
            xs.WriteFloat(xe, nameof(cfmAngAdd), cfmAngAdd);
            xs.WriteFloat(xe, nameof(cfmAngMul), cfmAngMul);
            xs.WriteFloat(xe, nameof(maxErrorDistance), maxErrorDistance);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPoweredChainData);
        }

        public bool Equals(hkpPoweredChainData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((atoms is null && other.atoms is null) || (atoms is not null && other.atoms is not null && atoms.Equals((IHavokObject)other.atoms))) &&
                   infos.SequenceEqual(other.infos) &&
                   tau.Equals(other.tau) &&
                   damping.Equals(other.damping) &&
                   cfmLinAdd.Equals(other.cfmLinAdd) &&
                   cfmLinMul.Equals(other.cfmLinMul) &&
                   cfmAngAdd.Equals(other.cfmAngAdd) &&
                   cfmAngMul.Equals(other.cfmAngMul) &&
                   maxErrorDistance.Equals(other.maxErrorDistance) &&
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
            hashcode.Add(cfmLinAdd);
            hashcode.Add(cfmLinMul);
            hashcode.Add(cfmAngAdd);
            hashcode.Add(cfmAngMul);
            hashcode.Add(maxErrorDistance);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

