//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Common.Tools.OpenCover;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core;
using Cake.Core.IO;

namespace Seth.CakeLib.TestRunner
{
    public class BaseTestRunner
    {
        // ---------------- Fields ----------------

        private readonly ICakeContext cakeContext;

        private readonly TestConfig testConfig;
        private readonly DirectoryPath resultsDir;

        // ---------------- Constructor ----------------

        public BaseTestRunner( 
            ICakeContext context,
            TestConfig testConfig,
            string testContext
        )
        {
            this.cakeContext = context;
            this.testConfig = testConfig;

            this.resultsDir = this.testConfig.ResultsFolder.Combine( new DirectoryPath( testContext ) );
        }

        // ---------------- Functions ----------------

        public void RunTests()
        {
            CreateResultsDir();
            this.RunTestsInternal( this.cakeContext );
        }

        private void RunTestsInternal( ICakeContext context )
        {
            FilePath resultFile = new FilePath(
                this.resultsDir.CombineWithFilePath( this.testConfig.TestCsProject.GetFilenameWithoutExtension() ).ToString() + ".xml"
            );

            var settings = new DotNetTestSettings
            {
                NoBuild = true,
                NoRestore = true,
                Configuration = "Debug",
                ResultsDirectory = this.resultsDir,
                VSTestReportPath = resultFile,
                Verbosity = DotNetVerbosity.Normal
            };

            // Need to restore to download the TestHost, which is a NuGet package.

            context.Information( "Restoring..." );
            context.DotNetRestore( this.testConfig.TestCsProject.ToString() );
            context.Information( "Restoring... Done!" );

            context.Information( "Running Tests..." );
            context.DotNetTest( this.testConfig.TestCsProject.ToString(), settings );
            context.Information( "Running Tests... Done!" );
        }

        /// <remarks>
        /// Please ensure the open cover and report generator tools
        /// are installed when creating the <see cref="Cake.Frosting.CakeHost"/>
        /// in your Program file, or this won't work.
        /// </remarks>
        public void RunCodeCoverage( string filter )
        {
            CreateResultsDir();
            DirectoryPath codeCoveragePath = this.resultsDir.Combine( new DirectoryPath( "CodeCoverage" ) );
            FilePath outputPath = codeCoveragePath.CombineWithFilePath( new FilePath( "coverage.xml" ) );
            this.cakeContext.EnsureDirectoryExists( codeCoveragePath );
            this.cakeContext.CleanDirectory( codeCoveragePath );

            OpenCoverSettings settings = new OpenCoverSettings
            {
                ReturnTargetCodeOffset = 0,
                OldStyle = true // This is needed or MissingMethodExceptions get thrown everywhere for some reason.
            }
            .WithRegisterUser()
            .WithFilter( filter );

            this.cakeContext.OpenCover(
                context => this.RunTestsInternal( context ),
                outputPath,
                settings
            );

            this.cakeContext.ReportGenerator( outputPath, codeCoveragePath );
        }

        private void CreateResultsDir()
        {
            DirectoryPath outputPath = this.resultsDir;
            this.cakeContext.EnsureDirectoryExists( outputPath );
            this.cakeContext.CleanDirectory( outputPath );
        }
    }
}
