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
    public class CleanDirectoryTask : FrostingTask
    {
        // ---------------- Functions ----------------

        public override void Run( ICakeContext context )
        {
            CleanDirectoryConfig config = context.CreateFromArguments<CleanDirectoryConfig>();

            context.Information( config );

            context.EnsureDirectoryExists( config.Path );
            context.CleanDirectory( config.Path );
        }
    }
}
