//
//          Copyright Seth Hendrick 2016-2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Threading;

namespace SethCS.Basic
{
    /// <summary>
    /// This class is an implementation of <see cref="EventExecutor"/>,
    /// but users can interrupt the execution thread.
    /// They can also allocate a limited number of time for each task
    /// to run before it gets interrupted.
    /// </summary>
    public class InterruptibleEventExecutor : EventExecutor
    {
        // ----------------- Fields -----------------

        private int maxRunTime;

        private AutoResetEvent eventCompletedEvent;

        /// <summary>
        /// This thread will interrupt events if they take to long to execute.
        /// </summary>
        private Thread eventWatcherThread;

        private bool isActive;
        private object isActiveLock;

        // ----------------- Constructor -----------------

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executeIfDisposed">
        /// Whether or not to execute events that were not executed
        /// while the thread was running upon being disposed.
        /// Set to true to ensure ALL events that were added will get run when Dispose() is called.
        /// Set to false, and any events that were not run will not be run
        /// during Dispose().
        /// 
        /// Note, events will STILL be interrupted during the dispose function if they take too long.
        /// </param>
        /// <param name="maxRunTime">
        /// How long each event is allowed to run for before being interrupted in milliseconds.
        /// 
        /// Set to <see cref="int.MaxValue"/> for no time limit.
        /// </param>
        public InterruptibleEventExecutor( bool executeIfDisposed = true, int maxRunTime = int.MaxValue ) :
            base( executeIfDisposed )
        {
            if( maxRunTime <= 0 )
            {
                throw new ArgumentException( "Must be greater than 0.", nameof( maxRunTime ) );
            }

            this.maxRunTime = maxRunTime;
            this.eventCompletedEvent = new AutoResetEvent( false );

            this.isActive = false;
            this.isActiveLock = new object();
        }

        // ----------------- Properties -----------------

        /// <summary>
        /// Is this class currently active?
        /// </summary>
        private bool IsActive
        {
            get
            {
                lock( this.isActiveLock )
                {
                    return this.isActive;
                }
            }
            set
            {
                lock( this.isActiveLock )
                {
                    this.isActive = value;
                }
            }
        }

        // ----------------- Functions -----------------

        public override void Start()
        {
            if( this.runnerThread != null )
            {
                throw new InvalidOperationException( "Already Started" );
            }

            // Set this to true first before starting the thread
            // so the thread doesn't exit right away.
            this.IsActive = true;
            if( this.maxRunTime != int.MaxValue )
            {
                this.eventWatcherThread = new Thread(
                    this.WatcherThreadRun
                );

                this.eventWatcherThread.Start();
            }

            base.Start();
        }

        /// <summary>
        /// Interrupts the event runner thread.
        /// </summary>
        public void Interrupt()
        {
            if( this.runnerThread == null )
            {
                throw new InvalidOperationException( "Not Started.  Call Start() first." );
            }
            this.runnerThread.Interrupt();
        }

        protected override void ExecuteEvent()
        {
            // If there's no maximum time, just execute the event normally.
            if( this.maxRunTime == int.MaxValue )
            {
                base.ExecuteEvent();
            }
            else
            {
                try
                {
                    this.eventCompletedEvent.Reset();
                    base.ExecuteEvent();
                }
                finally
                {
                    this.eventCompletedEvent.Set();
                }
            }
        }

        /// <summary>
        /// Disposes the event executor.
        /// The event queue stops, and gracefully waits for the thread to join.
        /// It will then execute any unran events if ExecuteIfDisposed is set to true.
        /// 
        /// Note that events will still be interrupted if ExecuteIfDisposed is set to true
        /// and they take longer than the allowed time.
        /// 
        /// Note, that if Dispose() was called before start was called and
        /// ExecuteIfDisposed is set to true, events
        /// will be executed, but they will not be interrupted (timing thread
        /// would not have been started).
        /// </summary>
        public override void Dispose()
        {
            try
            {
                base.Dispose();
            }
            finally
            {
                this.IsActive = false;
                this.eventCompletedEvent.Set();
                this.eventWatcherThread?.Join();
                this.eventCompletedEvent.Dispose();
            }
        }

        private void WatcherThreadRun()
        {
            while( this.IsActive )
            {
                // Wait for our AutoResetEvent to be set. If it is not, send an interrupt and wait
                // for the event to be interrupted.
                bool completed = this.eventCompletedEvent.WaitOne( this.maxRunTime );
                if( ( completed == false ) && this.IsActive )
                {
                    this.runnerThread.Interrupt();
                    this.eventCompletedEvent.WaitOne();
                }
            }
        }
    }
}
