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
using Seth.CakeLib.Git.LastCommitDate;

namespace Seth.CakeLib.Git
{
    public static partial class GitAliases
    {
        /// <summary>
        /// Gets the number of commits on the current branch of
        /// a local repo, and returns it.
        /// </summary>
        /// <param name="repoRoot">Path to the root of the repo.</param>
        /// <param name="config">
        /// The config to use.
        /// If null, this is created via passed in command line arguments.
        /// </param>
        [CakeMethodAlias]
        [CakeAliasCategory( "Last Commit Date" )]
        [CakeNamespaceImport( "Seth.CakeLib.Git.RevisionNumber" )]
        public static DateTime GitLastCommitDate(
            this ICakeContext context,
            DirectoryPath repoRoot,
            GitLastCommitDateConfig config = null
        )
        {
            var toolSettings = new GitToolSettings
            {
                WorkingDirectory = repoRoot
            };

            return GitLastCommitDate( context, toolSettings, config );
        }

        /// <summary>
        /// Gets the number of commits on the current branch of
        /// a local repo, and returns it.
        /// </summary>
        /// <param name="config">
        /// The config to use.
        /// If null, this is created via passed in command line arguments.
        /// </param>
        [CakeMethodAlias]
        [CakeAliasCategory( "Last Commit Date" )]
        [CakeNamespaceImport( "Seth.CakeLib.Git.RevisionNumber" )]
        public static DateTime GitLastCommitDate(
            this ICakeContext context,
            GitToolSettings toolSettings,
            GitLastCommitDateConfig config = null
        )
        {
            var runner = new GitLastCommitDateRunner( context, toolSettings );
            return runner.Run( config );
        }
    }
}
