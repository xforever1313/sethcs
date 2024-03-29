//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using NUnit.Framework;
using SethCS.Collections;

namespace Tests.Collections
{
    public sealed class CloneableClass
    {
        public CloneableClass( int value )
        {
            this.Value = value;
        }

        public int Value { get; private set; }

        public CloneableClass Clone()
        {
            return (CloneableClass)this.MemberwiseClone();
        }
    }

    [TestFixture]
    public sealed class CloningReadOnlyListTests
    {
        // ---------------- Fields ----------------

        private CloneableClass obj1;
        private CloneableClass obj2;
        private CloneableClass obj3;

        CloneableClass[] originals;

        private CloningReadOnlyList<CloneableClass> uut;

        // ---------------- Setup / Teardown ----------------

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            this.obj1 = new CloneableClass( 1 );
            this.obj2 = new CloneableClass( 2 );
            this.obj3 = new CloneableClass( 3 );

            this.originals = new CloneableClass[] { obj1, obj2, obj3 };

            this.uut = new CloningReadOnlyList<CloneableClass>( c => { return c.Clone(); }, originals );
        }

        [OneTimeTearDown]
        public void FixtureTeardown()
        {
        }

        [SetUp]
        public void TestSetup()
        {
        }

        [TearDown]
        public void TestTeardown()
        {
        }

        // ---------------- Tests ----------------

        [Test]
        public void CountTest()
        {
            Assert.AreEqual( 3, this.uut.Count );
        }

        [Test]
        public void IndexTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>( () => { int x = this.uut[-1].Value; } );
            Assert.Throws<ArgumentOutOfRangeException>( () => { int x = this.uut[3].Value; } );

            Assert.AreEqual( 1, this.uut[0].Value );
            Assert.AreEqual( 2, this.uut[1].Value );
            Assert.AreEqual( 3, this.uut[2].Value );

            Assert.AreNotSame( this.obj1, this.uut[0] );
            Assert.AreNotSame( this.obj2, this.uut[1] );
            Assert.AreNotSame( this.obj3, this.uut[2] );
        }

        [Test]
        public void ForEachTest()
        {
            int index = 0;
            CloneableClass[] originals = new CloneableClass[] { obj1, obj2, obj3 };

            foreach( CloneableClass c in this.uut )
            {
                Assert.AreEqual( originals[index].Value, c.Value );
                Assert.AreNotSame( originals[index], c );
                ++index;    
            }
        }

        // ---------------- Test Helpers ----------------
    }
}
