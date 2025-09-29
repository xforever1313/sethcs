//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
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
            if( startCharacter == null )
            {
                startCharacter = string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            foreach( T l in list )
            {
                builder.AppendLine( startCharacter + l.ToString() );
            }

            return builder.ToString();
        }

        public static bool IsEmpty<T>( this IEnumerable<T> list )
        {
            return ( list.Any() == false );
        }

        /// <summary>
        /// Checks to see if the two lists are the same, should both
        /// of them be ordered.
        /// </summary>
        public static bool EqualsIgnoreOrder<T>( this IEnumerable<T> left, IEnumerable<T> right )
        {
            // If either list is null, do a referenc equals.
            if( ReferenceEquals( left, null ) || ReferenceEquals( right, null ) )
            {
                return ReferenceEquals( left, right );
            }

            // If we don't care about order, just sort both lists, and call sequence equals.
            return left.OrderBy( t => t ).SequenceEqual( right.OrderBy( t => t ) );
        }

        public static void SerialPerformActionOnList<T>( this IEnumerable<T> list, Action<T> action, string context = null )
        {
            List<Exception> exceptions = null;
            foreach( T item in list )
            {
                try
                {
                    T local = item;
                    action( local );
                }
                catch( Exception e )
                {
                    if( exceptions == null )
                    {
                        exceptions = new List<Exception>();
                    }
                    exceptions.Add( e );
                }
            }

            if( exceptions != null )
            {
                throw new AggregateException(
                    context ?? "Errors when performing action on at least 1 item in the list",
                    exceptions
                );
            }
        }
    }
}
