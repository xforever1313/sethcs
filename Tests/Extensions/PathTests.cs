//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using NUnit.Framework;
using SethCS.Extensions;

namespace Tests.Extensions
{
    [TestFixture]
    public class PathTests
    {
        [Test]
        public void ToUriTests()
        {
            {
                string path = @"D:\Seth\source\Chaskis\Chaskis\CodeCoverage\index.htm";
                string expectedPath = @"file:///D:/Seth/source/Chaskis/Chaskis/CodeCoverage/index.htm";

                Assert.AreEqual( expectedPath, SethPath.ToUri( path ) );
            }

            {
                string path = "/home/seth/Downloads/canvas.png";
                string expectedPath = @"file:///home/seth/Downloads/canvas.png";

                Assert.AreEqual( expectedPath, SethPath.ToUri( path ) );
            }
        }
    }
}
