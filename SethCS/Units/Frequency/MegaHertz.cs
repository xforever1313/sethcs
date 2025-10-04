//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

namespace SethCS.Units
{
    public struct MegaHertz : Frequency
    {
        // ---------------- Fields ----------------

        internal static readonly decimal ProductValue = SiPrefixes.Mega;

        // ---------------- Constructor ----------------

        public MegaHertz( decimal megaHertz ) :
            base( megaHertz * ProductValue )
        {
            this.Value = megaHertz;
        }

        // ---------------- Properties ----------------

        public decimal Value { get; }

        // ---------------- Methods ----------------

        public override string ToString()
        {
            return $"{this.Value} MHz";
        }
    }

    public static class MegaHertzExtensions
    {
        public static decimal MegaHertz( this Frequency frequency )
        {
            return frequency.Hertz / MegaHertz.ProductValue;
        }

        public static MegaHertz ToMegaHertz( this Frequency frequency )
        {
            return new MegaHertz( frequency.MegaHertz() );
        }
    }
}
