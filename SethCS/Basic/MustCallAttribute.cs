//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;

namespace SethCS.Basic
{
    /// <summary>
    /// This attribute represents a function that
    /// must be called on a class before it goes out-of-scope,
    /// such as an Init function.
    /// </summary>
    [AttributeUsage( AttributeTargets.Method, AllowMultiple = false, Inherited = true )]
    public class MustCallAttribute : Attribute
    {
        // ---------------- Constructor ----------------

        public MustCallAttribute()
        {
        }
    }
}
