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
        private AutoResetEvent eventStartedEvent;

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
        /// <param name="maxRunTime">
        /// How long each event is allowed to run for before being interrupted in milliseconds.
        /// 
        /// Set to <see cref="int.MaxValue"/> for no time limit.
        /// </param>
        public InterruptibleEventExecutor( int maxRunTime = int.MaxValue )
        {
            if( maxRunTime <= 0 )
            {
                throw new ArgumentException( "Must be greater than 0.", nameof( maxRunTime ) );
            }

            this.maxRunTime = maxRunTime;
            this.eventCompletedEvent = new AutoResetEvent( false );
            this.eventStartedEvent = new AutoResetEvent( false );

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

        /// <summary>
        /// Add an event to the event queue.  This returns immediatly after
        /// the action is added to the queue.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public override void AddEvent( Action action )
        {
            // If there's no maximum time, just execute the event normally.
            if( this.maxRunTime == int.MaxValue )
            {
                base.AddEvent( action );
            }
            else
            {
                Action theAction = delegate ()
                {
                    try
                    {
                        this.eventStartedEvent.Set();
                        action();
                    }
                    finally
                    {
                        this.eventCompletedEvent.Set();
                    }
                };

                base.AddEvent( theAction );
            }
        }

        /// <summary>
        /// Disposes the event executor.
        /// The event queue stops, and gracefully waits for the thread to join.
        /// Note that any events that were NOT run will not be executed when Dispose is called.
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
                this.eventStartedEvent.Set();
                this.eventWatcherThread?.Join();

                this.eventStartedEvent.Dispose();
                this.eventCompletedEvent.Dispose();
            }
        }

        private void WatcherThreadRun()
        {
            while( this.IsActive )
            {
                // Wait for an event to fire.
                this.eventStartedEvent.WaitOne();

                if( this.IsActive )
                {
                    // Wait for our AutoResetEvent to be set. If it is not, send an interrupt and wait
                    // for the event to be interrupted.
                    bool completed = this.eventCompletedEvent.WaitOne( this.maxRunTime );
                    if( ( completed == false ) && this.IsActive )
                    {
                        this.runnerThread.Interrupt();

                        // Wait to be interrupted.
                        this.eventCompletedEvent.WaitOne();
                    }
                }
            }
        }
    }
}
