//
//          Copyright Seth Hendrick 2016-2018.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Threading;
using SethCS.Exceptions;

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

        public new const string DefaultThreadName = nameof( InterruptibleEventExecutor );

        private int maxRunTime;

        private AutoResetEvent eventCompletedEvent;
        private AutoResetEvent eventStartedEvent;
        private ManualResetEvent ableToInterrupt;

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
        /// <param name="name">
        /// What to name the event executor's thread.  Null for default value.
        /// </param>
        public InterruptibleEventExecutor( int maxRunTime = int.MaxValue, string name = DefaultThreadName ) :
            base( name )
        {
            if( maxRunTime <= 0 )
            {
                throw new ArgumentException( "Must be greater than 0.", nameof( maxRunTime ) );
            }

            ArgumentChecker.IsNotNull( name, nameof( name ) );

            this.maxRunTime = maxRunTime;
            this.eventCompletedEvent = new AutoResetEvent( false );
            this.eventStartedEvent = new AutoResetEvent( false );
            this.ableToInterrupt = new ManualResetEvent( false );

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
                this.eventWatcherThread.Name = this.name + "'s event watcher";

                this.eventWatcherThread.Start();
            }

            base.Start();
        }

        /// <summary>
        /// Interrupts the event runner thread.
        /// Blocks until the running thread is in an interruptable state
        /// (an event is actively being run).  This means that if no
        /// events are on the queue, this will block forever.
        /// This is why there is a supplied timeout.
        /// </summary>
        /// <param name="timeout">How long to wait before interrupting</param>
        /// <returns>True if interrupt was sent to the runner thread, else false.</returns>
        public bool Interrupt( int timeout = Timeout.Infinite )
        {
            if( this.runnerThread == null )
            {
                throw new InvalidOperationException( "Not Started.  Call Start() first." );
            }
            bool waitForInterrupt = this.ableToInterrupt.WaitOne( timeout );
            if( waitForInterrupt )
            {
                this.runnerThread.Interrupt();
            }

            return waitForInterrupt;
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
                        this.ableToInterrupt.Set();
                        action();
                        this.ableToInterrupt.Reset();
                    }
                    finally
                    {
                        this.ableToInterrupt.Reset();
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

                this.ableToInterrupt.Dispose();
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
