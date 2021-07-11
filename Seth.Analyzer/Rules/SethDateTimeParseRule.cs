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
    public static class SethDateTimeParseRule
    {
        // ---------------- Fields ----------------

        private const string Descriptor = nameof( SethDateTimeParseRule );

        private static readonly Type dateTimeType = typeof( DateTime );

        private static readonly string className = dateTimeType.Name;
        private static readonly string functionName = nameof( DateTime.Parse );
        private static readonly string signature = $"{className}.{functionName}";

        private static readonly LocalizableString Title = signature + " analyzier.";

        private static readonly LocalizableString MessageFormat = $"Avoid using {signature}";

        private static readonly LocalizableString Description =
            $"Do not use {signature}, it differs system to system.  Use {className}.{nameof( DateTime.ParseExact )} instead.";

        private static readonly string RuleCategory = DiagnosticCategory.Warning.ToString();

        private const DiagnosticSeverity Serverity = DiagnosticSeverity.Warning;

        // ---------------- Constructor ----------------

        static SethDateTimeParseRule()
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
            context.RegisterOperationAction( Run, OperationKind.Invocation );
        }

        private static void Run( OperationAnalysisContext context )
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
