//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

namespace SethCS.Units
{
    public struct KiloHertz : Frequency
    {
        // ---------------- Fields ----------------

        internal static readonly decimal ProductValue = SiPrefixes.Kilo;

        // ---------------- Constructor ----------------

        public KiloHertz( decimal kiloHertz ) :
            base( kiloHertz * ProductValue )
        {
            this.Value = kiloHertz;
        }

        // ---------------- Properties ----------------

        public decimal Value { get; }

        // ---------------- Methods ----------------

        public override string ToString()
        {
            return $"{this.Value} KHz";
        }
    }

    public static class KiloHertzExtensions
    {
        public static decimal KiloHertz( this Frequency frequency )
        {
            return frequency.Hertz / KiloHertz.ProductValue;
        }

        public static KiloHertz ToKiloHertz( this Frequency frequency )
        {
            return new KiloHertz( frequency.KiloHertz() );
        }
    }
}
