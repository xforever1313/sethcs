
//          Copyright Seth Hendrick 2016.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.Threading;

#if __MonoCS__
using Mono.Unix;
using Mono.Unix.Native;
#endif

namespace SethCS.OS.Unix
{
    #if __MonoCS__

    public class UnixCtrlCHandler : CtrlCHandler
    {
        // -------- Fields --------

        /// <summary>
        /// Signals to watch for to terminate the program.
        /// </summary>
        private static readonly UnixSignal[] signalsToWatch = {
            new UnixSignal( Signum.SIGTERM ), // Termination Signal
            new UnixSignal( Signum.SIGINT )   // Interrupt from the keyboard
        };

        Thread signalThread;

        // -------- Constructor --------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown when ran on windows.</exception>
        public UnixCtrlCHandler() :
            base()
        {
            if( Environment.OSVersion.Platform != PlatformID.Unix )
            {
                throw new PlatformNotSupportedException(
                    "Unix does not support WindowsCtrlCHandler!"
                );
            }

            this.signalThread = new Thread(
                delegate()
                {
                    try
                    {
                        UnixSignal.WaitAny( signalsToWatch );
                        Console.WriteLine( "Received Interrupt signal.  Terminating" );
                        this.signalEvent.Set();
                    }
                    catch( ThreadAbortException )
                    {
                    }
                }
            );

            this.signalThread.Start();
        }

        // -------- Functions --------

        /// <summary>
        /// Cleans up this class.
        /// </summary>
        protected override void CleanUp()
        {
            if( this.signalThread.IsAlive )
            {
                this.signalThread.Abort();
                this.signalThread.Join();
            }
        }

    }

    #else
    
    public class UnixCtrlCHandler : CtrlCHandler
    {
        // -------- Constructor --------

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown when ran on windows.</exception>
        public UnixCtrlCHandler() :
            base()
        {
            throw new PlatformNotSupportedException(
                "Windows can not use the UnixCtrlCHandler!"
            );
        }

        // -------- Functions --------

        /// <summary>
        /// Cleans up this class.
        /// </summary>
        protected override void CleanUp()
        {

        }
    }
#endif

}
