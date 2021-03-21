//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Seth.CakeLib.Git.QueryCurrentBranch;

namespace Seth.CakeLib.Git
{
    public static partial class GitAliases
    {
        /// <summary>
        /// Gets the name of the branch that a local repo is checked out on,
        /// anre returns it.
        /// </summary>
        /// <param name="repoRoot">Path to the root of the repo.</param>
        /// <param name="config">
        /// The config to use.
        /// If null, this is created via passed in command line arguments.
        /// </param>
        [CakeMethodAlias]
        [CakeAliasCategory( "Branch" )]
        [CakeNamespaceImport( "Seth.CakeLib.Git.QueryCurrentBranch" )]
        public static string GitQueryCurrentBranch(
            this ICakeContext context,
            DirectoryPath repoRoot,
            GitQueryCurrentBranchConfig config = null
        )
        {
            var toolSettings = new GitToolSettings
            {
                WorkingDirectory = repoRoot
            };

            return GitQueryCurrentBranch( context, toolSettings, config );
        }

        /// <summary>
        /// Gets the name of the branch that a local repo is checked out on,
        /// anre returns it.
        /// </summary>
        /// <param name="config">
        /// The config to use.
        /// If null, this is created via passed in command line arguments.
        /// </param>
        [CakeMethodAlias]
        [CakeAliasCategory( "Branch" )]
        [CakeNamespaceImport( "Seth.CakeLib.Git.QueryCurrentBranch" )]
        public static string GitQueryCurrentBranch(
            this ICakeContext context,
            GitToolSettings toolSettings,
            GitQueryCurrentBranchConfig config = null
        )
        {
            var runner = new GitQueryCurrentBranchRunner( context, toolSettings );
            return runner.Run( config );
        }
    }
}
