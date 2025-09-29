//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Tests.Analyzer
{
    public static partial class CSharpAnalyzerVerifier<TAnalyzer>
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        public class Test : CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
        {
            public Test()
            {
                SolutionTransforms.Add( ( solution, projectId ) =>
                 {
                     var compilationOptions = solution.GetProject( projectId ).CompilationOptions;
                     compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                         compilationOptions.SpecificDiagnosticOptions.SetItems( CSharpVerifierHelper.NullableWarnings ) );
                     solution = solution.WithProjectCompilationOptions( projectId, compilationOptions );

                     return solution;
                 } );
            }
        }
    }
}
