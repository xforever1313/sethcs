//
//          Copyright Seth Hendrick 2016-2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Threading;

namespace SethCS.Basic
{
    /// <summary>
    /// Executes one event at a time in a queue.
    /// 
    /// Any action added to the queue runs in the Event Executor's own SyncronizationContext.
    /// Therefore, you can use async/await here.  Anything that comes after an await will get
    /// enqueued to the event queue to be executed.
    /// </summary>
    public class EventExecutor : IDisposable
    {
        // ---------------- Events ----------------

        /// <summary>
        /// Action to take if an unhandled exception is thrown.
        /// </summary>
        public event Action<Exception> OnError;

        // ---------------- Fields ----------------

        /// <summary>
        /// Name of the event executor thread.
        /// </summary>
        public const string ThreadName = nameof( EventExecutor );

        /// <summary>
        /// Queue of actions to do.
        /// </summary>
        private Queue<Action> actionQueue;

        /// <summary>
        /// The semaphore that blocks the thread from running if there
        /// are no events to take place.
        /// </summary>
        private Semaphore actionSemaphore;

        /// <summary>
        /// Thread that executes the events.
        /// </summary>
        protected Thread runnerThread;

        /// <summary>
        /// Whether or not this thing is running or not.
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// The lock that protects isRunning.
        /// </summary>
        private object isRunningLock;

        // ---------------- Constructor ----------------

        /// <summary>
        /// Constructor
        /// </summary>
        public EventExecutor()
        {
            this.actionQueue = new Queue<Action>();
            this.actionSemaphore = new Semaphore( 0, int.MaxValue );

            this.isRunningLock = new object();
            this.IsRunning = false;
        }

        // ---------------- Properties ----------------

        /// <summary>
        /// Checks to see if the executor is running or not.
        /// Thread-safe.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock( this.isRunningLock )
                {
                    return this.isRunning;
                }
            }
            private set
            {
                lock( this.isRunningLock )
                {
                    this.isRunning = value;
                }
            }
        }

        // ----------------- Functions ----------------

        /// <summary>
        /// Starts the execution thread.
        /// </summary>
        public virtual void Start()
        {
            if( this.runnerThread != null )
            {
                throw new InvalidOperationException( "Already started." );
            }

            this.runnerThread = new Thread(
                () => this.Run()
            );
            this.runnerThread.Name = ThreadName;

            this.IsRunning = true;
            this.runnerThread.Start();
        }

        /// <summary>
        /// Add an event to the event queue.  This returns immediatly after
        /// the action is added to the queue.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public virtual void AddEvent( Action action )
        {
            lock( this.actionQueue )
            {
                this.actionQueue.Enqueue( action );
            }

            this.actionSemaphore.Release();
        }

        /// <summary>
        /// Disposes the event executor.
        /// The event queue stops, and gracefully waits for the thread to join.
        /// 
        /// Note that any unran events will NOT be run.  If you want to guarentee all events have 
        /// been executed, you can add one more event to the event queue, and wait
        /// for that event to execute before calling Dispose.
        /// </summary>
        public virtual void Dispose()
        {
            this.IsRunning = false;
            this.actionSemaphore.Release();
            this.runnerThread?.Join();
            this.actionQueue.Clear();
        }

        /// <summary>
        /// Runs the events.
        /// </summary>
        private void Run()
        {
            SyncContext context = new SyncContext( this );
            SynchronizationContext.SetSynchronizationContext( context );

            while( this.IsRunning )
            {
                try
                {
                    this.actionSemaphore.WaitOne();
                    ExecuteEvent();
                }
                catch( Exception e )
                {
                    this.OnError?.Invoke( e );
                }
            }

            // If we still have things going on in the background,
            // wait for those to finish before we terminate the synchronization context.
            while( context.IsBusy )
            {
                try
                {
                    ExecuteEvent();
                }
                catch( Exception e )
                {
                    this.OnError?.Invoke( e );
                }
            }
        }

        /// <summary>
        /// Grabs the latest event from the queue and executes it.
        /// </summary>
        protected void ExecuteEvent()
        {
            Action action = null;

            // Grab the latest action from the queue.
            lock( this.actionQueue )
            {
                if( this.actionQueue.Count > 0 )
                {
                    action = this.actionQueue.Dequeue();
                }
            }

            // Only execute event if there was something in the queue.
            // Its possible we ended up here due to the thread stopping.
            action?.Invoke();
        }

        // ---------------- Helper Classes ----------------

        /// <summary>
        /// SynchronizationContext so we can properly do async/awaits in the event executor.
        /// How this works is what comes after the await gets re-enqueued the the event executor
        /// as another event instead of running in a worker thread.
        /// </summary>
        private class SyncContext : SynchronizationContext
        {
            // ---------------- Fields ----------------

            private EventExecutor exe;
            private long asyncCount;

            // ---------------- Constructor ----------------

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="exe">The event executor we are adding the context to.</param>
            public SyncContext( EventExecutor exe )
            {
                this.exe = exe;
                this.asyncCount = 0;
            }

            // ---------------- Properties ----------------

            /// <summary>
            /// This will be set to true while there is an operation going
            /// on in a background thread.
            /// </summary>
            public bool IsBusy
            {
                get
                {
                    return Interlocked.Read( ref this.asyncCount ) > 0;
                }
            }

            // ---------------- Functions ----------------

            /// <summary>
            /// POST:
            /// Post means we want to add an async event.  This therefore adds
            /// an event to the event queue and returns.
            /// 
            /// Post is called after an await.  For example, if this delegate is added
            /// to the event queue.
            /// 
            /// delegate()
            /// {
            ///     int something = SomeFunction();
            ///     int somethingElse = await SomeOtherFunction( something );
            ///     DoTheThing( somethingElse );
            /// }
            /// 
            /// SomeFunction runs in the event queue.  SomeOtherFunction runs in a worker
            /// thread.  DoTheThing then gets added via this function back on the event queue.
            /// </summary>
            public override void Post( SendOrPostCallback d, object state )
            {
                this.exe.AddEvent( () => d.Invoke( state ) );
            }

            /// <summary>
            /// SEND:
            /// Send is like invoke, it blocks.
            /// This adds an event to the event queue and blocks until its done.
            /// </summary>
            public override void Send( SendOrPostCallback d, object state )
            {
                ManualResetEvent e = new ManualResetEvent( false );
                this.exe.AddEvent(
                    delegate ()
                    {
                        try
                        {
                            d.Invoke( state );
                        }
                        finally
                        {
                            e.Set();
                        }
                    }
                );
                e.WaitOne();
            }

            /// <summary>
            /// Called when a worker thread starts.
            /// </summary>
            public override void OperationStarted()
            {
                Interlocked.Increment( ref this.asyncCount );
                base.OperationStarted();
            }

            /// <summary>
            /// Triggered when an operation is completed.
            /// With async/await, this happens when everything after an await executes.
            /// </summary>
            public override void OperationCompleted()
            {
                try
                {
                    base.OperationCompleted();
                }
                finally
                {
                    Interlocked.Decrement( ref this.asyncCount );
                }
            }

            /// <summary>
            /// Honestly... have no idea what this does...
            /// </summary>
            public override int Wait( IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout )
            {
                return base.Wait( waitHandles, waitAll, millisecondsTimeout );
            }
        }
    }
}