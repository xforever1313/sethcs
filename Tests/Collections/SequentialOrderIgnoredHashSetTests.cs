//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SethCS.Collections;

namespace Tests.Collections
{
    [TestClass]
    public sealed class SequentialOrderIgnoredHashSetTests
    {
        // ---------------- Tests ----------------

        [TestMethod]
        public void EmptyIsEqualTest()
        {
            // Setup
            var uut1 = new SequentialOrderIgnoredHashSet<int>();
            var uut2 = new SequentialOrderIgnoredHashSet<int>();

            // Act
            bool areEqual = uut1.Equals( uut2 );

            // Check
            Assert.IsTrue( areEqual );
            Assert.AreEqual( uut1.GetHashCode(), uut2.GetHashCode() );
        }

        [TestMethod]
        public void EqualSameOrderTest()
        {
            // Setup
            var uut1 = new SequentialOrderIgnoredHashSet<int>
            {
                1, 2, 3
            };
            var uut2 = new SequentialOrderIgnoredHashSet<int>()
            {
                1, 2, 3
            };

            // Act
            bool areEqual = uut1.Equals( uut2 );

            // Check
            Assert.IsTrue( areEqual );
            Assert.AreEqual( uut1.GetHashCode(), uut2.GetHashCode() );
        }

        [TestMethod]
        public void EqualDifferentOrderTest()
        {
            // Setup
            var uut1 = new SequentialOrderIgnoredHashSet<int>
            {
                1, 3, 2
            };
            var uut2 = new SequentialOrderIgnoredHashSet<int>()
            {
                1, 2, 3
            };

            // Act
            bool areEqual = uut1.Equals( uut2 );

            // Check
            Assert.IsTrue( areEqual );
            Assert.AreEqual( uut1.GetHashCode(), uut2.GetHashCode() );
        }

        [TestMethod]
        public void EqualDuplicateEntriesTest()
        {
            // Setup
            var uut1 = new SequentialOrderIgnoredHashSet<int>
            {
                1, 2, 3, 3
            };
            var uut2 = new SequentialOrderIgnoredHashSet<int>()
            {
                1, 2, 2, 3
            };

            // Act
            bool areEqual = uut1.Equals( uut2 );

            // Check
            Assert.IsTrue( areEqual );
            Assert.AreEqual( uut1.GetHashCode(), uut2.GetHashCode() );
        }
    }
}
