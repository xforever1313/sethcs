//
//          Copyright Seth Hendrick 2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using SethCS.IO;

namespace Tests.IO
{
    [TestFixture]
    public class HttpGetterTest
    {
        // ---------------- Fields ----------------

        // ---------------- Setup / Teardown ----------------

        [OneTimeSetUp]
        public void FixtureSetup()
        {
        }

        [OneTimeTearDown]
        public void FixtureTeardown()
        {
        }

        [SetUp]
        public void TestSetup()
        {
        }

        [TearDown]
        public void TestTeardown()
        {
        }

        // ---------------- Tests ----------------

        [Test]
        public void GetTest()
        {
            string url = "https://files.shendrick.net/projects/sethcs/tests/test.txt";

            string str = HttpGetter.DownloadString( url ).TrimEnd();
            Assert.AreEqual( "Hello, World!", str );
        }

        /// <summary>
        /// Ensures if we get a binary, we don't crash too badly.
        /// </summary>
        [Test]
        public void GetBinaryTest()
        {
            string url = "https://files.shendrick.net/projects/sethcs/tests/test.png";

            Assert.DoesNotThrow( () => HttpGetter.DownloadString( url ) );
        }

        [Test]
        public void GetAsyncTest()
        {
            string url = "https://files.shendrick.net/projects/sethcs/tests/test.txt";

            Task<string> strTask = HttpGetter.AsyncDownloadString( url );
            strTask.Wait();

            string str = strTask.Result.TrimEnd();
            Assert.AreEqual( "Hello, World!", str );
        }

        [Test]
        public void Get404Test()
        {
            string url = "https://files.shendrick.net/projects/sethcs/dne.txt";

            WebException e = Assert.Throws<WebException>( () => HttpGetter.DownloadString( url ) );
            Assert.IsTrue( e.Message.Contains( Convert.ToInt32( HttpStatusCode.NotFound ).ToString() ) );
        }

        [Test]
        public void TimeoutTest()
        {
            string url = "http://127.0.0.0:13131"; // Hopefully this isn't in use...

            WebException e = Assert.Throws<WebException>( () => HttpGetter.DownloadString( url, timeout:10 ) );
            Assert.IsTrue( e.Message.Contains( "timed out" ) );
        }

        // ---------------- Test Helpers ----------------
    }
}
