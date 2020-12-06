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

        private readonly TimeSpan maxRunTime;

        private readonly EventExecutor executor;

        private readonly AutoResetEvent interruptEvent;

        // ----------------- Constructor -----------------

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxRunTime">
        /// How long each event is allowed to run for before being interrupted in milliseconds.
        /// </param>
        /// <param name="name">
        /// What to name the event executor's thread.  Null for default value.
        /// </param>
        public InterruptibleEventExecutor( int maxRunTime, string name = DefaultThreadName ) :
            this( TimeSpan.FromMilliseconds( maxRunTime ), name )
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxRunTime">
        /// How long each event is allowed to run for before being interrupted.
        /// </param>
        /// <param name="name">
        /// What to name the event executor's thread.  Null for default value.
        /// </param>
        public InterruptibleEventExecutor( TimeSpan maxRunTime, string name = DefaultThreadName ) :
            base( $"{name}: Interrruptor" )
        {
            if( maxRunTime <= TimeSpan.Zero )
            {
                throw new ArgumentException( "Must be greater than 0.", nameof( maxRunTime ) );
            }

            ArgumentChecker.IsNotNull( name, nameof( name ) );

            this.maxRunTime = maxRunTime;

            this.executor = new EventExecutor( $"{name}: Executor" );
            this.executor.OnError += this.Executor_OnError;
            this.interruptEvent = new AutoResetEvent( false );
        }

        // ----------------- Properties -----------------

        public override void Start()
        {
            if( this.runnerThread != null )
            {
                throw new InvalidOperationException( "Already Started" );
            }

            this.executor?.Start();
            base.Start();
        }

        /// <summary>
        /// Add an event to the event queue.  This returns immediatly after
        /// the action is added to the queue.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public override void AddEvent( Action action )
        {
            Action eventAction = delegate ()
            {
                try
                {
                    action();
                }
                finally
                {
                    this.interruptEvent.Set();
                }
            };

            Action interruptAction = delegate ()
            {
                if( this.interruptEvent.WaitOne( this.maxRunTime ) == false )
                {
                    this.executor.Interrupt();

                    // Wait to be interrupted.
                    this.interruptEvent.WaitOne();
                }
            };

            this.executor.AddEvent( eventAction );
            base.AddEvent( interruptAction );
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
                // First, stop the executor thread.  This means
                // no other events will trigger the interrupt event.
                // until below.
                this.executor.Dispose();
            }
            finally
            {
                // Set the interrupt event so this thread unblocks if its waiting.
                // No other actions should be being enqueued when dispose is called.
                this.interruptEvent.Set();
                base.Dispose();
                this.interruptEvent.Dispose();
            }
        }

        private void Executor_OnError( Exception obj )
        {
            this.InvokeOnError( obj );
        }
    }
}
