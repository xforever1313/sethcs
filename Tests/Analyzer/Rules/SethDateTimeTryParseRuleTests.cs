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
    public sealed class SethDateTimeTryParseRuleTests
    {
        [Test]
        public async Task DateTimeTryParseTest()
        {
            string test =
@"
using System;

namespace TestNamespace
{
    public class Program
    {
        private readonly DateTime time;

        public Program()
        {
            {|#0:DateTime.TryParse( ""lol"", out this.time )|};       
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethDateTimeTryParseRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }

        [Test]
        public async Task DateTimeTryParse4MethodTest()
        {
            string test =
@"
using System;
using System.Globalization;

namespace TestNamespace
{
    public class Program
    {
        private readonly DateTime time;

        public Program()
        {
            {|#0:DateTime.TryParse( ""lol"", CultureInfo.InvariantCulture, DateTimeStyles.None, out this.time )|};            
        }
    }
}
";
            var expected = VerifyCS.Diagnostic( SethDateTimeTryParseRule.Rule )
                .WithLocation( 0 )
                .WithSeverity( DiagnosticSeverity.Warning );

            await VerifyCS.VerifyAnalyzerAsync( test, expected );
        }
    }
}
