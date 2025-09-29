//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

namespace SethCS.Grid
{
    public class Cell<T>
    {
        // ---------------- Constructor ----------------

        public Cell( int xIndex, int yIndex, T payload )
        {
            this.X = xIndex;
            this.Y = yIndex;
            this.Payload = payload;
        }

        // ---------------- Properties ----------------

        /// <summary>
        /// Where in the X axis on the grid the cell is in.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Where in the Y axis on the grid the cell is in.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// The thing located in the cell.
        /// </summary>
        public T Payload { get; private set; }

        /// <summary>
        /// The cell located to the left of this one within the grid.
        /// Null means nothing is to the left.
        /// 
        /// "Left" means towards 0.
        /// </summary>
        public Cell<T> Left { get;internal set; }

        /// <summary>
        /// The cell located to the right of this one within the grid.
        /// Null means nothing is to the right.
        /// 
        /// "Right" means away from 0.
        /// </summary>
        public Cell<T> Right{ get; internal set; }

        /// <summary>
        /// The cell located to the top of this one within the grid.
        /// Null means nothing is to the top.
        /// 
        /// "Top" is towards 0.
        /// </summary>
        public Cell<T> Top { get; internal set; }

        /// <summary>
        /// The cell located to the bottom of this one within the grid.
        /// Null means nothing is to the bottom.
        /// 
        /// "Bottom" means away from 0.
        /// </summary>
        public Cell<T> Bottom { get; internal set; }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
    }
}
