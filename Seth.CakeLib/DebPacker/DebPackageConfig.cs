//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.Common.Tools.MSBuild;
using Cake.Core.IO;

namespace Seth.CakeLib.DebPacker
{
    public abstract class DebPackageConfig
    {
        // ---------------- Properties ----------------

        public abstract string PackageName { get; }

        public abstract Version PackageVersion { get; }

        public abstract string Maintainer { get; }

        public abstract PlatformTarget Architecture { get; }

        public abstract string Description { get; }
       
        public abstract string Homepage { get; }

        /// <summary>
        /// The package revision.  This should only be incremented
        /// if the package contents (e.g. the contents of the DEBIAN folder)
        /// changed, but the underlying software the package contains
        /// version did not change.
        /// 
        /// Each new version of software should reset this count to 0.
        /// </summary>
        public abstract uint PackageRevision { get; }

        /// <summary>
        /// The target operating system.
        /// This is really only needed if there is native code contained within the package
        /// compiled on a specific operating system.
        /// 
        /// Examples of this could be ubuntu~22.04 or ubuntu~20.04.
        /// 
        /// Leave this empty string if there is no need to have a package target
        /// a specific OS.
        /// </summary>
        public abstract string TargetOperatingSystem { get; }

        /// <summary>
        /// Where to put create the obj and bin folder
        /// when creating a deb package.
        /// </summary>
        public abstract DirectoryPath WorkingDirectory { get; }

        // ---------------- Functions ----------------

        /// <summary>
        /// Override this method to put needed files inside
        /// of the package.
        /// </summary>
        /// <param name="packageRoot">
        /// The path the maps to the root filesystem.
        /// This is the same folder that contains the DEBIAN folder.
        /// </param>
        public virtual void MoveFilesIntoPackage( DirectoryPath packageRoot )
        {
        }

        public string GetFullPackageName()
        {
            // Taken from here: https://www.debian.org/doc/manuals/debian-faq/pkg-basics.en.html
            return $"{this.PackageName}_{this.PackageVersion.ToString( 3 )}-{PackageRevision}{TargetOperatingSystem}_{this.Architecture.ToDebPackageArchitecture()}.deb";
        }

        public string GetControlFileContents()
        {
            return
$@"Package: {this.PackageName}
Version: {this.PackageVersion.ToString( 3 )}
Maintainer: {this.Maintainer}
Architecture: {this.Architecture.ToDebPackageArchitecture()}
Description: {this.Description}
Homepage: {this.Homepage}
";
        }
    }
}
