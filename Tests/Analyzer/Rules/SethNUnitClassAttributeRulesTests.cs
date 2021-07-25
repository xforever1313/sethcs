//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using Seth.Analyzer.Rules;
using VerifyCS = Tests.Analyzer.CSharpCodeFixVerifier<
    Seth.Analyzer.SethCodeAnalyzer,
    Seth.Analyzer.SethCodeFixProvider
>;

namespace Tests.Analyzer.Rules
{
    [TestFixture]
    public sealed class SethNUnitClassAttributeRulesTests
    {
        // ---------------- Fields ----------------

        private static IEnumerable<Assembly> assemblies = new List<Assembly>
        {
            typeof( TestFixtureAttribute ).Assembly
        }.AsReadOnly();

        // ---------------- Tests ----------------

        /// <summary>
        /// If there are zero attributes, there should be no warnings.
        /// </summary>
        [Test]
        public async Task NoAttributesNoWarningsTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    public class TestFixture
    {
        public void TestMethod()
        {
            HelperMethod();
        }

        private void HelperMethod()
        {
            Assert.Pass();
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }

        /// <summary>
        /// NUnit class is fine!  No warnings should appear.
        /// </summary>
        [Test]
        public async Task NoWarningsTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public sealed class TestFixture
    {
        [Test]
        public void TestMethod()
        {
            HelperMethod();
        }

        private void HelperMethod()
        {
            Assert.Pass();
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }

        /// <summary>
        /// Fixture is missing sealed.
        /// </summary>
        [Test]
        public async Task MissingSealedTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public class {|#0:TestFixture|}
    {
        [Test]
        public void TestMethod()
        {
            HelperMethod();
        }

        private void HelperMethod()
        {
            Assert.Pass();
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethNUnitClassAttributeRules.SethNUnitTestFixtureMustBeSealedRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "TestFixture" );
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies, expected );
        }

        /// <summary>
        /// Fixture is internal, not public.
        /// </summary>
        [Test]
        public async Task InternalClassTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    internal sealed class {|#0:TestFixture|}
    {
        [Test]
        public void TestMethod()
        {
            HelperMethod();
        }

        private void HelperMethod()
        {
            Assert.Pass();
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethNUnitClassAttributeRules.SethNUnitTestFixtureMustBePublicRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "TestFixture" );
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies, expected );
        }

        [Test]
        public async Task TestMethodHasNoModifierTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public sealed class TestFixture
    {
        [Test]
        void {|#0:TestMethod|}()
        {
            HelperMethod();
        }

        private void HelperMethod()
        {
            Assert.Pass();
        }
    }
}
";
            string fixTest =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public sealed class TestFixture
    {
        [Test]
        public void {|#0:TestMethod|}()
        {
            HelperMethod();
        }

        private void HelperMethod()
        {
            Assert.Pass();
        }
    }
}
";

            var expected = VerifyCS.Diagnostic( SethNUnitClassAttributeRules.SethNUnitTestMethodMustBePublicRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "TestMethod", "TestFixture" );

            //await VerifyCS.VerifyAnalyzerAsync( test, assemblies, expected );
            await VerifyCS.VerifyCodeFixAsync( test, expected, fixTest, assemblies );
        }

        [Test]
        public async Task TestMethodHasPrivateModifierTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public sealed class TestFixture
    {
        [Test]
        private void {|#0:TestMethod|}()
        {
            HelperMethod();
        }

        private void HelperMethod()
        {
            Assert.Pass();
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethNUnitClassAttributeRules.SethNUnitTestMethodMustBePublicRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "TestMethod", "TestFixture" );
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies, expected );
        }
    }
}

