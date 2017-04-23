//
//          Copyright Seth Hendrick 2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using NUnit.Framework;
using SethCS.Extensions;

namespace Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void NormalizeWhiteSpaceTest()
        {
            string startString = "Hello\t  \nWorld, how\nare\tyou?I am fine!";
            string expectedString = "Hello_World,_how_are_you?I_am_fine!";

            string actualString = startString.NormalizeWhiteSpace( '_' );

            Assert.AreEqual( expectedString, actualString );
        }
    }
}
