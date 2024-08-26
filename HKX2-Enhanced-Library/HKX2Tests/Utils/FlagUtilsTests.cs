using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HKX2E.Tests
{
    [TestClass]
    public class FlagUtilsTests
    {
        [TestMethod]
        public void EnumToNumber()
        {
            var test = VariableType.VARIABLE_TYPE_BOOL;
            Assert.AreEqual<uint>(0, test.ToEnumValue<VariableType, uint>());
        }

        [TestMethod]
        public void StringToEnumNumber()
        {
            var test = "VARIABLE_TYPE_BOOL";
            Assert.AreEqual<uint>(0, test.ToEnumValue<VariableType, uint>());
        }

        [TestMethod]
        public void StringToEnumNumberNotExist()
        {
            var test = "NOT_EXIST";
            Assert.ThrowsException<KeyNotFoundException>(() =>
            {
                test.ToEnumValue<VariableType, uint>();
            });
        }

        [TestMethod]
        public void StringToEnumNumberNotExistConvered()
        {
            // not exist, maybe custom enum
            string test = "9";
            Assert.AreEqual<uint>(9, test.ToEnumValue<VariableType, uint>());
        }

        [TestMethod]
        public void ValueToEnumName()
        {
            uint test = 0;
            Assert.AreEqual("VARIABLE_TYPE_BOOL", test.ToEnumName<VariableType, uint>());
        }

        [TestMethod]
        public void ValueToEnumNameNotExist()
        {
            // not exist, maybe custom enum
            uint test = 80;
            Assert.AreEqual("80", test.ToEnumName<VariableType, uint>());
        }

        [TestMethod]
        public void ValueToFlagString()
        {
            // FLAG_NOT_VARIABLE|FLAG_NORMALIZED|FLAG_RAGDOLL
            short flagValue = 4 | 2 | 1;
            Assert.AreEqual("FLAG_NOT_VARIABLE|FLAG_NORMALIZED|FLAG_RAGDOLL", flagValue.ToFlagString<RoleFlags, short>());
        }

        [TestMethod]
        public void ValueToFlagStringWithNotExist()
        {
            // FLAG_NOT_VARIABLE|FLAG_NORMALIZED|FLAG_RAGDOLL|192
            short flagValue = 4 | 2 | 1 | 192;
            Assert.AreEqual("FLAG_NOT_VARIABLE|FLAG_NORMALIZED|FLAG_RAGDOLL|192", flagValue.ToFlagString<RoleFlags, short>());
        }
        [TestMethod]
        public void ValueToFlagStringWithNotExist2()
        {
            // FLAG_NOT_VARIABLE|FLAG_RAGDOLL|FLAG_NONE|0x4c0
            short flagValue = 4 | 1 | 0 | 0x4c0;
            Assert.AreEqual("FLAG_NOT_VARIABLE|FLAG_RAGDOLL|1216", flagValue.ToFlagString<RoleFlags, short>());
        }

        [TestMethod]
        public void FlagStringToValueWithNotExist2()
        {
            // FLAG_NOT_VARIABLE|FLAG_RAGDOLL|FLAG_NONE|0x4c0
            string flagString = "FLAG_NOT_VARIABLE | FLAG_RAGDOLL | 0x4c0 ";
            Assert.AreEqual(4 | 1 | 0 | 0x4c0, flagString.ToFlagValue<RoleFlags, short>());
        }

        [TestMethod]
        public void ValueToFlagStringZero()
        {
            // FLAG_NONE
            short flagValue = 0;
            Assert.AreEqual("FLAG_NONE", flagValue.ToFlagString<RoleFlags, short>());
        }

        [TestMethod]
        public void FlagStringToValue()
        {
            string flagString = "FLAG_NOT_VARIABLE| FLAG_NORMALIZED | FLAG_RAGDOLL";
            Assert.AreEqual(4 | 2 | 1, flagString.ToFlagValue<RoleFlags, short>());
        }

        [TestMethod]
        public void FlagStringToValueWithNotExist()
        {
            string flagString = "FLAG_NOT_VARIABLE| FLAG_NORMALIZED | FLAG_RAGDOLL | 192";
            Assert.AreEqual(4 | 2 | 1 | 192, flagString.ToFlagValue<RoleFlags, short>());
        }

        [TestMethod]
        public void FlagStringToValueWithNotExistHex()
        {
            string flagString = "FLAG_NOT_VARIABLE| FLAG_NORMALIZED | FLAG_RAGDOLL | 0xC0";
            Assert.AreEqual(4 | 2 | 1 | 192, flagString.ToFlagValue<RoleFlags, short>());
        }
    }
}