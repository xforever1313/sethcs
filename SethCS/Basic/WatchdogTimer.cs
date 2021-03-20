//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Threading;
using SethCS.Exceptions;

namespace SethCS.Basic
{
    public class WatchdogTimer : IDisposable
    {
        // ---------------- Events ----------------

        /// <summary>
        /// Event that gets fired if the watchdog thread expired.
        /// </summary>
        public event Action OnTimeoutExpired;

        /// <summary>
        /// Event that gets fired if <see cref="OnTimeoutExpired"/> throws an unhandled exception.
        /// </summary>
        public event Action<Exception> OnTimeoutExpiredError;

        /// <summary>
        /// Event that gets fired when the watchdog thread starts.
        /// </summary>
        public event Action OnStarted;

        /// <summary>
        /// Event that gets fired if the watchdog timer gets reset.
        /// </summary>
        public event Action OnReset;

        /// <summary>
        /// Event that gets fired if the watchdog timer gets stopped.
        /// </summary>
        public event Action OnStopped;

        // ---------------- Fields ----------------

        private bool isDisposed;

        private Thread timerThread;

        private bool runThread;
        private object runThreadLock;

        private bool isStarted;
        private object isStartedLock;

        ManualResetEvent startedEvent;
        AutoResetEvent timeoutEvent;

        // ---------------- Constructor ----------------

        public WatchdogTimer( int timeout, string name )
        {
            if( timeout < 0 )
            {
                throw new ArgumentException( "Timeout must be greater than 0.", nameof( timeout ) );
            }
            ArgumentChecker.IsNotNull( name, nameof( name ) );

            this.isDisposed = false;

            this.runThreadLock = new object();
            this.RunThread = false;

            this.isStartedLock = new object();
            this.IsStarted = false;

            this.Timeout = timeout;
            this.Name = name;

            this.startedEvent = new ManualResetEvent( false );
            this.timeoutEvent = new AutoResetEvent( false );
            this.timerThread = null;
        }

        ~WatchdogTimer()
        {
            try
            {
                this.Dispose( false );
            }
            catch( Exception )
            {
                // Swallow exception, don't kill GC thread.
            }
        }

        // ---------------- Properties ----------------

        /// <summary>
        /// How long to wait until the watchdog timer
        /// times out in milliseconds.
        /// </summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// What we named the watchdog timer.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Is the watchdog started?
        /// </summary>
        public bool IsStarted
        {
            get
            {
                lock( this.isStartedLock )
                {
                    return this.isStarted;
                }
            }

            private set
            {
                lock( this.isStartedLock )
                {
                    this.isStarted = value;
                }
            }
        }

        /// <summary>
        /// Should the thread keep running?
        /// </summary>
        private bool RunThread
        {
            get
            {
                lock( this.runThreadLock )
                {
                    return this.runThread;
                }
            }

            set
            {
                lock( this.runThreadLock )
                {
                    this.runThread = value;
                }
            }
        }

        // ---------------- Functions ----------------
        
        /// <summary>
        /// Starts the watchdog timer.
        /// </summary>
        public void Start()
        {
            this.DisposeCheck();

            if( this.IsStarted )
            {
                throw new InvalidOperationException( "Watchdog Timer " + this.Name + " is already running!" );
            }

            // Start the thread if it hasn't been started yet.
            // We don't create/destroy a thread each time Start/Stop is called... it seems like
            // it could complicate programming.  That, and typically watchdog timers live forever anyways...
            if( this.timerThread == null )
            {
                this.RunThread = true; // <- THIS MUST BE SET BEFORE STARTING THE THREAD.
                                       //    Otherwise, the thread might get the CPU before
                                       //    we set this to true and exit right away.
                this.timerThread = new Thread( this.Run );
                this.timerThread.Name =  "SethCS Watchdog Timer " + this.Name;
                this.timerThread.Start();
            }

            // IsStarted must be set before we set the startedEvent.
            // If we do the opposite, the timer thread may run, timeout, and check IsStarted
            // before we set it to true in this function, which means, we will not run the 
            // TimeoutEvent as IsStarted will be still set
            // to false.
            this.IsStarted = true;
            this.startedEvent.Set();

            this.OnStarted?.Invoke();
        }

        /// <summary>
        /// Resets the watchdog timer and starts it again.
        /// </summary>
        public void Reset()
        {
            this.DisposeCheck();

            if( this.IsStarted == false )
            {
                throw new InvalidOperationException( "Watchdog Timer " + this.Name + " is not running, can not reset!" );
            }

            // Notify our timeoutEvent to say that we did NOT timeout.  The timer will be reset.
            this.timeoutEvent.Set();
            this.OnReset?.Invoke();
        }

        /// <summary>
        /// Stops the watchdog timer.
        /// </summary>
        public void Stop()
        {
            this.DisposeCheck();

            if( this.IsStarted == false )
            {
                throw new InvalidOperationException( "Watchdog Timer " + this.Name + " is not running, can not stop!" );
            }

            // Order of operations for Stop().
            // 1. Most important thing is to flag as we are not started ASAP.
            //    This was if our timeoutEvent just expired, and is just before checking IsStarted,
            //    we can set IsStarted to false, and it won't fire the event.
            this.IsStarted = false;

            // 2. Reset the startedEvent.  The timer thread will stop.  Even if it runs 1000 times
            //    after the last instruction in this function, the TimeoutEvent won't happen
            //    as IsStarted was set to false.
            this.startedEvent.Reset();

            // 3. Set our timeoutEvent, so the next time Start() is called, we have a clean timer.
            //    Since we reset our startedEvent, the thread will Sleep until Start() is called again.
            this.timeoutEvent.Set();

            // 4. Invoke any events.
            this.OnStopped?.Invoke();
        }

        public void Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected void Dispose( bool fromDispose )
        {
            if( this.isDisposed )
            {
                return;
            }

            try
            {
                // To exit the thread, set everything to false, and then
                // set all of our ResetEvents.  This should cause the thread to exit.
                this.RunThread = false;
                this.IsStarted = false;
                this.startedEvent.Set();
                this.timeoutEvent.Set();

                // Removed unmanaged code here.
                if( this.timerThread != null )
                {
                    this.timerThread.Join();
                }

                if( fromDispose )
                {
                    // Remove managed code here.
                    // Must dispose after timerThread exits, or that thread *could* throw an
                    // ObjectDisposedException.
                    this.startedEvent.Dispose();
                    this.timeoutEvent.Dispose();
                }
            }
            finally
            {
                this.isDisposed = true;
            }
        }

        private void Run()
        {
            while( this.RunThread )
            {
                // Wait until a user calls start.  If a user never calls start,
                // we'll never timeout.
                this.startedEvent.WaitOne();

                // Wait until a user calls "Reset".  If this times out,
                // this returns false, and we then know to run the expired event.
                // If reset is called before we timeout, we return true, and will not fire the event.
                bool wasReset = this.timeoutEvent.WaitOne( this.Timeout );

                // If we just so happened to call stop just as soon as we timeout, do not fire the timed out
                // event.
                if( this.IsStarted )
                {
                    if( wasReset == false )
                    {
                        try
                        {
                            this.OnTimeoutExpired();
                        }
                        catch( Exception e )
                        {
                            this.OnTimeoutExpiredError?.Invoke( e );
                        }
                    }
                }

                this.timeoutEvent.Reset();

                // If Stop() was called, we'll wait on the startedEvent until Start() is called again.
                // Otherwise, we'll continue the watchdog timer.
            }
        }

        private void DisposeCheck()
        {
            if( this.isDisposed )
            {
                throw new ObjectDisposedException( "Watchdog Timer " + this.Name + " is already disposed." );
            }
        }
    }
}
