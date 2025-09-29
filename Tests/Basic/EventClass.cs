//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Basic
{
    /// <summary>
    /// Event that is executed on the event executor
    /// </summary>
    public sealed class EventClass
    {
        // -------- Fields --------

        /// <summary>
        /// Whether or not the event was executed.
        /// </summary>
        private bool executed;

        /// <summary>
        /// Lock for this.executed.
        /// </summary>
        private object executedLock;

        /// <summary>
        /// Used to figure out if the event ran or not.
        /// </summary>
        private ManualResetEvent resetEvent;

        // -------- Constructor --------

        /// <summary>
        /// Constructor
        /// </summary>
        public EventClass()
        {
            this.executedLock = new object();
            this.executed = false;
            this.resetEvent = new ManualResetEvent( false );
        }

        // -------- Properties --------

        /// <summary>
        /// Whether or not the event was executed.
        /// </summary>
        public bool Executed
        {
            get
            {
                lock( this.executedLock )
                {
                    return this.executed;
                }
            }
            private set
            {
                lock( this.executedLock )
                {
                    this.executed = value;
                }
            }
        }

        // -------- Functions ---------

        /// <summary>
        /// Function that is executed.
        /// </summary>
        /// <param name="actionBeforeSetting">Action that is performed before our reset event is set.</param>
        public void Execute( Action actionBeforeSetting = null )
        {
            this.Executed = true;
            actionBeforeSetting?.Invoke();
            this.resetEvent.Set();
        }

        /// <summary>
        /// Waits the given amount of time before executing.
        /// </summary>
        public void WaitAndExecute( int waitTimeMs, Action actionBeforeSetting = null )
        {
            Thread.Sleep( waitTimeMs );
            this.Execute( actionBeforeSetting );
        }

        public Task AsyncWaitAndExecute( int waitTimeMs, Action actionBeforeSetting = null )
        {
            return Task.Run( () => this.WaitAndExecute( waitTimeMs, actionBeforeSetting ) );
        }

        /// <summary>
        /// Executes and then throws the given Exception.
        /// </summary>
        /// <param name="e">The exception to throw.</param>
        public void ExecuteAndThrow( Exception e )
        {
            this.Executed = true;
            throw e;
        }

        /// <summary>
        /// Sets this object's internal reset event.
        /// </summary>
        public void Release()
        {
            this.resetEvent.Set();
        }

        /// <summary>
        /// Waits for Execute() to run.
        /// </summary>
        public void Join()
        {
            this.resetEvent.WaitOne();
        }

        /// <summary>
        /// Waits for Execute() to run.
        /// </summary>
        /// <param name="timeoutMs">How long to wait before timing out. Milliseconds.</param>
        /// <returns>True if we did not timeout, else false.</returns>
        public bool Join( int timeoutMs )
        {
            return this.resetEvent.WaitOne( timeoutMs );
        }
    }

    /// <summary>
    /// Thrown when our events throw an exception.
    /// </summary>
    public class EventException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="index">The index of the event this was thrown from.</param>
        public EventException( int index ) :
            base( index.ToString() )
        {
            this.Index = index;
        }

        /// <summary>
        /// The index of the reset event.
        /// </summary>
        public int Index { get; private set; }
    }
}
