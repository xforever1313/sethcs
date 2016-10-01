//          Copyright Seth Hendrick 2016.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file ../../LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)

using System;

namespace SethCS.Exceptions
{
    /// <summary>
    /// Helpers to ensure that arguments passed into a function
    /// are valid.  Otherwise, ArgumentExceptions are thrown.
    /// </summary>
    public static class ArgumentChecker
    {
        /// <summary>
        /// Ensures the given object is not null.  If it is,
        /// this throws an ArgumentNullException.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="argumentName">The name of the argument checked</param>
        /// <exception cref="ArgumentNullException">Thrown if obj is null.</exception>
        public static void IsNotNull( object obj, string argumentName )
        {
            if( obj == null )
            {
                throw new ArgumentNullException( argumentName );
            }
        }

        /// <summary>
        /// Ensures the given string is not null or empty.  If it is,
        /// this throws an ArgumentNullException.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="argumentName">The name of the argument checked</param>
        /// <exception cref="ArgumentNullException">Thrown if obj is null.</exception>
        public static void StringIsNotNullOrEmpty( string str, string argumentName )
        {
            if( string.IsNullOrEmpty( str ) )
            {
                throw new ArgumentNullException( argumentName );
            }
        }
    }
}