using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SethCS.Basic
{
    /// <summary>
    /// Executes one event at a time in a queue.
    /// </summary>
    public class EventExecutor : IDisposable
    {
        // -------- Fields --------

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
        private Thread runnerThread;

        /// <summary>
        /// Whether or not this thing is running or not.
        /// </summary>
        private bool isRunning;

        /// <summary>
        /// The lock that protects isRunning.
        /// </summary>
        private object isRunningLock;

        /// <summary>
        /// Action to take if an unhandled exception is thrown.
        /// </summary>
        private Action<Exception> errorAction;

        // -------- Constructor --------

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executeIfDisposed">
        /// Whether or not to execute events that were not executed
        /// while the thread was running upon being disposed.
        /// Set to true to ensure ALL events that were added will get run when Dispose() is called.
        /// Set to false, and any events that were not run will not be run
        /// during Dispose().
        /// </param>
        /// <param name="errorAction">
        /// Action to take if an unhandled exception occurs 
        /// Null for swallowing it and take no action.
        /// </param>
        public EventExecutor( bool executeIfDisposed = true, Action<Exception> errorAction = null )
        {
            this.actionQueue = new Queue<Action>();
            this.actionSemaphore = new Semaphore( 0, int.MaxValue );
            this.runnerThread = new Thread(
                () => this.Run()
            );

            this.isRunningLock = new object();
            this.IsRunning = false;

            this.errorAction = errorAction;
            this.ExecuteWhenDisposed = executeIfDisposed;
        }

        // -------- Properties --------

        /// <summary>
        /// Checks to see if the executor is running or not.
        /// Thread-safe.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock ( this.isRunningLock )
                {
                    return this.isRunning;
                }
            }
            private set
            {
                lock ( this.isRunningLock )
                {
                    this.isRunning = value;
                }
            }
        }

        /// <summary>
        /// Whether or not to execute events that were not executed
        /// while the thread was running upon being disposed.
        /// Set to true to ensure ALL events that were added will get run when Dispose() is called.
        /// Set to false, and any events that were not run will not be run
        /// during Dispose().
        /// </summary>
        public bool ExecuteWhenDisposed { get; private set; }

        // --------- Functions --------

        /// <summary>
        /// Starts the execution thread.
        /// </summary>
        public void Start()
        {
            this.IsRunning = true;
            this.runnerThread.Start();
        }

        /// <summary>
        /// Add an event to the event queue.  This returns immediatly after
        /// the action is added to the queue.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public void AddEvent( Action action )
        {
            lock ( this.actionQueue )
            {
                this.actionQueue.Enqueue( action );
            }

            this.actionSemaphore.Release();
        }

        /// <summary>
        /// Disposes the event executor.
        /// The event queue stops, and gracefully waits for the thread to join.
        /// It will then execute any unran events if ExecuteIfDisposed is set to true.
        /// </summary>
        public void Dispose()
        {
            this.IsRunning = false;
            this.actionSemaphore.Release();
            if ( this.runnerThread.IsAlive )
            {
                this.runnerThread.Join();
            }

            if ( this.ExecuteWhenDisposed )
            {
                while ( this.actionQueue.Count > 0 )
                {
                    ExecuteEvent();
                }
            }
            else
            {
                this.actionQueue.Clear();
            }
        }

        /// <summary>
        /// Runs the events.
        /// </summary>
        private void Run()
        {
            while ( this.IsRunning )
            {
                this.actionSemaphore.WaitOne();
                ExecuteEvent();
            }
        }

        /// <summary>
        /// Grabs the latest event from the queue and executes it.
        /// </summary>
        private void ExecuteEvent()
        {
            try
            {
                Action action = null;

                // Grab the latest action from the queue.
                lock ( this.actionQueue )
                {
                    if ( this.actionQueue.Count > 0 )
                    {
                        action = this.actionQueue.Dequeue();
                    }
                }

                // Only execute event if there was something in the queue.
                // Its possible we ended up here due to the thread stopping.
                if ( action != null )
                {
                    action();
                }
            }
            catch ( Exception e )
            {
                if ( errorAction != null )
                {
                    errorAction( e );
                }
            }
        }
    }
}
