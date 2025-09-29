//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Reflection;

namespace Seth.CakeLib
{
    /// <summary>
    /// Class that can be used when creating <see cref="Cake.Frosting.CakeHost">
    /// to get information about this library.
    /// </summary>
    public static class SethCakeLib
    {
        /// <summary>
        /// Gets the assembly of this library.
        /// Useful to pass into <see cref="Cake.Frosting.CakeHost.AddAssembly(Assembly)"/>
        /// </summary>
        public static Assembly GetAssembly()
        {
            return typeof( SethCakeLib ).Assembly;
        }
    }
}