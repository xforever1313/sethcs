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
using Microsoft.CodeAnalysis;
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
    public sealed class SethNUnitMustAssertRuleTests
    {
        // ---------------- Fields ----------------

        private static IEnumerable<Assembly> assemblies = new List<Assembly>
        {
            typeof( TestFixtureAttribute ).Assembly
        }.AsReadOnly();

        // ---------------- Tests ----------------

        [Test]
        public async Task NoWarningTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    public class SomeFixture
    {
        [Test]
        public void SomeTest()
        {
            Assert.Pass( ""Success"" );
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }

        [Test]
        public async Task AssertCallWithCallStackTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    public class SomeFixture
    {
        [Test]
        public void SomeTest()
        {
            CallAssert();
        }

        private void CallAssert()
        {
            Assert.Pass( ""Success!"" );
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }

        [Test]
        public async Task NoAssertCallTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    public class SomeFixture
    {
        [Test]
        public void {|#0:SomeTest|}()
        {
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethNUnitMustAssertRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "SomeTest" );
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies, expected );
        }
    }
}
