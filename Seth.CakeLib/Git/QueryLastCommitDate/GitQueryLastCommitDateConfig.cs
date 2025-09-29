//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.ArgumentBinder;

namespace Seth.CakeLib.Git.QueryLastCommitDate
{
    public sealed class GitQueryLastCommitDateConfig : BaseGitQueryTask
    {
        // ---------------- Properties ----------------

        [StringArgument(
            "format",
            DefaultValue = null,
            Description = "What format to pass into " + nameof(DateTime) + "." + nameof(Object.ToString) +
                          ".  Don't specify to call the default " + nameof(Object.ToString) + "function."
        )]
        public string DateTimeFormat { get; set; }
    }
}
