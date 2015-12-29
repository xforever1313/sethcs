using System;
using System.IO;
using NUnit.Framework;
using SethCS.IO;

namespace Tests
{
namespace IO
{
    [TestFixture]
    public class ConsoleHelpersTest
    {
        // -------- Fields --------

        /// <summary>
        /// TextReader that acts like Console.In from the user.
        /// </summary>
        private StringReader mockCin;

        /// <summary>
        /// TextWriter that acts like Console.Out to the user.
        /// </summary>
        private StringWriter mockCout;

        // -------- Setup / Teardown --------

        [SetUp]
        public void TestSetup()
        {
            // Cin needs a string, must be defined in the test.
            this.mockCout = new StringWriter();
        }

        [TearDown]
        public void TestTeardown()
        {
            this.mockCin = null;
            this.mockCout = null;
        }

        // -------- Tests --------

        [Test]
        public void ConsoleHelpersGetBoolTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "herp" + Environment.NewLine +  // 1: Fail
                "true" + Environment.NewLine +  // 2: Returns true
                "false" + Environment.NewLine + // 3: Returns false
                "True" + Environment.NewLine +  // 4: Returns true
                "False" + Environment.NewLine + // 5: Returns false
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseBoolErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseBoolErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseBoolErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect true).
             {
                bool? case2 = ConsoleHelpers.GetBool(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsTrue( case2.Value );
             }

             // Case 3 returns false.
             {
                bool? case3 = ConsoleHelpers.GetBool(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsFalse( case3.Value );
             }

             // Case 4 returns true.
             {
                bool? case4 = ConsoleHelpers.GetBool(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsTrue( case4.Value );
             }

             // Case 5 returns false.
             {
                bool? case5 = ConsoleHelpers.GetBool(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsFalse( case5.Value );
             }

             // Case 6 we expect EOF
             {
                bool? case6 = ConsoleHelpers.GetBool(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }

        [Test]
        public void ConsoleHelpersGetShortTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "herp" + Environment.NewLine +  // 1: Fail
                "0" + Environment.NewLine +  // 2: Returns 0
                short.MinValue + Environment.NewLine + // 3: Returns short.MinValue
                short.MaxValue + Environment.NewLine +  // 4: Returns short.MaxValue
                int.MaxValue + Environment.NewLine + // 5: Fail (overflow)
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseShortErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseShortErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseShortErrorMessage + Environment.NewLine + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseShortErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect 0).
             {
                short? case2 = ConsoleHelpers.GetShort(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 0, case2.Value );
             }

             // Case 3 returns minValue.
             {
                short? case3 = ConsoleHelpers.GetShort(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( short.MinValue, case3.Value );
             }

             // Case 4 returns true.
             {
                short? case4 = ConsoleHelpers.GetShort(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( short.MaxValue, case4.Value );
             }

             // Case 5/6 we expect EOF
             {
                short? case6 = ConsoleHelpers.GetShort(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }

        [Test]
        public void ConsoleHelpersGetUShortTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "-1" + Environment.NewLine +  // 1: Fail (negative number)
                "100" + Environment.NewLine +  // 2: Returns 0
                ushort.MinValue + Environment.NewLine + // 3: Returns ushort.MinValue
                ushort.MaxValue + Environment.NewLine +  // 4: Returns ushort.MaxValue
                int.MaxValue + Environment.NewLine + // 5: Fail (overflow)
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseUShortErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseUShortErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseUShortErrorMessage + Environment.NewLine + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseUShortErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect 0).
             {
                ushort? case2 = ConsoleHelpers.GetUShort(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 100, case2.Value );
             }

             // Case 3 returns minValue.
             {
                ushort? case3 = ConsoleHelpers.GetUShort(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( ushort.MinValue, case3.Value );
             }

             // Case 4 returns true.
             {
                ushort? case4 = ConsoleHelpers.GetUShort(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( ushort.MaxValue, case4.Value );
             }

             // Case 5/6 we expect EOF
             {
                ushort? case6 = ConsoleHelpers.GetUShort(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }

        [Test]
        public void ConsoleHelpersGetIntTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "herp" + Environment.NewLine +  // 1: Fail
                "0" + Environment.NewLine +  // 2: Returns 0
                int.MinValue + Environment.NewLine + // 3: Returns int.MinValue
                int.MaxValue + Environment.NewLine +  // 4: Returns int.MaxValue
                long.MaxValue + Environment.NewLine + // 5: Fail (overflow)
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseIntErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseIntErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseIntErrorMessage + Environment.NewLine + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseIntErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect 0).
             {
                int? case2 = ConsoleHelpers.GetInt(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 0, case2.Value );
             }

             // Case 3 returns minValue.
             {
                int? case3 = ConsoleHelpers.GetInt(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( int.MinValue, case3.Value );
             }

             // Case 4 returns true.
             {
                int? case4 = ConsoleHelpers.GetInt(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( int.MaxValue, case4.Value );
             }

             // Case 5/6 we expect EOF
             {
                int? case6 = ConsoleHelpers.GetInt(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }

        [Test]
        public void ConsoleHelpersGetUIntTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "-1" + Environment.NewLine +  // 1: Fail (negative number)
                "100" + Environment.NewLine +  // 2: Returns 100
                uint.MinValue + Environment.NewLine + // 3: Returns uint.MinValue
                uint.MaxValue + Environment.NewLine +  // 4: Returns uint.MaxValue
                long.MaxValue + Environment.NewLine + // 5: Fail (overflow)
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseUIntErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseUIntErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseUIntErrorMessage + Environment.NewLine + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseUIntErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect 0).
             {
                uint? case2 = ConsoleHelpers.GetUInt(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 100, case2.Value );
             }

             // Case 3 returns minValue.
             {
                uint? case3 = ConsoleHelpers.GetUInt(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( uint.MinValue, case3.Value );
             }

             // Case 4 returns true.
             {
                uint? case4 = ConsoleHelpers.GetUInt(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( uint.MaxValue, case4.Value );
             }

             // Case 5/6 we expect EOF
             {
                uint? case6 = ConsoleHelpers.GetUInt(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }

        [Test]
        public void ConsoleHelpersGetLongTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "herp" + Environment.NewLine +  // 1: Fail
                "0" + Environment.NewLine +  // 2: Returns 0
                long.MinValue + Environment.NewLine + // 3: Returns long.MinValue
                long.MaxValue + Environment.NewLine +  // 4: Returns long.MaxValue
                ulong.MaxValue + Environment.NewLine + // 5: Fail (overflow)
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseLongErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseLongErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseLongErrorMessage + Environment.NewLine + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseLongErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect 0).
             {
                long? case2 = ConsoleHelpers.GetLong(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 0, case2.Value );
             }

             // Case 3 returns minValue.
             {
                long? case3 = ConsoleHelpers.GetLong(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( long.MinValue, case3.Value );
             }

             // Case 4 returns true.
             {
                long? case4 = ConsoleHelpers.GetLong(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( long.MaxValue, case4.Value );
             }

             // Case 5/6 we expect EOF
             {
                long? case6 = ConsoleHelpers.GetLong(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }

        [Test]
        public void ConsoleHelpersGetULongTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "-1" + Environment.NewLine +  // 1: Fail (negative number)
                "100" + Environment.NewLine +  // 2: Returns 100
                ulong.MinValue + Environment.NewLine + // 3: Returns ulong.MinValue
                ulong.MaxValue + Environment.NewLine +  // 4: Returns ulong.MaxValue
                "2" + ulong.MaxValue + Environment.NewLine + // 5: Fail (overflow)
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseULongErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseULongErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseULongErrorMessage + Environment.NewLine + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseULongErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect 0).
             {
                ulong? case2 = ConsoleHelpers.GetULong(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 100, case2.Value );
             }

             // Case 3 returns minValue.
             {
                ulong? case3 = ConsoleHelpers.GetULong(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( ulong.MinValue, case3.Value );
             }

             // Case 4 returns true.
             {
                ulong? case4 = ConsoleHelpers.GetULong(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( ulong.MaxValue, case4.Value );
             }

             // Case 5/6 we expect EOF
             {
                ulong? case6 = ConsoleHelpers.GetULong(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }

        [Test]
        public void ConsoleHelpersGetFloatTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "herp" + Environment.NewLine +  // 1: Fail
                "0.0" + Environment.NewLine +  // 2: Returns 0.0
                "-100.5" + Environment.NewLine + // 3: Returns -100.5
                "100.5" + Environment.NewLine +  // 4: Returns 100.5
                double.MaxValue + Environment.NewLine + // 5: Fail (overflow)
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseFloatErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseFloatErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseFloatErrorMessage + Environment.NewLine + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseFloatErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect 0).
             {
                float? case2 = ConsoleHelpers.GetFloat(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 0, case2.Value );
             }

             // Case 3 returns minValue.
             {
                float? case3 = ConsoleHelpers.GetFloat(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );

                Assert.AreEqual( -100.5, case3.Value );
             }

             // Case 4 returns true.
             {
                float? case4 = ConsoleHelpers.GetFloat(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 100.5, case4.Value );
             }

             // Case 5/6 we expect EOF
             {
                float? case6 = ConsoleHelpers.GetFloat(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }

        [Test]
        public void ConsoleHelpersGetDoubleTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "herp" + Environment.NewLine +  // 1: Fail
                "0.0" + Environment.NewLine +  // 2: Returns 0.0
                "-100.5" + Environment.NewLine + // 3: Returns -100.5
                "100.5" + Environment.NewLine +  // 4: Returns 100.5
                "2" + double.MaxValue + Environment.NewLine + // 5: Fail (overflow)
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseDoubleErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseDoubleErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseDoubleErrorMessage + Environment.NewLine + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseDoubleErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect 0).
             {
                double? case2 = ConsoleHelpers.GetDouble(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 0, case2.Value );
             }

             // Case 3 returns minValue.
             {
                double? case3 = ConsoleHelpers.GetDouble(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );

                Assert.AreEqual( -100.5, case3.Value );
             }

             // Case 4 returns true.
             {
                double? case4 = ConsoleHelpers.GetDouble(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 100.5, case4.Value );
             }

             // Case 5/6 we expect EOF
             {
                double? case6 = ConsoleHelpers.GetDouble(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }

        [Test]
        public void ConsoleHelpersGetDecimalTest()
        {
            string cin =
                "derp" + Environment.NewLine +  // 0: Fail
                "herp" + Environment.NewLine +  // 1: Fail
                "0.0" + Environment.NewLine +  // 2: Returns 0.0
                "-100.5" + Environment.NewLine + // 3: Returns -100.5
                "100.5" + Environment.NewLine +  // 4: Returns 100.5
                "2" + decimal.MaxValue + Environment.NewLine + // 5: Fail (overflow)
                "dsf";                          // 6: Fail - EOF.

            this.mockCin = new StringReader( cin );

            string expectedCout =
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseDecimalErrorMessage + Environment.NewLine + // Case 0
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseDecimalErrorMessage + Environment.NewLine + // Case 1
                ConsoleHelpers.DefaultCinMessage + // Case 2
                ConsoleHelpers.DefaultCinMessage + // Case 3
                ConsoleHelpers.DefaultCinMessage + // Case 4
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseDecimalErrorMessage + Environment.NewLine + // Case 5
                ConsoleHelpers.DefaultCinMessage + ConsoleHelpers.ParseDecimalErrorMessage + Environment.NewLine + // Case 6
                ConsoleHelpers.DefaultCinMessage; // EOF

             // Case 2 is the first one that returns (expect 0).
             {
                decimal? case2 = ConsoleHelpers.GetDecimal(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 0, case2.Value );
             }

             // Case 3 returns minValue.
             {
                decimal? case3 = ConsoleHelpers.GetDecimal(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );

                Assert.AreEqual( -100.5, case3.Value );
             }

             // Case 4 returns true.
             {
                decimal? case4 = ConsoleHelpers.GetDecimal(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.AreEqual( 100.5, case4.Value );
             }

             // Case 5/6 we expect EOF
             {
                decimal? case6 = ConsoleHelpers.GetDecimal(
                    ConsoleHelpers.DefaultCinMessage,
                    this.mockCin,
                    this.mockCout
                );
                Assert.IsNull( case6 );
             }

             Assert.AreEqual( expectedCout, this.mockCout.ToString() );
        }
    }
}
}

