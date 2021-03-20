//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.Frosting;

namespace DevOps.AppVeyor
{
    [TaskName( "appveyor" )]
    [Dependency( typeof( UnitTests.RunUnitTestTask ) )]
    public class AppVeyorTasks : DevOpsTask
    {
    }
}
