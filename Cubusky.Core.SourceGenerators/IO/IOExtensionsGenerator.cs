using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Cubusky.SourceGenerators.IO
{
    [Generator(LanguageNames.CSharp)]
    internal class IOExtensionsGenerator : IIncrementalGenerator
    {
        public const string FullyQualifiedAttributeName = "Cubusky.IO.IOExtensionsAttribute";
        public const string FullyQualifiedSaveExtensionsAttributeName = "Cubusky.IO.SaveExtensionsAttribute";
        public const string FullyQualifiedLoadExtensionsAttributeName = "Cubusky.IO.LoadExtensionsAttribute";

        public static readonly ParameterSyntax ioParameter = Parameter(Identifier("io")).WithType(ParseTypeName("IIO")).AddModifiers(Token(SyntaxKind.ThisKeyword));
        public static readonly ParameterSyntax enumerableIOParameter = Parameter(Identifier("io")).WithType(ParseTypeName("IEnumerableIO")).AddModifiers(Token(SyntaxKind.ThisKeyword));
        public static readonly ParameterSyntax compressionProviderParameter = Parameter(Identifier("compressionProvider")).WithType(ParseTypeName("ICompressionStreamProvider"));
        public static readonly ParameterSyntax compressionLevelParameter = Parameter(Identifier("compressionLevel")).WithType(ParseTypeName("CompressionLevel")).WithDefault(EqualsValueClause(ParseExpression("default")));

        public static readonly StatementSyntax inputStatement = ParseStatement("using var ioStream = io.OpenWrite();");
        public static readonly StatementSyntax outputStatement = ParseStatement("using var ioStream = io.OpenRead();");
        public static readonly StatementSyntax compressionStatement = ParseStatement("using var compressionStream = compressionProvider.CreateCompressionStream(ioStream, compressionLevel);");
        public static readonly StatementSyntax decompressionStatement = ParseStatement("using var compressionStream = compressionProvider.CreateDecompressionStream(ioStream);");

        public static readonly XmlTextSyntax xmlComment = XmlNewLine(string.Empty);
        public static readonly XmlTextSyntax xmlNewLine = XmlText(CarriageReturnLineFeed.ToFullString());
        public static readonly XmlElementSyntax xmlInputElement = XmlParamElement("io", XmlText("The input to save to."));
        public static readonly XmlElementSyntax xmlOutputElement = XmlParamElement("io", XmlText("The output to load from."));
        public static readonly XmlElementSyntax xmlCompressionProviderElement = XmlParamElement("compressionProvider", XmlText("Provides the compression and decompression algorithm."));
        public static readonly XmlElementSyntax xmlCompressionLevelElement = XmlParamElement("compressionLevel", XmlText("Compression level whether to emphasize speed or efficiency."));

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}

            var methodsByClasses = new Dictionary<ClassDeclarationSyntax, List<KeyValuePair<MethodDeclarationSyntax, AttributeData>>>();

            var saveClassesProviders = context.SyntaxProvider.ForAttributeWithMetadataName(FullyQualifiedAttributeName
                , (node, _) => IsValid(node)
                , (context, _) => ToSource(context)
            )
                .Where(SourceHasUniqueClassDeclaration)
                .Select((methodAttributePair, _) => (ClassDeclarationSyntax)methodAttributePair.Key.Parent!);

            context.RegisterSourceOutput(saveClassesProviders, (context, source) => GenerateIOExtensions(context, source, methodsByClasses[source]));

            static bool IsValid(SyntaxNode node)
                => node is MethodDeclarationSyntax methodDeclaration
                && methodDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword)
                && methodDeclaration.Parent is ClassDeclarationSyntax classDeclaration
                && classDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword)
                && classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);

            static KeyValuePair<MethodDeclarationSyntax, AttributeData> ToSource(GeneratorAttributeSyntaxContext context)
                => context.TargetSymbol is IMethodSymbol methodSymbol
                && methodSymbol.Parameters.Length > 0
                && methodSymbol.Parameters[0].Type.ToString() == "System.IO.Stream"
                ? new KeyValuePair<MethodDeclarationSyntax, AttributeData>((MethodDeclarationSyntax)context.TargetNode, context.Attributes[0])
                : default;

            bool SourceHasUniqueClassDeclaration(KeyValuePair<MethodDeclarationSyntax, AttributeData> methodAttributePair)
            {
                if (methodAttributePair.Key == null)
                {
                    return false;
                }

                var classDeclaration = (ClassDeclarationSyntax)methodAttributePair.Key.Parent!;
                if (methodsByClasses.TryGetValue(classDeclaration, out _))
                {
                    methodsByClasses[classDeclaration].Add(methodAttributePair);
                    return false;
                }

                methodsByClasses.Add(classDeclaration, new List<KeyValuePair<MethodDeclarationSyntax, AttributeData>> { methodAttributePair });
                return true;
            }
        }

        public void GenerateIOExtensions(SourceProductionContext context, ClassDeclarationSyntax sourceClass, IEnumerable<KeyValuePair<MethodDeclarationSyntax, AttributeData>> methodAttributePairs)
        {
            var classDeclaration = sourceClass.Simplify(methodAttributePairs.SelectMany(GenerateExtensionMethods).ToArray());

            // add / sort / remove duplicate usings
            var root = classDeclaration
                .Scaffold(sourceClass.Parent)
                .AddUsings(
                    UsingDirective(ParseName("Cubusky.IO")),
                    UsingDirective(ParseName("Cubusky.IO.Compression")),
                    UsingDirective(ParseName("System.Collections.Generic")),
                    UsingDirective(ParseName("System.IO")),
                    UsingDirective(ParseName("System.IO.Compression"))
                ).SortUsings();

            context.AddSource($"{sourceClass.Identifier.Text}.Generated.cs", root.NormalizeWhitespace().ToFullString());
        }

        private static IEnumerable<MethodDeclarationSyntax> GenerateExtensionMethods(KeyValuePair<MethodDeclarationSyntax, AttributeData> methodAttributePair)
        {
            var sourceMethod = methodAttributePair.Key;
            var attributeData = methodAttributePair.Value;
            var namedArguments = attributeData.NamedArguments.ToDictionary(namedArgument => namedArgument.Key, namedArgument => namedArgument.Value.BoxedValue());

            // Validate attribute data
            Debug.Assert(attributeData.ConstructorArguments[0].Value is byte, "IOExtensionsAttribute's first constructor argument must be IOExtensionType.");
            Debug.Assert(attributeData.ConstructorArguments[1].Value is string, "IOExtensionsAttribute's second constructor argument must be string.");
            if (namedArguments.TryGetValue("CompressionOverrides", out var compressionOverrides))
            {
                Debug.Assert(compressionOverrides is bool, "IOExtensionsAttribute's named argument 'CompressionOverrides' must be bool.");
            }

            // Create method declaration
            var methodDeclaration = MethodDeclaration(sourceMethod.ReturnType, sourceMethod.Identifier)
                .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                .WithTypeParameterList(sourceMethod.TypeParameterList);

            // Create attribute list
            var attributeReference = attributeData.ApplicationSyntaxReference?.GetSyntax();
            Debug.Assert(attributeReference != null, "The attribute must not be null.");
            foreach (var attributeList in sourceMethod.AttributeLists)
            {
                var newAttributeList = attributeList.RemoveNode(attributeReference!, SyntaxRemoveOptions.KeepNoTrivia);
                if (newAttributeList?.Attributes.Count > 0)
                {
                    methodDeclaration = methodDeclaration.AddAttributeLists(newAttributeList);
                }
            }

            // Create parameters
            var sourceParameters = sourceMethod.ParameterList.Parameters;
            var ioParameterList = sourceMethod.ParameterList.ReplaceNode(sourceParameters[0], ioParameter);
            var ioEnumerableParameterList = sourceMethod.ParameterList.ReplaceNode(sourceParameters[0], enumerableIOParameter);

            // Create comments
            var comments = sourceMethod.GetLeadingTrivia();
            var whitespace = comments.LastOrDefault().ToFullString();

            var xmlElements = comments
                .Select(trivia => trivia.GetStructure())
                .OfType<DocumentationCommentTriviaSyntax>()
                .SelectMany(documentationCommentTrivia => documentationCommentTrivia.Content)
                .OfType<XmlElementSyntax>();

            // Create a documentation stack for adding comments
            if (!(xmlElements.LastOrDefault()?.Parent is DocumentationCommentTriviaSyntax documentationStack))
            {
                comments = comments.Add(Trivia(documentationStack = DocumentationComment())).Add(Space);
            }
            var stackTrivia = documentationStack.ParentTrivia;

            void AddXmlParam(XmlNodeSyntax xmlParam)
            {
                if (documentationStack.Content.LastOrDefault() is XmlNodeSyntax lastContent)
                {
                    var content = documentationStack.Content.Replace(lastContent, XmlText(lastContent.ToFullString() + whitespace + xmlComment.ToFullString()));
                    documentationStack = documentationStack.WithContent(content);
                }
                else
                {
                    documentationStack = documentationStack.AddContent(xmlComment);
                }

                //documentationStack = documentationStack.AddContent(xmlParams.SelectMany(xmlParam => new[] { xmlParam, xmlNewLine }).ToArray());
                documentationStack = documentationStack.AddContent(new[] { xmlParam, xmlNewLine });
                comments = comments.Replace(stackTrivia, Trivia(documentationStack), out stackTrivia);
            }

            // Remove the first parameter xml param element if it exists
            var xmlStreamElement = xmlElements.FirstOrDefault(xmlElement => xmlElement.IsParamName(sourceParameters[0].Identifier.Text));
            if (xmlStreamElement?.Parent is DocumentationCommentTriviaSyntax documentationComment)
            {
                var xmlStreamElementIndex = documentationComment.Content.IndexOf(xmlStreamElement);
                var nodes = new[] { xmlStreamElement, documentationComment.Content[xmlStreamElementIndex - 1] };

                SyntaxTrivia newTrivia = default;
                var newDocumentationComment = documentationComment.RemoveNodes(nodes, SyntaxRemoveOptions.KeepNoTrivia);
                comments = newDocumentationComment != null
                    ? comments.Replace(documentationComment.ParentTrivia, Trivia(newDocumentationComment), out newTrivia)
                    : comments.Remove(documentationComment.ParentTrivia);

                if (documentationComment.ParentTrivia == stackTrivia)
                {
                    stackTrivia = newDocumentationComment != null ? newTrivia : Trivia(DocumentationComment());
                    documentationStack = (DocumentationCommentTriviaSyntax)stackTrivia.GetStructure()!;
                }
            }

            // Add empty xml param elements for missing parameters
            var missingXmlParams = sourceParameters
                .Skip(1)
                .Select(parameter => parameter.Identifier.Text)
                .Where(paramName => !xmlElements.Any(xmlElement => xmlElement.IsParamName(paramName)))
                .Select(paramName => XmlParamElement(paramName))
                .ToArray();
            foreach (var xmlParam in missingXmlParams)
            {
                AddXmlParam(xmlParam);
            }

            //    // Create comments

            //    // Create TryLoad overrides
            //    // Create LoadOrDefault overrides
            //    // Create Delete overrides

            // Prepare statements
            var parametersExpression = string.Join(", ", ioParameterList.Parameters.Skip(1).Select(parameter => parameter.Identifier.Text));
            var (returnText, yieldReturnText, enumerableReturnType)
                = sourceMethod.ReturnType is PredefinedTypeSyntax predefinedType
                    && predefinedType.Keyword.IsKind(SyntaxKind.VoidKeyword)
                ? (string.Empty, string.Empty, sourceMethod.ReturnType)
                : ("return ", "yield return ", ParseTypeName($"IEnumerable<{sourceMethod.ReturnType}>"));

            var serializeIOStatement = ParseStatement($"{returnText}{sourceMethod.Identifier}(ioStream, {parametersExpression});");
            var serializeEnumerableIOStatement = ParseStatement($"{yieldReturnText}{sourceMethod.Identifier}(ioStream, {parametersExpression});");
            var serializeCompressionStatement = ParseStatement($"{returnText}{sourceMethod.Identifier}(compressionStream, {parametersExpression});");
            var serializeEnumerableCompressionStatement = ParseStatement($"{yieldReturnText}{sourceMethod.Identifier}(compressionStream, {parametersExpression});");

            // Generate methods
            switch ((byte)attributeData.ConstructorArguments[0].Value!)
            {
                case 1: // Generate load methods
                    comments = comments.Add(Trivia(DocumentationComment(xmlOutputElement)));

                    yield return methodDeclaration
                        .WithLeadingTrivia(comments)
                        .WithParameterList(ioParameterList)
                        .WithBody(Block(outputStatement, serializeIOStatement));

                    yield return methodDeclaration
                        .WithLeadingTrivia(comments)
                        .WithReturnType(enumerableReturnType)
                        .WithParameterList(ioEnumerableParameterList)
                        .WithBody(EnumerableLoadBlock(serializeEnumerableIOStatement));

                    if (compressionOverrides is true)
                    {
                        comments = comments.Add(Trivia(DocumentationComment(xmlCompressionProviderElement)));

                        yield return methodDeclaration
                            .WithLeadingTrivia(comments)
                            .WithParameterList(ioParameterList)
                            .AddParameterListParameters(compressionProviderParameter)
                            .WithBody(Block(outputStatement, decompressionStatement, serializeCompressionStatement));

                        yield return methodDeclaration
                            .WithLeadingTrivia(comments)
                            .WithReturnType(enumerableReturnType)
                            .WithParameterList(ioEnumerableParameterList)
                            .AddParameterListParameters(compressionProviderParameter)
                            .WithBody(EnumerableLoadBlock(decompressionStatement, serializeEnumerableCompressionStatement));
                    }

                    static BlockSyntax EnumerableLoadBlock(params StatementSyntax[] statements)
                        => Block(
                            ParseStatement("foreach (var ioStream in io.OpenRead())"),
                            Block(
                                ParseStatement("using (ioStream)"),
                                Block(statements)
                            )
                        );

                    break;

                case 2: // Generate save methods
                    AddXmlParam(xmlInputElement);

                    yield return methodDeclaration
                        .WithLeadingTrivia(comments)
                        .WithParameterList(ioParameterList)
                        .WithBody(Block(inputStatement, serializeIOStatement));

                    yield break;

                //// Change parameter
                //var pluralIdentifier = Identifier(attributeData.ConstructorArguments[1].Value!.ToString());
                //var foreachStatement = ParseStatement($"foreach (var {sourceParameters[1].Identifier} in {pluralIdentifier})");
                //var pluralParameter = Parameter(pluralIdentifier).WithType(ParseTypeName($"IEnumerable<{sourceParameters[1].Type}>"));
                //ioEnumerableParameterList = ioEnumerableParameterList.ReplaceNode(ioEnumerableParameterList.Parameters[1], pluralParameter);

                //// Change comment
                //var xmlSingleElement = xmlElements.First(xmlElement => xmlElement.IsParamName(sourceParameters[1].Identifier.Text));
                //var xmlSingleNameAttribute = (XmlNameAttributeSyntax)xmlSingleElement.StartTag.Attributes.First(attribute => attribute.IsName(sourceParameters[1].Identifier.Text));
                //var xmlPluralAttributes = xmlSingleElement.StartTag.Attributes.Replace(xmlSingleNameAttribute, xmlSingleNameAttribute.WithIdentifier(IdentifierName(pluralIdentifier)));
                //var xmlPluralElement = xmlSingleElement.WithStartTag(xmlSingleElement.StartTag.WithAttributes(xmlPluralAttributes));

                //var xmlSingleElementParentTrivia = comments.First(trivia => trivia.GetStructure() is DocumentationCommentTriviaSyntax documentation && documentation.Content.Any(node => node.IsEquivalentTo(xmlSingleElement)));
                //comments = comments.Replace(xmlSingleElementParentTrivia, Trivia((DocumentationCommentTriviaSyntax)xmlSingleElement.Parent!.ReplaceNode(xmlSingleElement, xmlPluralElement)));

                //yield return methodDeclaration
                //    .WithLeadingTrivia(comments)
                //    .WithReturnType(enumerableReturnType)
                //    .WithParameterList(ioEnumerableParameterList)
                //    .WithBody(Block(foreachStatement, Block(inputStatement, serializeEnumerableIOStatement)));

                //if (compressionOverrides is true)
                //{
                //    comments = comments.Add(Trivia(DocumentationComment(xmlCompressionProviderElement, xmlCompressionLevelElement)));

                //    yield return methodDeclaration
                //        .WithLeadingTrivia(comments)
                //        .WithParameterList(ioParameterList)
                //        .AddParameterListParameters(compressionProviderParameter, compressionLevelParameter)
                //        .WithBody(Block(inputStatement, compressionStatement, serializeCompressionStatement));

                //    yield return methodDeclaration
                //        .WithLeadingTrivia(comments)
                //        .WithReturnType(enumerableReturnType)
                //        .WithParameterList(ioEnumerableParameterList)
                //        .AddParameterListParameters(compressionProviderParameter, compressionLevelParameter)
                //        .WithBody(Block(foreachStatement, Block(inputStatement, compressionStatement, serializeEnumerableCompressionStatement)));
                //}

                //break;

                default:
                    throw new InvalidOperationException($"Unsupported IOExtensionType: {attributeData.ConstructorArguments[0].Value}");
            }
        }

        private static SyntaxTrivia DocumentationTrivia(params XmlNodeSyntax[] content) => Trivia(DocumentationComment(new List<XmlNodeSyntax>(content) { XmlText(CarriageReturnLineFeed.ToFullString()) }.ToArray()));
    }
}
