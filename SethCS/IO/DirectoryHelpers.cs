//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.IO;

namespace SethCS.IO
{
    public static class DirectoryHelpers
    {
        public static void EnsureDirectoryExists( string path )
        {
            if( Directory.Exists( path ) == false )
            {
                Directory.CreateDirectory( path );
            }
        }
    }
}
