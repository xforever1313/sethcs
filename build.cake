//
//          Copyright Seth Hendrick 2019-2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

#load "./CakeFiles/Includes.cake"

const string buildTarget = "build";
string target = Argument( "target", buildTarget );
FilePath sln = File( "SethCS.sln" );

Task( buildTarget )
.Does(
    () =>
    {
        DoMsBuild( sln );
    }
);


Task( "run_unit_tests" )
.Does(
    ( context ) =>
    {
        DirectoryPath resultsDir = Directory( "TestResults" );
        UnitTestRunner runner = new UnitTestRunner( context, resultsDir, File( "Tests/Tests.csproj" ) );

        if( context.Argument<bool>( "coverage", false ) )
        {
            runner.RunCodeCoverage( "+[*]SethCS*" );
        }
        else
        {
            runner.RunTests();
        }
    }
).IsDependentOn( buildTarget );

RunTarget( target );
