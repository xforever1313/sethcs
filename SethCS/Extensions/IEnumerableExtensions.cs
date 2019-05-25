//
//          Copyright Seth Hendrick 2019.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SethCS.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Iterates through each element in the Enumerable and calls ToString() on each element,
        /// putting the output on a newline for each element in the list.
        /// </summary>
        /// <param name="startCharacter">What the start character of each line should be (e.g. "- ").  Defaults to none (null).</param>
        public static string ToListString<T>( this IEnumerable<T> list, string startCharacter = null )
        {
            if ( startCharacter == null )
            {
                startCharacter = string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            foreach ( T l in list )
            {
                builder.AppendLine( startCharacter + l.ToString() );
            }

            return builder.ToString();
        }

        public static bool IsEmpty<T>( this IEnumerable<T> list )
        {
            return list.LongCount() == 0;
        }
    }
}
