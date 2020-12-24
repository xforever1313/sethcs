//
//          Copyright Seth Hendrick 2019-2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

/// <summary>
/// Calls MSBuild to compile
/// </summary>
/// <param name="configuration">The configuration to use (e.g. Debug, Release, etc.).</param>
void DoMsBuild( FilePath sln, string configuration = "Debug" )
{
    DotNetCoreMSBuildSettings msBuildSettings = new DotNetCoreMSBuildSettings
    {
        WorkingDirectory = sln.GetDirectory()
    }
    .SetMaxCpuCount( System.Environment.ProcessorCount )
    .SetConfiguration( configuration );

    DotNetCoreBuildSettings settings = new DotNetCoreBuildSettings
    {
        MSBuildSettings = msBuildSettings
    };

    DotNetCoreBuild( sln.ToString(), settings );
}
