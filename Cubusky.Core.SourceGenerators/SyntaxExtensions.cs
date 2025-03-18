using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Cubusky.SourceGenerators
{
    internal static class SyntaxExtensions
    {
        /// <summary>Simplifies the type declaration to its name, modifiers, and specified members.</summary>
        /// <typeparam name="T">The type of <see cref="TypeDeclarationSyntax"/>.</typeparam>
        /// <returns>A simplified version of the <see cref="TypeDeclarationSyntax"/> without parents, children or trivia.</returns>
        public static T Simplify<T>(this T typeDeclaration, params MemberDeclarationSyntax[] items)
            where T : TypeDeclarationSyntax
            => (T)TypeDeclaration(typeDeclaration.Kind(), typeDeclaration.Identifier).WithModifiers(typeDeclaration.Modifiers).WithoutTrivia().AddMembers(items);

        public static CompilationUnitSyntax Scaffold(this MemberDeclarationSyntax memberDeclaration, SyntaxNode? parent)
        {
            // Simplify the syntax tree
            while (parent is MemberDeclarationSyntax member)
            {
                memberDeclaration = member switch
                {
                    TypeDeclarationSyntax typeDeclaration => typeDeclaration.Simplify(memberDeclaration),
                    BaseNamespaceDeclarationSyntax memberNamespaceDeclaration => NamespaceDeclaration(ParseName(memberNamespaceDeclaration.Name.ToString()))
                        .WithoutTrivia()
                        .AddMembers(memberDeclaration),
                    _ => throw new NotSupportedException($"Unsupported parent type: {parent.GetType().Name}"),
                };

                parent = parent.Parent;
            }

            // Simplify the compilation unit
            if (!(parent is CompilationUnitSyntax compilationUnit))
            {
                throw new ArgumentException("MemberDeclarationSyntax should always have a CompilationUnit as its root", nameof(memberDeclaration));
            }

            return CompilationUnit()
                .WithUsings(compilationUnit.Usings)
                .WithoutTrivia()
                .AddMembers(memberDeclaration);
        }

        /// <summary>Sorts the using directives in the provided <see cref="CompilationUnitSyntax"/> by name and removes duplicate usings.</summary>
        /// <returns>Creates a <see cref="CompilationUnitSyntax"/> from this <see cref="CompilationUnitSyntax"/> with unique and sorted usings.</returns>
        public static CompilationUnitSyntax SortUsings(this CompilationUnitSyntax compilationUnitSyntax)
        {
            var usingsComparer = Comparer<UsingDirectiveSyntax>.Create((x, y) => x.Name?.ToString().CompareTo(y.Name?.ToString()) ?? 0);
            var sortedUsings = compilationUnitSyntax.Usings.ToImmutableSortedSet(usingsComparer);
            return compilationUnitSyntax.WithUsings(List(sortedUsings));
        }

        public static object? BoxedValue(this TypedConstant typedConstant)
            => typedConstant.Kind == TypedConstantKind.Array
            ? typedConstant.Values.Select(value => value.Value).ToArray()
            : typedConstant.Value;

        public static bool IsParamName(this XmlElementSyntax xmlElement, string paramName)
            => xmlElement.StartTag.Name.LocalName.Text == "param"
            && xmlElement.StartTag.Attributes.Any(attribute => attribute.IsName(paramName));

        public static bool IsName(this XmlAttributeSyntax xmlAttribute, string name)
            => xmlAttribute is XmlNameAttributeSyntax xmlNameAttribute
            && xmlNameAttribute.Identifier.Identifier.Text == name;

        /// <inheritdoc cref="SyntaxTriviaList.Replace(Microsoft.CodeAnalysis.SyntaxTrivia, Microsoft.CodeAnalysis.SyntaxTrivia)"/>
        public static SyntaxTriviaList Replace(this SyntaxTriviaList triviaList, SyntaxTrivia triviaInList, SyntaxTrivia newTrivia, out SyntaxTrivia newTriviaReference)
        {
            var triviaIndex = triviaList.IndexOf(triviaInList);
            var newTriviaList = triviaList.Replace(triviaInList, newTrivia);
            newTriviaReference = newTriviaList[triviaIndex];
            return newTriviaList;
        }

        public static DocumentationCommentTriviaSyntax AddXmlParam(this DocumentationCommentTriviaSyntax documentationComment, string paramName, string? description = null)
        {
            var xmlParamName = XmlName("param");
            var paramElement = XmlElement(XmlElementStartTag(xmlParamName).AddAttributes(XmlNameAttribute(paramName)), XmlElementEndTag(xmlParamName));
            if (description != null)
            {
                paramElement = paramElement.AddContent(XmlText(description));
            }

            return documentationComment.AddContent(paramElement);
        }

        //public static SyntaxTriviaList Update(this SyntaxTriviaList triviaList, SyntaxTrivia updatedTrivia)
        //{
        //    if (!(updatedTrivia.GetStructure() is SyntaxNode newTriviaNode))
        //    {
        //        throw new ArgumentException("The trivia must have a structure.", nameof(updatedTrivia));
        //    }

        //    var newTrivia = triviaList.FirstOrDefault(trivia => trivia.GetStructure()?.IsIncrementallyIdenticalTo(newTriviaNode) is true);
        //    return triviaList.Replace(newTrivia, updatedTrivia, out newTrivia);
        //}
    }
}
