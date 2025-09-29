//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SethCS.Extensions;

namespace Tests.Extensions
{
    [TestClass]
    public sealed class FileInfoExtensionsTests
    {
        // ---------------- Tests ----------------

        [TestMethod]
        public void GetMimeTypeWithStringTest()
        {
            DoMimeTypeTest( @"c:\somewhere\file.html", "text/html" );
            DoMimeTypeTest( @"file.gif", "image/gif" );
            DoMimeTypeTest( @"/file/somewhere.ico", "image/vnd.microsoft.icon" );
            DoMimeTypeTest( @"/file/../somewhere.jar", "application/java-archive" );
            DoMimeTypeTest( @"./file/something.csv", "text/csv" );
            DoMimeTypeTest( @"../file/something.css", "text/css" );
            DoMimeTypeTest( "noextension", "application/octet-stream" );
            DoMimeTypeTest( "somewhere/noextension", "application/octet-stream" );
            DoMimeTypeTest( "/somewhere/noextension", "application/octet-stream" );
            DoMimeTypeTest( "../somewhere/noextension", "application/octet-stream" );
        }

        // ---------------- Test Helpers ----------------

        private static void DoMimeTypeTest( string path, string expectedMimeType )
        {
            string actual = path.GetMimeType();
            Assert.AreEqual( expectedMimeType, actual );
        }
    }
}
