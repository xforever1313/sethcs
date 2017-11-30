//
//          Copyright Seth Hendrick 2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace SethCS.Collections
{
    /// <summary>
    /// This class is meant to be a base class.
    /// Its purpose is to treat the parent class as a list,
    /// but when a caller accesses an element, they ALWAYS receive a clone,
    /// so the underlying data is never modified.
    /// </summary>
    public abstract class BaseCloningReadOnlyList<T> : IReadOnlyList<T>
    {
        // ---------------- Fields ----------------

        protected List<T> list;

        // ---------------- Constructor ----------------

        protected BaseCloningReadOnlyList()
        {
            this.list = new List<T>();
        }

        // ---------------- Properties ----------------

        // -------- IReadOnlyList Implementation --------

        /// <summary>
        /// Returns a copy of the object at the given index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                return this.CloneInstructions( this.list[index] );
            }
        }

        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        // ---------------- Functions ----------------

        /// <summary>
        /// Implementors must have this return a clone of the original
        /// object.  That is, the returned object is equivalent to the original,
        /// but they are NOT the same reference.
        /// </summary>
        protected abstract T CloneInstructions( T original );

        // -------- IEnumerable Implementation --------

        public IEnumerator<T> GetEnumerator()
        {
            return new CloningReadOnlyListEnumerator( this );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        // ---------------- Helper Classes ----------------

        private class CloningReadOnlyListEnumerator : IEnumerator<T>
        {
            // ---------------- Fields ----------------

            private BaseCloningReadOnlyList<T> cloningList;
            int position;

            // ---------------- Constructor ----------------

            public CloningReadOnlyListEnumerator( BaseCloningReadOnlyList<T> cloningList )
            {
                this.cloningList = cloningList;

                // Enumerators are positioned before the first element
                // until the first MoveNext() call.
                this.position = -1;
            }

            // ---------------- Properties ----------------

            // -------- IEnumerator Implementation --------

            public T Current
            {
                get
                {
                    return this.cloningList.CloneInstructions( this.cloningList.list[this.position] );
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            // ---------------- Functions ----------------

            // -------- IEnumerator Implementation --------

            public void Dispose()
            {
                // Nothing to do.
            }

            public bool MoveNext()
            {
                ++this.position;
                return this.position < this.cloningList.list.Count;
            }

            public void Reset()
            {
                this.position = -1;
            }
        }
    }

    /// <summary>
    /// This is a version of the CloningReadOnlyList, where instead of inheriting from
    /// BaseCloningReadOnlyList, the cloning action is passed in as a parameter during
    /// construction.
    /// </summary>
    public class CloningReadOnlyList<T> : BaseCloningReadOnlyList<T>
    {
        // ---------------- Fields ----------------

        private Func<T, T> cloningAction;

        // ---------------- Constructor ----------------

        public CloningReadOnlyList( Func<T, T> cloningAction )
        {
            this.cloningAction = cloningAction;
        }

        // ---------------- Functions ----------------

        protected override T CloneInstructions( T original )
        {
            return cloningAction( original );
        }
    }
}
