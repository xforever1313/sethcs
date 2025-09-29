//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Tests.Analyzer
{
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic()"/>
        public static DiagnosticResult Diagnostic()
            => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic();

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic(string)"/>
        public static DiagnosticResult Diagnostic( string diagnosticId )
            => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic( diagnosticId );

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic(DiagnosticDescriptor)"/>
        public static DiagnosticResult Diagnostic( DiagnosticDescriptor descriptor )
            => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic( descriptor );

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
        public static async Task VerifyAnalyzerAsync( string source, params DiagnosticResult[] expected )
        {
            await VerifyAnalyzerAsync( source, null, expected );
        }

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
        public static async Task VerifyAnalyzerAsync( string source, IEnumerable<Assembly> assemblies, params DiagnosticResult[] expected )
        {
            var test = new Test
            {
                TestCode = source,
                ReferenceAssemblies = ReferenceAssemblies.Net.Net80
            };

            test.ExpectedDiagnostics.AddRange( expected );
            test.AddAssenblies( assemblies );
            await test.RunAsync( CancellationToken.None );
        }

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, string)"/>
        public static async Task VerifyCodeFixAsync( string source, string fixedSource )
            => await VerifyCodeFixAsync( source, DiagnosticResult.EmptyDiagnosticResults, fixedSource );

        public static async Task VerifyCodeFixAsync( string source, string fixedSource, IEnumerable<Assembly> assemblies )
            => await VerifyCodeFixAsync( source, DiagnosticResult.EmptyDiagnosticResults, fixedSource, assemblies );

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, DiagnosticResult, string)"/>
        public static async Task VerifyCodeFixAsync( string source, DiagnosticResult expected, string fixedSource )
            => await VerifyCodeFixAsync( source, expected, fixedSource, null );

        public static async Task VerifyCodeFixAsync( string source, DiagnosticResult expected, string fixedSource, IEnumerable<Assembly> assemblies )
            => await VerifyCodeFixAsync( source, new[] { expected }, fixedSource, assemblies );

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, DiagnosticResult[], string)"/>
        public static async Task VerifyCodeFixAsync( string source, DiagnosticResult[] expected, string fixedSource )
        {
            await VerifyCodeFixAsync( source, expected, fixedSource, null );
        }

        public static async Task VerifyCodeFixAsync( string source, DiagnosticResult[] expected, string fixedSource, IEnumerable<Assembly> assemblies )
        {
            var test = new Test
            {
                ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
                TestCode = source,
                FixedCode = fixedSource
            };

            test.ExpectedDiagnostics.AddRange( expected );
            test.AddAssenblies( assemblies );
            await test.RunAsync( CancellationToken.None );
        }
    }
}
