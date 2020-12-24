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

RunTarget( target );
