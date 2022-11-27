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
    public sealed class InterruptibleEventExecutorTest
    {
        // ---------------- Fields ----------------

        // ---------------- Setup / Teardown ----------------

        // ---------------- Tests ----------------

        /// <summary>
        /// Ensure if an event takes too long to execute,
        /// we interrupt it.
        /// </summary>
        [Test]
        public void InterruptTest()
        {
            EventClass longEvent = new EventClass();
            EventClass shortEvent = new EventClass();

            Exception err = null;

            using( InterruptibleEventExecutor uut = new InterruptibleEventExecutor( 5000 ) )
            {
                uut.OnError += delegate ( Exception e )
                {
                    err = e;
                };

                uut.Start();
                uut.AddEvent(
                    delegate ()
                    {
                        Thread.Sleep( 10000 );
                        longEvent.Execute();
                    }
                );

                // Ensure getting a TheadInterruptedException doesn't
                // napalm our event executor by ensuring a second event
                // will execute.
                uut.AddEvent(
                    delegate ()
                    {
                        shortEvent.Execute();
                    }
                );

                Assert.IsTrue( shortEvent.Join( 30 * 1000 ) );
                Assert.IsTrue( shortEvent.Executed );
                Assert.IsFalse( longEvent.Executed ); // Should have been interrupted.
            }

            Assert.IsNotNull( err );
            Assert.IsTrue( err is ThreadInterruptedException );
        }

        /// <summary>
        /// Ensures if we don't have interrupts turned on,
        /// we don't interrupt the thread.
        /// </summary>
        [Test]
        public void NoInterruptTest()
        {
            EventClass longEvent = new EventClass();
            EventClass shortEvent = new EventClass();

            Exception err = null;

            using( InterruptibleEventExecutor uut = new InterruptibleEventExecutor( int.MaxValue ) )
            {
                uut.OnError += delegate ( Exception e )
                {
                    err = e;
                };

                uut.Start();
                uut.AddEvent(
                    delegate ()
                    {
                        Thread.Sleep( 10000 );
                        longEvent.Execute();
                    }
                );

                uut.AddEvent(
                    delegate ()
                    {
                        shortEvent.Execute();
                    }
                );

                Assert.IsTrue( longEvent.Join( 30 * 1000 ) );
                Assert.IsTrue( longEvent.Executed );
                Assert.IsTrue( shortEvent.Join( 30 * 1000 ) );
                Assert.IsTrue( shortEvent.Executed );
            }

            Assert.IsNull( err );
        }

        /// <summary>
        /// Ensures that if no events have been added,
        /// we DON'T abort.
        /// </summary>
        [Test]
        public void NoEventsAddedTest()
        {
            Exception err = null;

            using( InterruptibleEventExecutor uut = new InterruptibleEventExecutor( 2000 ) )
            {
                uut.OnError += delegate ( Exception e )
                {
                    err = e;
                };

                uut.Start();

                Thread.Sleep( 6000 );
            }

            Assert.IsNull( err );
        }

        /// <summary>
        /// Tests to ensure 1000 events are executed successfully.
        /// </summary>
        [Test]
        public void ThousandEventTest()
        {
            using( InterruptibleEventExecutor executor = new InterruptibleEventExecutor( 5000 ) )
            {
                executor.Start();

                EventClass[] events = new EventClass[1000];
                for( int i = 0; i < 1000; ++i )
                {
                    EventClass ec = new EventClass();
                    events[i] = ec;
                    executor.AddEvent( () => ec.Execute() );
                }

                foreach( EventClass ec in events )
                {
                    // Wait for everything to complete before calling Asset.
                    Assert.IsTrue( ec.Join( 30 * 1000 ) );
                    Assert.IsTrue( ec.Executed );
                }
            }
        }
    }
}
