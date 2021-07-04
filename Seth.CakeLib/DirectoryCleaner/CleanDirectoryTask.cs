//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.ArgumentBinder;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Frosting;

namespace Seth.CakeLib.DirectoryCleaner
{
    [TaskName( "clean_directory" )]
    [TaskDescription( "Cleans the specified directory.  If it does not exist, it will be created." )]
    public class CleanDirectoryTask : FrostingTask
    {
        // ---------------- Functions ----------------

        public override void Run( ICakeContext context )
        {
            CleanDirectoryConfig config = context.CreateFromArguments<CleanDirectoryConfig>();

            context.Information( config );

            if( context.DirectoryExists( config.Path ) )
            {
                context.DeleteDirectory( config.Path, new DeleteDirectorySettings { Force = true, Recursive = true } );
            }

            context.CreateDirectory( config.Path );
        }
    }
}
