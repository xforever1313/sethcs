//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using System.Linq;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;

namespace Seth.CakeLib.DeleteHelpers
{
    public static class DeleteRunner
    {
        public static void DeleteDirectories( this ICakeContext cakeContext, DeleteHelpersConfig config )
        {
            DirectoryPathCollection dirs = cakeContext.GetDirectories( config.FullDirectory.ToString() );
            List<DirectoryPath> orderedDirs = dirs.OrderBy( f => System.IO.Directory.GetCreationTime( f.ToString() ) ).ToList();

            while( orderedDirs.Count > config.NumberOfFilesToKeep )
            {
                DirectoryPath dir = orderedDirs[0];
                cakeContext.Information( $"Deleting '{dir}'" );

                if( config.DryRun == false )
                {
                    DeleteDirectorySettings dirSettings = new DeleteDirectorySettings
                    {
                        Force = true,
                        Recursive = true
                    };

                    cakeContext.DeleteDirectory( dir, dirSettings );
                }
                orderedDirs.RemoveAt( 0 );
            }
        }

        public static void DeleteFiles( this ICakeContext cakeContext, DeleteHelpersConfig config )
        {
            FilePathCollection files = cakeContext.GetFiles( config.FullDirectory.ToString() );
            List<FilePath> orderedFiles = files.OrderBy( f => System.IO.File.GetCreationTime( f.ToString() ) ).ToList();

            while( orderedFiles.Count > config.NumberOfFilesToKeep )
            {
                FilePath file = orderedFiles[0];
                cakeContext.Information( $"Deleting '{file}'" );
                if( config.DryRun == false )
                {
                    cakeContext.DeleteFile( file );
                }
                orderedFiles.RemoveAt( 0 );
            }
        }
    }
}
