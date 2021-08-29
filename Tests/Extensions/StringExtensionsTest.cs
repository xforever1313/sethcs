//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using NUnit.Framework;
using SethCS.Extensions;

namespace Tests.Extensions
{
    [TestFixture]
    public sealed class StringExtensionsTest
    {
        // ---------------- NormalizeWhiteSpace Tests ----------------

        [Test]
        public void NormalizeWhiteSpaceTest()
        {
            string startString = "Hello\t  \nWorld, how\nare\tyou?I am fine!";
            string expectedString = "Hello_World,_how_are_you?I_am_fine!";

            string actualString = startString.NormalizeWhiteSpace( '_' );

            Assert.AreEqual( expectedString, actualString );
        }

        // ---------------- SplitByLength Tests ----------------

        /// <summary>
        /// Ensures we get argument exceptions if we get a length of 0 or 1.
        /// </summary>
        [Test]
        public void SplitByLength_ArgumentCheck()
        {
            string str = "12345";
            Assert.Throws<ArgumentException>( () => str.SplitByLength( 0 ) );
            Assert.Throws<ArgumentException>( () => str.SplitByLength( -1 ) );
        }

        /// <summary>
        /// Ensures that if we are an empty string, we get an array that is empty.
        /// </summary>
        [Test]
        public void SplitByLength_EmptyString()
        {
            string str = string.Empty;

            string[] array = str.SplitByLength( 1 );

            Assert.AreEqual( 1, array.Length );
            Assert.AreEqual( array[0], string.Empty );
        }

        /// <summary>
        /// Ensures if our string is one character, we'll only get that character
        /// back in the array.
        /// </summary>
        [Test]
        public void SplitByLength_OneCharacterString()
        {
            string str = "1";

            string[] array = str.SplitByLength( 10);
            Assert.AreEqual( 1, array.Length );
            Assert.AreEqual( array[0], str );
        }

        /// <summary>
        /// Ensures we get an array of 1 if our size is one less than the length.
        /// </summary>
        [Test]
        public void SplitByLength_OneLessThanLength()
        {
            string str = "12345";

            string[] array = str.SplitByLength( str.Length + 1 );
            Assert.AreEqual( 1, array.Length );
            Assert.AreEqual( array[0], str );
        }

        /// <summary>
        /// Ensures we get an array of 1 if our size is equal to the length.
        /// </summary>
        [Test]    
        public void SplitByLength_ExactLength()
        {
            string str = "12345";

            string[] array = str.SplitByLength( str.Length );
            Assert.AreEqual( 1, array.Length );
            Assert.AreEqual( str, array[0] );
        }

        /// <summary>
        /// Ensures we get an array of 5 if our length is 1
        /// </summary>
        [Test]
        public void SplitByLength_1CharacterLength()
        {
            string str = "12345";

            string[] array = str.SplitByLength( 1 );
            Assert.AreEqual( 5, array.Length );

            for( int i = 1; i <= 5; ++i )
            {
                Assert.AreEqual( "" + i, array[i - 1] );
            }
        }

        /// <summary>
        /// Ensures we get an array of 3 if our length is 2
        /// </summary>
        [Test]
        public void SplitByLength_2CharacterLength_OddLength()
        {
            string str = "12345";

            string[] array = str.SplitByLength( 2 );
            Assert.AreEqual( 3, array.Length );

            Assert.AreEqual( "12", array[0] );
            Assert.AreEqual( "34", array[1] );
            Assert.AreEqual( "5", array[2] );
        }

        /// <summary>
        /// Ensures we get an array of 2 if our length is 2
        /// </summary>
        [Test]
        public void SplitByLength_2CharacterLength_EvenLength()
        {
            string str = "1234";

            string[] array = str.SplitByLength( 2 );
            Assert.AreEqual( 2, array.Length );

            Assert.AreEqual( "12", array[0] );
            Assert.AreEqual( "34", array[1] );
        }

        /// <summary>
        /// Ensures things are correct if our length is 3.
        /// </summary>
        [Test]
        public void SplitByLength_3CharacterLength_10Characters()
        {
            string str = "1234567890";

            string[] array = str.SplitByLength( 3 );
            Assert.AreEqual( 4, array.Length );

            Assert.AreEqual( "123", array[0] );
            Assert.AreEqual( "456", array[1] );
            Assert.AreEqual( "789", array[2] );
            Assert.AreEqual( "0", array[3] );
        }

        /// <summary>
        /// Ensures things are correct if our length is 3.
        /// </summary>
        [Test]
        public void SplitByLength_3CharacterLength_9Characters()
        {
            string str = "123456789";

            string[] array = str.SplitByLength( 3 );
            Assert.AreEqual( 3, array.Length );

            Assert.AreEqual( "123", array[0] );
            Assert.AreEqual( "456", array[1] );
            Assert.AreEqual( "789", array[2] );
        }

        /// <summary>
        /// Ensures things are correct if our length is 3.
        /// </summary>
        [Test]
        public void SplitByLength_3CharacterLength_11Characters()
        {
            string str = "12345678901";

            string[] array = str.SplitByLength( 3 );
            Assert.AreEqual( 4, array.Length );

            Assert.AreEqual( "123", array[0] );
            Assert.AreEqual( "456", array[1] );
            Assert.AreEqual( "789", array[2] );
            Assert.AreEqual( "01", array[3] );
        }

        /// <summary>
        /// Ensures we get an array of 2 if our size is one more than the length.
        /// </summary>
        [Test]
        public void SplitByLength_OneMoreThanTheLength()
        {
            string str = "12345";

            string[] array = str.SplitByLength( str.Length - 1 );
            Assert.AreEqual( 2, array.Length );
            Assert.AreEqual( "1234", array[0] );
            Assert.AreEqual( "5", array[1] );
        }

        [Test]
        public void ToSnakeCaseTest()
        {
            // Setup
            const string original = "Hello world  HOW are You";
            const string expected = "hello_world_how_are_you";

            // Act
            string actual = original.ToSnakeCase();

            // Check
            Assert.AreEqual( expected, actual );
        }

        [Test]
        public void ToMacroCaseTest()
        {
            // Setup
            const string original = "Hello world  HOW are You";
            const string expected = "HELLO_WORLD_HOW_ARE_YOU";

            // Act
            string actual = original.ToMacroCase();

            // Check
            Assert.AreEqual( expected, actual );
        }

        [Test]
        public void ToPascalCaseTest()
        {
            // Setup
            const string original = "Hello world  HOW are You";
            const string expected = "HelloWorldHowAreYou";

            // Act
            string actual = original.ToPascalCase();

            // Check
            Assert.AreEqual( expected, actual );
        }

        [Test]
        public void ToCamelCaseTest()
        {
            // Setup
            const string original = "Hello world  HOW are You";
            const string expected = "helloWorldHowAreYou";

            // Act
            string actual = original.ToCamelCase();

            // Check
            Assert.AreEqual( expected, actual );
        }

        [Test]
        public void ToLowerKebabCaseTest()
        {
            // Setup
            const string original = "Hello world  HOW are You";
            const string expected = "hello-world-how-are-you";

            // Act
            string actual = original.ToLowerKebabCase();

            // Check
            Assert.AreEqual( expected, actual );
        }

        [Test]
        public void ToUpperKebabCaseTest()
        {
            // Setup
            const string original = "Hello world  HOW are You";
            const string expected = "HELLO-WORLD-HOW-ARE-YOU";

            // Act
            string actual = original.ToUpperKebabCase();

            // Check
            Assert.AreEqual( expected, actual );
        }
    }
}
