//
//          Copyright Seth Hendrick 2017.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;

namespace SethCS.Extensions
{
    public static class SethPath
    {
        /// <summary>
        /// Takes a path to the given file and returns
        /// a URI that can be used in a browser or an HTTP request (file://).
        /// </summary>
        /// <param name="path">The path to convert.</param>
        public static string ToUri( string path )
        {
            return new Uri( new Uri( "file://" ), path ).AbsoluteUri;
        }
    }
}
