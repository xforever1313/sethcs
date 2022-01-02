//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace SethCS.Extensions
{
    public static class EnumExtensions
    {
        // ---------------- Functions ----------------

        /// <summary>
        /// Sorts the enums by their numeric values (starting from the lowest)
        /// and returns them.
        /// </summary>
        public static IEnumerable<TEnum> SortByNumber<TEnum>()
            where TEnum : Enum
        {
            return
                Enum.GetValues( typeof( TEnum ) )
                    .OfType<TEnum>()
                    .OrderBy( x => x );
        }

        /// <summary>
        /// Sorts the enums by their numeric values (starting from the highest)
        /// and returns them.
        /// </summary>
        public static IEnumerable<TEnum> SortByNumberDecending<TEnum>()
            where TEnum : Enum
        {
            return
                Enum.GetValues( typeof( TEnum ) )
                    .OfType<TEnum>()
                    .OrderByDescending( x => x );
        }
    }
}
