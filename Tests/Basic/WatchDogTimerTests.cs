//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Threading;
using NUnit.Framework;
using SethCS.Basic;

namespace Tests.Basic
{
    [TestFixture]
    public sealed class WatchDogTimerTests
    {
        // ---------------- Tests ----------------

        [Test]
        public void InvalidArgumentTest()
        {
            Assert.Throws<ArgumentException>( () => new WatchdogTimer( -1, string.Empty ) );
            Assert.Throws<ArgumentNullException>( () => new WatchdogTimer( 1000, null ) );
        }

        /// <summary>
        /// Ensures that we do things out-of-order we get Exceptions.
        /// </summary>
        [Test]
        public void InvalidOperationTests()
        {
            WatchdogTimer uut = new WatchdogTimer( int.MaxValue, "InvalidOperationTests" );

            bool started = false;
            bool stopped = false;
            bool reset = false;

            uut.OnStarted += () => { started = true; };
            uut.OnStopped += () => { stopped = true; };
            uut.OnReset += () => { reset = true; };

            Assert.Throws<InvalidOperationException>( () => uut.Stop() ); // Can't stop if its not started.
            Assert.Throws<InvalidOperationException>( () => uut.Reset() );

            // Ensure events didn't fire.
            Assert.IsFalse( started );
            Assert.IsFalse( stopped );
            Assert.IsFalse( reset );

            // Call Start
            uut.Start();
            Assert.IsTrue( started );

            started = false;
            // Calling start again should get an InvalidOperationException
            Assert.Throws<InvalidOperationException>( () => uut.Start() );
            Assert.IsFalse( started );
            Assert.IsFalse( stopped );
            Assert.IsFalse( reset );

            uut.Reset();
            Assert.IsFalse( started );
            Assert.IsFalse( stopped );
            Assert.IsTrue( reset );
            reset = false;

            uut.Stop();
            Assert.IsFalse( started );
            Assert.IsTrue( stopped );
            Assert.IsFalse( reset );
            stopped = false;

            uut.Dispose();

            // Should get ObjectDisposedExceptions
            Assert.Throws<ObjectDisposedException>( () => uut.Start() );
            Assert.Throws<ObjectDisposedException>( () => uut.Stop() );
            Assert.Throws<ObjectDisposedException>( () => uut.Reset() );

            Assert.IsFalse( started );
            Assert.IsFalse( stopped );
            Assert.IsFalse( reset );

            // Nothing bad should happen if we call Dispose again
            uut.Dispose();
        }

        /// <summary>
        /// Do we timeout properly?
        /// </summary>
        [Test]
        public void TimeoutEvent()
        {
            using( WatchdogTimer uut = new WatchdogTimer( 1000, "TimeoutEvent" ) )
            {
                AutoResetEvent resetEvent = new AutoResetEvent( false );

                uut.OnTimeoutExpired += delegate ()
                {
                    resetEvent.Set();
                };

                uut.Start();

                Assert.IsTrue( resetEvent.WaitOne( 5000 ) );
                Assert.IsTrue( resetEvent.WaitOne( 5000 ) );
                Assert.IsTrue( resetEvent.WaitOne( 5000 ) );
            }
        }

        /// <summary>
        /// Ensures we don't fire an event if we call "Stop".
        /// </summary>
        [Test]
        public void StopEvent()
        {
            using( WatchdogTimer uut = new WatchdogTimer( 500, "StopEvent" ) )
            {
                AutoResetEvent resetEvent = new AutoResetEvent( false );

                uut.OnTimeoutExpired += delegate ()
                {
                    resetEvent.Set();

                    // By forcing Stop() to be called on itself, we can guarentee that
                    // we won't hit a racecondition anywhere, as we are stopping it on the timer thread.
                    uut.Stop();
                };

                uut.Start();

                Assert.IsTrue( resetEvent.WaitOne( 2000 ) );
                Assert.IsFalse( resetEvent.WaitOne( 3000 ) );

                uut.Start();

                Assert.IsTrue( resetEvent.WaitOne( 2000 ) );
                Assert.IsFalse( resetEvent.WaitOne( 3000 ) );
            }
        }

        /// <summary>
        /// Do we properly handle exceptions from TimeoutEvents?
        /// </summary>
        [Test]
        public void ExceptionThrownEvent()
        {
            using( WatchdogTimer uut = new WatchdogTimer( 500, "ExceptionThrownEvent" ) )
            {
                ManualResetEvent resetEvent = new ManualResetEvent( false );

                Exception err = new Exception( "My Exception" );

                uut.OnTimeoutExpired += delegate ()
                {
                    throw err;
                };

                Exception foundException = null;
                uut.OnTimeoutExpiredError += delegate ( Exception e )
                {
                    foundException = e;
                    resetEvent.Set();
                };

                uut.Start();

                Assert.IsTrue( resetEvent.WaitOne( 3000 ) );
                Assert.AreSame( err, foundException );
            }
        }

        /// <summary>
        /// Ensures the Reset() function resets the timer.
        /// </summary>
        [Test]
        public void ResetTest()
        {
            using( WatchdogTimer uut = new WatchdogTimer( 3000, "ResetTest" ) )
            {
                bool resetCalled = false;

                Exception err = new Exception( "My Exception" );

                uut.OnTimeoutExpired += delegate ()
                {
                    resetCalled = true;
                };

                uut.Start();
                // Calling Reset every half second should
                // prevent the watchdog from firing, which is set to
                // expire after 3 seconds.
                for( int i = 0; i < 12; ++i )
                {
                    uut.Reset();
                    Thread.Sleep( 500 );
                }
                uut.Stop();

                Assert.IsFalse( resetCalled );
            }
        }

        /// <summary>
        /// Ensures calling start and dispose won't cause any problems.
        /// </summary>
        [Test]
        public void StartDisposeTest()
        {
            WatchdogTimer uut = new WatchdogTimer( 1000, "StartDisposeTest" );
            uut.Dispose();
        }
    }
}
