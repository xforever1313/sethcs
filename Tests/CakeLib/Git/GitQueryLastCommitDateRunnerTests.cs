//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Seth.CakeLib.Git.QueryLastCommitDate;

namespace Tests.CakeLib.Git
{
    [TestClass]
    public sealed class GitQueryLastCommitDateRunnerTests
    {
        [TestMethod]
        public void ParseTest()
        {
            // Setup
            const string timeStamp = "2025-08-30T17:00:00-04:00";
            var expectedTime = new DateTimeOffset( 2025, 8, 30, 17, 00, 00, new TimeSpan( -4, 0, 0 ) );

            // Act
            DateTimeOffset? actualTime = GitQueryLastCommitDateRunner.TryParse( timeStamp );

            // Check
            Assert.IsNotNull( actualTime );
            Assert.AreEqual( expectedTime, actualTime.Value );
        }
    }
}
