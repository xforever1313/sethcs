
//          Copyright Seth Hendrick 2016.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.Text;
using NUnit.Framework;
using SethCS.Basic;

namespace Tests.Basic
{
    public class StaticLoggerTest
    {
        // ---------------- Fields ----------------

        private StringBuilder loggedMessages;

        // ---------------- Setup / Teardown ----------------

        [SetUp]
        public void TestSetup()
        {
            this.loggedMessages = new StringBuilder();
            StaticLogger.OnWriteLine += StaticLogger_OnWriteLine;
        }

        [TearDown]
        public void TestTeardown()
        {
            StaticLogger.OnWriteLine -= StaticLogger_OnWriteLine;
        }

        // ---------------- Tests ----------------

        [Test]
        public void WriteEmptyLineTest()
        {
            StaticLogger.WriteLine();
            Assert.AreEqual( string.Empty + Environment.NewLine, this.loggedMessages.ToString() );
        }

        [Test]
        public void WriteLineTest()
        {
            const string expectedString = "Hello, World!";
            StaticLogger.WriteLine( expectedString );
            Assert.AreEqual( expectedString + Environment.NewLine, this.loggedMessages.ToString() );
        }

        [Test]
        public void WriteLineFormatTest()
        {
            const string formatString = "{0} + {1} = {2}";
            string expectedString = "1 + 2 = 3" + Environment.NewLine;

            StaticLogger.WriteLine( formatString, 1, 2, 3 );
            Assert.AreEqual( expectedString, this.loggedMessages.ToString() );
        }

        // ---------------- Test Helpers ----------------

        private void StaticLogger_OnWriteLine( string line )
        {
            this.loggedMessages.Append( line );
        }
    }
}
