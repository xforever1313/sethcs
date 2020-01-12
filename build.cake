//
//          Copyright Seth Hendrick 2019.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

#load "./CakeFiles/Includes.cake"

string target = Argument( "target", "default" );

Task("Hello")
.Does(
    () =>
    {
        Information("Hello World");
    }
);

RunTarget( target );