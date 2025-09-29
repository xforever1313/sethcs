//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.ArgumentBinder;

namespace Seth.CakeLib.Git
{
    public abstract class BaseGitQueryTask
    {
        // ---------------- Properties ----------------

        [StringArgument(
            "output_file",
            DefaultValue = null,
            Description = "Where to output the git query to.  Don't specify to not write a file",
            Required = false
        )]
        public string OutputFile { get; set; }

        [BooleanArgument(
            "no_print",
            DefaultValue = false,
            Description = "Set to true to not print the value from git to the console"
        )]
        public bool NoPrint { get; set; }
    }
}
