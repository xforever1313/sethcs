//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.ArgumentBinder;
using Cake.Core;
using Cake.Frosting;

namespace Seth.CakeLib.DeleteHelpers
{
    [TaskName( "delete_files" )]
    public class DeleteFilesTask : FrostingTask
    {
        // ----------------- Functions -----------------

        public override void Run( ICakeContext context )
        {
            DeleteHelpersConfig config = ArgumentBinder.FromArguments<DeleteHelpersConfig>( context );
            context.DeleteFiles( config );
        }
    }

    [TaskName( "delete_dirs" )]
    public class DeleteDirsTask : FrostingTask
    {
        // ----------------- Functions -----------------

        public override void Run( ICakeContext context )
        {
            DeleteHelpersConfig config = ArgumentBinder.FromArguments<DeleteHelpersConfig>( context );
            context.DeleteDirectories( config );
        }
    }
}
