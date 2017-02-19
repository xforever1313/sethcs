
//          Copyright Seth Hendrick 2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;

namespace SethCS.Basic
{
    public static class StaticLogger
    {
        // ---------------- Fields ----------------

        /// <summary>
        /// Event that is triggered when WriteLine is called.
        /// 
        /// Does not need to be thread-safe, all functions in this class are.
        /// </summary>
        public static event Action<string> OnWriteLine;

        private static object onWriteLock = new object();

        // ---------------- Functions ----------------
        
        /// <summary>
        /// Writes an empty line to the listening events.
        /// 
        /// Thread-Safe.
        /// </summary>
        public static void WriteLine()
        {
            WriteLine( string.Empty );
        }

        /// <summary>
        /// Writes a formatted string to the listening events.
        /// </summary>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        public static void WriteLine( string formatStr, params object[] objects )
        {
            WriteLine( string.Format( formatStr, objects ) );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all the events.
        /// 
        /// Thread-Safe.
        /// </summary>
        /// <param name="line">The string to write.</param>
        public static void WriteLine( string line )
        {
            lock( onWriteLock )
            {
                Action<string> action = OnWriteLine;
                action?.Invoke( line + Environment.NewLine );
            }
        }
    }
}
