//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Seth.Analyzer.Rules;

namespace Seth.Analyzer
{
    [DiagnosticAnalyzer( LanguageNames.CSharp )]
    public sealed class SethCodeAnalyzer : DiagnosticAnalyzer
    {
        // ---------------- Fields ----------------

        private readonly ImmutableArray<DiagnosticDescriptor> descriptors;

        // ---------------- Constructor ----------------

        public SethCodeAnalyzer()
        {
            this.descriptors = ImmutableArray.Create(
                SethDateTimeParseRule.Rule,
                SethDateTimeTryParseRule.Rule,
                SethClassAccessModifierRule.Rule,

                // Doesn't work at the moment (Well, it does, but not for complex cases).
                // Don't include for now.
                //SethNUnitMustAssertRule.Rule,
                SethNUnitClassAttributeRules.SethNUnitClassContainsTestMethodMustBeAFixtureRule.Rule,
                SethNUnitClassAttributeRules.SethNUnitTestFixtureMustBePublicRule.Rule,
                SethNUnitClassAttributeRules.SethNUnitTestFixtureMustBeSealedRule.Rule,
                SethNUnitClassAttributeRules.SethNUnitTestMethodMustBePublicRule.Rule
            );
        }

        // ---------------- Properties ----------------

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => this.descriptors;

        // ---------------- Functions ----------------

        public override void Initialize( AnalysisContext context )
        {
            context.ConfigureGeneratedCodeAnalysis( GeneratedCodeAnalysisFlags.None );
            context.EnableConcurrentExecution();

            SethDateTimeParseRule.Init( context );
            SethDateTimeTryParseRule.Init( context );
            SethClassAccessModifierRule.Init( context );
            //SethNUnitMustAssertRule.Init( context );
            SethNUnitClassAttributeRules.Init( context );
        }
    }
}
