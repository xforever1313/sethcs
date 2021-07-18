//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Seth.Analyzer.Rules
{
    /// <remarks>
    /// This doesn't work since it can't handle dynamic behavior.
    /// However, its a good example, so it stays.
    /// </remarks>
    public static class SethNUnitMustAssertRule
    {
        // ---------------- Fields ----------------

        private const string Descriptor = nameof( SethNUnitMustAssertRule );

        private static readonly LocalizableString Title = "NUnit tests must call Assert";
        private static readonly LocalizableString MessageFormat = "Assert must be called at least once in test '{0}'";
        private static readonly LocalizableString Description =
            "Assert should be called at least once in an NUnit test to ensure we actually test for something.";

        private static readonly string RuleCategory = DiagnosticCategory.Warning.ToString();

        private const DiagnosticSeverity Serverity = DiagnosticSeverity.Warning;

        public static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            Descriptor,
            Title,
            MessageFormat,
            RuleCategory.ToString(),
            Serverity,
            isEnabledByDefault: true,
            description: Description
        );

        // ---------------- Functions ----------------

        public static void Init( AnalysisContext context )
        {
            context.RegisterOperationAction( Run, OperationKind.MethodBody );
        }

        private static void Run( OperationAnalysisContext context )
        {
            ISymbol symbol = context.ContainingSymbol;
            if( symbol is IMethodSymbol == false )
            {
                return;
            }

            IMethodSymbol testMethod = (IMethodSymbol)symbol;
            var attributes = testMethod.GetAttributes();
            if( attributes.Length == 0 )
            {
                return;
            }

            bool foundAttribute = false;
            foreach( AttributeData attribute in attributes )
            {
                INamedTypeSymbol attributeSymbol = attribute.AttributeClass;
                if( attributeSymbol == null )
                {
                    continue;
                }

                INamespaceSymbol namespaceSymbol = attributeSymbol.ContainingNamespace;
                if( namespaceSymbol == null )
                {
                    continue;
                }

                if(
                    ( "NUnit.Framework".Equals( namespaceSymbol.ToString() ) ) &&
                    ( "TestAttribute".Equals( attributeSymbol.Name ) )
                )
                {
                    foundAttribute = true;
                    break;
                }
            }

            if( foundAttribute == false )
            {
                return;
            }

            Stack<IOperation> operations = new Stack<IOperation>();
            operations.Push( context.Operation );

            while( operations.Count != 0 )
            {
                IOperation currentOp = operations.Pop();
                if( IsAssertInvocation( currentOp, operations ) )
                {
                    return;
                }

                foreach( IOperation child in currentOp.Children )
                {
                    operations.Push( child );
                }
            }

            var diagnostic = Diagnostic.Create(
                Rule,
                testMethod.Locations[0],
                testMethod.Name ?? string.Empty
            );
            context.ReportDiagnostic( diagnostic );
        }

        private static bool IsAssertInvocation( IOperation operation, Stack<IOperation> operations )
        {
            if( operation is IInvocationOperation invokeOp )
            {
                IMethodSymbol invocationMethod = invokeOp.TargetMethod;
                if( invocationMethod != null )
                {
                    INamedTypeSymbol classType = invocationMethod.ContainingType;
                    if( "NUnit.Framework.Assert".Equals( classType.ToString() ) )
                    {
                        return true;
                    }
                }

                foreach( Location location in invocationMethod.Locations )
                {
                    SyntaxTree tree = location.SourceTree;
                    if( tree == null )
                    {
                        continue;
                    }

                    if( tree.TryGetRoot( out SyntaxNode root ) && ( root != null ) )
                    {
                        SyntaxNode operationNode = root.FindNode( location.SourceSpan );
                        if( operationNode == null )
                        {
                            continue;
                        }

                        IOperation callingOp = operation.SemanticModel?.GetOperation( operationNode );
                        if( callingOp != null )
                        {
                            operations.Push( callingOp );
                        }
                    }
                }
            }

            return false;
        }
    }
}
