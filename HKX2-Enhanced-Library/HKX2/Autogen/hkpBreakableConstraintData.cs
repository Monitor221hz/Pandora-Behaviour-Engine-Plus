using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBreakableConstraintData Signatire: 0x7d6310c8 size: 72 flags: FLAGS_NONE

    // atoms class: hkpBridgeAtoms Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // constraintData class: hkpConstraintData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // childRuntimeSize class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // childNumSolverResults class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 58 flags: FLAGS_NONE enum: 
    // solverResultLimit class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 60 flags: FLAGS_NONE enum: 
    // removeWhenBroken class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // revertBackVelocityOnBreak class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 65 flags: FLAGS_NONE enum: 
    public partial class hkpBreakableConstraintData : hkpConstraintData, IEquatable<hkpBreakableConstraintData?>
    {
        public hkpBridgeAtoms atoms { set; get; } = new();
        public hkpConstraintData? constraintData { set; get; }
        public ushort childRuntimeSize { set; get; }
        public ushort childNumSolverResults { set; get; }
        public float solverResultLimit { set; get; }
        public bool removeWhenBroken { set; get; }
        public bool revertBackVelocityOnBreak { set; get; }

        public override uint Signature { set; get; } = 0x7d6310c8;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            atoms.Read(des, br);
            constraintData = des.ReadClassPointer<hkpConstraintData>(br);
            childRuntimeSize = br.ReadUInt16();
            childNumSolverResults = br.ReadUInt16();
            solverResultLimit = br.ReadSingle();
            removeWhenBroken = br.ReadBoolean();
            revertBackVelocityOnBreak = br.ReadBoolean();
            br.Position += 6;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            atoms.Write(s, bw);
            s.WriteClassPointer(bw, constraintData);
            bw.WriteUInt16(childRuntimeSize);
            bw.WriteUInt16(childNumSolverResults);
            bw.WriteSingle(solverResultLimit);
            bw.WriteBoolean(removeWhenBroken);
            bw.WriteBoolean(revertBackVelocityOnBreak);
            bw.Position += 6;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            atoms = xd.ReadClass<hkpBridgeAtoms>(xe, nameof(atoms));
            constraintData = xd.ReadClassPointer<hkpConstraintData>(this, xe, nameof(constraintData));
            childRuntimeSize = xd.ReadUInt16(xe, nameof(childRuntimeSize));
            childNumSolverResults = xd.ReadUInt16(xe, nameof(childNumSolverResults));
            solverResultLimit = xd.ReadSingle(xe, nameof(solverResultLimit));
            removeWhenBroken = xd.ReadBoolean(xe, nameof(removeWhenBroken));
            revertBackVelocityOnBreak = xd.ReadBoolean(xe, nameof(revertBackVelocityOnBreak));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpBridgeAtoms>(xe, nameof(atoms), atoms);
            xs.WriteClassPointer(xe, nameof(constraintData), constraintData);
            xs.WriteNumber(xe, nameof(childRuntimeSize), childRuntimeSize);
            xs.WriteNumber(xe, nameof(childNumSolverResults), childNumSolverResults);
            xs.WriteFloat(xe, nameof(solverResultLimit), solverResultLimit);
            xs.WriteBoolean(xe, nameof(removeWhenBroken), removeWhenBroken);
            xs.WriteBoolean(xe, nameof(revertBackVelocityOnBreak), revertBackVelocityOnBreak);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBreakableConstraintData);
        }

        public bool Equals(hkpBreakableConstraintData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((atoms is null && other.atoms is null) || (atoms is not null && other.atoms is not null && atoms.Equals((IHavokObject)other.atoms))) &&
                   ((constraintData is null && other.constraintData is null) || (constraintData is not null && other.constraintData is not null && constraintData.Equals((IHavokObject)other.constraintData))) &&
                   childRuntimeSize.Equals(other.childRuntimeSize) &&
                   childNumSolverResults.Equals(other.childNumSolverResults) &&
                   solverResultLimit.Equals(other.solverResultLimit) &&
                   removeWhenBroken.Equals(other.removeWhenBroken) &&
                   revertBackVelocityOnBreak.Equals(other.revertBackVelocityOnBreak) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(atoms);
            hashcode.Add(constraintData);
            hashcode.Add(childRuntimeSize);
            hashcode.Add(childNumSolverResults);
            hashcode.Add(solverResultLimit);
            hashcode.Add(removeWhenBroken);
            hashcode.Add(revertBackVelocityOnBreak);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

