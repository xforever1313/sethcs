Task( "print_git_commit_date")
.Does(
    () =>
    {
        DateTime timeStamp = DateTime.MinValue;
        string onStdOut( string line )
        {
            if( string.IsNullOrWhiteSpace( line ) == false )
            {
                if( DateTime.TryParse( line, out timeStamp ) == false )
                {
                    timeStamp = DateTime.MinValue;
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

        int exitCode = StartProcess( "git", processSettings );
        if( exitCode != 0 )
        {
            throw new InvalidOperationException(
                "Git exited with exit code " + exitCode
            );
        }

        if( timeStamp == DateTime.MinValue )
        {
            throw new InvalidOperationException(
                "Could not get timestamp from git"
            );
        }

        Information( "Last commit was at: " + timeStamp );
    }
);

Task( "print_git_rev_number" )
.Does(
    () =>
    {
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

        int exitCode = StartProcess( "git", processSettings );
        if( exitCode != 0 )
        {
            throw new InvalidOperationException(
                "Git exited with exit code " + exitCode
            );
        }

        if( revNumber < 0 )
        {
            throw new InvalidOperationException(
                "Could not get rev number from git"
            );
        }

        Information( "Current Revision Number: " + revNumber );
    }
);
