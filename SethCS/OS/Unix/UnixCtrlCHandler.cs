using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
#if __MonoCS__
using Mono.Unix;
using Mono.Native;
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
            if ( Environment.OSVersion.Platform != PlatformID.Unix )
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
            if ( this.signalThread.IsAlive )
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
