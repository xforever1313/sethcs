//
//          Copyright Seth Hendrick 2016-2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SethCS.Basic;

namespace Tests.Basic
{
    [TestFixture]
    public class EventExecutorTest
    {
        // -------- Fields --------

        // -------- Setup / Teardown --------

        // -------- Tests --------

        /// <summary>
        /// Tests to ensure 10 events are executed successfully.
        /// </summary>
        [Test]
        public void TenEventTest()
        {
            // Set the run-on-dispose to false such that we don't get any false-positives
            // simply by having the executor executing events when the thread exits.
            using ( EventExecutor executor = new EventExecutor( false ) )
            {
                executor.Start();

                EventClass[] events = new EventClass[10];
                for ( int i = 0; i < 10; ++i )
                {
                    EventClass ec = new EventClass();
                    events[i] = ec;
                    executor.AddEvent( () => ec.Execute() );
                }

                foreach ( EventClass ec in events )
                {
                    // Wait for everything to complete before calling Asset.
                    Assert.IsTrue( ec.Join( 30 * 1000 ) );
                    Assert.IsTrue( ec.Executed );
                }
            }
        }

        /// <summary>
        /// Tests to ensure 1000 events are executed successfully.
        /// </summary>
        [Test]
        public void ThousandEventTest()
        {
            // Set the run-on-dispose to false such that we don't get any false-positives
            // simply by having the executor executing events when the thread exits.
            using ( EventExecutor executor = new EventExecutor( false ) )
            {
                executor.Start();

                EventClass[] events = new EventClass[1000];
                for ( int i = 0; i < 1000; ++i )
                {
                    EventClass ec = new EventClass();
                    events[i] = ec;
                    executor.AddEvent( () => ec.Execute() );
                }

                foreach ( EventClass ec in events )
                {
                    // Wait for everything to complete before calling Asset.
                    Assert.IsTrue( ec.Join( 30 * 1000 ) );
                    Assert.IsTrue( ec.Executed );
                }
            }
        }

        /// <summary>
        /// Tests to ensure 10 events are executed successfully after dispose is called
        /// when the option is enabled.
        /// </summary>
        [Test]
        public void TenEventAfterDisposeEnabledTest()
        {
            EventClass[] events = new EventClass[10];

            using ( EventExecutor executor = new EventExecutor( true ) )
            {
                // Don't call start, we want to make sure these events
                // are called during dispose.
                for ( int i = 0; i < 10; ++i )
                {
                    EventClass ec = new EventClass();
                    events[i] = ec;
                    executor.AddEvent( () => ec.Execute() );
                }
            }

            foreach ( EventClass ec in events )
            {
                // Wait for everything to complete before calling Asset.
                Assert.IsTrue( ec.Join( 30 * 1000 ) );
                Assert.IsTrue( ec.Executed );
            }
        }

        /// <summary>
        /// Tests to ensure 10 events NOT executed successfully after dispose is called
        /// when the option is disabled.
        /// </summary>
        [Test]
        public void TenEventAfterDisposeDisabledTest()
        {
            EventClass[] events = new EventClass[10];

            using ( EventExecutor executor = new EventExecutor( false ) )
            {
                // Don't call start, we want to make sure these events
                // aren't called during dispose.
                for ( int i = 0; i < 10; ++i )
                {
                    EventClass ec = new EventClass();
                    events[i] = ec;
                    executor.AddEvent( () => ec.Execute() );
                }
            }

            foreach ( EventClass ec in events )
            {
                Assert.IsFalse( ec.Executed );
            }
        }

        /// <summary>
        /// Tests to make sure unhandled exceptions in the executor behave correctly.
        /// </summary>
        [Test]
        public void TenEventUnhandledExceptionTest()
        {
            EventClass[] events = new EventClass[10];

            Action<Exception> onFail =
                delegate ( Exception e )
                {
                    EventException err = ( EventException ) e;
                    events[err.Index].Release();
                };

            // Set the run-on-dispose to false such that we don't get any false-positives
            // simply by having the executor executing events when the thread exits.
            using ( EventExecutor executor = new EventExecutor( false ) )
            {
                executor.OnError += onFail;
                executor.Start();
                for ( int i = 0; i < 10; ++i )
                {
                    EventClass ec = new EventClass();
                    EventException exception = new EventException( i );
                    events[i] = ec;
                    executor.AddEvent( () => ec.ExecuteAndThrow( exception ) );
                }

                foreach ( EventClass ec in events )
                {
                    Assert.IsTrue( ec.Join( 60 * 1000 ) ); // We should not hang if this works.
                    Assert.IsTrue( ec.Executed );
                }
            }
        }

        /// <summary>
        /// Tests to make sure unhandled exceptions in the executor behave correctly
        /// when dispose is called and execute on dispose is enabled.
        /// </summary>
        [Test]
        public void TenEventUnhandledExceptionTestWithDisposeEnabled()
        {
            EventClass[] events = new EventClass[10];

            Action<Exception> onFail =
                delegate ( Exception e )
                {
                    EventException err = ( EventException ) e;
                    events[err.Index].Release();
                };

            using ( EventExecutor executor = new EventExecutor( true ) )
            {
                executor.OnError += onFail;

                // Don't call start, we want to make sure these events
                // aren't called during dispose.
                for ( int i = 0; i < 10; ++i )
                {
                    EventClass ec = new EventClass();
                    EventException exception = new EventException( i );
                    events[i] = ec;
                    executor.AddEvent( () => ec.ExecuteAndThrow( exception ) );
                }
            }

            foreach ( EventClass ec in events )
            {
                Assert.IsTrue( ec.Join( 60 * 1000 ) ); // We should not hang if this works.
                Assert.IsTrue( ec.Executed );
            }
        }

        /// <summary>
        /// Tests to make sure our 
        /// </summary>
        [Test]
        public void AsyncWaitTest()
        {
            EventClass e1 = new EventClass();
            EventClass e2 = new EventClass();
            EventClass e3 = new EventClass();
            EventClass syncClass = new EventClass();

            EventClass[] events = new EventClass[4] { e1, e2, e3, syncClass };

            List<EventClass> completedEvents = new List<EventClass>();

            using( EventExecutor executor = new EventExecutor( false ) )
            {
                executor.Start();
                executor.AddEvent(
                    async delegate ()
                    {
                        Assert.AreEqual( EventExecutor.ThreadName, Thread.CurrentThread.Name );
                        await e1.AsyncWaitAndExecute(
                            2 * 1000,
                            delegate()
                            {
                                lock( completedEvents )
                                {
                                    completedEvents.Add( e1 );
                                }
                            }
                        );
                        Assert.AreEqual( EventExecutor.ThreadName, Thread.CurrentThread.Name );

                    }
                );

                executor.AddEvent(
                    async delegate ()
                    {
                        Assert.AreEqual( EventExecutor.ThreadName, Thread.CurrentThread.Name );
                        await e2.AsyncWaitAndExecute(
                            3 * 1000,
                            delegate()
                            {
                                lock( completedEvents )
                                {
                                    completedEvents.Add( e2 );
                                }
                            }
                        );
                        Assert.AreEqual( EventExecutor.ThreadName, Thread.CurrentThread.Name );
                    }
                );


                executor.AddEvent(
                    async delegate ()
                    {
                        Assert.AreEqual( EventExecutor.ThreadName, Thread.CurrentThread.Name );
                        await e3.AsyncWaitAndExecute(
                            4 * 1000,
                            delegate()
                            {
                                lock( completedEvents )
                                {
                                    completedEvents.Add( e3 );
                                }
                            }
                        );
                        Assert.AreEqual( EventExecutor.ThreadName, Thread.CurrentThread.Name );
                    }
                );

                executor.AddEvent(
                    delegate ()
                    {
                        Assert.AreEqual( EventExecutor.ThreadName, Thread.CurrentThread.Name );
                        syncClass.WaitAndExecute(
                            10 * 1000,
                            delegate ()
                            {
                                lock( completedEvents )
                                {
                                    completedEvents.Add( syncClass );
                                }
                            }
                        );
                        Assert.AreEqual( EventExecutor.ThreadName, Thread.CurrentThread.Name );
                    }
                );

                foreach( EventClass ec in events )
                {
                    Assert.IsTrue( ec.Join( 30 * 1000 ) );
                    Assert.IsTrue( ec.Executed );
                }
            }

            Assert.AreEqual( 4, completedEvents.Count );
        }

        /// <summary>
        /// Ensures our async/await functions complete when we are disposing.
        /// </summary>
        [Test]
        public void AsyncAwaitDisposeTest()
        {
            EventClass e1 = new EventClass();
            EventClass e2 = new EventClass();
            EventClass e3 = new EventClass();
            EventClass syncClass = new EventClass();

            EventClass[] events = new EventClass[4] { e1, e2, e3, syncClass };

            List<EventClass> completedEvents = new List<EventClass>();

            using( EventExecutor executor = new EventExecutor( true ) )
            {
                executor.Start();
                executor.AddEvent(
                    async delegate ()
                    {
                        // May or not execute in the EventExecutor thread, depending on when it finishes;
                        // could finish in this thread with Dispose.
                        await e1.AsyncWaitAndExecute(
                            2 * 1000,
                            delegate ()
                            {
                                lock( completedEvents )
                                {
                                    completedEvents.Add( e1 );
                                }
                            }
                        );
                    }
                );

                executor.AddEvent(
                    async delegate ()
                    {
                        // May or not execute in the EventExecutor thread, depending on when it finishes;
                        // could finish in this thread with Dispose.
                        await e2.AsyncWaitAndExecute(
                            3 * 1000,
                            delegate ()
                            {
                                lock( completedEvents )
                                {
                                    completedEvents.Add( e2 );
                                }
                            }
                        );
                    }
                );


                executor.AddEvent(
                    async delegate ()
                    {
                        // May or not execute in the EventExecutor thread, depending on when it finishes;
                        // could finish in this thread with Dispose.
                        await e3.AsyncWaitAndExecute(
                            4 * 1000,
                            delegate ()
                            {
                                lock( completedEvents )
                                {
                                    completedEvents.Add( e3 );
                                }
                            }
                        );
                    }
                );

                executor.AddEvent(
                    delegate ()
                    {
                        // May or not execute in the EventExecutor thread, depending on when it finishes;
                        // could finish in this thread with Dispose.
                        syncClass.WaitAndExecute(
                            10 * 1000,
                            delegate ()
                            {
                                lock( completedEvents )
                                {
                                    completedEvents.Add( syncClass );
                                }
                            }
                        );
                    }
                );
            }

            foreach( EventClass ec in events )
            {
                Assert.IsTrue( ec.Join( 30 * 1000 ) );
                Assert.IsTrue( ec.Executed );
            }

            Assert.AreEqual( 4, completedEvents.Count );
        }

        // -------- Helper Classes --------

        /// <summary>
        /// Event that is executed on the event executor
        /// </summary>
        private class EventClass
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
                    lock ( this.executedLock )
                    {
                        return this.executed;
                    }
                }
                private set
                {
                    lock ( this.executedLock )
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
        private class EventException : Exception
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
}
