//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;

namespace Seth.Analyzer
{
    [ExportCodeFixProvider( LanguageNames.CSharp, Name = nameof( SethCodeFixProvider ) ), Shared]
    public sealed class SethCodeFixProvider : CodeFixProvider
    {
        // ---------------- Constructor ----------------

        public SethCodeFixProvider()
        {
        }

        // ---------------- Properties ----------------

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray<string>.Empty;

        // ---------------- Functions ----------------

        public override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override async Task RegisterCodeFixesAsync( CodeFixContext context )
        {
            await context.Document.GetSyntaxRootAsync( context.CancellationToken ).ConfigureAwait( false );

            // TODO: Add things
        }
    }
}
