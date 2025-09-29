//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Globalization;
using Cake.ArgumentBinder;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;

namespace Seth.CakeLib.Git.QueryLastCommitDate
{
    public class GitQueryLastCommitDateRunner : GitRunner
    {
        // ---------------- Fields ----------------

        private readonly ICakeContext context;
        private readonly GitToolSettings toolSettings;

        internal const string dateTimeFormat = "yyyy-MM-dd'T'HH:mm:sszzz";

        // ---------------- Constructor ----------------

        public GitQueryLastCommitDateRunner(
            ICakeContext context,
            GitToolSettings toolSettings = null
        ) : base( context )
        {
            this.context = context;
            this.toolSettings = toolSettings ?? new GitToolSettings();
        }

        // ---------------- Methods ----------------

        /// <summary>
        /// Runs git on the local repository and returns
        /// the <see cref="DateTimeOffset"/> of the last commit.
        /// </summary>
        /// <param name="config">
        /// Configuration.  If null, it grabs the configuration
        /// from the passed in command-line arguments.
        /// </param>
        public DateTimeOffset Run( GitQueryLastCommitDateConfig config = null )
        {
            if( config == null )
            {
                config = ArgumentBinder.FromArguments<GitQueryLastCommitDateConfig>( this.context );
            }

            DateTimeOffset? timeStamp = null;
            string onStdOut( string line )
            {
                if( string.IsNullOrWhiteSpace( line ) == false )
                {
                    if( timeStamp is not null )
                    {
                        timeStamp = TryParse( line );
                    }
                }

                return line;
            };

            ProcessSettings processSettings = new ProcessSettings
            {
                Arguments = ProcessArgumentBuilder.FromString( "show -s --format=%cI" ),
                RedirectStandardOutput = true,
                RedirectedStandardOutputHandler = onStdOut
            };

            this.Run( this.toolSettings, processSettings.Arguments, processSettings, null );

            if( timeStamp == null )
            {
                throw new InvalidOperationException(
                    "Could not get timestamp from git"
                );
            }

            string timeStampStr;
            if( string.IsNullOrEmpty( config.DateTimeFormat ) )
            {
                timeStampStr = timeStamp.Value.ToString();
            }
            else
            {
                timeStampStr = timeStamp.Value.ToString( config.DateTimeFormat );
            }

            if( config.NoPrint == false )
            {
                context.Information( $"Last commit was at: {timeStampStr}" );
            }

            if( string.IsNullOrWhiteSpace( config.OutputFile ) == false )
            {
                System.IO.File.WriteAllText( config.OutputFile, timeStampStr );
            }

            return timeStamp.Value;
        }

        internal static DateTimeOffset? TryParse( string input )
        {
            if( DateTimeOffset.TryParseExact( input, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset foundTimeStamp ) )
            {
                return foundTimeStamp;
            }
            else
            {
                return null;
            }
        }
    }
}
