//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace DevOps
{
    public class BuildContext : FrostingContext
    {
        // ---------------- Constructor ----------------

        public BuildContext( ICakeContext context ) :
            base( context )
        {
            this.RepoRoot = context.Environment.WorkingDirectory;
            this.SrcDir = this.RepoRoot.Combine( "src" );
            this.Solution = this.RepoRoot.CombineWithFilePath( "SethCS.sln" );
            this.DistFolder = this.RepoRoot.Combine( "dist" );
        }

        // ---------------- Properties ----------------

        public DirectoryPath RepoRoot { get; private set; }

        public DirectoryPath SrcDir { get; private set; }

        public FilePath Solution { get; private set; }

        public DirectoryPath DistFolder { get; private set; }
    }
}
