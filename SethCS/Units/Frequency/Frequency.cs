//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;

namespace SethCS.Units
{
    public struct Frequency : IEquatable<Frequency>
    {
        // ---------------- Fields ---------------

        public static readonly Frequency Zero = new Frequency( 0 );

        // ---------------- Constructor ---------------

        protected Frequency( decimal hertz )
        {
            this.Hertz = hertz;
        }

        // ---------------- Properties ---------------

        public decimal Hertz { get; }

        // ---------------- Methods ---------------

        public bool Equals( Frequency other )
        {
            return Hertz == other.Hertz;
        }

        public override sealed bool Equals( object obj )
        {
            return Equals( obj as Frequency );
        }

        public override sealed int GetHashCode()
        {
            return this.Hertz.GetHashCode();
        }

        public static Frequency operator +( Frequency left, Frequency right )
        {
            return new Frequency( left.Hertz + right.Hertz );
        }

        public static Frequency operator -( Frequency left, Frequency right )
        {
            return new Frequency( left.Hertz - right.Hertz );
        }

        public static Frequency operator *( Frequency left, Frequency right )
        {
            return new Frequency( left.Hertz * right.Hertz );
        }

        public static Frequency operator /( Frequency left, Frequency right )
        {
            return new Frequency( left.Hertz / right.Hertz );
        }

        public static bool operator ==( Frequency left, Frequency right )
        {
            return left.Hertz == right.Hertz;
        }

        public static bool operator !=( Frequency left, Frequency right )
        {
            return left.Hertz != right.Hertz;
        }

        public static Frequency Abs( Frequency freq )
        {
            return new Frequency( Math.Abs( freq.Hertz ) );
        }
    }
}
