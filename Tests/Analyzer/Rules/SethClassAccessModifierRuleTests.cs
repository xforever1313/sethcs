//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

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
    public sealed class SethClassAccessModifierRuleTests
    {
        // ---------------- Tests ----------------

        // -------- Classes --------

        /// <summary>
        /// No access declaration, make warning.
        /// </summary>
        [Test]
        public async Task ClassNoAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    class {|#0:Program|}
    {
        public Program()
        {
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethClassAccessModifierRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "class", "Program" );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        /// <summary>
        /// No access declaration on inner class, make warning.
        /// </summary>
        [Test]
        public async Task InnerClassNoAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        class {|#0:InnerProgram|}
        {
            public InnerProgram()
            {
            }
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethClassAccessModifierRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "class", "InnerProgram" );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        /// <summary>
        /// Has public access declaration, no warning expected.
        /// </summary>
        [Test]
        public async Task ClassPublicAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        public Program()
        {
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task ClassProtectedAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        protected class InnerProgram
        {
            public InnerProgram()
            {
            }
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task ClassPrivateAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        private class InnerProgram
        {
            public InnerProgram()
            {
            }
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task ClassInternalAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    internal class Program
    {
        public Program()
        {
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task ClassProtectedInternalAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        protected internal class InnerProgram
        {
            public InnerProgram()
            {
            }
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }
        [Test]
        public async Task ClassPrivateProtectedAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        private protected class InnerProgram
        {
            public InnerProgram()
            {
            }
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        // -------- Structs --------

        /// <summary>
        /// No access declaration, make warning.
        /// </summary>
        [Test]
        public async Task StructNoAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    struct {|#0:Point|}
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethClassAccessModifierRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "struct", "Point" );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        /// <summary>
        /// No access declaration on inner struct, make warning.
        /// </summary>
        [Test]
        public async Task InnerStructNoAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        struct {|#0:InnerPoint|}
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethClassAccessModifierRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "struct", "InnerPoint" );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        /// <summary>
        /// Has public access declaration, no warning expected.
        /// </summary>
        [Test]
        public async Task StructPublicAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task StructProtectedAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        protected struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task StructPrivateAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        private struct InnerPoint
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task StructInternalAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    internal struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task StructProtectedInternalAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        protected internal struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task StructPrivateProtectedAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        private protected struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        // -------- Interfaces --------

        /// <summary>
        /// No access declaration, make warning.
        /// </summary>
        [Test]
        public async Task InterfaceNoAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    interface {|#0:IProgram|}
    {
        void Run();
    }
}
";
            var expected = VerifyCS.Diagnostic( SethClassAccessModifierRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "interface", "IProgram" );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        /// <summary>
        /// No access declaration on inner class, make warning.
        /// </summary>
        [Test]
        public async Task InnerInterfaceNoAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        interface {|#0:IInnerProgram|}
        {
            void Run();
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethClassAccessModifierRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "interface", "IInnerProgram" );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        /// <summary>
        /// Has public access declaration, no warning expected.
        /// </summary>
        [Test]
        public async Task InterfacePublicAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public interface IProgram
    {
        void Run();
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task InterfaceProtectedAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        protected interface IInnerProgram
        {
            void Run();
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task InterfacePrivateAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        private interface IInnerProgram
        {
            void Run();
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task InterfaceInternalAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    internal interface Program
    {
        void Run();
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task InterfaceProtectedInternalAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        protected internal interface IInnerProgram
        {
            void Run();
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }
        [Test]
        public async Task InterfacePrivateProtectedAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        private protected interface IInnerProgram
        {
            void Run();
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        // -------- Enums --------

        /// <summary>
        /// No access declaration, make warning.
        /// </summary>
        [Test]
        public async Task EnumNoAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    enum {|#0:SomeEnum|}
    {
        Value1,
        Value2
    }
}
";
            var expected = VerifyCS.Diagnostic( SethClassAccessModifierRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "Enum", "SomeEnum" );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        /// <summary>
        /// No access declaration on inner class, make warning.
        /// </summary>
        [Test]
        public async Task InnerEnumNoAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        enum {|#0:InnerEnum|}
        {
            Value1,
            Value2
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethClassAccessModifierRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning )
                .WithArguments( "Enum", "InnerEnum" );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        /// <summary>
        /// Has public access declaration, no warning expected.
        /// </summary>
        [Test]
        public async Task EnumPublicAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public enum SomeEnum
    {
        Value1,
        Value2
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task EnumProtectedAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        protected enum InnerEnum
        {
            Value1,
            Value2
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task EnumPrivateAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        private enum InnerEnum
        {
            Value1,
            Value2
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task EnumInternalAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    internal enum SomeEnum
    {
        Value1,
        Value2
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task EnumProtectedInternalAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        protected internal enum InnerEnum
        {
            Value1,
            Value2
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }
        [Test]
        public async Task EnumPrivateProtectedAccessModifierDeclarationTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        private protected enum InnerEnum
        {
            Value1,
            Value2
        }
    }
}
";
            await VerifyCS.VerifyAnalyzerAsync( test );
        }
    }
}
