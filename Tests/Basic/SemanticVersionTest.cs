//          Copyright Seth Hendrick 2015.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;
using NUnit.Framework;
using SethCS.Basic;

namespace Tests.Basic
{
    [TestFixture]
    public class SemanticVersionTest
    {
        /// <summary>
        /// Ensures the default constructor sets everything to zero.
        /// </summary>
        [Test]
        public void DefaultConstructorTest()
        {
            SemanticVersion empty = new SemanticVersion();

            Assert.AreEqual( 0, empty.Major );
            Assert.AreEqual( 0, empty.Minor );
            Assert.AreEqual( 0, empty.Revision );

            Assert.AreEqual( "0.0.0", empty.ToString() );
        }

        /// <summary>
        /// Ensures the constructor sets everything to what was passed in.
        /// </summary>
        [Test]
        public void ConstructorTest()
        {
            SemanticVersion uut = new SemanticVersion( 10, 11, 12 );

            Assert.AreEqual( 10, uut.Major );
            Assert.AreEqual( 11, uut.Minor );
            Assert.AreEqual( 12, uut.Revision );

            Assert.AreEqual( "10.11.12", uut.ToString() );
        }

        /// <summary>
        /// Ensures the behavior is correct when various strings are passed into TryParse.
        /// </summary>
        [Test]
        public void TryParseTests()
        {
            // Null Parameter.
            SemanticVersion outVersion;
            Assert.IsFalse( SemanticVersion.TryParse( null, out outVersion ) );
            Assert.IsNull( outVersion );

            // Bad Major number
            Assert.IsFalse( SemanticVersion.TryParse( "d.3.4", out outVersion ) );
            Assert.IsNull( outVersion );

            // Bad Minor number
            Assert.IsFalse( SemanticVersion.TryParse( "1.d.4", out outVersion ) );
            Assert.IsNull( outVersion );

            // Bad revision number
            Assert.IsFalse( SemanticVersion.TryParse( "1.3.d", out outVersion ) );
            Assert.IsNull( outVersion );

            // Bad all numbers
            Assert.IsFalse( SemanticVersion.TryParse( "d.d.d", out outVersion ) );
            Assert.IsNull( outVersion );

            // Overflow
            Assert.IsFalse( SemanticVersion.TryParse( "10000000000000000000000000.1.1", out outVersion ) );
            Assert.IsNull( outVersion );

            Assert.IsTrue( SemanticVersion.TryParse( "1.2.3", out outVersion ) );
            Assert.AreEqual(
                new SemanticVersion( 1, 2, 3 ),
                outVersion
            );

            Assert.IsTrue( SemanticVersion.TryParse( "10.20.30", out outVersion ) );
            Assert.AreEqual(
                new SemanticVersion( 10, 20, 30 ),
                outVersion
            );
        }

        /// <summary>
        /// Ensures the behavior is correct when various strings are passed into TryParse.
        /// </summary>
        [Test]
        public void BadVersionStringParseTests()
        {
            // Null Parameter.
            Assert.Throws<ArgumentNullException>(
                delegate()
                {
                    SemanticVersion.Parse( null );
                }
            );

            // Bad Major number
            Assert.Throws<FormatException>(
                delegate ()
                {
                    SemanticVersion.Parse( "d.1.2" );
                }
            );

            // Bad Minor number
            Assert.Throws<FormatException>(
                delegate ()
                {
                    SemanticVersion.Parse( "1.d.2" );
                }
            );

            // Bad revision number
            Assert.Throws<FormatException>(
                delegate ()
                {
                    SemanticVersion.Parse( "0.1.d" );
                }
            );

            // Bad all numbers
            Assert.Throws<FormatException>(
                delegate ()
                {
                    SemanticVersion.Parse( "d.d.d" );
                }
            );

            // Overflow
            Assert.Throws<OverflowException>(
                delegate ()
                {
                    SemanticVersion.Parse( "10000000000000000000000.1.2" );
                }
            );

            Assert.AreEqual(
                new SemanticVersion( 1, 2, 3 ),
                SemanticVersion.Parse( "1.2.3" )
            );

            Assert.AreEqual(
                new SemanticVersion( 10, 20, 30 ),
                SemanticVersion.Parse( "10.20.30" )
            );
        }

        /// <summary>
        /// Checks to make sure the Equals method works.
        /// </summary>
        [Test]
        public void EqualsTest()
        {
            // Both defaulted to 0.0.0
            SemanticVersion uut1 = new SemanticVersion();
            SemanticVersion uut2 = new SemanticVersion();

            Assert.IsTrue( uut1.Equals( uut2 ) );
            Assert.IsTrue( uut2.Equals( uut1 ) );

            // Change major number.
            uut1.Major = 1;
            Assert.IsFalse( uut1.Equals( uut2 ) );
            Assert.IsFalse( uut2.Equals( uut1 ) );
            uut1 = new SemanticVersion();

            // Change minor number.
            uut1.Minor = 1;
            Assert.IsFalse( uut1.Equals( uut2 ) );
            Assert.IsFalse( uut2.Equals( uut1 ) );
            uut1 = new SemanticVersion();

            // Change revision number.
            uut1.Revision = 1;
            Assert.IsFalse( uut1.Equals( uut2 ) );
            Assert.IsFalse( uut2.Equals( uut1 ) );
        }

        /// <summary>
        /// Checks to ensure operators work.
        /// </summary>
        [Test]
        public void OperatorTest()
        {
            // Both are same to start out.
            SemanticVersion uut1 = new SemanticVersion();
            SemanticVersion uut2 = new SemanticVersion();

            Assert.IsFalse( uut1 < uut2 );
            Assert.IsFalse( uut1 > uut2 );
            Assert.IsTrue( uut1 <= uut2 );
            Assert.IsTrue( uut1 >= uut2 );

            // Change majors.
            uut1.Major = 1;
            Assert.IsFalse( uut1 < uut2 );
            Assert.IsFalse( uut1 <= uut2 );
            Assert.IsTrue( uut1 > uut2 );
            Assert.IsTrue( uut1 >= uut2 );

            uut2.Major = 2;
            Assert.IsTrue( uut1 < uut2 );
            Assert.IsTrue( uut1 <= uut2 );
            Assert.IsFalse( uut1 > uut2 );
            Assert.IsFalse( uut1 >= uut2 );

            // Change minors
            uut1 = new SemanticVersion();
            uut2 = new SemanticVersion();

            uut1.Minor = 1;
            Assert.IsFalse( uut1 < uut2 );
            Assert.IsFalse( uut1 <= uut2 );
            Assert.IsTrue( uut1 > uut2 );
            Assert.IsTrue( uut1 >= uut2 );

            uut2.Minor = 2;
            Assert.IsTrue( uut1 < uut2 );
            Assert.IsTrue( uut1 <= uut2 );
            Assert.IsFalse( uut1 > uut2 );
            Assert.IsFalse( uut1 >= uut2 );

            // Change Revisions
            uut1 = new SemanticVersion();
            uut2 = new SemanticVersion();

            uut1.Revision = 1;
            Assert.IsFalse( uut1 < uut2 );
            Assert.IsFalse( uut1 <= uut2 );
            Assert.IsTrue( uut1 > uut2 );
            Assert.IsTrue( uut1 >= uut2 );

            uut2.Revision = 2;
            Assert.IsTrue( uut1 < uut2 );
            Assert.IsTrue( uut1 <= uut2 );
            Assert.IsFalse( uut1 > uut2 );
            Assert.IsFalse( uut1 >= uut2 );
        }

        /// <summary>
        /// Checks to ensure operators throw ArugmentNullException when they are expected to.
        /// </summary>
        [Test]
        public void OperatorNullCheckTest()
        {
            SemanticVersion uut1 = null;
            SemanticVersion uut2 = null;

            // Ensure argument nulls get thrown.
            Assert.Throws<ArgumentNullException>(
                delegate ()
                {
                    bool success = uut1 < uut2;
                }
            );
            Assert.Throws<ArgumentNullException>(
                delegate ()
                {
                    bool success = uut1 > uut2;
                }
            );
            Assert.Throws<ArgumentNullException>(
                delegate ()
                {
                    bool success = uut1 <= uut2;
                }
            );
            Assert.Throws<ArgumentNullException>(
                delegate ()
                {
                    bool success = uut1 >= uut2;
                }
            );

            // Set uut1 to not null so the second argument can be tested.
            uut1 = new SemanticVersion();

            // Ensure argument nulls get thrown.
            Assert.Throws<ArgumentNullException>(
                delegate ()
                {
                    bool success = uut1 < uut2;
                }
            );
            Assert.Throws<ArgumentNullException>(
                delegate ()
                {
                    bool success = uut1 > uut2;
                }
            );
            Assert.Throws<ArgumentNullException>(
                delegate ()
                {
                    bool success = uut1 <= uut2;
                }
            );
            Assert.Throws<ArgumentNullException>(
                delegate ()
                {
                    bool success = uut1 >= uut2;
                }
            );
        }
    }
}
