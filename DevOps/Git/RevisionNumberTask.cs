//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.Frosting;
using Seth.CakeLib.Git;
using Seth.CakeLib.Git.RevisionNumber;

namespace DevOps.Git
{
    [TaskName( "print_git_revision" )]
    [TaskDescription( "Prints the number of commits on the current branch." )]
    public sealed class RevisionNumberTask : DevOpsTask
    {
        public override void Run( BuildContext context )
        {
            GitToolSettings toolSettings = new GitToolSettings
            {
                WorkingDirectory = context.RepoRoot
            };

            GitRevisionNumberRunner runner = new GitRevisionNumberRunner( context, toolSettings );
            runner.Run();
        }
    }
}
