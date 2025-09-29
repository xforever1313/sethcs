//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;

namespace SethCS.Extensions
{
    public static class IListExtensions
    {
        // ---------------- Functions ----------------

        public static void AddRange<T>( this IList<T> list, IEnumerable<T> valuesToAdd )
        {
            foreach( T value in valuesToAdd )
            {
                list.Add( value );
            }
        }
    }
}
