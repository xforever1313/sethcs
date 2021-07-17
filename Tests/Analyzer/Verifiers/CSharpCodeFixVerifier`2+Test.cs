using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace Tests.Analyzer
{
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, NUnitVerifier>
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

            public void AddAssenblies( IEnumerable<Assembly> assemblies )
            {
                if( assemblies == null )
                {
                    return;
                }

                List<string> assemblyList = new List<string>();
                foreach( Assembly assembly in assemblies )
                {
                    // ReferenceAssemblies takes in the assembly path minus the .dll at the end.
                    string assemblyLocation = assembly.Location;
                    string assemblyName = Path.GetFileNameWithoutExtension( assemblyLocation );
                    assemblyLocation = Path.Combine( Path.GetDirectoryName( assemblyLocation ), assemblyName );

                    assemblyList.Add( assemblyLocation );
                }

                this.ReferenceAssemblies = this.ReferenceAssemblies.AddAssemblies(
                    ImmutableArray.Create( assemblyList.ToArray() )
                );
            }
        }
    }
}
