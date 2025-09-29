//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SethCS.Basic;

namespace SethCS.Collections
{
    /// <summary>
    /// Version of <see cref="HashSet{T}"/> where the Equals method
    /// invokes SequenceEquals instead of doing a reference equals.
    /// Order is also ignored.
    /// </summary>
    public class SequentialOrderIgnoredHashSet<T> : HashSet<T>, IEquatable<SequentialOrderIgnoredHashSet<T>>
    {
        // ---------------- Constructor ----------------

        public SequentialOrderIgnoredHashSet() :
            base( new EqualityComparer() )
        {
        }

        // ---------------- Methods ----------------

        public override bool Equals( object? obj )
        {
            return Equals( obj as SequentialOrderIgnoredHashSet<T> );
        }

        public bool Equals( SequentialOrderIgnoredHashSet<T>? other )
        {
            if( other is null )
            {
                return false;
            }

            if( this.Count != other.Count )
            {
                return false;
            }

            return Enumerable.SequenceEqual(
                this.OrderBy( t => t ),
                other.OrderBy( t => t )
            );
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach( T value in this.OrderBy( t => t ) )
            {
                hashCode = HashCode.Combine( value );
            }

            return hashCode;
        }

        // ---------------- Helper Classes ----------------

        private sealed class EqualityComparer : IEqualityComparer<T?>
        {
            public bool Equals( T? x, T? y )
            {
                return EqualsHelpers.OperatorDoubleEqualsHelper( x, y );
            }

            public int GetHashCode( [DisallowNull] T? obj )
            {
                return obj.GetHashCode();
            }
        }
    }
}
