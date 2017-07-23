//
//          Copyright Seth Hendrick 2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;

namespace SethCS.Basic
{
    /// <summary>
    /// A Thread-Safe generic logger that allows the user to add or remove events
    /// that should be logged somewhere.
    /// </summary>
    public class GenericLogger
    {
        // ---------------- Fields ----------------

        /// <summary>
        /// Event that is triggered when WriteLine is called.
        /// 
        /// Does not need to be thread-safe, all functions in this class are.
        /// </summary>
        public event Action<string> OnWriteLine;

        /// <summary>
        /// Event that is triggered when ErrorWriteLine is called.
        /// 
        /// Does not need to be thread-safe, all functions in this class are.
        /// </summary>
        public event Action<string> OnErrorWriteLine;

        private object onWriteLock;

        // ---------------- Constructor ----------------

        public GenericLogger()
        {
            this.onWriteLock = new object();
        }

        // ---------------- Functions ----------------

        // -------- WriteLine --------

        /// <summary>
        /// Writes an empty line to the listening <see cref="OnWriteLine"/> events.
        /// 
        /// Thread-Safe.
        /// </summary>
        public void WriteLine()
        {
            WriteLineInternal( OnWriteLine );
        }

        /// <summary>
        /// Writes a formatted string to the listening <see cref="OnWriteLine"/> events.
        /// </summary>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        public void WriteLine( string formatStr, params object[] objects )
        {
            WriteLineInternal( OnWriteLine, formatStr, objects );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all <see cref="OnWriteLine"/> events.
        /// 
        /// Thread-Safe.
        /// </summary>
        /// <param name="line">The string to write.</param>
        public void WriteLine( string line )
        {
            WriteLineInternal( OnWriteLine, line );
        }

        // -------- ErrorWriteLine --------

        /// <summary>
        /// Writes an empty line to the listening <see cref="OnErrorWriteLine"/> events.
        /// 
        /// Thread-Safe.
        /// </summary>
        public void ErrorWriteLine()
        {
            WriteLineInternal( OnErrorWriteLine );
        }

        /// <summary>
        /// Writes a formatted string to the listening <see cref="OnErrorWriteLine"/> events.
        /// </summary>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        public void ErrorWriteLine( string formatStr, params object[] objects )
        {
            WriteLineInternal( OnErrorWriteLine, formatStr, objects );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all <see cref="OnErrorWriteLine"/> events.
        /// 
        /// Thread-Safe.
        /// </summary>
        /// <param name="line">The string to write.</param>
        public void ErrorWriteLine( string line )
        {
            WriteLineInternal( OnErrorWriteLine, line );
        }

        // -------- Internal Functions --------

        /// <summary>
        /// Writes an empty line to the listening events.
        /// 
        /// Thread-Safe.
        /// </summary>
        /// <param name="e">The event to pass the line into.</param>
        private void WriteLineInternal( Action<string> e )
        {
            WriteLine( e, string.Empty );
        }

        /// <summary>
        /// Writes a formatted string to the listening events.
        /// </summary>
        /// <param name="e">The event to pass the line into.</param>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        private void WriteLineInternal( Action<string> e, string formatStr, params object[] objects )
        {
            WriteLine( e, string.Format( formatStr, objects ) );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all the events.
        /// 
        /// Thread-Safe.
        /// </summary>
        /// <param name="e">The event to pass the line into.</param>
        /// <param name="line">The string to write.</param>
        private void WriteLine( Action<string> e, string line )
        {
            lock( onWriteLock )
            {
                Action<string> action = e;
                action?.Invoke( line + Environment.NewLine );
            }
        }
    }

    /// <summary>
    /// A single, global instance of <see cref="GenericLogger"/>
    /// </summary>
    public static class StaticLogger
    {
        static StaticLogger()
        {
            Log = new GenericLogger();
        }

        /// <summary>
        /// The global <see cref="GenericLogger"/>
        /// instance.
        /// </summary>
        public static GenericLogger Log { get; private set; }
    }
}
