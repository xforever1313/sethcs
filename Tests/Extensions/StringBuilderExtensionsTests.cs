//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SethCS.Extensions;

namespace Tests.Extensions
{
    [TestClass]
    public sealed class StringBuilderExtensionsTests
    {
        [TestMethod]
        public void EmptyBuilderTest()
        {
            // Setup
            var uut = new StringBuilder();

            // Act
            StringBuilder returnedBuilder = uut.RemoveLastCharacter();

            // Check
            Assert.AreEqual( "", uut.ToString() );
            Assert.AreSame( uut, returnedBuilder );
        }

        [TestMethod]
        public void OneCharacterLongTest()
        {
            // Setup
            var uut = new StringBuilder();
            uut.Append( "a" );

            // Act
            StringBuilder returnedBuilder = uut.RemoveLastCharacter();

            // Check
            Assert.AreEqual( "", uut.ToString() );
            Assert.AreSame( uut, returnedBuilder );
        }

        [TestMethod]
        public void TwoCharactersLongTest()
        {
            // Setup
            var uut = new StringBuilder();
            uut.Append( "a," );

            // Act
            StringBuilder returnedBuilder = uut.RemoveLastCharacter();

            // Check
            Assert.AreEqual( "a", uut.ToString() );
            Assert.AreSame( uut, returnedBuilder );
        }

        [TestMethod]
        public void ThreeCharactersLongTest()
        {
            // Setup
            var uut = new StringBuilder();
            uut.Append( "ab," );

            // Act
            StringBuilder returnedBuilder = uut.RemoveLastCharacter();

            // Check
            Assert.AreEqual( "ab", uut.ToString() );
            Assert.AreSame( uut, returnedBuilder );
        }
    }
}
