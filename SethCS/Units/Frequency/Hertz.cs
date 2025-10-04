//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

namespace SethCS.Units
{
    public struct Hertz : Frequency
    {
        // ---------------- Constructor ----------------

        public Hertz( decimal hertz ) :
            base( hertz )
        {
            this.Value = hertz;
        }

        // ---------------- Properties ----------------

        public decimal Value { get; }

        // ---------------- Methods ----------------

        public override string ToString()
        {
            return $"{this.Value} Hz";
        }
    }

    public static class HertzExtensions
    {
        public static Hertz ToHertz( this Frequency frequency )
        {
            return new Hertz( frequency.Hertz );
        }
    }
}
