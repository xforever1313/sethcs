//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using NUnit.Framework;
using VerifyCS = Tests.Analyzer.CSharpCodeFixVerifier<
    Seth.Analyzer.SethCodeAnalyzer,
    Seth.Analyzer.SethCodeFixProvider
>;

namespace Tests.Analyzer.Rules
{
    [TestFixture]
    public sealed class SethDateTimeParseRule
    {
        [Test]
        public async Task NoDiasnosticTest()
        {
            string test = @"";

            await VerifyCS.VerifyAnalyzerAsync( test );
        }

        [Test]
        public async Task DateTimeParseTest()
        {
            string test =
@"
using System;

namespace ConsoleApplication1
{
    class Program
    {
        private readonly DateTime time;

        public Program()
        {
            this.time = {|#0:DateTime.Parse( ""lol"" )|};            
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( nameof( SethDateTimeParseRule ) )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        [Test]
        public async Task DateTimeParse2MethodTest()
        {
            string test =
@"
using System;
using System.Globalization;

namespace ConsoleApplication1
{
    class Program
    {
        private readonly DateTime time;

        public Program()
        {
            this.time = {|#0:DateTime.Parse( ""lol"", CultureInfo.InvariantCulture )|};            
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( nameof( SethDateTimeParseRule ) )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        [Test]
        public async Task DateTimeParse3MethodTest()
        {
            
            string test =
@"
using System;
using System.Globalization;

namespace ConsoleApplication1
{
    class Program
    {
        private readonly DateTime time;

        public Program()
        {
            this.time = {|#0:DateTime.Parse( ""lol"", CultureInfo.InvariantCulture, DateTimeStyles.None )|};            
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( nameof( SethDateTimeParseRule ) )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }
    }
}
