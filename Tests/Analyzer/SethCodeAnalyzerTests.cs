//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using NUnit.Framework;
using Seth.Analyzer;

namespace Tests.Analyzer
{
    [TestFixture]
    public sealed class SethCodeAnalyzerTests
    {
        [Test]
        public void ConstructorTest()
        {
            SethCodeAnalyzer uut = new SethCodeAnalyzer();
            Assert.AreNotEqual( 0, uut.SupportedDiagnostics.Length );
        }
    }
}
