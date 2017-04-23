//
//          Copyright Seth Hendrick 2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Text.RegularExpressions;

namespace SethCS.Extensions
{
    /// <summary>
    /// Extensions for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces all white space with the given single character.
        /// Includes tabs and new lines.
        /// 
        /// If there are multiple whitespaces in a row, it is only replaced
        /// with the single character.
        /// </summary>
        /// <example>
        /// Hello    World
        /// How are you?
        /// 
        /// becomes (if ch is '_')
        /// Hello_World_How_are_you?
        /// </example>
        public static string NormalizeWhiteSpace( this string str, char replaceCharacter = ' ' )
        {
            Regex regex = new Regex( @"\s+" );
            return regex.Replace( str, "" + replaceCharacter );
        }
    }
}
