//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Seth.CakeLib.Git
{
    public abstract class GitRunner : Tool<GitToolSettings>
    {
        // ---------------- Constructor ----------------

        protected GitRunner( ICakeContext context ) :
            this( context.FileSystem, context.Environment, context.ProcessRunner, context.Tools )
        {

        }

        protected GitRunner( IFileSystem fileSystem, ICakeEnvironment cakeEnv, IProcessRunner processRunner, IToolLocator toolLocator ) :
            base( fileSystem, cakeEnv, processRunner, toolLocator )
        {
        }

        // ---------------- Functions ----------------

        protected override string GetToolName()
        {
            return "Git";
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[]
            {
                "git",
                "git.exe"
            };
        }
    }
}
