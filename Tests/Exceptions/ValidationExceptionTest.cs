//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using NUnit.Framework;
using SethCS.Exceptions;

namespace Tests.Exceptions
{
    [TestFixture]
    public sealed class ValidationExceptionTest
    {
        /// <summary>
        /// Ensures the exception gets constructed correctly.
        /// </summary>
        [Test]
        public void ConstructorTest()
        {
            const string message = "msg";

            // Ensure ex.Message becomes the message property.
            ValidationException ex = new ValidationException( message );
            Assert.AreEqual( message, ex.Message );

            // Ensure ex.Message and ex.InnerException are passed in properly.
            Exception inner = new Exception( "innerException" );
            ex = new ValidationException( message, inner );
            Assert.AreEqual( message, ex.Message );
            Assert.AreSame( inner, ex.InnerException );
        }
    }
}
