using System;
using NUnit.Framework;
using SethCS;
using System.Threading;

namespace Tests
{
    [TestFixture]
    public class RAIITimerTest
    {
        // -------- Fields --------

        /// <summary>
        /// Default timeout in ms.
        /// </summary>
        private const int defaultTimeout = 250;

        private bool triggered;

        private RAIITimer uut;

        // -------- Setup / Teardown --------

        [SetUp]
        public void TestSetup()
        {
            this.triggered = false;
        }

        [TearDown]
        public void TearDown()
        {
        }

        // -------- Tests --------

        /// <summary>
        /// Tests to make sure our Expired function fires
        /// when our time goes over.
        /// </summary>
        [Test]
        public void OnExpiredTest()
        {
            using( RAIITimer timer = new RAIITimer( defaultTimeout, this.Trigger, null ) )
            {
                // Sleep longer than our timer's tolerance.  We should get a trigger.
                Thread.Sleep( defaultTimeout * 2 );
            }

            Assert.IsTrue( triggered );
        }

        /// <summary>
        /// Tests to make sure our Not Expired function fires
        /// when our time DOES NOT go over.
        /// </summary>
        [Test]
        public void OnNotExpiredTest()
        {
            using( RAIITimer timer = new RAIITimer( defaultTimeout * 2, null, this.Trigger ) )
            {
                // Sleep shorter than our timer's tolerance.  We should get a trigger.
                Thread.Sleep( defaultTimeout );
            }

            Assert.IsTrue( triggered );
        }

        private void Trigger( long elaspedTime )
        {
            this.triggered = true;
        }
    }
}

