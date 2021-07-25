//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Seth.Analyzer.Rules;

namespace Seth.Analyzer.CodeFixes
{
    public sealed class SethNUnitTestMethodMustBePublicCodeFix : ICodeFix
    {
        // ---------------- Fields ----------------

        private static readonly string title = "Add 'public' modifier to Test Method";

        private static readonly HashSet<SyntaxKind> nonPublicSyntax = new HashSet<SyntaxKind>
        {
            SyntaxKind.PrivateKeyword,
            SyntaxKind.ProtectedKeyword,
            SyntaxKind.InternalKeyword
        };

        // ---------------- Constructor ----------------

        public SethNUnitTestMethodMustBePublicCodeFix()
        {
        }

        public DiagnosticDescriptor Rule => SethNUnitClassAttributeRules.SethNUnitTestMethodMustBePublicRule.Rule;

        // ---------------- Functions ----------------

        public async Task RegisterCodeFixesAsync( CodeFixContext context, Diagnostic diagnostic )
        {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync( context.CancellationToken ).ConfigureAwait( false );

            TextSpan span = diagnostic.Location.SourceSpan;
            if( span == null )
            {
                return;
            }

            MethodDeclarationSyntax declaration = root.FindToken( span.Start ).Parent as MethodDeclarationSyntax;
            if( declaration == null )
            {
                return;
            }

            CodeAction action = CodeAction.Create(
                title,
                c => AddPublicModifier( root, context.Document, declaration, c ),
                nameof( SethNUnitTestMethodMustBePublicCodeFix )
            );

            context.RegisterCodeFix( action, diagnostic );
        }

        private async Task<Document> AddPublicModifier( SyntaxNode root, Document document, MethodDeclarationSyntax dec, CancellationToken cancelToken )
        {
            return await Task.Run(
                () =>
                {
                    SyntaxTokenList modList = dec.Modifiers;
                    foreach( SyntaxToken token in dec.Modifiers )
                    {
                        if( nonPublicSyntax.Contains( token.Kind() ) == false )
                        {
                            continue;
                        }

                        modList = dec.Modifiers.Remove( token );
                    }

                    SyntaxToken newToken = SyntaxFactory.Token( SyntaxKind.PublicKeyword );
                    modList = modList.Insert( 0, newToken );
                    MethodDeclarationSyntax newSyntax = dec.WithModifiers( modList );

                    SyntaxNode newRoot = root.ReplaceNode( dec, newSyntax );
                    Document newDoc = document.WithSyntaxRoot( newRoot );

                    return newDoc;
                },
                cancelToken
            ).ConfigureAwait( false );
        }
    }
}
