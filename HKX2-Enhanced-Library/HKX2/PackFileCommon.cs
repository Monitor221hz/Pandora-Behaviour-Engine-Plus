using System;
using System.Collections.Generic;

namespace HKX2E
{
    public class HKXHeader
    {
        public byte BaseClass;
        public int ContentsClassNameSectionIndex;
        public int ContentsClassNameSectionOffset;
        public int ContentsSectionIndex;
        public int ContentsSectionOffset;
        public string ContentsVersionString;
        public byte Endian;
        public int FileVersion;
        public int Flags;
        public uint Magic0;
        public uint Magic1;
        public short MaxPredicate;
        public byte PaddingOption;
        public byte PointerSize;
        public int SectionCount;
        public short SectionOffset;
        public short Unk40;
        public short Unk42;
        public uint Unk44;
        public uint Unk48;
        public uint Unk4C;
        public int UserTag;

        private HKXHeader()
        {
        }

        internal HKXHeader(BinaryReaderEx br)
        {
            Magic0 = br.AssertUInt32(0x57E0E057);
            Magic1 = br.AssertUInt32(0x10C0C010);
            UserTag = br.ReadInt32();
            FileVersion = br.AssertInt32(0x0B, 0x08);
            PointerSize = br.AssertByte(4, 8);
            Endian = br.AssertByte(0, 1);
            PaddingOption = br.AssertByte(0, 1);
            BaseClass = br.AssertByte(1);
            SectionCount = br.AssertInt32(3);
            ContentsSectionIndex = br.ReadInt32();
            ContentsSectionOffset = br.ReadInt32();
            ContentsClassNameSectionIndex = br.ReadInt32();
            ContentsClassNameSectionOffset = br.ReadInt32();
            ContentsVersionString = br.ReadFixStr(16);
            Flags = br.ReadInt32();
            MaxPredicate = br.ReadInt16();
            SectionOffset = br.ReadInt16();

            if (SectionOffset != 16) return;

            Unk40 = br.ReadInt16();
            Unk42 = br.ReadInt16();
            Unk44 = br.ReadUInt32();
            Unk48 = br.ReadUInt32();
            Unk4C = br.ReadUInt32();
        }

        internal void Write(BinaryWriterEx bw)
        {
            bw.WriteUInt32(Magic0);
            bw.WriteUInt32(Magic1);
            bw.WriteInt32(UserTag);
            bw.WriteInt32(FileVersion);
            bw.WriteByte(PointerSize);
            bw.WriteByte(Endian);
            bw.WriteByte(PaddingOption);
            bw.WriteByte(BaseClass);
            bw.WriteInt32(SectionCount);

            bw.WriteInt32(ContentsSectionIndex);
            bw.WriteInt32(ContentsSectionOffset);
            bw.WriteInt32(ContentsClassNameSectionIndex);
            bw.WriteInt32(ContentsClassNameSectionOffset);
            bw.WriteFixStr(ContentsVersionString, 16, 0xFF);
            bw.WriteInt32(Flags);
            bw.WriteInt16(MaxPredicate);
            bw.WriteInt16(SectionOffset);

            if (SectionOffset != 16) return;

            bw.WriteInt16(Unk40);
            bw.WriteInt16(Unk42);
            bw.WriteUInt32(Unk44);
            bw.WriteUInt32(Unk48);
            bw.WriteUInt32(Unk4C);
        }

        public static HKXHeader BotwWiiu()
        {
            return new HKXHeader
            {
                Magic0 = 0x57E0E057,
                Magic1 = 0x10C0C010,
                UserTag = 0,
                FileVersion = 0x0B,
                PointerSize = 4,
                Endian = 0,
                PaddingOption = 0,
                BaseClass = 1,
                SectionCount = 3,
                ContentsSectionIndex = 2,
                ContentsSectionOffset = 0,
                ContentsClassNameSectionIndex = 0,
                ContentsClassNameSectionOffset = 0x4B,
                ContentsVersionString = "hk_2014.2.0-r1",
                Flags = 0,
                MaxPredicate = 21,
                SectionOffset = 0,
                Unk40 = 20,
                Unk42 = 0,
                Unk44 = 0,
                Unk48 = 0,
                Unk4C = 0
            };
        }

        public static HKXHeader BotwNx()
        {
            return new HKXHeader
            {
                Magic0 = 0x57E0E057,
                Magic1 = 0x10C0C010,
                UserTag = 0,
                FileVersion = 0x0B,
                PointerSize = 8,
                Endian = 1,
                PaddingOption = 1,
                BaseClass = 1,
                SectionCount = 3,
                ContentsSectionIndex = 2,
                ContentsSectionOffset = 0,
                ContentsClassNameSectionIndex = 0,
                ContentsClassNameSectionOffset = 0x4B,
                ContentsVersionString = "hk_2014.2.0-r1",
                Flags = 0,
                MaxPredicate = 21,
                SectionOffset = 0,
                Unk40 = 20,
                Unk42 = 0,
                Unk44 = 0,
                Unk48 = 0,
                Unk4C = 0
            };
        }

        public static HKXHeader SkyrimSE()
        {
            return new HKXHeader
            {
                Magic0 = 0x57E0E057,
                Magic1 = 0x10C0C010,
                UserTag = 0,
                FileVersion = 0x08,
                PointerSize = 8,
                Endian = 1,
                PaddingOption = 0,
                BaseClass = 1,
                SectionCount = 3,
                ContentsSectionIndex = 2,
                ContentsSectionOffset = 0,
                ContentsClassNameSectionIndex = 0,
                ContentsClassNameSectionOffset = 0x4B,
                ContentsVersionString = "hk_2010.2.0-r1",
                Flags = 0,
                MaxPredicate = -1,
                SectionOffset = -1,
                Unk40 = 0,
                Unk42 = 0,
                Unk44 = 0,
                Unk48 = 0,
                Unk4C = 0
            };
        }
    }

    public interface Fixup
    {
    }

    public class LocalFixup : Fixup
    {
        public LocalFixup()
        {
        }

        internal LocalFixup(BinaryReaderEx br)
        {
            Src = br.ReadUInt32();
            Dst = br.ReadUInt32();
        }

        public uint Src { get; set; }
        public uint Dst { get; set; }

        internal void Write(BinaryWriterEx bw)
        {
            bw.WriteUInt32(Src);
            bw.WriteUInt32(Dst);
        }

        private bool Equals(LocalFixup other)
        {
            return Src == other.Src && Dst == other.Dst;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((LocalFixup)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Src, Dst);
        }
    }

    public class GlobalFixup : Fixup
    {
        public GlobalFixup()
        {
        }

        internal GlobalFixup(BinaryReaderEx br)
        {
            Src = br.ReadUInt32();
            DstSectionIndex = br.ReadUInt32();
            Dst = br.ReadUInt32();
        }

        public uint Src { get; set; }
        public uint DstSectionIndex { get; set; }
        public uint Dst { get; set; }

        internal void Write(BinaryWriterEx bw)
        {
            bw.WriteUInt32(Src);
            bw.WriteUInt32(DstSectionIndex);
            bw.WriteUInt32(Dst);
        }

        private bool Equals(GlobalFixup other)
        {
            return Src == other.Src && Dst == other.Dst;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((GlobalFixup)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Src, Dst);
        }
    }

    public class VirtualFixup : Fixup
    {
        internal VirtualFixup()
        {
        }

        internal VirtualFixup(BinaryReaderEx br)
        {
            Src = br.ReadUInt32();
            DstSectionIndex = br.ReadUInt32();
            Dst = br.ReadUInt32();
        }

        public uint Src { get; set; }
        public uint DstSectionIndex { get; set; }
        public uint Dst { get; set; }

        internal void Write(BinaryWriterEx bw)
        {
            bw.WriteUInt32(Src);
            bw.WriteUInt32(DstSectionIndex);
            bw.WriteUInt32(Dst);
        }

        private bool Equals(VirtualFixup other)
        {
            return Src == other.Src && Dst == other.Dst;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((VirtualFixup)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Src, Dst);
        }
    }

    public class HKXClassName
    {
        public string ClassName;
        public uint Signature;

        internal HKXClassName()
        {
        }

        internal HKXClassName(BinaryReaderEx br)
        {
            Signature = br.ReadUInt32();
            br.AssertByte(0x09); // Seems random but ok
            ClassName = br.ReadASCII();
        }

        public void Write(BinaryWriterEx bw)
        {
            bw.WriteUInt32(Signature);
            bw.WriteByte(0x09);
            bw.WriteASCII(ClassName, true);
        }
    }

    // Class names data found in the __classnames__ section of the hkx
    internal class HKXClassNames
    {
        private List<HKXClassName> ClassNames;
        public Dictionary<uint, HKXClassName> OffsetClassNamesMap;

        public void Read(BinaryReaderEx br)
        {
            ClassNames = new List<HKXClassName>();
            OffsetClassNamesMap = new Dictionary<uint, HKXClassName>();
            while (true)
            {
                if (br.Position >= br.Length || br.Position + 5 >= br.Length)
                {
                    break;
                }

                br.ReadUInt32(); // signature
                var separator = br.ReadByte();
                if (separator != 0x09)
                {
                    break;
                }
                br.Position -= 5;

                var stringStart = (uint)br.Position + 5;
                var className = new HKXClassName(br);
                ClassNames.Add(className);
                OffsetClassNamesMap.Add(stringStart, className);
                if (br.Position == br.Length) break;
            }
        }
    }

    public class HKXSection
    {
        public readonly Dictionary<uint, GlobalFixup> _globalMap = new();

        public readonly Dictionary<uint, LocalFixup> _localMap = new();
        public readonly Dictionary<uint, VirtualFixup> _virtualMap = new();
        public List<GlobalFixup> GlobalFixups = new();

        public List<LocalFixup> LocalFixups = new();

        public byte[] SectionData;
        public int SectionID;

        public string SectionTag;
        public List<VirtualFixup> VirtualFixups = new();

        public string ContentsVersionString;

        internal HKXSection()
        {
        }

        internal HKXSection(BinaryReaderEx br, string ContentsVersionString)
        {
            SectionTag = br.ReadFixStr(19);
            br.AssertByte(0xFF);
            var AbsoluteDataStart = br.ReadUInt32();
            var LocalFixupsOffset = br.ReadUInt32();
            var GlobalFixupsOffset = br.ReadUInt32();
            var VirtualFixupsOffset = br.ReadUInt32();
            var ExportsOffset = br.ReadUInt32();
            var ImportsOffset = br.ReadUInt32();
            var EndOffset = br.ReadUInt32();

            // Read Data
            br.StepIn(AbsoluteDataStart);
            SectionData = br.ReadBytes((int)LocalFixupsOffset);
            br.StepOut();

            // Local fixups
            LocalFixups = new List<LocalFixup>();
            br.StepIn(AbsoluteDataStart + LocalFixupsOffset);
            for (var i = 0; i < (GlobalFixupsOffset - LocalFixupsOffset) / 8; i++)
                if (br.ReadUInt32() != 0xFFFFFFFF)
                {
                    br.Position -= 4;
                    var f = new LocalFixup(br);
                    _localMap.Add(f.Src, f);
                    LocalFixups.Add(f);
                }

            br.StepOut();

            // Global fixups
            GlobalFixups = new List<GlobalFixup>();
            br.StepIn(AbsoluteDataStart + GlobalFixupsOffset);
            for (var i = 0; i < (VirtualFixupsOffset - GlobalFixupsOffset) / 12; i++)
                if (br.ReadUInt32() != 0xFFFFFFFF)
                {
                    br.Position -= 4;
                    var f = new GlobalFixup(br);
                    _globalMap.Add(f.Src, f);
                    GlobalFixups.Add(f);
                }

            br.StepOut();

            // Virtual fixups
            VirtualFixups = new List<VirtualFixup>();
            br.StepIn(AbsoluteDataStart + VirtualFixupsOffset);
            for (var i = 0; i < (ExportsOffset - VirtualFixupsOffset) / 12; i++)
                if (br.ReadUInt32() != 0xFFFFFFFF)
                {
                    br.Position -= 4;
                    var f = new VirtualFixup(br);
                    _virtualMap.Add(f.Src, f);
                    VirtualFixups.Add(f);
                }

            br.StepOut();

            // skyrim se dont have padding?
            if (ContentsVersionString == "hk_2010.2.0-r1") return;
            br.AssertUInt32(0xFFFFFFFF);
            br.AssertUInt32(0xFFFFFFFF);
            br.AssertUInt32(0xFFFFFFFF);
            br.AssertUInt32(0xFFFFFFFF);
        }

        public void WriteHeader(BinaryWriterEx bw)
        {
            bw.WriteFixStr(SectionTag, 19);
            bw.WriteByte(0xFF);
            bw.ReserveUInt32("absoffset" + SectionID);
            bw.ReserveUInt32("locoffset" + SectionID);
            bw.ReserveUInt32("globoffset" + SectionID);
            bw.ReserveUInt32("virtoffset" + SectionID);
            bw.ReserveUInt32("expoffset" + SectionID);
            bw.ReserveUInt32("impoffset" + SectionID);
            bw.ReserveUInt32("endoffset" + SectionID);

            // skyrim se dont have padding?
            if (ContentsVersionString == "hk_2010.2.0-r1") return;
            bw.WriteUInt32(0xFFFFFFFF);
            bw.WriteUInt32(0xFFFFFFFF);
            bw.WriteUInt32(0xFFFFFFFF);
            bw.WriteUInt32(0xFFFFFFFF);
        }

        public void WriteData(BinaryWriterEx bw)
        {
            var absoluteOffset = (uint)bw.Position;
            bw.FillUInt32("absoffset" + SectionID, absoluteOffset);
            bw.WriteBytes(SectionData);
            while (bw.Position % 16 != 0) bw.WriteByte(0xFF); // 16 byte align

            // Local fixups
            bw.FillUInt32("locoffset" + SectionID, (uint)bw.Position - absoluteOffset);
            foreach (var loc in LocalFixups) loc.Write(bw);
            while (bw.Position % 16 != 0) bw.WriteByte(0xFF); // 16 byte align

            // Global fixups
            bw.FillUInt32("globoffset" + SectionID, (uint)bw.Position - absoluteOffset);
            foreach (var glob in GlobalFixups) glob.Write(bw);
            while (bw.Position % 16 != 0) bw.WriteByte(0xFF); // 16 byte align

            // Virtual fixups
            bw.FillUInt32("virtoffset" + SectionID, (uint)bw.Position - absoluteOffset);
            foreach (var virt in VirtualFixups) virt.Write(bw);
            while (bw.Position % 16 != 0) bw.WriteByte(0xFF); // 16 byte align

            bw.FillUInt32("expoffset" + SectionID, (uint)bw.Position - absoluteOffset);
            bw.FillUInt32("impoffset" + SectionID, (uint)bw.Position - absoluteOffset);
            bw.FillUInt32("endoffset" + SectionID, (uint)bw.Position - absoluteOffset);
        }

        // Only use for a classnames structure after preliminary deserialization
        internal HKXClassNames ReadClassnames(BinaryReaderEx _br)
        {
            var br = new BinaryReaderEx(_br.BigEndian, _br.USizeLong, SectionData);
            var classnames = new HKXClassNames();
            classnames.Read(br);
            return classnames;
        }
    }
}