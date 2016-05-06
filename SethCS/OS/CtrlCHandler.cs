
//          Copyright Seth Hendrick 2016.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.Threading;
using SethCS.OS.Windows;

#if __MonoCS__
using SethCS.OS.Unix;
#endif

namespace SethCS.OS
{
    /// <summary>
    /// Handles when a user hit CTRL+C on Windows,
    /// or sends a SIGTERM or SIGINT on linux.
    /// </summary>
    public abstract class CtrlCHandler : IDisposable
    {
        // -------- Fields --------

        /// <summary>
        /// The wait event that waits for the signal.
        /// </summary>
        protected ManualResetEvent signalEvent;

        // -------- Constructor ---------

        public static CtrlCHandler CreateHandler()
        {
#if __MonoCS__
            if( Environment.OSVersion.Platform == PlatformID.Unix )
            {
                return new UnixCtrlCHandler();
            }
            else
            {
                return new WindowsCtrlCHandler();
            }
#else
            return new WindowsCtrlCHandler();
#endif
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CtrlCHandler()
        {
            this.signalEvent = new ManualResetEvent( false );
        }

        // -------- Functions ---------

        /// <summary>
        /// Waits for the signal to occur.
        /// </summary>
        public void WaitForSignal()
        {
            this.signalEvent.WaitOne();
        }

        /// <summary>
        /// Cleans up this class.
        /// </summary>
        public void Dispose()
        {
            CleanUp();
        }

        // ---- Abstract Functions ----

        /// <summary>
        /// Cleans up this class.
        /// </summary>
        protected abstract void CleanUp();
    }
}
