//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SethCS.Extensions;

namespace Tests.Extensions
{
    [TestClass]
    public sealed class IEnumerableExtensionsTests
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            List<int> list = new List<int>();
            Assert.IsTrue( list.IsEmpty() );

            list.Add( 1 );
            Assert.IsFalse( list.IsEmpty() );
        }

        [TestMethod]
        public void EqualsIgnoreOrderTest()
        {
            List<int> list1 = new List<int>();
            List<int> list2 = new List<int>();

            Assert.IsFalse( list1.EqualsIgnoreOrder( null ) );
            Assert.IsFalse( IEnumerableExtensions.EqualsIgnoreOrder( null, list2 ) );
            Assert.IsTrue( IEnumerableExtensions.EqualsIgnoreOrder<int>( null, null ) );

            // Empty should be true.
            Assert.IsTrue( list1.EqualsIgnoreOrder( list2 ) );
            Assert.IsTrue( list2.EqualsIgnoreOrder( list1 ) );

            // Adding one to each should be equal
            list1.Add( 1 );
            list2.Add( 1 );
            Assert.IsTrue( list1.EqualsIgnoreOrder( list2 ) );
            Assert.IsTrue( list2.EqualsIgnoreOrder( list1 ) );

            // Add 2 to both, but different orders.
            list1.Add( 2 );
            list2.Insert( 0, 2 );
            Assert.IsTrue( list1.EqualsIgnoreOrder( list2 ) );
            Assert.IsTrue( list2.EqualsIgnoreOrder( list1 ) );

            // Add a third.
            list1.Add( 3 );
            list2.Add( 3 );
            Assert.IsTrue( list1.EqualsIgnoreOrder( list2 ) );
            Assert.IsTrue( list2.EqualsIgnoreOrder( list1 ) );

            // Now, add a duplicate, should still be true.
            list1.Add( 3 );
            list2.Insert( 0, 3 );
            Assert.IsTrue( list1.EqualsIgnoreOrder( list2 ) );
            Assert.IsTrue( list2.EqualsIgnoreOrder( list1 ) );

            // Add a duplicate to one list, but not the other, should start failing.
            list1.Add( 1 );
            Assert.IsFalse( list1.EqualsIgnoreOrder( list2 ) );
            Assert.IsFalse( list2.EqualsIgnoreOrder( list1 ) );

            // Add another value, but one that is different.  Should still fail.
            list2.Add( 2 );
            Assert.IsFalse( list1.EqualsIgnoreOrder( list2 ) );
            Assert.IsFalse( list2.EqualsIgnoreOrder( list1 ) );
        }
    }
}
