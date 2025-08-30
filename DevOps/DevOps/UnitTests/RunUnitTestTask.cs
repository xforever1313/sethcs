//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Text;
using Cake.Common;
using Cake.Frosting;
using Seth.CakeLib.TestRunner;

namespace DevOps.UnitTests
{
    [TaskName( "run_unit_tests" )]
    public class RunUnitTestTask : DevOpsTask
    {
        public override void Run( BuildContext context )
        {
            var unitTestConfig = new TestConfig
            {
                ResultsFolder = context.RepoRoot.Combine( "TestResults" ),
                TestCsProject = context.RepoRoot.CombineWithFilePath( "Tests/Tests.csproj" )
            };

            UnitTestRunner runner = new UnitTestRunner( context, unitTestConfig );

            if( context.Argument( "coverage", false ) )
            {
                runner.RunCodeCoverage( "+[*]SethCS*" );
            }
            else
            {
                runner.RunTests();
            }
        }
    }
}
