using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    ec.Join();
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
                    ec.Join();
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
                ec.Join();
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
            using ( EventExecutor executor = new EventExecutor( false, onFail ) )
            {
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

            using ( EventExecutor executor = new EventExecutor( true, onFail ) )
            {
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
            public void Execute()
            {
                this.Executed = true;
                this.resetEvent.Set();
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
