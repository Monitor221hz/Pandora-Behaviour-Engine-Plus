using Pandora.Models.Extensions;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class SetCondition
{
	public string VariableName { get; private set; } = string.Empty;

	public int Value1 { get; private set; } = 0;

	public int Value2 { get; private set; } = 0;

	public static SetCondition ReadCondition(StreamReader reader)
	{
		var condition = new SetCondition
		{
			VariableName = reader.ReadLineOrEmpty()
		};


		if (!int.TryParse(reader.ReadLineOrEmpty(), out int value1) || !int.TryParse(reader.ReadLineOrEmpty(), out int value2)) return condition;

		condition.Value1 = value1;
		condition.Value2 = value2;

		return condition;
	}

	public override string ToString()
	{
		StringBuilder sb = new();

		sb.AppendLine(VariableName);
		sb.AppendLine(Value1.ToString());
		sb.AppendLine(Value2.ToString());

		return sb.ToString();

	}
}