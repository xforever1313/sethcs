//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;
using Seth.Analyzer.Rules;
using SethCS.Basic;
using VerifyCS = Tests.Analyzer.CSharpCodeFixVerifier<
    Seth.Analyzer.SethCodeAnalyzer,
    Seth.Analyzer.SethCodeFixProvider
>;

namespace Tests.Analyzer.Rules
{
    [TestFixture]
    public sealed class SethMustCallRuleTests
    {
        // ---------------- Fields ----------------

        private static IEnumerable<Assembly> assemblies = new List<Assembly>
        {
            typeof( MustCallAttribute ).Assembly
        }.AsReadOnly();

        // ---------------- Tests ----------------

        [Test]
        public async Task NoWarningTest()
        {
            string test =
@"
using System;
using SethCS.Basic;

namespace TestNamespace
{
    public class MustCallClass
    {
        [MustCall]
        public void Init()
        {
        }
    }

    public class Program
    {
        public void SomeFunction()
        {
            var instance = new MustCallClass();
            instance.Init();
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }
    }
}
