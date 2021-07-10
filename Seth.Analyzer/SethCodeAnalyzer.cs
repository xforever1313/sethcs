//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Seth.Analyzer.Rules;

namespace Seth.Analyzer
{
    [DiagnosticAnalyzer( LanguageNames.CSharp )]
    public sealed class SethCodeAnalyzer : DiagnosticAnalyzer
    {
        // ---------------- Fields ----------------

        private readonly List<BaseRule> rules;

        private readonly ImmutableArray<DiagnosticDescriptor> descriptors;

        // ---------------- Constructor ----------------

        public SethCodeAnalyzer()
        {
            this.rules = new List<BaseRule>();

            foreach( Type type in typeof( SethCodeAnalyzer ).Assembly.GetTypes() )
            {
                if( type.IsSubclassOf( typeof( BaseRule ) ) )
                {
                    this.rules.Add( (BaseRule)Activator.CreateInstance( type ) );
                }
            }

            this.descriptors = ImmutableArray.Create( this.rules.Select( r => r.Rule ).ToArray() );
        }

        // ---------------- Properties ----------------

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => this.descriptors;

        // ---------------- Functions ----------------

        public override void Initialize( AnalysisContext context )
        {
            context.ConfigureGeneratedCodeAnalysis( GeneratedCodeAnalysisFlags.None );
            context.EnableConcurrentExecution();

            foreach( BaseRule rule in this.rules )
            {
                rule.Init( context );
            }
        }
    }
}
