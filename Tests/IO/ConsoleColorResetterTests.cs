//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SethCS.IO;

namespace Tests.IO
{
    [TestClass]
    [DoNotParallelize]
    public sealed class ConsoleColorResetterTests
    {
        // ---------------- Fields ----------------

        private static readonly object theLock = new object();

        // ---------------- Tests ----------------

        [TestMethod]
        public void BackgroundOnlyTest()
        {
            lock( theLock )
            {
                ConsoleColor originalBackground = Console.BackgroundColor;
                ConsoleColor originalForeground = Console.ForegroundColor;

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
        }

        [TestMethod]
        public void ForegroundOnlyTest()
        {
            lock( theLock )
            {
                ConsoleColor originalBackground = Console.BackgroundColor;
                ConsoleColor originalForeground = Console.ForegroundColor;

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
        }

        [TestMethod]
        public void BackgroundAndForegroundTest()
        {
            lock( theLock )
            {
                ConsoleColor originalBackground = Console.BackgroundColor;
                ConsoleColor originalForeground = Console.ForegroundColor;

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
}
