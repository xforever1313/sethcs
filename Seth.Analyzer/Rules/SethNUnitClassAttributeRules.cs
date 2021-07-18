//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Seth.Analyzer.Rules
{
    public static class SethNUnitClassAttributeRules
    {
        public static class SethNUnitTestMethodMustBePublicRule
        {
            // ---------------- Fields ----------------

            private const string Descriptor = nameof( SethNUnitTestMethodMustBePublicRule );

            private static readonly LocalizableString Title = "NUnit test methods should be public.";
            private static readonly LocalizableString MessageFormat = "Method '{0}' within '{1}' has a TestAttribute on it, but is not public.";
            private static readonly LocalizableString Description =
                "Method with the TestAttribute should be public so they can be found by runners.";

            private static readonly string RuleCategory = DiagnosticCategory.Warning.ToString();

            private const DiagnosticSeverity Serverity = DiagnosticSeverity.Warning;

            // ---------------- Constructor ----------------

            static SethNUnitTestMethodMustBePublicRule()
            {
                Rule = new DiagnosticDescriptor(
                    Descriptor,
                    Title,
                    MessageFormat,
                    RuleCategory.ToString(),
                    Serverity,
                    isEnabledByDefault: true,
                    description: Description
                );
            }

            // ---------------- Properties ----------------

            public static DiagnosticDescriptor Rule { get; private set; }
        }

        public static class SethNUnitClassContainsTestMethodMustBeAFixtureRule
        {
            // ---------------- Fields ----------------

            private const string Descriptor = nameof( SethNUnitClassContainsTestMethodMustBeAFixtureRule );

            private static readonly LocalizableString Title = "Class that contains TestAttribute is not a TestFixture.";
            private static readonly LocalizableString MessageFormat = "Class '{0}' has a method that contains a TestAttribute on it, but it lacks a TestFixtureAttribute.";
            private static readonly LocalizableString Description =
                "Classes that contain a TestAttribute should contain a TestFixtureAttribute so they are not missed by runners.";

            private static readonly string RuleCategory = DiagnosticCategory.Warning.ToString();

            private const DiagnosticSeverity Serverity = DiagnosticSeverity.Warning;

            // ---------------- Constructor ----------------

            static SethNUnitClassContainsTestMethodMustBeAFixtureRule()
            {
                Rule = new DiagnosticDescriptor(
                    Descriptor,
                    Title,
                    MessageFormat,
                    RuleCategory.ToString(),
                    Serverity,
                    isEnabledByDefault: true,
                    description: Description
                );
            }

            // ---------------- Properties ----------------

            public static DiagnosticDescriptor Rule { get; private set; }
        }

        public static class SethNUnitTestFixtureMustBePublicRule
        {
            // ---------------- Fields ----------------

            private const string Descriptor = nameof( SethNUnitTestFixtureMustBePublicRule );

            private static readonly LocalizableString Title = "Class that contains the TestFixture attribute should be public.";
            private static readonly LocalizableString MessageFormat = "Class '{0}' has a TestFixtureAttribute, and it should be public.";
            private static readonly LocalizableString Description =
                "Classes that contain a TestFixtureAttribute should be public so runners pick them up.";

            private static readonly string RuleCategory = DiagnosticCategory.Warning.ToString();

            private const DiagnosticSeverity Serverity = DiagnosticSeverity.Warning;

            // ---------------- Constructor ----------------

            static SethNUnitTestFixtureMustBePublicRule()
            {
                Rule = new DiagnosticDescriptor(
                    Descriptor,
                    Title,
                    MessageFormat,
                    RuleCategory.ToString(),
                    Serverity,
                    isEnabledByDefault: true,
                    description: Description
                );
            }

            // ---------------- Properties ----------------

            public static DiagnosticDescriptor Rule { get; private set; }
        }

        public static class SethNUnitTestFixtureMustBeSealedRule
        {
            // ---------------- Fields ----------------

            private const string Descriptor = nameof( SethNUnitTestFixtureMustBeSealedRule );

            private static readonly LocalizableString Title = "Class that contains the TestFixture attribute should be sealed.";
            private static readonly LocalizableString MessageFormat = "Class '{0}' has a TestFixtureAttribute, and it should be sealed.";
            private static readonly LocalizableString Description =
                "A TestFixtureAttribute class should be sealed as it should not be inherited from.";

            private static readonly string RuleCategory = DiagnosticCategory.Warning.ToString();

            private const DiagnosticSeverity Serverity = DiagnosticSeverity.Warning;

            // ---------------- Constructor ----------------

            static SethNUnitTestFixtureMustBeSealedRule()
            {
                Rule = new DiagnosticDescriptor(
                    Descriptor,
                    Title,
                    MessageFormat,
                    RuleCategory.ToString(),
                    Serverity,
                    isEnabledByDefault: true,
                    description: Description
                );
            }

            // ---------------- Properties ----------------

            public static DiagnosticDescriptor Rule { get; private set; }
        }

        public static void Init( AnalysisContext context )
        {
            context.RegisterSymbolAction( Run, SymbolKind.NamedType );
        }

        public static void Run( SymbolAnalysisContext context )
        {
            INamedTypeSymbol classSymbol = context.Symbol as INamedTypeSymbol;
            if( classSymbol == null )
            {
                return;
            }

            // First, see if the class contains the TestFixture attribute.
            bool hasFixtureAttribute = false;
            foreach( AttributeData attribute in classSymbol.GetAttributes() )
            {
                if( "NUnit.Framework.TestFixtureAttribute".Equals( attribute.AttributeClass?.ToString() ) )
                {
                    hasFixtureAttribute = true;
                    break;
                }
            }

            IReadOnlyList<IMethodSymbol> methods = SearchForTestMethods( classSymbol );

            // Flag a warning if we lack a fixture attribute, but contain a test method.
            if( ( hasFixtureAttribute == false ) && ( methods.Count > 0 ) )
            {
                var diagnostic = Diagnostic.Create(
                    SethNUnitClassContainsTestMethodMustBeAFixtureRule.Rule,
                    classSymbol.Locations[0],
                    classSymbol.Name ?? string.Empty
                );
                context.ReportDiagnostic( diagnostic );
            }

            if( hasFixtureAttribute )
            {
                LookForClassSyntaxErrors( context, classSymbol );
            }

            foreach( IMethodSymbol method in methods )
            {
                LookForMethodSyntaxErrors( context, classSymbol, method );
            }
        }

        private static IReadOnlyList<IMethodSymbol> SearchForTestMethods( INamedTypeSymbol classSymbol )
        {
            List<IMethodSymbol> methods = new List<IMethodSymbol>();

            foreach( ISymbol member in classSymbol.GetMembers() )
            {
                IMethodSymbol method = member as IMethodSymbol;
                if( method == null )
                {
                    continue;
                }

                IEnumerable<AttributeData> attributes = method.GetAttributes();
                foreach( AttributeData attribute in attributes )
                {
                    if( "NUnit.Framework.TestAttribute".Equals( attribute.AttributeClass?.ToString() ) )
                    {
                        methods.Add( method );
                        break;
                    }
                }
            }

            return methods;
        }

        private static void LookForClassSyntaxErrors( SymbolAnalysisContext context, INamedTypeSymbol classSymbol )
        {
            foreach( Location location in classSymbol.Locations )
            {
                TypeDeclarationSyntax syntaxNode = location.GetSyntaxNode() as TypeDeclarationSyntax;
                if( syntaxNode == null )
                {
                    continue;
                }

                bool foundSealedModifiers = syntaxNode.Modifiers.Where( m => m.Text == "sealed" ).Any();
                if( foundSealedModifiers == false )
                {
                    var diagnostic = Diagnostic.Create(
                        SethNUnitTestFixtureMustBeSealedRule.Rule,
                        location,
                        classSymbol.Name ?? string.Empty
                    );
                    context.ReportDiagnostic( diagnostic );
                }

                bool foundPublicModifiers = syntaxNode.Modifiers.Where( m => m.Text == "public" ).Any();
                if( foundPublicModifiers == false )
                {
                    var diagnostic = Diagnostic.Create(
                        SethNUnitTestFixtureMustBePublicRule.Rule,
                        location,
                        classSymbol.Name ?? string.Empty
                    );
                    context.ReportDiagnostic( diagnostic );
                }
            }
        }

        private static void LookForMethodSyntaxErrors( SymbolAnalysisContext context, INamedTypeSymbol classSymbol, IMethodSymbol method )
        {
            foreach( Location location in method.Locations )
            {
                MethodDeclarationSyntax syntaxNode = location.GetSyntaxNode() as MethodDeclarationSyntax;
                if( syntaxNode == null )
                {
                    continue;
                }

                bool foundPublicModifiers = syntaxNode.Modifiers.Where( m => m.Text == "public" ).Any();
                if( foundPublicModifiers == false )
                {
                    var diagnostic = Diagnostic.Create(
                        SethNUnitTestMethodMustBePublicRule.Rule,
                        location,
                        method.Name ?? string.Empty,
                        classSymbol.Name ?? string.Empty
                    );
                    context.ReportDiagnostic( diagnostic );
                }
            }
        }
    }
}
