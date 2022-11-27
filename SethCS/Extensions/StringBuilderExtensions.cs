//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SethCS.Extensions
{
    public static class StringBuilderExtensions
    {
        /// <returns>
        /// The passed in <see cref="StringBuilder"/> so calls can be chained.
        /// </returns>
        public static StringBuilder RemoveLastCharacter( this StringBuilder builder )
        {
            if( builder.Length == 0 )
            {
                return builder;
            }

            builder.Remove( builder.Length - 1, 1 );

            return builder;
        }
    }
}