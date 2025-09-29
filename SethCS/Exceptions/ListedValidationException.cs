//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using SethCS.Extensions;

namespace SethCS.Exceptions
{
    /// <summary>
    /// This is a <seealso cref="ValidationException"/>, but allows for a list of
    /// error messages instead of a single one.
    /// </summary>
    public class ListedValidationException : ValidationException
    {
        // ---------------- Constructor ----------------

        public ListedValidationException( string context, IEnumerable<string> errors ) :
            base( context + Environment.NewLine + errors.ToListString( " - " ) )
        {
            this.Context = context;
            this.Errors = errors;
        }

        // ---------------- Properties ----------------

        /// <summary>
        /// The reason why this exception was thrown.
        /// </summary>
        public string Context { get; private set; }

        /// <summary>
        /// Collection of errors that caused the exception.
        /// </summary>
        public IEnumerable<string> Errors { get; private set; }
    }
}
