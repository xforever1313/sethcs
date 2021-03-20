//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

// ----------------- Constants ----------------

const string devopsTarget = "run_devops";
const string buildTask = "build";
bool forceBuild = Argument<bool>( "force_build", false );

string target = Argument( "target", buildTask );

FilePath devopsExe = File( "./DevOps/bin/Debug/netcoreapp3.1/DevOps.dll" );

FilePath sln = File( "./SethCS.sln" );

// ----------------- Build Targets ----------------

Task( buildTask )
.Does(
    () =>
    {
        if( forceBuild == false )
        {
            if( target != buildTask )
            {
                Information( "DevOps.dll not found, compiling" );
            }
        }

        DotNetCoreBuildSettings settings = new DotNetCoreBuildSettings
        {
        };

        DotNetCoreBuild( sln.ToString(), settings );
    }
);

var runTask = Task( devopsTarget )
.Does(
    () =>
    {
        List<string> args = new List<string>( System.Environment.GetCommandLineArgs() );
        args.RemoveAt( 0 );
        args.Insert( 0, devopsExe.ToString() );

        ProcessSettings processSettings = new ProcessSettings
        {
            Arguments = ProcessArgumentBuilder.FromStrings( args )
        };

        int exitCode = StartProcess( "dotnet", processSettings );
        if( exitCode != 0 )
        {
            throw new Exception( $"DevOps.exe Exited with exit code: {exitCode}" );
        }
    }
);

if( forceBuild || ( FileExists( devopsExe ) == false ) )
{
    runTask.IsDependentOn( buildTask );
}

// ---------------- Run ----------------

if( target == buildTask )
{
    RunTarget( target );
}
else
{
    RunTarget( devopsTarget );
}
