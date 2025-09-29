//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace SethCS.Grid
{
    public class Grid<T>
    {
        // ---------------- Fields ----------------

        private readonly Cell<T>[,] gridContents;

        // ---------------- Constructor ----------------

        public Grid( int width, int height, Func<T> constructorAction )
        {
            if( width <= 0 )
            {
                throw new ArgumentException(
                    "Width can not be 0 or less",
                    nameof( width )
                );
            }

            if( height <= 0 )
            {
                throw new ArgumentException(
                    "Height can not be 0 or less",
                    nameof( height)
                );
            }

            if( constructorAction is null )
            {
                throw new ArgumentNullException( nameof( constructorAction ) );
            }

            this.Width = width;
            this.Height = height;

            this.gridContents = new Cell<T>[this.Width, this.Height];

            for( int x = 0; x < this.Width; ++x )
            {
                for( int y = 0; y < this.Height; ++y )
                {
                    this.gridContents[x, y] = new Cell<T>( x, y, constructorAction() );
                }
            }

            PerformActionOnGrid(
                ( x, y ) =>
                {
                    if( x > 0 )
                    {
                        this.gridContents[x, y].Left = this.gridContents[x - 1, y];
                    }

                    if( x < ( this.Width - 1 ) )
                    {
                        this.gridContents[x, y].Right = this.gridContents[x + 1, y];
                    }

                    if( y > 0 )
                    {
                        this.gridContents[x, y].Top = this.gridContents[x, y - 1];
                    }

                    if( y < ( this.Height - 1 ) )
                    {
                        this.gridContents[x, y].Bottom = this.gridContents[x, y + 1];
                    }
                }
            );
        }

        // ---------------- Properties ----------------

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Length => this.gridContents.Length;

        // ---------------- Functions ----------------

        public Cell<T> GetCell( int x, int y )
        {
            return this.gridContents[x, y];
        }

        public T Get( int x, int y )
        {
            return this.gridContents[x, y].Payload;
        }

        public void PerformActionOnGrid( Action<int, int> action )
        {
            for( int x = 0; x < this.Width; ++x )
            {
                for( int y = 0; y < this.Height; ++y )
                {
                    action( x, y );
                }
            }
        }

        /// <summary>
        /// Iterates row-by-row.
        /// </summary>
        public IEnumerable<Cell<T>> ToEnumerable()
        {
            foreach( Cell<T> item in this.gridContents )
            {
                yield return item;
            }
        }
    }
}
