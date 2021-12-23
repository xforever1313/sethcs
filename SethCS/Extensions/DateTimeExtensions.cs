//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;

namespace SethCS.Extensions
{
    public static class DateTimeExtensions
    {
        // ---------------- Fields ----------------

        public static readonly string TimeStampFormatString = "O";

        public static readonly string FileNameStringFormat = "yyyy-MM-dd_HH-mm-ss-ffff";

        // ---------------- Functions ----------------

        /// <summary>
        /// Returns a string of the given timestamp in the form of ISO 8601
        /// The same as DateTime.ToString("o");
        /// </summary>
        /// <param name="timeStamp">Time stamp to get string of.</param>
        /// <returns>String representation of the timestamp in ISO 8601 format.</returns>
        public static string ToTimeStampString( this DateTime timeStamp )
        {
            return timeStamp.ToString( TimeStampFormatString );
        }

        /// <summary>
        /// Returns a string of the given timestamp that works well with file names.
        /// yyyy-MM-dd_HH-mm-ss-ffff
        /// </summary>
        /// <param name="timeStamp">Time stamp to get string of.</param>
        /// <returns>A string in the form of yyyy-MM-dd_HH-mm-ss-ffff</returns>
        public static string ToFileNameString( this DateTime timeStamp )
        {
            return timeStamp.ToString( FileNameStringFormat );
        }
    }
}
