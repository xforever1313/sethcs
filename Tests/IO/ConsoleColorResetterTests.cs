//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using NUnit.Framework;
using SethCS.IO;

namespace Tests.IO
{
    [TestFixture]
    public sealed class ConsoleColorResetterTests
    {
        // ---------------- Fields ----------------

        private ConsoleColor originalForeground;

        private ConsoleColor originalBackground;

        // ---------------- Setup / Teardown ----------------

        [SetUp]
        public void TestSetup()
        {
            this.originalBackground = Console.BackgroundColor;
            this.originalForeground = Console.ForegroundColor;
        }

        [TearDown]
        public void TestTeardown()
        {
        }

        // ---------------- Tests ----------------

        [Test]
        public void BackgroundOnlyTest()
        {
            const ConsoleColor newBgColor = ConsoleColor.DarkGreen;
            using( ConsoleColorResetter uut = new ConsoleColorResetter( null, newBgColor ) )
            {
                Assert.AreEqual( originalForeground, Console.ForegroundColor );
                Assert.AreEqual( newBgColor, Console.BackgroundColor );
            }

            // Should be restored after dispose is called.
            Assert.AreEqual( originalForeground, Console.ForegroundColor );
            Assert.AreEqual( originalBackground, Console.BackgroundColor );
        }

        [Test]
        public void ForegroundOnlyTest()
        {
            const ConsoleColor newFgColor = ConsoleColor.DarkGreen;
            using( ConsoleColorResetter uut = new ConsoleColorResetter( newFgColor, null ) )
            {
                Assert.AreEqual( newFgColor, Console.ForegroundColor );
                Assert.AreEqual( originalBackground, Console.BackgroundColor );
            }

            // Should be restored after dispose is called.
            Assert.AreEqual( originalForeground, Console.ForegroundColor );
            Assert.AreEqual( originalBackground, Console.BackgroundColor );
        }

        [Test]
        public void BackgroundAndForegroundTest()
        {
            const ConsoleColor newFgColor = ConsoleColor.DarkGreen;
            const ConsoleColor newBgColor = ConsoleColor.DarkMagenta;
            using( ConsoleColorResetter uut = new ConsoleColorResetter( newFgColor, newBgColor ) )
            {
                Assert.AreEqual( newFgColor, Console.ForegroundColor );
                Assert.AreEqual( newBgColor, Console.BackgroundColor );
            }

            // Should be restored after dispose is called.
            Assert.AreEqual( originalForeground, Console.ForegroundColor );
            Assert.AreEqual( originalBackground, Console.BackgroundColor );
        }
    }
}
