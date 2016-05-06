
//          Copyright Seth Hendrick 2016.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.Runtime.InteropServices;

namespace SethCS.OS.Windows
{
    public class WindowsCtrlCHandler : CtrlCHandler
    {
        // -------- Fields --------

        /// <summary>
        /// Adds or removes an application-defined HandlerRoutine function from the list of handler functions for the calling process
        /// </summary>
        /// <param name="handler">A pointer to the application-defined HandlerRoutine function to be added or removed. This parameter can be NULL.</param>
        /// <param name="add">If this parameter is TRUE, the handler is added; if it is FALSE, the handler is removed.</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport( "Kernel32" )]
        private static extern bool SetConsoleCtrlHandler( CtrlCHandler handler, bool add );

        private delegate bool CtrlCHandler( int ctrlType );

        // -------- Constructor --------

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowsCtrlCHandler() :
            base()
        {
            if( Environment.OSVersion.Platform == PlatformID.Unix )
            {
                throw new PlatformNotSupportedException(
                    "Unix does not support WindowsCtrlCHandler!"
                );
            }

            CtrlCHandler handler = new CtrlCHandler( SignalHandler );
            SetConsoleCtrlHandler( handler, true );
        }

        /// <summary>
        /// Cleans up this class.
        /// </summary>
        protected override void CleanUp()
        {

        }

        /// <summary>
        /// Handles the ctrl signal
        /// </summary>
        /// <param name="ctrl">Control type passed in.</param>
        /// <returns>Always true.</returns>
        private bool SignalHandler( int ctrl )
        {
            Console.WriteLine( "Received Interrupt signal.  Terminating" );
            this.signalEvent.Set();
            return true;
        }

    }
}
