//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.ArgumentBinder;
using Cake.Core.IO;

namespace Seth.CakeLib.DirectoryCleaner
{
    public class CleanDirectoryConfig
    {
        // ---------------- Constructor ----------------

        public CleanDirectoryConfig()
        {
        }

        // ---------------- Properties ----------------

        [DirectoryPathArgument(
            "path",
            Description = "Path to the directory to clean.  It will be created if it does not exist",
            Required = true,
            MustExist = false
        )]
        public DirectoryPath Path { get; set; }

        // ---------------- Functions ----------------

        public override string ToString()
        {
            return ArgumentBinder.ConfigToStringHelper( this );
        }
    }
}
