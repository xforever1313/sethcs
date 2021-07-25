//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

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

            SyntaxToken token = SyntaxFactory.Token( SyntaxKind.PublicKeyword );
            MethodDeclarationSyntax newSyntax = declaration.AddModifiers( token );

            SyntaxNode newRoot = root.ReplaceNode( declaration, newSyntax );
            Document newDoc = context.Document.WithSyntaxRoot( newRoot );

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
                    SyntaxToken token = SyntaxFactory.Token( SyntaxKind.PublicKeyword );
                    MethodDeclarationSyntax newSyntax = dec.AddModifiers( token );

                    SyntaxNode newRoot = root.ReplaceNode( dec, newSyntax );
                    Document newDoc = document.WithSyntaxRoot( newRoot );

                    return newDoc;
                },
                cancelToken
            ).ConfigureAwait( false );
        }
    }
}
