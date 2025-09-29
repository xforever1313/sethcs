//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Seth.Analyzer.CodeFixes;

namespace Seth.Analyzer
{
    [ExportCodeFixProvider( LanguageNames.CSharp, Name = nameof( SethCodeFixProvider ) ), Shared]
    public sealed class SethCodeFixProvider : CodeFixProvider
    {
        // ---------------- Fields ----------------

        private readonly Dictionary<string, ICodeFix> fixes;

        private ImmutableArray<string> fixableDiagnosticIds;

        // ---------------- Constructor ----------------

        public SethCodeFixProvider()
        {
            // Use reflection so we don't need to manuall register everything
            // like we're forced to do with rules.
            fixes = new Dictionary<string, ICodeFix>();
            Assembly assm = typeof( SethCodeFixProvider ).Assembly;
            foreach( Type type in assm.GetTypes() )
            {
                if(
                    typeof( ICodeFix ).IsAssignableFrom( type ) &&
                    ( type.IsAbstract == false ) && 
                    ( type.IsInterface == false )
                )
                {
                    ICodeFix codeFix = (ICodeFix)Activator.CreateInstance( type );
                    fixes.Add( codeFix.Rule.Id, codeFix );
                }
            }

            fixableDiagnosticIds = ImmutableArray.Create(
                fixes.Keys.ToArray()
            );
        }

        // ---------------- Properties ----------------

        public override ImmutableArray<string> FixableDiagnosticIds => this.fixableDiagnosticIds;

        // ---------------- Functions ----------------

        public override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override async Task RegisterCodeFixesAsync( CodeFixContext context )
        {
            foreach( Diagnostic diagnostic in context.Diagnostics )
            {
                if( this.fixes.ContainsKey( diagnostic.Id ) == false )
                {
                    continue;
                }

                await this.fixes[diagnostic.Id].RegisterCodeFixesAsync( context, diagnostic );
            }
        }
    }
}
