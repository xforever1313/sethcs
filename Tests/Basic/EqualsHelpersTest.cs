//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SethCS.Basic;

namespace Tests.Basic
{
    [TestClass]
    public sealed class EqualsHelpersTest
    {
        // ---------------- Tests ----------------

        [TestMethod]
        public void AreEqualTest()
        {
            TestClass uut1 = new TestClass
            {
                IgnoredIntProp = 2,
                IntProp = 1,
                StringProp = "Hello"
            };

            TestClass uut2 = new TestClass( uut1 );

            this.ExepctEquals( uut1, uut2 );

            // Change ignored prop, should still equal since its ignore.
            uut2.IgnoredIntProp = uut1.IgnoredIntProp + 1;
            this.ExepctEquals( uut1, uut2 );
            uut2 = new TestClass( uut1 );

            // Change any other property, should report false.
            uut2.IntProp = uut1.IntProp + 1;
            this.ExepctNotEquals( uut1, uut2 );
            uut2 = new TestClass( uut1 );

            uut2.StringProp= uut1.StringProp + 1;
            this.ExepctNotEquals( uut1, uut2 );
            uut2 = new TestClass( uut1 );
        }

        [TestMethod]
        public void NullParamTest()
        {
            TestClass uut = new TestClass
            {
                IgnoredIntProp = 2,
                IntProp = 1,
                StringProp = "Hello"
            };

            Assert.IsFalse( null == uut );
            Assert.IsFalse( uut == null );
            Assert.IsTrue( null != uut );
            Assert.IsTrue( uut != null );
            Assert.IsFalse( uut.Equals( null ) );
        }

        [TestMethod]
        public void SameReferenceTest()
        {
            TestClass uut = new TestClass
            {
                IgnoredIntProp = 2,
                IntProp = 1,
                StringProp = "Hello"
            };

            this.ExepctEquals( uut, uut );
        }

        [TestMethod]
        public void NullPropertyTest()
        {
            TestClass uut1 = new TestClass
            {
                IgnoredIntProp = 2,
                IntProp = 1,
                StringProp = null
            };

            TestClass uut2 = new TestClass( uut1 );

            this.ExepctEquals( uut1, uut2 );

            uut2.StringProp = "Not Null";
            this.ExepctNotEquals( uut1, uut2 );
            uut2 = new TestClass( uut1 );
        }

        // ---------------- Test Helpers ----------------

        public void ExepctEquals( TestClass left, TestClass right )
        {
            Assert.IsTrue( left.Equals( right ) );
            Assert.IsTrue( right.Equals( left ) );
            Assert.IsTrue( left == right );
            Assert.IsTrue( right == left );
            Assert.IsFalse( left != right );
            Assert.IsFalse( right != left );
        }

        public void ExepctNotEquals( TestClass left, TestClass right )
        {
            Assert.IsFalse( left.Equals( right ) );
            Assert.IsFalse( right.Equals( left ) );
            Assert.IsFalse( left == right );
            Assert.IsFalse( right == left );
            Assert.IsTrue( left != right );
            Assert.IsTrue( right != left );
        }

        // ---------------- Helper Classes ----------------

        public class TestClass : IEquatable<TestClass>
        {
            // ---------------- Constructor ----------------

            public TestClass()
            {
            }

            public TestClass( TestClass other )
            {
                this.IntProp = other.IntProp;
                this.StringProp = other.StringProp;
                this.IgnoredIntProp = other.IgnoredIntProp;
            }

            // ---------------- Properties ----------------

            public int IntProp { get; set; }

            [EqualsIgnore( false )]
            public string StringProp { get; set; }

            [EqualsIgnore]
            public int IgnoredIntProp { get; set; }

            // ---------------- Properties ----------------

            public override bool Equals( object obj )
            {
                return Equals( obj as TestClass );
            }

            public bool Equals( TestClass other )
            {
                return EqualsHelpers.ArePropertiesEqual( this, other );
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public static bool operator ==( TestClass left, TestClass right )
            {
                return EqualsHelpers.OperatorDoubleEqualsHelper( left, right );
            }

            public static bool operator !=( TestClass left, TestClass right )
            {
                return EqualsHelpers.OperatorNotEqualsHelper( left, right );
            }
        }
    }
}
