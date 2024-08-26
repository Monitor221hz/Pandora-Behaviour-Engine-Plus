using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Xml.Linq;
using FastMember;
using System.Linq.Expressions;
namespace HKX2E.Tests
{
    [TestClass]
    public class CompareTests
    {
		string rawValueLong = @"<hkobject name=""#1786"" class=""hkbStateMachineTransitionInfoArray"" signature=""0xe397b11e"">
      <!--memSizeAndFlags SERIALIZE_IGNORED-->
      <!--referenceCount SERIALIZE_IGNORED-->
      <hkparam name=""transitions"" numelements=""6"">
        <hkobject>
          <hkparam name=""triggerInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""initiateInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""transition"">DefaultBlendTransition</hkparam>
          <hkparam name=""condition"">null</hkparam>
          <hkparam name=""eventId"">809</hkparam>
          <hkparam name=""toStateId"">22</hkparam>
          <hkparam name=""fromNestedStateId"">0</hkparam>
          <hkparam name=""toNestedStateId"">0</hkparam>
          <hkparam name=""priority"">0</hkparam>
          <hkparam name=""flags"">FLAG_TO_NESTED_STATE_ID_IS_VALID|FLAG_IS_LOCAL_WILDCARD|FLAG_DISABLE_CONDITION</hkparam>
        </hkobject>
        <hkobject>
          <hkparam name=""triggerInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""initiateInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""transition"">DefaultBlendResetTransition</hkparam>
          <hkparam name=""condition"">#1787</hkparam>
          <hkparam name=""eventId"">176</hkparam>
          <hkparam name=""toStateId"">23</hkparam>
          <hkparam name=""fromNestedStateId"">0</hkparam>
          <hkparam name=""toNestedStateId"">0</hkparam>
          <hkparam name=""priority"">0</hkparam>
          <hkparam name=""flags"">FLAG_IS_LOCAL_WILDCARD</hkparam>
        </hkobject>
        <hkobject>
          <hkparam name=""triggerInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""initiateInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""transition"">null</hkparam>
          <hkparam name=""condition"">null</hkparam>
          <hkparam name=""eventId"">1009</hkparam>
          <hkparam name=""toStateId"">23</hkparam>
          <hkparam name=""fromNestedStateId"">0</hkparam>
          <hkparam name=""toNestedStateId"">0</hkparam>
          <hkparam name=""priority"">0</hkparam>
          <hkparam name=""flags"">FLAG_IS_LOCAL_WILDCARD|FLAG_DISABLE_CONDITION</hkparam>
        </hkobject>
        <hkobject>
          <hkparam name=""triggerInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""initiateInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""transition"">DefaultBlendTransition</hkparam>
          <hkparam name=""condition"">null</hkparam>
          <hkparam name=""eventId"">1010</hkparam>
          <hkparam name=""toStateId"">22</hkparam>
          <hkparam name=""fromNestedStateId"">0</hkparam>
          <hkparam name=""toNestedStateId"">1</hkparam>
          <hkparam name=""priority"">0</hkparam>
          <hkparam name=""flags"">FLAG_TO_NESTED_STATE_ID_IS_VALID|FLAG_IS_GLOBAL_WILDCARD|FLAG_DISABLE_CONDITION</hkparam>
        </hkobject>
        <hkobject>
          <hkparam name=""triggerInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""initiateInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""transition"">DefaultBlendTransition</hkparam>
          <hkparam name=""condition"">null</hkparam>
          <hkparam name=""eventId"">1172</hkparam>
          <hkparam name=""toStateId"">115</hkparam>
          <hkparam name=""fromNestedStateId"">0</hkparam>
          <hkparam name=""toNestedStateId"">0</hkparam>
          <hkparam name=""priority"">0</hkparam>
          <hkparam name=""flags"">FLAG_IS_LOCAL_WILDCARD|FLAG_IS_GLOBAL_WILDCARD|FLAG_DISABLE_CONDITION</hkparam>
        </hkobject>
        <hkobject>
          <hkparam name=""triggerInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""initiateInterval"">
            <hkobject>
              <hkparam name=""enterEventId"">-1</hkparam>
              <hkparam name=""exitEventId"">-1</hkparam>
              <hkparam name=""enterTime"">0.000000</hkparam>
              <hkparam name=""exitTime"">0.000000</hkparam>
            </hkobject>
          </hkparam>
          <hkparam name=""transition"">DefaultBlendTransition</hkparam>
          <hkparam name=""condition"">null</hkparam>
          <hkparam name=""eventId"">1188</hkparam>
          <hkparam name=""toStateId"">122</hkparam>
          <hkparam name=""fromNestedStateId"">0</hkparam>
          <hkparam name=""toNestedStateId"">0</hkparam>
          <hkparam name=""priority"">0</hkparam>
          <hkparam name=""flags"">FLAG_IS_LOCAL_WILDCARD|FLAG_IS_GLOBAL_WILDCARD|FLAG_DISABLE_CONDITION</hkparam>
        </hkobject>
      </hkparam>
    </hkobject>";
		[TestMethod]
        public void PartialDeserializeTest()
		{
            var rawValue = "<hkobject name=\"#0051\" class=\"hkbStateMachineStateInfo\" signature=\"0x0ed7f9d0\"><hkparam name=\"variableBindingSet\">null</hkparam><hkparam name=\"listeners\" numelements=\"0\"></hkparam><hkparam name=\"enterNotifyEvents\">null</hkparam><hkparam name=\"exitNotifyEvents\">null</hkparam><hkparam name=\"transitions\">null</hkparam><hkparam name=\"generator\">RootModifierGenerator</hkparam><hkparam name=\"name\">Root</hkparam><hkparam name=\"stateId\">0</hkparam><hkparam name=\"probability\">1.000000</hkparam><hkparam name=\"enable\">true</hkparam></hkobject>";


			XElement element = XElement.Parse(rawValueLong);
            HavokXmlDeserializerOptions options = HavokXmlDeserializerOptions.IgnoreNonFatalErrors | HavokXmlDeserializerOptions.IgnoreMissingPointers;
            HavokXmlPartialDeserializer deserializer = new(options);
            var obj = deserializer.DeserializeRuntimeObject(element);
            RuntimePatcher.SetProperty(obj, "name", "piss");
            HavokXmlPartialSerializer serializer = new();
            XElement outelement = serializer.SerializeObject(obj);
            Debug.WriteLine(outelement.ToString());
        }
        [TestMethod]
        public void TypeTest()
        {
            IHavokObject obj = new hkbClipGenerator(); 
            Debug.WriteLine(obj.GetType());
        }
    }
}