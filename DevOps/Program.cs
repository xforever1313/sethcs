//
//          Copyright Seth Hendrick 2019-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.IO;
using System.Reflection;
using Cake.Frosting;

namespace DevOps
{
    static class Program
    {
        static int Main( string[] args )
        {
            string exeDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
            string repoRoot = Path.Combine(
                exeDir, // app
                "..", // Debug
                "..", // Bin
                "..", // DevOps
                ".."  // Root
            );

            return new CakeHost()
                .UseContext<BuildContext>()
                .SetToolPath( ".cake" )
                .UseWorkingDirectory( repoRoot )
                .Run( args );
        }
    }
}
