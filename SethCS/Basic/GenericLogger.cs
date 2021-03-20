//
//          Copyright Seth Hendrick 2015-2021.
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
        /// Event that is triggered when WarningWriteLine is called.
        ///
        /// Does not need to be thread-safe, all functions in this class are.
        /// </summary>
        public event Action<string> OnWarningWriteLine;

        /// <summary>
        /// Event that is triggered when ErrorWriteLine is called.
        /// 
        /// Does not need to be thread-safe, all functions in this class are.
        /// </summary>
        public event Action<string> OnErrorWriteLine;

        private object onWriteLock;

        private int verbosity;

        private object verbosityLock;

        // ---------------- Constructor ----------------

        public GenericLogger( int verbosityLevel = 0 )
        {
            this.onWriteLock = new object();

            this.verbosity = verbosityLevel;
            this.verbosityLock = new object();
        }

        // ---------------- Properties ----------------

        /// <summary>
        /// What the verbosity level is set to.
        /// Defaulted to 0.
        /// 
        /// If a Write function is called, but the passed in 
        /// verbosity level is greater than this value, nothing
        /// gets written.
        /// 
        /// If this is set to a negative number, NOTHING (Inclduing WriteLines that do not
        /// take in a verbosity level) gets written.
        /// </summary>
        public int Verbosity
        {
            get
            {
                lock( this.verbosityLock )
                {
                    return this.verbosity;
                }
            }
            set
            {
                lock( this.verbosityLock )
                {
                    this.verbosity = value;
                }
            }
        }

        // ---------------- Functions ----------------

        // -------- WriteLine --------

        /// <summary>
        /// Writes an empty line to the listening <see cref="OnWriteLine"/> events.
        /// </summary>
        public void WriteLine()
        {
            this.WriteLine( 0 );
        }

        /// <summary>
        /// Writes an empty line to the lisenting <see cref="OnWriteLine"/> events, but
        /// only is <see cref="Verbosity"/> is greater than the passed in verbosity level.
        /// </summary>
        /// <param name="verbosityLevel">The verbosity level required to print this message.</param>
        public void WriteLine( int verbosityLevel )
        {
            this.WriteLineInternal( verbosityLevel, OnWriteLine );
        }

        /// <summary>
        /// Writes a formatted string to the listening <see cref="OnWriteLine"/> events.
        /// </summary>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        public void WriteLine( string formatStr, params object[] objects )
        {
            this.WriteLine( 0, formatStr, objects );
        }

        /// <summary>
        /// Writes a formatted string to the listening <see cref="OnWriteLine"/> events, but
        /// only is <see cref="Verbosity"/> is greater than the passed in verbosity level.
        /// </summary>
        /// <param name="verbosityLevel">The verbosity level required to print this message.</param>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        public void WriteLine( int verbosityLevel, string formatStr, params object[] objects )
        {
            this.WriteLineInternal( verbosityLevel, OnWriteLine, formatStr, objects );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all <see cref="OnWriteLine"/> events.
        /// </summary>
        /// <param name="line">The string to write.</param>
        public void WriteLine( string line )
        {
            WriteLine( 0, line );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all <see cref="OnWriteLine"/> events but
        /// only is <see cref="Verbosity"/> is greater than the passed in verbosity level.
        /// </summary>
        /// <param name="verbosityLevel">The verbosity level required to print this message.</param>
        /// <param name="line">The string to write.</param>
        public void WriteLine( int verbosityLevel, string line )
        {
            WriteLineInternal( verbosityLevel, OnWriteLine, line );
        }

        // -------- WarningWriteLine --------

        /// <summary>
        /// Writes an empty line to the listening <see cref="OnWarningWriteLine"/> events.
        /// </summary>
        public void WarningWriteLine()
        {
            this.WarningWriteLine( 0 );
        }

        /// <summary>
        /// Writes an empty line to the lisenting <see cref="OnWarningWriteLine"/> events, but
        /// only is <see cref="Verbosity"/> is greater than the passed in verbosity level.
        /// </summary>
        /// <param name="verbosityLevel">The verbosity level required to print this message.</param>
        public void WarningWriteLine( int verbosityLevel )
        {
            this.WriteLineInternal( verbosityLevel, OnWarningWriteLine );
        }

        /// <summary>
        /// Writes a formatted string to the listening <see cref="OnWarningWriteLine"/> events.
        /// </summary>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        public void WarningWriteLine( string formatStr, params object[] objects )
        {
            this.WarningWriteLine( 0, formatStr, objects );
        }

        /// <summary>
        /// Writes a formatted string to the listening <see cref="OnWarningWriteLine"/> events, but
        /// only is <see cref="Verbosity"/> is greater than the passed in verbosity level.
        /// </summary>
        /// <param name="verbosityLevel">The verbosity level required to print this message.</param>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        public void WarningWriteLine( int verbosityLevel, string formatStr, params object[] objects )
        {
            this.WriteLineInternal( verbosityLevel, OnWarningWriteLine, formatStr, objects );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all <see cref="OnWarningWriteLine"/> events.
        /// </summary>
        /// <param name="line">The string to write.</param>
        public void WarningWriteLine( string line )
        {
            this.WarningWriteLine( 0, line );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all <see cref="OnWarningWriteLine"/> events but
        /// only is <see cref="Verbosity"/> is greater than the passed in verbosity level.
        /// </summary>
        /// <param name="verbosityLevel">The verbosity level required to print this message.</param>
        /// <param name="line">The string to write.</param>
        public void WarningWriteLine( int verbosityLevel, string line )
        {
            this.WriteLineInternal( verbosityLevel, OnWarningWriteLine, line );
        }

        // -------- ErrorWriteLine --------

        /// <summary>
        /// Writes an empty line to the listening <see cref="OnErrorWriteLine"/> events.
        /// </summary>
        public void ErrorWriteLine()
        {
            ErrorWriteLine( 0 );
        }

        /// <summary>
        /// Writes an empty line to the listening <see cref="OnErrorWriteLine"/> events, but
        /// only is <see cref="Verbosity"/> is greater than the passed in verbosity level.
        /// </summary>
        /// <param name="verbosityLevel">The verbosity level required to print this message.</param>
        public void ErrorWriteLine( int verbosityLevel )
        {
            WriteLineInternal( verbosityLevel, OnErrorWriteLine );
        }

        /// <summary>
        /// Writes a formatted string to the listening <see cref="OnErrorWriteLine"/> events
        /// </summary>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        public void ErrorWriteLine( string formatStr, params object[] objects )
        {
            ErrorWriteLine( 0, formatStr, objects );
        }

        /// <summary>
        /// Writes a formatted string to the listening <see cref="OnErrorWriteLine"/> events, but
        /// only is <see cref="Verbosity"/> is greater than the passed in verbosity level.
        /// </summary>
        /// <param name="verbosityLevel">The verbosity level required to print this message.</param>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        public void ErrorWriteLine( int verbosityLevel, string formatStr, params object[] objects )
        {
            WriteLineInternal( verbosityLevel, OnErrorWriteLine, formatStr, objects );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all <see cref="OnErrorWriteLine"/> events.
        /// </summary>
        /// <param name="line">The string to write.</param>
        public void ErrorWriteLine( string line )
        {
            ErrorWriteLine( 0, line );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all <see cref="OnErrorWriteLine"/> events, but
        /// only is <see cref="Verbosity"/> is greater than the passed in verbosity level.
        /// </summary>
        /// <param name="verbosityLevel">The verbosity level required to print this message.</param>
        /// <param name="line">The string to write.</param>
        public void ErrorWriteLine( int verbosityLevel, string line )
        {
            WriteLineInternal( verbosityLevel, OnErrorWriteLine, line );
        }

        // -------- Internal Functions --------

        /// <summary>
        /// Writes an empty line to the listening events.
        /// 
        /// Thread-Safe.
        /// </summary>
        /// <param name="e">The event to pass the line into.</param>
        private void WriteLineInternal( int verbosityLevel, Action<string> e )
        {
            WriteLineInternal( verbosityLevel, e, string.Empty );
        }

        /// <summary>
        /// Writes a formatted string to the listening events.
        /// </summary>
        /// <param name="e">The event to pass the line into.</param>
        /// <param name="formatStr">The format string.</param>
        /// <param name="objects">The objects used to format the string.</param>
        private void WriteLineInternal( int verbosityLevel, Action<string> e, string formatStr, params object[] objects )
        {
            WriteLineInternal( verbosityLevel, e, string.Format( formatStr, objects ) );
        }

        /// <summary>
        /// Writes a string with a new line at the end
        /// to all the events.
        /// 
        /// Thread-Safe.
        /// </summary>
        /// <param name="verbosityLevel">
        /// The Verbosity level required to print this message.
        /// If <see cref="Verbosity"/> is less than this number, the message is ignored.
        /// </param>
        /// <param name="e">The event to pass the line into.</param>
        /// <param name="line">The string to write.</param>
        private void WriteLineInternal( int verbosityLevel, Action<string> e, string line )
        {
            int verbosity = this.Verbosity;

            if( verbosityLevel <= verbosity )
            {
                lock( onWriteLock )
                {
                    Action<string> action = e;
                    action?.Invoke( line + Environment.NewLine );
                }
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
            Log = new GenericLogger( 0 );
        }

        /// <summary>
        /// The global <see cref="GenericLogger"/>
        /// instance.
        /// </summary>
        public static GenericLogger Log { get; private set; }
    }
}
