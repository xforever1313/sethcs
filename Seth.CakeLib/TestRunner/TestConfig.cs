//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.IO;

namespace Seth.CakeLib.TestRunner
{
    public class TestConfig
    {
        // ---------------- Properties ----------------

        public DirectoryPath ResultsFolder { get; set; }

        public FilePath TestCsProject { get; set; }
    }
}
