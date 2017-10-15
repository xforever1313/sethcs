//
//          Copyright Seth Hendrick 2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SethCS.Basic;

namespace Tests.Basic
{
    [TestFixture]
    public class GenericLoggerTest
    {
        // ---------------- Fields ----------------

        private StringBuilder writeLineLoggedMessages;

        private StringBuilder errorWriteLineLoggedMessages;

        private GenericLogger uut;

        // ---------------- Setup / Teardown ----------------

        [OneTimeSetUp]
        public void FixtureSetup()
        {

        }

        [OneTimeTearDown]
        public void FixtureTeardown()
        {
        }

        [SetUp]
        public void TestSetup()
        {
            this.uut = new GenericLogger();

            this.writeLineLoggedMessages = new StringBuilder();
            this.errorWriteLineLoggedMessages = new StringBuilder();
            this.uut.OnWriteLine += Uut_OnWriteLine;
            this.uut.OnErrorWriteLine += Uut_OnErrorWriteLine;
        }

        [TearDown]
        public void TestTeardown()
        {
        }

        // ---------------- Tests ----------------

        [Test]
        public void WriteEmptyLineTest()
        {
            // Required Verbosity: 101.  Nothing written.
            this.uut.Verbosity = 100;
            this.uut.WriteLine( 101 );

            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );

            // Required Verbosity: -1, nothing written.
            this.uut.Verbosity = -1;
            this.uut.WriteLine();

            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );

            // Required Verbosity: 0.  Something should be written.
            this.uut.Verbosity = 0;
            this.uut.WriteLine();

            Assert.AreEqual( string.Empty + Environment.NewLine, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
        }

        [Test]
        public void WriteLineTest()
        {
            const string expectedString = "Hello, World!";

            // Required Verbosity: 101.  Nothing written.
            this.uut.Verbosity = 100;
            this.uut.WriteLine( 101, expectedString );

            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );

            // Required Verbosity: -1, nothing written.
            this.uut.Verbosity = -1;
            this.uut.WriteLine( expectedString );

            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );

            // Required Verbosity: 0.  Something should be written.
            this.uut.Verbosity = 0;
            this.uut.WriteLine( expectedString );

            Assert.AreEqual( expectedString + Environment.NewLine, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
        }

        [Test]
        public void WriteLineFormatTest()
        {
            const string formatString = "{0} + {1} = {2}";
            string expectedString = "1 + 2 = 3";

            // Required Verbosity: 101.  Nothing written.
            this.uut.Verbosity = 100;
            this.uut.WriteLine( 101, formatString, 1, 2, 3 );

            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );

            // Required Verbosity: -1, nothing written.
            this.uut.Verbosity = -1;
            this.uut.WriteLine( formatString, 1, 2, 3 );

            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );

            // Required Verbosity: 0.  Something should be written.
            this.uut.Verbosity = 0;
            this.uut.WriteLine( formatString, 1, 2, 3 );

            Assert.AreEqual( expectedString + Environment.NewLine, this.writeLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
        }

        [Test]
        public void ErrorWriteEmptyLineTest()
        {
            // Required Verbosity: 101.  Nothing written.
            this.uut.Verbosity = 100;
            this.uut.ErrorWriteLine( 101 );

            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );

            // Required Verbosity: -1, nothing written.
            this.uut.Verbosity = -1;
            this.uut.ErrorWriteLine();

            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );

            // Required Verbosity: 0.  Something should be written.
            this.uut.Verbosity = 0;
            this.uut.ErrorWriteLine();

            Assert.AreEqual( string.Empty + Environment.NewLine, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
        }

        [Test]
        public void ErrorWriteLineTest()
        {
            const string expectedString = "Hello, World!";

            // Required Verbosity: 101.  Nothing written.
            this.uut.Verbosity = 100;
            this.uut.ErrorWriteLine( 101, expectedString );

            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );

            // Required Verbosity: -1, nothing written.
            this.uut.Verbosity = -1;
            this.uut.ErrorWriteLine( expectedString );

            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );

            // Required Verbosity: 0.  Something should be written.
            this.uut.Verbosity = 0;
            this.uut.ErrorWriteLine( expectedString );

            Assert.AreEqual( expectedString + Environment.NewLine, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
        }

        [Test]
        public void ErrorWriteLineFormatTest()
        {
            const string formatString = "{0} + {1} = {2}";
            string expectedString = "1 + 2 = 3";

            // Required Verbosity: 101.  Nothing written.
            this.uut.Verbosity = 100;
            this.uut.ErrorWriteLine( 101, formatString, 1, 2, 3 );

            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );

            // Required Verbosity: -1, nothing written.
            this.uut.Verbosity = -1;
            this.uut.ErrorWriteLine( formatString, 1, 2, 3 );

            Assert.AreEqual( string.Empty, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );

            // Required Verbosity: 0.  Something should be written.
            this.uut.Verbosity = 0;
            this.uut.ErrorWriteLine( formatString, 1, 2, 3 );

            Assert.AreEqual( expectedString + Environment.NewLine, this.errorWriteLineLoggedMessages.ToString() );
            Assert.AreEqual( string.Empty, this.writeLineLoggedMessages.ToString() );
        }

        // ---------------- Test Helpers ----------------

        private void Uut_OnWriteLine( string obj )
        {
            this.writeLineLoggedMessages.Append( obj );
        }

        private void Uut_OnErrorWriteLine( string obj )
        {
            this.errorWriteLineLoggedMessages.Append( obj );
        }
    }
}
