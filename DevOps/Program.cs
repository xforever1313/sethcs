//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.IO;
using System.Reflection;
using Cake.Frosting;
using Seth.CakeLib;

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
                .AddAssembly( SethCakeLib.GetAssembly() )
                .InstallTool( new Uri( "nuget:?package=OpenCover&version=4.6.519" ) )
                .InstallTool( new Uri( "nuget:?package=ReportGenerator&version=4.0.10" ) )
                .SetToolPath( ".cake" )
                .UseWorkingDirectory( repoRoot )
                .Run( args );
        }
    }
}
