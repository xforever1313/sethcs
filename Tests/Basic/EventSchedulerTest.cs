
//          Copyright Seth Hendrick 2015.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using SethCS.Basic;

namespace Tests
{
    [TestFixture]
    public class EventSchedulerTest
    {
        // -------- Fields --------

        /// <summary>
        /// Default timeout in ms.
        /// </summary>
        private const int defaultTimeout = 500;

        /// <summary>
        /// Unit under test.
        /// </summary>
        private EventScheduler uut;

        /// <summary>
        /// IDs that were cleaned up.
        /// </summary>
        private BlockingCollection<int> cleanedUpQueue;

        // -------- Setup / Teardown --------

        [SetUp]
        public void TestSetup()
        {
            this.cleanedUpQueue = new BlockingCollection<int>();
            this.uut = new EventScheduler();
            this.uut.OnCleanup += delegate( int id )
            {
                this.cleanedUpQueue.Add( id );
            };
        }

        [TearDown]
        public void TearDown()
        {
            this.uut?.Dispose();
        }

        // -------- Tests --------

        /// <summary>
        /// Tests with just one signle recurring event with no delay.
        /// </summary>
        [Test]
        public void SingleNonRecurringEventTestWithNoDelay()
        {
            TimerEvent event1 = new TimerEvent();

            this.uut.ScheduleEvent( TimeSpan.Zero, event1.Post );

            // Event should fire instantly.
            Assert.IsTrue( event1.TryWait( defaultTimeout ) );

            // Wait for our ID to get cleaned up.
            int id;
            Assert.IsTrue( this.cleanedUpQueue.TryTake( out id, defaultTimeout ) );

            // The event we cleaned up is id 1.
            Assert.AreEqual( 1, id );

            // Ensure there are no timers left.
            Assert.AreEqual( 0, this.uut.ActiveTimers.Count );
        }

        /// <summary>
        /// Tests with just one signle recurring event *with* delay.
        /// </summary>
        [Test]
        public void SingleNonRecurringEventTestWithDelay()
        {
            TimerEvent event1 = new TimerEvent();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            this.uut.ScheduleEvent( new TimeSpan( 0, 0, 0, 0, defaultTimeout ), event1.Post );

            Assert.IsTrue( event1.TryWait( defaultTimeout * 2 ) );
            stopWatch.Stop();

            // The event should not have been fired at least until our delay happened.
            Assert.GreaterOrEqual( stopWatch.ElapsedMilliseconds, defaultTimeout );

            // The event should have been fired waaaayyy before double our timeout.
            Assert.LessOrEqual( stopWatch.ElapsedMilliseconds, defaultTimeout * 2 );

            // Wait for our ID to get cleaned up.
            int id;
            Assert.IsTrue( this.cleanedUpQueue.TryTake( out id, defaultTimeout ) );

            // The event we cleaned up is id 1.
            Assert.AreEqual( 1, id );

            // Ensure there are no timers left.
            Assert.AreEqual( 0, this.uut.ActiveTimers.Count );
        }

        // -------- Helper Classes --------

        private class TimerEvent
        {
            // -------- Fields --------

            /// <summary>
            /// Event that will get triggered when the timer
            /// expires.
            /// </summary>
            private AutoResetEvent waitEvent;

            // -------- Constructor --------

            /// <summary>
            /// Constructor.
            /// </summary>
            public TimerEvent()
            {
                this.waitEvent = new AutoResetEvent( false );
            }
             
            // -------- Functions --------

            /// <summary>
            /// Waits on the timeout for the specified time.
            /// Returns true if the event gets triggered BEFORE
            /// the timeout, else false.
            /// </summary>
            /// <returns>True if event gets triggered before the timeout, else false.</returns>
            public bool TryWait( int timeout )
            {
                return this.waitEvent.WaitOne( timeout );
            }

            /// <summary>
            /// Post event.
            /// </summary>
            public void Post()
            {
                this.waitEvent.Set();
            }
        }
    }
}

