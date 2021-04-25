//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Text;
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

        public static bool EqualsIgnoreCase( this string str, string other )
        {
            if( str == null )
            {
                return ( other == null );
            }

            return str.Equals( other, StringComparison.InvariantCultureIgnoreCase );
        }

        public static bool StartsWithIgnoreCase( this string str, string value )
        {
            return str.StartsWith( value, StringComparison.OrdinalIgnoreCase );
        }

        public static bool EndsWithIgnoreCase( this string str, string value  )
        {
            return str.EndsWith( value, StringComparison.OrdinalIgnoreCase );
        }

        /// <summary>
        /// Takes the string and splits it up by length.
        /// For example, "12345" with a length of 2 will return an array
        /// that contains "12", "34", "5".
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <param name="maxLength">
        /// The length to split the strings to.
        /// 
        /// Must be greater than 0.
        /// </param>
        /// <returns>An array of split strings by length.</returns>
        public static string[] SplitByLength( this string str, int maxLength )
        {
            if( maxLength <= 0 )
            {
                throw new ArgumentException(
                    "Length must be greater than 0.",
                    nameof( maxLength )
                );
            }

            if( string.IsNullOrEmpty( str ) )
            {
                return new string[] { string.Empty };
            }

            int arrayLength = ( str.Length / maxLength );

            // If we are not divisable by the max length, we need one more slot
            // for the left-overs.
            if( ( str.Length % maxLength ) != 0 )
            {
                arrayLength += 1;
            }

            string[] array = new string[arrayLength];

            int strIndex = 0;
            int arrayIndex = 0;
            for( ; strIndex < ( str.Length - maxLength); strIndex += maxLength )
            {
                array[arrayIndex] = str.Substring( strIndex, maxLength );
                ++arrayIndex;
            }
            array[arrayIndex] = str.Substring( strIndex );

            return array;
        }

        /// <summary>
        /// Takes a string and replaces any character that is not a character
        /// to its hex value ([0x01]). So a SOHHelloWorldEOT will turn into [0x01]HelloWorld[0x04]
        /// </summary>
        public static string EscapeNonCharacters( this IEnumerable<char> str )
        {
            StringBuilder builder = new StringBuilder();
            foreach ( char ch in str )
            {
                if ( char.IsControl( ch ) )
                {
                    builder.Append( "[0x" + Convert.ToInt32( ch ).ToString( "X4" ) + "]" );
                }
                else
                {
                    builder.Append( ch );
                }
            }

            return builder.ToString();
        }
    }
}
