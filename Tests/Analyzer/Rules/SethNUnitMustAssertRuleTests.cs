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

        /// <summary>
        /// If we don't call Assert, we should get a warning.
        /// </summary>
        [Test]
        public async Task NoAssertCallTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
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

        /// <summary>
        /// If we invoke Assert in the method of the test, we should not get a warning.
        /// </summary>
        [Test]
        public async Task AssertInTestBodyTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
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

        /// <summary>
        /// If we call Assert within the a method of the same class,
        /// we should get no warning.
        /// </summary>
        [Test]
        public async Task AssertCallWithCallStackTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
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

        /// <summary>
        /// If an instance class calls assert, we should
        /// not get a warning.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task InstanceAssertCallTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public class SomeFixture
    {
        [Test]
        public void SomeTest()
        {
            ClassA a = new ClassA();
            a.CallAssert();
        }
    }

    public class ClassA
    {
        public void CallAssert()
        {
            Assert.Pass( ""Passed!"" );
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }

        /// <summary>
        /// Ensure if we call assert in an override method,
        /// but call that class that overrides, we get no warning.
        /// </summary>
        [Test]
        public async Task OverrideAndInvokeChildClassTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public class SomeFixture
    {
        [Test]
        public void SomeTest()
        {
            ClassB a = new ClassB();
            a.CallAssert();
        }
    }

    public class ClassA
    {
        public virtual void CallAssert()
        {
        }
    }

    public class ClassB : ClassA
    {
        public override void CallAssert()
        {
            Assert.Pass( ""Passed!"" );
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }

        /// <summary>
        /// Ensure if we call assert in an override method,
        /// but the type is the base type, we don't get the warning.
        /// </summary>
        [Test]
        public async Task OverrideAndInvokeBaseClassTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public class SomeFixture
    {
        [Test]
        public void SomeTest()
        {
            ClassA a = new ClassB();
            a.CallAssert();
        }
    }

    public class ClassA
    {
        public virtual void CallAssert()
        {
        }
    }

    public class ClassB : ClassA
    {
        public override void CallAssert()
        {
            Assert.Pass( ""Passed!"" );
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }

        /// <summary>
        /// If we call Assert through an action, we
        /// should not get a warning.
        /// </summary>
        [Test]
        public async Task AssertThroughActionTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public class SomeFixture
    {
        [Test]
        public void SomeTest()
        {
            Action assertAction = new Action(
                () =>
                {
                    Assert.Pass( ""Success!"" );
                }
            );

             assertAction();
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }

        /// <summary>
        /// If we call Assert through an action on a property, we
        /// should not get a warning.
        /// </summary>
        [Test]
        public async Task AssertThroughPropertyFuncTest()
        {
            string test =
@"
using System;
using NUnit.Framework;

namespace TestNamespace
{
    [TestFixture]
    public class SomeFixture
    {
        [Test]
        public void SomeTest()
        {
            ClassA a = new ClassA();
            a.AssertFunc();
        }
    }

    public class ClassA
    {
        public ClassA()
        {
            this.AssertFunc = delegate ()
            {
                Assert.Pass( ""Success!"" );
                return true;
            };
        }

        public Func<bool> AssertFunc { get; private set; }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test, assemblies );
        }
    }
}
