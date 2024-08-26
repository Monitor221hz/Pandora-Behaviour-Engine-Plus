using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Diagnostics;

namespace HKX2E.SourceGenerators;
[Generator]
public class PathGenerator : ISourceGenerator
{
	public void Execute(GeneratorExecutionContext context)
	{
		var syntaxTrees = context.Compilation.SyntaxTrees.Where(tree => tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Any());
		foreach (SyntaxTree tree in syntaxTrees)
		{
			var semanticModel = context.Compilation.GetSemanticModel(tree);
			foreach (var declaredClass in tree.GetRoot()
					.DescendantNodes()
					.OfType<ClassDeclarationSyntax>())
			{
				var root = declaredClass.SyntaxTree.GetRoot();
				var model = context.Compilation.GetSemanticModel(root.SyntaxTree);
				ISymbol? symbol = model.GetDeclaredSymbol(root.DescendantNodes().OfType<ClassDeclarationSyntax>().First());
				if (symbol == null) continue;
				var implementedInterfaces = ((ITypeSymbol)symbol).AllInterfaces;
				if (implementedInterfaces.Any(intf => intf.Name == nameof(IHavokObject)))
				{
					Debug.WriteLine($"{tree.FilePath}");
				}
			}
		}
	}

	public void Initialize(GeneratorInitializationContext context)
	{
	}
}
