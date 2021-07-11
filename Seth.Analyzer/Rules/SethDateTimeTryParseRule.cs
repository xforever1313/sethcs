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
    public static class SethDateTimeTryParseRule
    {
        // ---------------- Fields ----------------

        private const string Descriptor = nameof( SethDateTimeParseRule );

        private static readonly Type dateTimeType = typeof( DateTime );

        private static readonly string className = dateTimeType.Name;
        private static readonly string functionName = nameof( DateTime.TryParse );
        private static readonly string signature = $"{className}.{functionName}";

        // ---------------- Constructor ----------------

        static SethDateTimeTryParseRule()
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

        private static LocalizableString Title => signature + " analyzier.";

        private static LocalizableString MessageFormat =>
            $"Avoid using {signature}";

        private static LocalizableString Description =>
            $"Do not use {signature}, it differs system to system.  Use {className}.{nameof( DateTime.TryParseExact )} instead.";

        private static string RuleCategory => DiagnosticCategory.Warning.ToString();

        private static DiagnosticSeverity Serverity => DiagnosticSeverity.Warning;

        // ---------------- Properties ----------------

        public static void Init( AnalysisContext context )
        {
            context.RegisterOperationAction( LookForDateTimeParse, OperationKind.Invocation );
        }

        private static void LookForDateTimeParse( OperationAnalysisContext context )
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
                var diagnostic = Diagnostic.Create( Rule, op.Syntax.GetLocation() );
                context.ReportDiagnostic( diagnostic );
            }
        }
    }
}
