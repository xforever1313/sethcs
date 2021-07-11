//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Threading.Tasks;
using NUnit.Framework;
using VerifyCS = Tests.Analyzer.CSharpCodeFixVerifier<
    Seth.Analyzer.SethCodeAnalyzer,
    Seth.Analyzer.SethCodeFixProvider
>;


namespace Tests.Analyzer
{
    [TestFixture]
    public sealed class SethCodeAnalyzerTests
    {
        [Test]
        public async Task NoDiasnosticTest()
        {
            string test = @"";

            await VerifyCS.VerifyAnalyzerAsync( test );
        }
    }
}
