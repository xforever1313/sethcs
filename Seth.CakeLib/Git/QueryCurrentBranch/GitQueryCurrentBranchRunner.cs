//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.ArgumentBinder;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;

namespace Seth.CakeLib.Git.QueryCurrentBranch
{
    public class GitQueryCurrentBranchRunner : GitRunner
    {
        // ---------------- Fields ----------------

        private readonly ICakeContext context;
        private readonly GitToolSettings toolSettings;

        // ---------------- Constructor ----------------

        public GitQueryCurrentBranchRunner(
            ICakeContext context,
            GitToolSettings toolSettings = null
        ) : base( context )
        {
            this.context = context;
            this.toolSettings = toolSettings ?? new GitToolSettings();
        }

        // ---------------- Functions ----------------

        /// <summary>
        /// Runs git on the local repository and returns
        /// the name of the current branch.
        /// </summary>
        /// <param name="config">
        /// Configuration.  If null, it grabs the configuration
        /// from the passed in command-line arguments.
        /// </param>
        public string Run( GitQueryCurrentBranchConfig config = null )
        {
            if( config == null )
            {
                config = ArgumentBinder.FromArguments<GitQueryCurrentBranchConfig>( this.context );
            }

            string branch = null;

            string onStdOut( string line )
            {
                if( string.IsNullOrWhiteSpace( line ) == false )
                {
                    branch = line;
                }

                return line;
            };

            ProcessSettings processSettings = new ProcessSettings
            {
                Arguments = ProcessArgumentBuilder.FromString( "rev-parse --abbrev-ref HEAD" ),
                RedirectStandardOutput = true,
                RedirectedStandardOutputHandler = onStdOut
            };

            this.Run( this.toolSettings, processSettings.Arguments, processSettings, null );

            if( branch == null )
            {
                throw new InvalidOperationException(
                    "Could not get the current branch from Git"
                );
            }

            if( config.NoPrint == false )
            {
                context.Information( $"Current Branch: {branch}" );
            }

            return branch;
        }
    }
}
