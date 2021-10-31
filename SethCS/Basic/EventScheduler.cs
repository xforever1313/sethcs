//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
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
        // ---------------- Fields ----------------

        /// <summary>
        /// Dictionary of the events.
        /// Key is the ID, value is the event info.
        /// </summary>
        private readonly Dictionary<int, Timer> events;

        private bool isDisposed;

        private int nextId;
        private readonly object nextIdLock;

        // ---------------- Constructor ----------------

        public EventScheduler()
        {
            this.isDisposed = false;
            this.events = new Dictionary<int, Timer>();

            this.nextId = 1;
            this.nextIdLock = new object();
        }

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

        // ---------------- Properties ----------------

        /// <summary>
        /// Returns a Read-only list of of event IDs that are active.
        /// </summary>
        public IEnumerable<int> ActiveEventIds
        {
            get
            {
                CheckDisposed();
                return this.events.Keys;
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

        // ---------------- Functions ----------------

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
            if( this.isDisposed )
            {
                return;
            }

            if( disposing )
            {
                // Remove unmanaged code here
                foreach( Timer timer in this.events.Values )
                {
                    timer.Dispose();
                }
            }

            this.isDisposed = true;
        }

        /// <summary>
        /// Schedules a recurring event to be run.
        /// </summary>
        /// <param name="interval">The interval to fire the event at.</param>
        /// <param name="action">The action to perform after the delay.</param>
        /// <param name="startRightAway">
        /// If set to false, the event will not start executing until <see cref="StartEvent(int)"/> is called.
        /// </param>
        /// <returns>The id of the event which can be used to start or stop it</returns>
        public int ScheduleRecurringEvent( TimeSpan interval, Action action, bool startRightAway = true )
        {
            CheckDisposed();

            ArgumentChecker.IsNotNull( interval, nameof( interval ) );
            ArgumentChecker.IsNotNull( action, nameof( action ) );

            int id = this.NextId;

            void Timer_Elapsed( object sender, ElapsedEventArgs e )
            {
                action();
            }

            var timer = new Timer( interval.TotalMilliseconds );
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;

            if( startRightAway )
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }

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
        /// Enables the given event.
        /// </summary>
        /// <exception cref="ArgumentException">If the event does not exist.</exception>
        /// <param name="id">ID of the event to stop.</param>
        public void StartEvent( int id )
        {
            CheckDisposed();

            lock( this.events )
            {
                if( this.events.ContainsKey( id ) == false )
                {
                    throw new ArgumentException(
                        $"Event id {id} does not exist",
                        nameof( id )
                    );
                }

                this.events[id].Start();
            }
        }

        /// <summary>
        /// Stops the event from running.
        /// </summary>
        /// <exception cref="ArgumentException">If the event does not exist.</exception>
        /// <param name="id">ID of the event to stop.</param>
        public void StopEvent( int id )
        {
            CheckDisposed();

            lock( this.events )
            {
                if( this.events.ContainsKey( id ) == false )
                {
                    throw new ArgumentException(
                        $"Event id {id} does not exist",
                        nameof( id )
                    );
                }

                this.events[id].Stop();
            }
        }

        public void DisposeEvent( int id )
        {
            lock( this.events )
            {
                if( this.events.ContainsKey( id ) == false )
                {
                    // Make this a no-op.  They want it disposed anyways.
                    return;
                }

                this.events[id].Stop();
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
    }
}
