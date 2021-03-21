//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.ArgumentBinder;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;

namespace Seth.CakeLib.Git.RevisionNumber
{
    public sealed class GitRevisionNumberRunner : GitRunner
    {
        // ---------------- Fields ----------------

        private readonly ICakeContext context;
        private readonly GitToolSettings toolSettings;

        // ---------------- Constructor ----------------

        public GitRevisionNumberRunner(
            ICakeContext context,
            GitToolSettings toolSettings = null
        ) : base( context )
        {
            this.context = context;
            this.toolSettings = toolSettings ?? new GitToolSettings();
        }

        // ---------------- Functions ----------------

        /// <summary>
        /// Runs git and gets the number of commits on the current branch.
        /// </summary>
        /// <param name="config">
        /// Configuration.  If null, it grabs the configuration
        /// from the passed in command-line arguments.
        /// </param>
        /// <returns>
        /// The number of commits that have happend on the current git branch.
        /// </returns>
        public int Run( GitRevisionNumberConfig config = null )
        {
            if( config == null )
            {
                config = ArgumentBinder.FromArguments<GitRevisionNumberConfig>( this.context );
            }

            int revNumber = -1;
            string onStdOut( string line )
            {
                if( string.IsNullOrWhiteSpace( line ) == false )
                {
                    if( int.TryParse( line, out revNumber ) == false )
                    {
                        revNumber = -1;
                    }
                }

                return line;
            };

            ProcessSettings processSettings = new ProcessSettings
            {
                Arguments = ProcessArgumentBuilder.FromString( "rev-list --count HEAD" ),
                RedirectStandardOutput = true,
                RedirectedStandardOutputHandler = onStdOut
            };

            this.Run( this.toolSettings, processSettings.Arguments, processSettings, null );

            if( revNumber < 0 )
            {
                throw new InvalidOperationException(
                    "Could not get rev number from git"
                );
            }

            if( config.NoPrint == false )
            {
                context.Information( "Current Revision Number: " + revNumber );
            }

            if( string.IsNullOrWhiteSpace( config.OutputFile ) == false )
            {
                System.IO.File.WriteAllText( config.OutputFile, revNumber.ToString() );
            }

            return revNumber;
        }
    }
}
