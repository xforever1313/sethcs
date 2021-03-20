//
//          Copyright Seth Hendrick 2015-2021.
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

        /// <summary>
        /// The wrapped list.
        /// This is also used as the lock for this class (and can be used by any base classes that 
        /// inherit this class).
        /// </summary>
        protected IList<T> list;

        // ---------------- Constructor ----------------

        protected BaseCloningReadOnlyList( IList<T> wrappedList )
        {
            this.list = wrappedList;
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
                lock( this.list )
                {
                    return this.CloneInstructions( this.list[index] );
                }
            }
        }

        public int Count
        {
            get
            {
                lock( this.list )
                {
                    return this.list.Count;
                }
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

        /// <summary>
        /// Creates an enumerator whose elements are clones of the items in this
        /// list.  Any changes made to this enumerator will not impact
        /// the elements saved in this list.
        /// 
        /// This function:
        /// 1. Creates a new List{T}.
        /// 2. Locks this class's list
        /// 3. Adds the elements to this class's list to the new List{T} as CLONES.
        /// 4. Returns the new List's enumerator.
        /// </summary>
        /// <remarks>
        /// Foreach depends on the fact that a collection will not change.
        /// Therefore, we'll just return a new list that contains copies of the elements
        /// in this class.
        /// </remarks>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            List<T> cloneList = new List<T>();
            lock( this.list )
            {
                foreach( T t in this.list )
                {
                    cloneList.Add( this.CloneInstructions( t ) );
                }
            }

            return cloneList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cloningAction">
        /// The action to take while cloning an object in the list.
        /// The action should return a copy of the passed in object that is equal,
        /// but not the same reference.
        /// </param>
        /// <param name="wrappedList">The list to wrap.</param>
        public CloningReadOnlyList( Func<T, T> cloningAction, IList<T> wrappedList ) :
            base( wrappedList )
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
