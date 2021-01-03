//
//          Copyright Seth Hendrick 2019-2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

public class UnitTestRunner : TestRunner
{
    // ---------------- Constructor ----------------

    public UnitTestRunner( ICakeContext context, DirectoryPath resultsFolder, FilePath testProject ) :
        base(
            context,
            resultsFolder,
            "UnitTests",
            testProject
        )
    {
    }
}
