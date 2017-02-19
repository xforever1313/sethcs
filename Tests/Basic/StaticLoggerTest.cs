
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

        private StringBuilder writeLineLoggedMessages;

        private StringBuilder errorWriteLineLoggedMessages;

        // ---------------- Setup / Teardown ----------------

        [SetUp]
        public void TestSetup()
        {
            this.writeLineLoggedMessages = new StringBuilder();
            this.errorWriteLineLoggedMessages = new StringBuilder();
            StaticLogger.OnWriteLine += StaticLogger_OnWriteLine;
            StaticLogger.OnErrorWriteLine += StaticLogger_OnErrorWriteLine;
        }

        [TearDown]
        public void TestTeardown()
        {
            StaticLogger.OnWriteLine -= StaticLogger_OnWriteLine;
            StaticLogger.OnErrorWriteLine -= StaticLogger_OnErrorWriteLine;
        }

        // ---------------- Tests ----------------

        // -------- WriteLine Tests --------

        [Test]
        public void WriteEmptyLineTest()
        {
            StaticLogger.WriteLine();
            Assert.AreEqual( string.Empty + Environment.NewLine, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
        }

        [Test]
        public void WriteLineTest()
        {
            const string expectedString = "Hello, World!";
            StaticLogger.WriteLine( expectedString );
            Assert.AreEqual( expectedString + Environment.NewLine, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
        }

        [Test]
        public void WriteLineFormatTest()
        {
            const string formatString = "{0} + {1} = {2}";
            string expectedString = "1 + 2 = 3" + Environment.NewLine;

            StaticLogger.WriteLine( formatString, 1, 2, 3 );
            Assert.AreEqual( expectedString, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
        }

        // -------- ErrorWriteLine Tests --------

        [Test]
        public void ErrorWriteEmptyLineTest()
        {
            StaticLogger.ErrorWriteLine();
            Assert.AreEqual( string.Empty + Environment.NewLine, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
        }

        [Test]
        public void ErrorWriteLineTest()
        {
            const string expectedString = "Hello, World!";
            StaticLogger.ErrorWriteLine( expectedString );
            Assert.AreEqual( expectedString + Environment.NewLine, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
        }

        [Test]
        public void ErrorWriteLineFormatTest()
        {
            const string formatString = "{0} + {1} = {2}";
            string expectedString = "1 + 2 = 3" + Environment.NewLine;

            StaticLogger.ErrorWriteLine( formatString, 1, 2, 3 );
            Assert.AreEqual( expectedString, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
        }

        // ---------------- Test Helpers ----------------

        private void StaticLogger_OnWriteLine( string line )
        {
            this.writeLineLoggedMessages.Append( line );
        }

        private void StaticLogger_OnErrorWriteLine( string line )
        {
            this.errorWriteLineLoggedMessages.Append( line );
        }
    }
}
