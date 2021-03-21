//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.Frosting;
using Seth.CakeLib.Git;
using Seth.CakeLib.Git.LastCommitDate;

namespace DevOps.Git
{
    [TaskName( "print_git_commit_date" )]
    [TaskDescription( "Prints the commit date" )]
    public class LastCommitDateTask : DevOpsTask
    {
        public override void Run( BuildContext context )
        {
            GitToolSettings toolSettings = new GitToolSettings
            {
                WorkingDirectory = context.RepoRoot
            };

            LastCommitDateRunner runner = new LastCommitDateRunner( context, toolSettings );
            runner.Run();
        }
    }
}
