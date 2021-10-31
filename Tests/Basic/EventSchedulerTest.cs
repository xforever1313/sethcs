//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using SethCS.Basic;

namespace Tests.Basic
{
    [TestFixture]
    public sealed class EventSchedulerTest
    {
        // ---------------- Fields ----------------

        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds( 2 );

        private EventScheduler uut;

        // ---------------- Setup / Teardown ----------------

        [SetUp]
        public void TestSetup()
        {
            this.uut = new EventScheduler();
        }

        [TearDown]
        public void TearDown()
        {
            this.uut?.Dispose();
        }

        // ---------------- Tests ----------------

        [Test]
        public void DisposeThrowsExceptionsTest()
        {
            this.uut.Dispose();

            Assert.Throws<ObjectDisposedException>( () => this.uut.ActiveEventIds.ToList() );
            Assert.Throws<ObjectDisposedException>( () => this.uut.ContainsEvent( 1 ) );
            Assert.Throws<ObjectDisposedException>( () => this.uut.StartEvent( 1 ) );
            Assert.Throws<ObjectDisposedException>( () => this.uut.StopEvent( 1 ) );
            Assert.Throws<ObjectDisposedException>( () => this.uut.ScheduleRecurringEvent( defaultTimeout, null ) );
        }

        [Test]
        public void IdDoesntExistTestTest()
        {
            const int fakeId = 1;

            Assert.IsFalse( this.uut.ContainsEvent( fakeId ) );
            Assert.Throws<ArgumentException>( () => this.uut.StartEvent( fakeId ) );
            Assert.Throws<ArgumentException>( () => this.uut.StopEvent( fakeId ) );
            Assert.DoesNotThrow( () => this.uut.DisposeEvent( fakeId ) ); // <- No-op if it does not exist.
        }

        /// <summary>
        /// Tests with just one signle recurring event delay that is
        /// not started right-away.
        /// </summary>
        [Test]
        public void SingleRecurringEventDontStartAtFirstTest()
        {
            TimerEvent event1 = new TimerEvent();

            // Should not do anything if not started.
            int id = this.uut.ScheduleRecurringEvent( defaultTimeout, event1.Post, false );
            Assert.IsFalse( event1.TryWait( defaultTimeout * 2 ) );

            // Now start it.
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            this.uut.StartEvent( id );

            Assert.IsTrue( event1.TryWait( defaultTimeout * 2 ) );
            stopWatch.Stop();
            this.uut.StopEvent( id );

            // The event should not have been fired at least until our delay happened.
            Assert.GreaterOrEqual( stopWatch.Elapsed, defaultTimeout );

            // The event should have been fired waaaayyy before double our timeout.
            Assert.LessOrEqual( stopWatch.Elapsed, defaultTimeout * 2 );

            // Now that we've stopped, we should return false.
            Assert.IsFalse( event1.TryWait( defaultTimeout * 2 ) );

            // Dispose.  Should remove timer.
            Assert.IsTrue( uut.ContainsEvent( id ) );
            this.uut.DisposeEvent( id );
            Assert.AreEqual( 0, this.uut.ActiveEventIds.Count() );
        }

        /// <summary>
        /// Tests with just one signle recurring event delay that is
        /// not started right-away.
        /// </summary>
        [Test]
        public void SingleRecurringEventStartRightAway()
        {
            TimerEvent event1 = new TimerEvent();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int id = this.uut.ScheduleRecurringEvent( defaultTimeout, event1.Post, true );
            Assert.IsTrue( event1.TryWait( defaultTimeout * 2 ) );
            TimeSpan timeStamp1 = stopWatch.Elapsed;

            Assert.IsTrue( event1.TryWait( defaultTimeout * 2 ) );
            TimeSpan timeStamp2 = stopWatch.Elapsed;

            stopWatch.Stop();
            this.uut.StopEvent( id );

            // The first event should not have been fired until first interval.
            Assert.GreaterOrEqual( timeStamp1, defaultTimeout );
            Assert.LessOrEqual( timeStamp1, defaultTimeout * 2 );

            // The second event should not have been fired until first interval.
            Assert.GreaterOrEqual( timeStamp2, defaultTimeout * 2 );
            Assert.LessOrEqual( timeStamp2, defaultTimeout * 3 );
        }

        // ---------------- Helper Classes ----------------

        private sealed class TimerEvent
        {
            // ---------------- Fields ----------------

            /// <summary>
            /// Event that will get triggered when the timer
            /// expires.
            /// </summary>
            private readonly AutoResetEvent waitEvent;

            // ---------------- Constructor ----------------

            /// <summary>
            /// Constructor.
            /// </summary>
            public TimerEvent()
            {
                this.waitEvent = new AutoResetEvent( false );
            }

            // ---------------- Functions ----------------

            /// <summary>
            /// Waits on the timeout for the specified time.
            /// Returns true if the event gets triggered BEFORE
            /// the timeout, else false.
            /// </summary>
            /// <returns>True if event gets triggered before the timeout, else false.</returns>
            public bool TryWait( TimeSpan timeout )
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

