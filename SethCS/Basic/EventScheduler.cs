
//          Copyright Seth Hendrick 2015.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using SethCS.Exceptions;

namespace SethCS.Basic
{
    /// <summary>
    /// This class schedules events to be run at some point in the future.
    /// Can either be triggered once, or at an interval.
    /// This class is not guarenteed to be completely accurrate, but it should be
    /// close enough for most uses.
    /// </summary>
    public class EventScheduler : IDisposable
    {
        // -------- Fields --------

        /// <summary>
        /// Event that happens when a timer gets cleaned-up
        /// The parameter is the event ID.
        /// 
        /// This event gets fired on the cleanup thread.
        /// Non-trivial operations should probably happen in a separate thread
        /// to prevent the cleanup thread from being blocked.
        /// </summary>
        public event Action<int> OnCleanup;

        /// <summary>
        /// Dictionary of the events.
        /// Key is the ID, value is the event info.
        /// </summary>
        private Dictionary<int, Timer> events;

        /// <summary>
        /// Whether or not this object is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// The next identifier.
        /// </summary>
        private int nextId;

        /// <summary>
        /// The next identifier lock.
        /// </summary>
        private object nextIdLock;

        /// <summary>
        /// Thread that cleans up single events (but not repeating).
        /// </summary>
        private Thread cleanupThread;

        /// <summary>
        /// Queue of event IDs that need to be cleaned up.
        /// </summary>
        private BlockingCollection<int> cleanupQueue;

        /// <summary>
        /// The number that will cause the cleanup thread to exit gracefully.
        /// </summary>
        private const int cleanupThreadExitId = -1;

        // -------- Constructor --------

        /// <summary>
        /// Constructor.
        /// </summary>
        public EventScheduler()
        {
            this.isDisposed = false;
            this.events = new Dictionary<int, Timer>();

            this.nextId = 1;
            this.nextIdLock = new object();

            this.cleanupQueue = new BlockingCollection<int>();
            this.cleanupThread = new Thread( this.CleanupThreadLoop );
            this.cleanupThread.Start();
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~EventScheduler()
        {
            try
            {
                this.Dispose( false );
            }
            catch( Exception )
            {
                // Swallow Exception, we don't want our GC thread dying.
            }
        }

        // --------- Properties --------

        /// <summary>
        /// Returns a Read-only list of of event IDs that are active.
        /// </summary>
        public IList<int> ActiveTimers
        {
            get
            {
                List<int> eventIdList;
                lock( this.events )
                {
                    eventIdList = new List<int>( this.events.Keys );
                }
                return eventIdList.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the next ID and increments the counter.
        /// </summary>
        private int NextId
        {
            get
            {
                lock( this.nextIdLock )
                {
                    return this.nextId++;
                }
            }
        }

        // -------- Functions --------

        /// <summary>
        /// Releases all resource used by the <see cref="SethCS.Basic.EventScheduler"/> object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Disposes this class.
        /// </summary>
        /// <param name="disposing">True if called from Dispose, false is called from Finalizer.</param>
        protected void Dispose( bool disposing )
        {
            if( this.isDisposed == false )
            {
                try
                {
                    this.cleanupQueue.Add( cleanupThreadExitId );
                    bool joined = this.cleanupThread.Join( 3000 );
                    if ( joined == false )
                    {
                        this.cleanupThread.Abort();
                        this.cleanupThread.Join( 3000 );
                    }

                    if( disposing )
                    {
                        // Remove managed code here.
                        this.cleanupQueue.Dispose();
                        foreach( Timer timer in this.events.Values )
                        {
                            timer.Dispose();
                        }
                    }

                    // Remove unmanaged code here.
                }
                finally
                {
                    this.isDisposed = true;
                }
            }
        }

        /// <summary>
        /// Schedules a recurring event to be run.
        /// </summary>
        /// <param name="delay">How long to wait until we fire the first event.  Set to TimerSpan.Zero to fire right away.</param>
        /// <param name="interval">The interval to fire the event at.</param>
        /// <param name="action">The action to perform after the delay.</param>
        /// <returns>The id of the event which can be used to stop it</returns>
        public int ScheduleRecurringEvent( TimeSpan delay, TimeSpan interval, Action action )
        {
            CheckDisposed();

            ArgumentChecker.IsNotNull( delay, nameof( delay ) );
            ArgumentChecker.IsNotNull( interval, nameof( interval ) );
            ArgumentChecker.IsNotNull( action, nameof( action ) );

            int id = this.NextId;

            TimerCallback timerAction = new TimerCallback( 
                delegate( object obj )
                {
                    action();
                }
            );

            Timer timer = new Timer( timerAction, null, delay, interval );

            lock( this.events )
            {
                this.events.Add( id, timer );
            }

            return id;
        }

        /// <summary>
        /// Schedules a single event
        /// </summary>
        /// <returns>The event.</returns>
        /// <param name="delay">How long to wait until we fire the first event.</param>
        /// <param name="action">The action to perform after the delay.</param>
        /// <returns>The id of the event which can be used to stop it</returns>
        public int ScheduleEvent( TimeSpan delay, Action action )
        {
            CheckDisposed();

            ArgumentChecker.IsNotNull( delay, nameof( delay ) );
            ArgumentChecker.IsNotNull( action, nameof( action ) );

            int id = this.NextId;

            TimerCallback timerAction = new TimerCallback( 
                delegate( object obj )
                {
                    action();

                    // This timer has completed its single instance.  Tell the cleanup thread
                    // to remove and dispose this timer.
                    this.cleanupQueue.Add( id );
                }
            );

            Timer timer = new Timer( timerAction, null, (int) delay.TotalMilliseconds, -1 );

            lock( this.events )
            {
                this.events.Add( id, timer );
            }

            return id;
        }

        /// <summary>
        /// Whether or not we are still running the given event id.
        /// </summary>
        /// <param name="id">ID of the event to stop.</param>
        /// <returns><c>true</c>, is still running, <c>false</c> otherwise.</returns>
        public bool ContainsEvent( int id )
        {
            CheckDisposed();

            lock( this.events )
            {
                return this.events.ContainsKey( id );
            }
        }

        /// <summary>
        /// Stops the event from running.
        /// No-Op if the event is not running.
        /// </summary>
        /// <param name="id">ID of the event to stop.</param>
        public void StopEvent( int id )
        {
            CheckDisposed();

            lock( this.events )
            {
                if( this.events.ContainsKey( id ) == false )
                {
                    // Make this a no-op.  We don't want a race condition where someone
                    // calls StopEvent(), but the cleanup thread already disposed of it.
                    return;
                }

                this.events[id].Dispose();
                this.events.Remove( id );
            }
        }

        private void CheckDisposed()
        {
            if( this.isDisposed )
            {
                throw new ObjectDisposedException( nameof( EventScheduler ) );
            }
        }

        private void CleanupThreadLoop()
        {
            int idToClean = this.cleanupQueue.Take();
            while( idToClean != cleanupThreadExitId )
            {
                lock( this.events )
                {
                    this.events[idToClean].Dispose();
                    this.events.Remove( idToClean );
                }

                this.OnCleanup?.Invoke( idToClean );

                idToClean = this.cleanupQueue.Take();
            }
        }
    }
}

