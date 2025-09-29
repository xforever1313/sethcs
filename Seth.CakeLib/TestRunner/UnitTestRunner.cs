//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.Core;

namespace Seth.CakeLib.TestRunner
{
    public class UnitTestRunner : BaseTestRunner
    {
        // ---------------- Cosntructor ----------------

        public UnitTestRunner( ICakeContext context, TestConfig testConfig) :
            base(
                context,
                testConfig,
                "UnitTests"
            )
        {
        }
    }
}
