//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SethCS.Exceptions;

namespace Tests.Exceptions
{
    [TestClass]
    public sealed class ArgumentCheckerTest
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
        [TestMethod]
        public void IsNotNullTest()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>( () =>
                ArgumentChecker.IsNotNull( null, argName )
            );

            Assert.AreEqual( argName, ex.ParamName );

            // Should not throw.
            ArgumentChecker.IsNotNull( "Hello", argName );
        }

        /// <summary>
        /// Ensures StringIsNotNullOrEmpty function works correctly.
        /// </summary>
        [TestMethod]
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

            // Should not throw.
            ArgumentChecker.StringIsNotNullOrEmpty( "Hello", argName );
        }
    }
}
