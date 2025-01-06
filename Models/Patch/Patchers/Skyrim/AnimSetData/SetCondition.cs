using System.IO;
using System.Text;

namespace Pandora.Patch.Patchers.Skyrim.AnimSetData
{
    public class SetCondition
	{
		public string VariableName { get; private set; } = string.Empty;

		public int Value1 { get; private set; } = 0;

		public int Value2 { get; private set; } = 0;

		public static SetCondition ReadCondition(StreamReader reader)
		{
			var condition = new SetCondition();

			condition.VariableName = reader.ReadLineSafe();

			int value1;
			int value2;

			if (!int.TryParse(reader.ReadLineSafe(), out value1) || !int.TryParse(reader.ReadLineSafe(), out value2)) return condition;

			condition.Value1 = value1;
			condition.Value2 = value2;

			return condition; 
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(VariableName);
			sb.AppendLine(Value1.ToString());
			sb.AppendLine(Value2.ToString());

			return sb.ToString();

		}
	}
}
