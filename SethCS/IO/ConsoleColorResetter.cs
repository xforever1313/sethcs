//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;

namespace SethCS.IO
{

    /// <summary>
    /// Helper class that changes the console color and then puts it back
    /// when Dispose() is called.
    /// 
    /// This is designed like C++'s RAII, where the Constructor sets the colors,
    /// and Dispose() resets them when the variable goes out-of-scope in a using statement.
    /// </summary>
    public class ConsoleColorResetter : IDisposable
    {
        // ---------------- Fields ----------------

        private ConsoleColor? originalForeground;
        private ConsoleColor? originalBackground;

        // ---------------- Constructor ----------------

        /// <summary>
        /// Constructor - sets the background colors.
        /// </summary>
        /// <param name="newForeground">The new foreground (text) color of the console.  Null to NOT change it.</param>
        /// <param name="newBackground">The new background color of the console.  Null to NOT change it.</param>
        public ConsoleColorResetter( ConsoleColor? newForeground, ConsoleColor? newBackground )
        {
            if( newForeground != null )
            {
                this.originalForeground = Console.ForegroundColor;
                Console.ForegroundColor = newForeground.Value;
            }

            if( newBackground != null )
            {
                this.originalBackground = Console.BackgroundColor;
                Console.BackgroundColor = newBackground.Value;
            }
        }

        // ---------------- Functions ---------------

        public void Dispose()
        {
            if( this.originalBackground != null )
            {
                Console.BackgroundColor = this.originalBackground.Value;
            }

            if( this.originalForeground != null )
            {
                Console.ForegroundColor = this.originalForeground.Value;
            }
        }
    }
}