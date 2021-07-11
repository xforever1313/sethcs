//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Seth.Analyzer.Rules
{
    public static class SethClassAccessModifierRule
    {
        // ---------------- Fields ----------------

        private const string Descriptor = nameof( SethClassAccessModifierRule );

        private static readonly LocalizableString Title = "Class requires access modifer.";

        private static readonly LocalizableString MessageFormat = "{0} {1} does not have an access modifer.";

        private static readonly LocalizableString Description =
            $"Type declaration should have an explicit access modifier, otherwise it will defaulted.";

        private static readonly string RuleCategory = DiagnosticCategory.Warning.ToString();

        private const DiagnosticSeverity Serverity = DiagnosticSeverity.Warning;

        // ---------------- Constructor ----------------

        static SethClassAccessModifierRule()
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

        // ---------------- Functions ----------------

        public static void Init( AnalysisContext context )
        {
            context.RegisterSyntaxNodeAction(
                Run,
                SyntaxKind.ClassDeclaration,
                SyntaxKind.StructDeclaration,
                SyntaxKind.RecordDeclaration,
                SyntaxKind.EnumDeclaration,
                SyntaxKind.InterfaceDeclaration
            );
        }

        private static void Run( SyntaxNodeAnalysisContext context )
        {
            SyntaxToken identifier;
            string keyword;
            SyntaxTokenList modifiers;

            if( context.Node is TypeDeclarationSyntax typeNode )
            {
                identifier = typeNode.Identifier;
                keyword = typeNode.Keyword.Text ?? string.Empty;
                modifiers = typeNode.Modifiers;
            }
            else if( context.Node is EnumDeclarationSyntax enumNode )
            {
                identifier = enumNode.Identifier;
                keyword = typeof( Enum ).Name;
                modifiers = enumNode.Modifiers;
            }
            else
            {
                return;
            }

            // Need to search for modifiers like this;
            // as things like "static" could show up here.
            var foundModifiers = modifiers.Where(
                m =>
                    m.Text.Equals( "private" ) ||
                    m.Text.Equals( "protected" ) ||
                    m.Text.Equals( "public" ) ||
                    m.Text.Equals( "internal" )
            );

            if( foundModifiers.Count() == 0 )
            {
                var diagnostic = Diagnostic.Create(
                    Rule,
                    identifier.GetLocation(),
                    keyword,
                    identifier.Text ?? string.Empty
                );
                context.ReportDiagnostic( diagnostic );
            }
        }
    }
}
