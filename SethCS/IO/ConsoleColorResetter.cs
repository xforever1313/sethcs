//
//          Copyright Seth Hendrick 2018.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

namespace SethCS.IO
{

    /// <summary>
    /// Helper class that changes the console color and then puts it back
    /// when Dispose() is called.
    /// </summary>
    public class ConsoleColorReseter : IDisposable
    {
        // ---------------- Fields ----------------

        private ConsoleColor? originalForeground;
        private ConsoleColor? originalBackground;

        // ---------------- Constructor ----------------

        public ConsoleColorReseter( ConsoleColor? newForeground, ConsoleColor? newBackground )
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