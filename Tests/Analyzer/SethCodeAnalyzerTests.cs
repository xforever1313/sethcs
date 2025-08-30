//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = Tests.Analyzer.CSharpCodeFixVerifier<
    Seth.Analyzer.SethCodeAnalyzer,
    Seth.Analyzer.SethCodeFixProvider
>;


namespace Tests.Analyzer
{
    [TestClass]
    public sealed class SethCodeAnalyzerTests
    {
        [TestMethod]
        public async Task NoDiasnosticTest()
        {
            string test = @"";

            await VerifyCS.VerifyAnalyzerAsync( test );
        }
    }
}
