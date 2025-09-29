//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SethCS.Extensions;

namespace Tests.Extensions
{
    [TestClass]
    public sealed class EnumExtensionTests
    {
        // ---------------- Enums ----------------

        private enum OrderedEnum
        {
            Negative1 = -1,
            Zero = 0,
            One = 1,
            Two = 2,
            Three = 3
        }

        public enum UnorderedEnum
        {
            One = 1,
            Three = 3,
            Two = 2,
            Zero = 0,
            Negative1 = -1
        }

        // ---------------- Tets ----------------

        [TestMethod]
        public void SortByNumber_OrderedEnumTest()
        {
            // Act
            OrderedEnum[] list = EnumExtensions.SortByNumber<OrderedEnum>().ToArray();

            // Check
            Assert.AreEqual( OrderedEnum.Negative1, list[0] );
            Assert.AreEqual( OrderedEnum.Zero, list[1] );
            Assert.AreEqual( OrderedEnum.One, list[2] );
            Assert.AreEqual( OrderedEnum.Two, list[3] );
            Assert.AreEqual( OrderedEnum.Three, list[4] );
        }

        [TestMethod]
        public void SortByNumber_UnorderedEnumTest()
        {
            // Act
            UnorderedEnum[] list = EnumExtensions.SortByNumber<UnorderedEnum>().ToArray();

            // Check
            Assert.AreEqual( UnorderedEnum.Negative1, list[0] );
            Assert.AreEqual( UnorderedEnum.Zero, list[1] );
            Assert.AreEqual( UnorderedEnum.One, list[2] );
            Assert.AreEqual( UnorderedEnum.Two, list[3] );
            Assert.AreEqual( UnorderedEnum.Three, list[4] );
        }

        [TestMethod]
        public void SortByNumberDecending_OrderedEnumTest()
        {
            // Act
            OrderedEnum[] list = EnumExtensions.SortByNumberDecending<OrderedEnum>().ToArray();

            // Check
            Assert.AreEqual( OrderedEnum.Negative1, list[4] );
            Assert.AreEqual( OrderedEnum.Zero, list[3] );
            Assert.AreEqual( OrderedEnum.One, list[2] );
            Assert.AreEqual( OrderedEnum.Two, list[1] );
            Assert.AreEqual( OrderedEnum.Three, list[0] );
        }

        [TestMethod]
        public void SortByNumberDecending_UnorderedEnumTest()
        {
            // Act
            UnorderedEnum[] list = EnumExtensions.SortByNumberDecending<UnorderedEnum>().ToArray();

            // Check
            Assert.AreEqual( UnorderedEnum.Negative1, list[4] );
            Assert.AreEqual( UnorderedEnum.Zero, list[3] );
            Assert.AreEqual( UnorderedEnum.One, list[2] );
            Assert.AreEqual( UnorderedEnum.Two, list[1] );
            Assert.AreEqual( UnorderedEnum.Three, list[0] );
        }
    }
}
