//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Seth.Analyzer.Rules
{
    public sealed class SethDateTimeParseRule : BaseRule
    {
        // ---------------- Fields ----------------

        private static readonly Type dateTimeType = typeof( DateTime );

        private static readonly string className = dateTimeType.Name;
        private static readonly string functionName = nameof( DateTime.Parse );
        private static readonly string signature = $"{className}.{functionName}";

        // ---------------- Constructor ----------------

        public SethDateTimeParseRule()
        {
        }

        // ---------------- Properties ----------------

        protected override LocalizableString Title => signature + " analyzier.";

        protected override LocalizableString MessageFormat =>
            $"Avoid using {signature}";

        protected override LocalizableString Description =>
            $"Do not use {signature}, it differs system to system.  Use {className}.{nameof( DateTime.ParseExact )} instead.";

        protected override Category RuleCategory => Category.Warning;

        protected override DiagnosticSeverity Serverity => DiagnosticSeverity.Warning;

        // ---------------- Properties ----------------

        public override void Init( AnalysisContext context )
        {
            context.RegisterOperationAction( this.LookForDateTimeParse, OperationKind.Invocation );
        }

        private void LookForDateTimeParse( OperationAnalysisContext context )
        {
            IInvocationOperation op = context.Operation as IInvocationOperation;
            if( op == null )
            {
                return;
            }

            IMethodSymbol method = op.TargetMethod;
            if( method == null )
            {
                return;
            }

            INamedTypeSymbol containingType = method.ContainingType;
            if( containingType == null )
            {
                return;
            }

            INamespaceSymbol nameSpace = containingType.ContainingNamespace;
            if( nameSpace == null )
            {
                return;
            }

            if(
                ( nameSpace.Name.Equals( dateTimeType.Namespace ) ) &&
                ( containingType.Name.Equals( dateTimeType.Name ) ) &&
                ( method.Name.Equals( functionName ) )
            )
            {
                var diagnostic = Diagnostic.Create( this.Rule, op.Syntax.GetLocation() );
                context.ReportDiagnostic( diagnostic );
            }
        }
    }
}
