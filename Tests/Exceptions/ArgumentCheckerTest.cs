
//          Copyright Seth Hendrick 2016.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;
using NUnit.Framework;
using SethCS.Exceptions;

namespace Tests.Exceptions
{
    [TestFixture]
    public class ArgumentCheckerTest
    {
        // -------- Fields --------

        /// <summary>
        /// The arg name to pass into the exception.
        /// </summary>
        private const string argName = "arg";

        // -------- Tests --------

        /// <summary>
        /// Ensures the IsNotNull function works correctly.
        /// </summary>
        [Test]
        public void IsNotNullTest()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>( () =>
                ArgumentChecker.IsNotNull( null, argName )
            );

            Assert.AreEqual( argName, ex.ParamName );

            Assert.DoesNotThrow( () =>
                ArgumentChecker.IsNotNull( "Hello", argName )
            );
        }

        /// <summary>
        /// Ensures StringIsNotNullOrEmpty function works correctly.
        /// </summary>
        [Test]
        public void StringIsNotNullOrEmptyTest()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>( () =>
                ArgumentChecker.StringIsNotNullOrEmpty( null, argName )
            );
            Assert.AreEqual( argName, ex.ParamName );

            ex = Assert.Throws<ArgumentNullException>( () =>
                ArgumentChecker.StringIsNotNullOrEmpty( string.Empty, argName )
            );
            Assert.AreEqual( argName, ex.ParamName );

            Assert.DoesNotThrow( () =>
                ArgumentChecker.StringIsNotNullOrEmpty( "Hello", argName )
            );
        }
    }
}
