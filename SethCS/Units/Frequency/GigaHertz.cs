//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

namespace SethCS.Units
{
    public struct GigaHertz : Frequency
    {
        // ---------------- Fields ----------------

        internal static readonly decimal ProductValue = SiPrefixes.Giga;

        // ---------------- Constructor ----------------

        public GigaHertz( decimal gigaHertz ) :
            base( gigaHertz * ProductValue )
        {
            this.Value = gigaHertz;
        }

        // ---------------- Properties ----------------

        public decimal Value { get; }

        // ---------------- Methods ----------------

        public override string ToString()
        {
            return $"{this.Value} GHz";
        }
    }

    public static class KiloHertzExtensions
    {
        public static decimal GigaHertz( this Frequency frequency )
        {
            return frequency.Hertz / GigaHertz.ProductValue;
        }

        public static GigaHertz ToGigaHertz( this Frequency frequency )
        {
            return new GigaHertz( frequency.GigaHertz() );
        }
    }
}