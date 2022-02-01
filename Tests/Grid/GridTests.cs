//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Linq;
using NUnit.Framework;
using SethCS.Extensions;
using SethCS.Grid;

namespace Tests.Grid
{
    [TestFixture]
    public sealed class GridTests
    {
        // ---------------- Fields ----------------

        private int currentIndex;

        // ---------------- Setup / Teardown ----------------

        [SetUp]
        public void TestSetup()
        {
            this.currentIndex = 0;
        }

        [TearDown]
        public void TestTeardown()
        {
        }

        // ---------------- Tests ----------------

        [Test]
        public void InvalidConstructorTest()
        {
            Assert.Throws<ArgumentException>(
                () => new Grid<int>( 0, 1, () => currentIndex++ )
            );

            Assert.Throws<ArgumentException>(
                () => new Grid<int>( -1, 1, () => currentIndex++ )
            );

            Assert.Throws<ArgumentException>(
                () => new Grid<int>( 1, 0, () => currentIndex++ )
            );

            Assert.Throws<ArgumentException>(
                () => new Grid<int>( 1, -1, () => currentIndex++ )
            );

            Assert.Throws<ArgumentNullException>(
                () => new Grid<int>( 1, 1, null )
            );
        }

        [Test]
        public void OneByOneContructorTest()
        {
            // Act
            var grid = new Grid<int>( 1, 1, () => currentIndex++ );

            // Check
            Assert.AreEqual( 1, grid.Length );
            Assert.AreEqual( 1, grid.Width );
            Assert.AreEqual( 1, grid.Height );

            Assert.AreEqual( 0, grid.Get( 0, 0 ) );
            Assert.AreEqual( 0, grid.GetCell( 0, 0 ).Payload );

            Cell<int> cell = grid.GetCell( 0, 0 );
            Assert.IsNull( cell.Top );
            Assert.IsNull( cell.Bottom );
            Assert.IsNull( cell.Left );
            Assert.IsNull( cell.Right );
        }

        [Test]
        public void ThreeByOneContructorTest()
        {
            // Act
            var grid = new Grid<int>( 3, 1, () => currentIndex++ );

            // Check
            Assert.AreEqual( 3, grid.Length );
            Assert.AreEqual( 3, grid.Width );
            Assert.AreEqual( 1, grid.Height );

            Assert.AreEqual( 0, grid.Get( 0, 0 ) );
            Assert.AreEqual( 1, grid.Get( 1, 0 ) );
            Assert.AreEqual( 2, grid.Get( 2, 0 ) );

            {
                Cell<int> cell = grid.GetCell( 0, 0 );
                Assert.IsNull( cell.Top );
                Assert.IsNull( cell.Bottom );
                Assert.IsNull( cell.Left );
                Assert.AreSame( cell.Right, grid.GetCell( 1, 0 ) );
            }

            {
                Cell<int> cell = grid.GetCell( 1, 0 );
                Assert.IsNull( cell.Top );
                Assert.IsNull( cell.Bottom );
                Assert.AreSame( cell.Left, grid.GetCell( 0, 0 ) );
                Assert.AreSame( cell.Right, grid.GetCell( 2, 0 ) );
            }

            {
                Cell<int> cell = grid.GetCell( 2, 0 );
                Assert.IsNull( cell.Top );
                Assert.IsNull( cell.Bottom );
                Assert.AreSame( cell.Left, grid.GetCell( 1, 0 ) );
                Assert.IsNull( cell.Right );
            }
        }

        [Test]
        public void OneByThreeContructorTest()
        {
            // Act
            var grid = new Grid<int>( 1, 3, () => currentIndex++ );

            // Check
            Assert.AreEqual( 3, grid.Length );
            Assert.AreEqual( 1, grid.Width );
            Assert.AreEqual( 3, grid.Height );

            Assert.AreEqual( 0, grid.Get( 0, 0 ) );
            Assert.AreEqual( 1, grid.Get( 0, 1 ) );
            Assert.AreEqual( 2, grid.Get( 0, 2 ) );

            {
                Cell<int> cell = grid.GetCell( 0, 0 );
                Assert.IsNull( cell.Top );
                Assert.AreSame( cell.Bottom, grid.GetCell( 0, 1 ) );
                Assert.IsNull( cell.Left );
                Assert.IsNull( cell.Right );
            }

            {
                Cell<int> cell = grid.GetCell( 0, 1 );
                Assert.AreSame( cell.Top, grid.GetCell( 0, 0 ) );
                Assert.AreSame( cell.Bottom, grid.GetCell( 0, 2 ) );
                Assert.IsNull( cell.Left );
                Assert.IsNull( cell.Right );
            }

            {
                Cell<int> cell = grid.GetCell( 0, 2 );
                Assert.AreSame( cell.Top, grid.GetCell( 0, 1 ) );
                Assert.IsNull( cell.Bottom );
                Assert.IsNull( cell.Left );
                Assert.IsNull( cell.Right );
            }
        }

        [Test]
        public void ThreeByThreeContructorTest()
        {
            // Act
            var grid = new Grid<int>( 3, 3, () => currentIndex++ );

            // Check
            Assert.AreEqual( 9, grid.Length );
            Assert.AreEqual( 3, grid.Width );
            Assert.AreEqual( 3, grid.Height );

            Assert.AreEqual( 0, grid.Get( 0, 0 ) );
            Assert.AreEqual( 1, grid.Get( 0, 1 ) );
            Assert.AreEqual( 2, grid.Get( 0, 2 ) );
            Assert.AreEqual( 3, grid.Get( 1, 0 ) );
            Assert.AreEqual( 4, grid.Get( 1, 1 ) );
            Assert.AreEqual( 5, grid.Get( 1, 2 ) );
            Assert.AreEqual( 6, grid.Get( 2, 0 ) );
            Assert.AreEqual( 7, grid.Get( 2, 1 ) );
            Assert.AreEqual( 8, grid.Get( 2, 2 ) );

            // 00 10 20
            // 01 11 21
            // 02 12 22

            // First Row
            {
                Cell<int> cell = grid.GetCell( 0, 0 );
                Assert.IsNull( cell.Top );
                Assert.AreSame( cell.Bottom, grid.GetCell( 0, 1 ) );
                Assert.IsNull( cell.Left );
                Assert.AreSame( cell.Right, grid.GetCell( 1, 0 ) );
            }

            {
                Cell<int> cell = grid.GetCell( 1, 0 );
                Assert.IsNull( cell.Top );
                Assert.AreSame( cell.Bottom, grid.GetCell( 1, 1 ) );
                Assert.AreSame( cell.Left, grid.GetCell( 0, 0 ) );
                Assert.AreSame( cell.Right, grid.GetCell( 2, 0 ) );
            }

            {
                Cell<int> cell = grid.GetCell( 2, 0 );
                Assert.IsNull( cell.Top );
                Assert.AreSame( cell.Bottom, grid.GetCell( 2, 1 ) );
                Assert.AreSame( cell.Left, grid.GetCell( 1, 0 ) );
                Assert.IsNull( cell.Right );
            }

            // Second Row
            {
                Cell<int> cell = grid.GetCell( 0, 1 );
                Assert.AreSame( cell.Top, grid.GetCell( 0, 0 ) );
                Assert.AreSame( cell.Bottom, grid.GetCell( 0, 2 ) );
                Assert.IsNull( cell.Left );
                Assert.AreSame( cell.Right, grid.GetCell( 1, 1 ) );
            }

            {
                Cell<int> cell = grid.GetCell( 1, 1 );
                Assert.AreSame( cell.Top, grid.GetCell( 1, 0 ) );
                Assert.AreSame( cell.Bottom, grid.GetCell( 1, 2 ) );
                Assert.AreSame( cell.Left, grid.GetCell( 0, 1 ) );
                Assert.AreSame( cell.Right, grid.GetCell( 2, 1 ) );
            }

            {
                Cell<int> cell = grid.GetCell( 2, 1 );
                Assert.AreSame( cell.Top, grid.GetCell( 2, 0 ) );
                Assert.AreSame( cell.Bottom, grid.GetCell( 2, 2 ) );
                Assert.AreSame( cell.Left, grid.GetCell( 1, 1 ) );
                Assert.IsNull( cell.Right );
            }

            // Third Row
            {
                Cell<int> cell = grid.GetCell( 0, 2 );
                Assert.AreSame( cell.Top, grid.GetCell( 0, 1 ) );
                Assert.IsNull( cell.Bottom );
                Assert.IsNull( cell.Left );
                Assert.AreSame( cell.Right, grid.GetCell( 1, 2 ) );
            }

            {
                Cell<int> cell = grid.GetCell( 1, 2 );
                Assert.AreSame( cell.Top, grid.GetCell( 1, 1 ) );
                Assert.IsNull( cell.Bottom );
                Assert.AreSame( cell.Left, grid.GetCell( 0, 2 ) );
                Assert.AreSame( cell.Right, grid.GetCell( 2, 2 ) );
            }

            {
                Cell<int> cell = grid.GetCell( 2, 2 );
                Assert.AreSame( cell.Top, grid.GetCell( 2, 1 ) );
                Assert.IsNull( cell.Bottom );
                Assert.AreSame( cell.Left, grid.GetCell( 1, 2 ) );
                Assert.IsNull( cell.Right );
            }
        }

        [Test]
        public void EnumerableTest()
        {
            var grid = new Grid<int>( 2, 2, () => currentIndex++ );

            int index = 0;
            foreach( Cell<int> cell in grid.ToEnumerable() )
            {
                Assert.AreEqual( index++, cell.Payload );
            }
        }
    }
}
