//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace Tests.Analyzer
{
    public static partial class CSharpCodeRefactoringVerifier<TCodeRefactoring>
        where TCodeRefactoring : CodeRefactoringProvider, new()
    {
        public class Test : CSharpCodeRefactoringTest<TCodeRefactoring, DefaultVerifier>
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
