//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.ArgumentBinder;
using Cake.Core.IO;

namespace Seth.CakeLib.DeleteHelpers
{
    public class DeleteHelpersConfig
    {
        // ---------------- Constructor ----------------

        public DeleteHelpersConfig()
        {
        }

        // ---------------- Properties ----------------

        /// <summary>
        /// The directory to delete things from.
        /// </summary>
        [StringArgument(
            "path",
            Description = "The path to delete from.",
            Required = true
        )]
        public string Directory { get; set; }

        /// <summary>
        /// The number of files or directories 
        /// to keep that match the given pattern.
        /// Defaulted to 0.
        /// Can not be negative.
        /// </summary>
        [IntegerArgument(
            "num_to_keep",
            Description = "The number of the most recent files/directories to keep that match the pattern.",
            DefaultValue = 0,
            Min = 0,
            Max = 255
        )]
        public int NumberOfFilesToKeep { get; set; }

        /// <summary>
        /// The glob of the deletion pattern to use.
        /// </summary>
        [StringArgument(
            "pattern",
            Description = "The glob pattern to delete files/directories from.",
            DefaultValue = "*"
        )]
        public string DeletionPattern { get; set; }

        [BooleanArgument(
            "dry_run",
            Description = "Set to 'true' to not delete any files, this will simply print what files will be deleted.",
            DefaultValue = false
        )]
        public bool DryRun { get; set; }

        public DirectoryPath FullDirectory
        {
            get
            {
                DirectoryPath baseDir = new DirectoryPath( this.Directory );
                DirectoryPath globPath = new DirectoryPath( this.DeletionPattern );

                return baseDir.Combine( globPath );
            }
        }
    }
}
