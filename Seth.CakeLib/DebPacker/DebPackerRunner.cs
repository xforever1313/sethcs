//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;

namespace Seth.CakeLib.DebPacker
{
    public sealed class DebPackerRunner
    {
        // ---------------- Fields ----------------

        private readonly ICakeContext context;

        // ---------------- Constructor ----------------

        public DebPackerRunner( ICakeContext context )
        {
            this.context = context;
        }

        // ---------------- Functions ----------------

        public static bool CanRun( ICakeContext context )
        {
            bool canRun = context.IsRunningOnLinux();

            if( canRun == false )
            {
                context.Error( "Can only be run on Linux!" );
            }

            return canRun;
        }

        public void DebianPack( DebPackageConfig config )
        {
            // First, create the folders
            DirectoryPath workingDir = config.WorkingDirectory;
            DirectoryPath binDir = workingDir.Combine( new DirectoryPath( "bin" ) );
            DirectoryPath objDir = workingDir.Combine( new DirectoryPath( "obj" ) );
            DirectoryPath debDir = objDir.Combine( new DirectoryPath( "DEBIAN" ) );

            this.context.EnsureDirectoryExists( workingDir );
            this.context.CleanDirectory( workingDir );
            this.context.EnsureDirectoryExists( binDir );
            this.context.EnsureDirectoryExists( objDir );
            this.context.EnsureDirectoryExists( debDir );

            // Next, create control file.
            System.IO.File.WriteAllText(
                debDir.CombineWithFilePath( new FilePath( "control" ) ).ToString(),
                config.GetControlFileContents()
            );

            // Now, invoke the child class to tell it to move its files
            // into the package.
            config.MoveFilesIntoPackage( objDir );

            FilePath outputLocation = binDir.CombineWithFilePath(
                new FilePath( config.GetFullPackageName() )
            );

            // Lastly, build the package.
            ProcessArgumentBuilder arguments = ProcessArgumentBuilder.FromString(
                $"--root-owner-group --build . {outputLocation}"
            );
            ProcessSettings settings = new ProcessSettings
            {
                Arguments = arguments,
                WorkingDirectory = objDir
            };
            int exitCode = context.StartProcess( "dpkg-deb", settings );
            if( exitCode != 0 )
            {
                throw new ApplicationException(
                    "Could not package deb, got exit code: " + exitCode
                );
            }
        }
    }
}
