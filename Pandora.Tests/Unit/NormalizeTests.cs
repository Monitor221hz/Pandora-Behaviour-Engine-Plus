// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using HKX2E.Extensions;

namespace PandoraTests.Unit;

public class NormalizeTests
{
	[Fact]
	public void Normalize_SimpleSpaceSeparatedValues_SplitsCorrectly()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("1.0 2.0 3.0").ToArray();

		Assert.Equal(["1.0", "2.0", "3.0"], result);
	}

	[Fact]
	public void Normalize_ParenthesizedVector_StripsParensAndSplits()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("(1.0 2.0 3.0 4.0)").ToArray();

		Assert.Equal(["1.0", "2.0", "3.0", "4.0"], result);
	}

	[Fact]
	public void Normalize_MultipleParenthesizedGroups_FlattenedCorrectly()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("(1.0 2.0)(3.0 4.0)").ToArray();

		Assert.Equal(["1.0", "2.0", "3.0", "4.0"], result);
	}

	[Fact]
	public void Normalize_CommaSeparatedValues_SplitsCorrectly()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("1.0,2.0,3.0").ToArray();

		Assert.Equal(["1.0", "2.0", "3.0"], result);
	}

	[Fact]
	public void Normalize_MixedDelimiters_SplitsCorrectly()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("(1.0, 2.0, 3.0)").ToArray();

		Assert.Equal(["1.0", "2.0", "3.0"], result);
	}

	[Fact]
	public void Normalize_TabAndNewlineDelimiters_SplitsCorrectly()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("1.0\n2.0\t3.0").ToArray();

		Assert.Equal(["1.0", "2.0", "3.0"], result);
	}

	[Fact]
	public void Normalize_CarriageReturnLineFeed_SplitsCorrectly()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("1.0\r\n2.0\r\n3.0").ToArray();

		Assert.Equal(["1.0", "2.0", "3.0"], result);
	}

	[Fact]
	public void Normalize_IndeterminateFloat_ReplacedWithZero()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("-1.#IND00").ToArray();

		Assert.Equal(["0.0"], result);
	}

	[Fact]
	public void Normalize_IndeterminateFloatMixedWithValues_OnlyIndeterminateReplaced()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("(0.5 -1.#IND00 1.5)").ToArray();

		Assert.Equal(["0.5", "0.0", "1.5"], result);
	}

	[Fact]
	public void Normalize_MultipleIndeterminateFloats_AllReplaced()
	{
		var result = HavokXmlDeserializerExtensions
			.Normalize("-1.#IND00 -1.#IND00 -1.#IND00")
			.ToArray();

		Assert.Equal(["0.0", "0.0", "0.0"], result);
	}

	[Fact]
	public void Normalize_EmptyString_ReturnsEmpty()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("").ToArray();

		Assert.Empty(result);
	}

	[Fact]
	public void Normalize_OnlyDelimiters_ReturnsEmpty()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("( , , )").ToArray();

		Assert.Empty(result);
	}

	[Fact]
	public void Normalize_OnlyWhitespace_ReturnsEmpty()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("   \t  \n  \r\n  ").ToArray();

		Assert.Empty(result);
	}

	[Fact]
	public void Normalize_ExtraWhitespace_TrimsAndSplitsCorrectly()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("  1.0   2.0   3.0  ").ToArray();

		Assert.Equal(["1.0", "2.0", "3.0"], result);
	}

	[Fact]
	public void Normalize_SingleValue_ReturnsSingleElement()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("42.0").ToArray();

		Assert.Equal(["42.0"], result);
	}

	[Fact]
	public void Normalize_NegativeValues_PreservedCorrectly()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("-1.0 -2.5 -0.001").ToArray();

		Assert.Equal(["-1.0", "-2.5", "-0.001"], result);
	}

	[Fact]
	public void Normalize_RealisticHkxVector4_ParsedCorrectly()
	{
		var result = HavokXmlDeserializerExtensions
			.Normalize("(0.000000 0.000000 0.000000 1.000000)")
			.ToArray();

		Assert.Equal(["0.000000", "0.000000", "0.000000", "1.000000"], result);
	}

	[Fact]
	public void Normalize_RealisticHkxMatrix4_ParsedCorrectly()
	{
		var input =
			"(1.000000 0.000000 0.000000 0.000000)"
			+ "(0.000000 1.000000 0.000000 0.000000)"
			+ "(0.000000 0.000000 1.000000 0.000000)"
			+ "(0.000000 0.000000 0.000000 1.000000)";

		var result = HavokXmlDeserializerExtensions.Normalize(input).ToArray();

		Assert.Equal(16, result.Length);
		Assert.Equal("1.000000", result[0]);
		Assert.Equal("1.000000", result[5]);
		Assert.Equal("1.000000", result[10]);
		Assert.Equal("1.000000", result[15]);
		Assert.Equal("0.000000", result[1]);
		Assert.Equal("0.000000", result[4]);
	}

	[Fact]
	public void Normalize_RealisticHkxQuaternion_ParsedCorrectly()
	{
		var result = HavokXmlDeserializerExtensions
			.Normalize("(0.000000 0.707107 0.000000 0.707107)")
			.ToArray();

		Assert.Equal(["0.000000", "0.707107", "0.000000", "0.707107"], result);
	}

	[Fact]
	public void Normalize_IntegerValues_PreservedAsStrings()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("1 2 3 4").ToArray();

		Assert.Equal(["1", "2", "3", "4"], result);
	}

	[Fact]
	public void Normalize_ConsecutiveDelimiters_NoEmptyEntries()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("1.0,,,,2.0    3.0").ToArray();

		Assert.Equal(["1.0", "2.0", "3.0"], result);
	}

	[Fact]
	public void Normalize_NestedParentheses_HandledAsDelimiters()
	{
		var result = HavokXmlDeserializerExtensions.Normalize("((1.0))").ToArray();

		Assert.Equal(["1.0"], result);
	}

	[Fact]
	public void Normalize_ResultsAreAllParsableAsFloat()
	{
		var input = "(0.5 -1.#IND00 1.5 -3.14159)";
		var result = HavokXmlDeserializerExtensions.Normalize(input);

		var floats = result.Select(float.Parse).ToArray();

		Assert.Equal([0.5f, 0.0f, 1.5f, -3.14159f], floats);
	}

	[Fact]
	public void Normalize_ChunkedResultMatchesExpectedGrouping()
	{
		var input = "(1.0 2.0 3.0 4.0)(5.0 6.0 7.0 8.0)";
		var result = HavokXmlDeserializerExtensions.Normalize(input);

		var chunks = result.Select(float.Parse).Chunk(4).ToList();

		Assert.Equal(2, chunks.Count);
		Assert.Equal([1.0f, 2.0f, 3.0f, 4.0f], chunks[0]);
		Assert.Equal([5.0f, 6.0f, 7.0f, 8.0f], chunks[1]);
	}

	[Fact]
	public void Normalize_LeadingAndTrailingDelimiters_NoEmptyEntries()
	{
		var result = HavokXmlDeserializerExtensions.Normalize(" , (  1.0  ) , ").ToArray();

		Assert.Equal(["1.0"], result);
	}

	[Fact]
	public void Normalize_IndeterminateNotPartiallyMatched_PreservesOtherValues()
	{
		// Ensure only the exact string "-1.#IND00" is replaced, not partial matches
		var result = HavokXmlDeserializerExtensions.Normalize("-1.0 -1.5").ToArray();

		Assert.Equal(["-1.0", "-1.5"], result);
	}

	[Fact]
	public void Normalize_ScientificNotationLikeStrings_PreservedAsIs()
	{
		// Normalize does not interpret values, it just splits and replaces IND
		var result = HavokXmlDeserializerExtensions.Normalize("1.0e5 2.5e-3").ToArray();

		Assert.Equal(["1.0e5", "2.5e-3"], result);
	}

	[Fact]
	public void Normalize_VeryLongInput_HandledCorrectly()
	{
		var values = Enumerable.Range(0, 1000).Select(i => $"{i}.0");
		var input = $"({string.Join(" ", values)})";

		var result = HavokXmlDeserializerExtensions.Normalize(input).ToArray();

		Assert.Equal(1000, result.Length);
		Assert.Equal("0.0", result[0]);
		Assert.Equal("999.0", result[999]);
	}
}
