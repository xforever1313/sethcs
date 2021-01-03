//
//          Copyright Seth Hendrick 2019-2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

public class TestRunner
{
    // ---------------- Fields ----------------

    private readonly ICakeContext cakeContext;
    
    private readonly DirectoryPath resultsDir;
    private readonly FilePath testProject;

    // ---------------- Constructor ----------------

    public TestRunner(
        ICakeContext context,
        DirectoryPath resultFolder,
        string testContext,
        FilePath testCsProjectPath
    )
    {
        this.cakeContext = context;

        this.resultsDir = resultFolder.Combine( new DirectoryPath( testContext ) );
        this.testProject = testCsProjectPath;
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
            this.resultsDir.CombineWithFilePath( this.testProject.GetFilenameWithoutExtension() ).ToString() + ".xml"
        );

        DotNetCoreTestSettings settings = new DotNetCoreTestSettings
        {
            NoBuild = true,
            NoRestore = true,
            Configuration = "Debug",
            ResultsDirectory = this.resultsDir,
            VSTestReportPath = resultFile,
            Verbosity = DotNetCoreVerbosity.Normal
        };

        // Need to restore to download the TestHost, which is a NuGet package.

        context.Information( "Restoring..." );
        context.DotNetCoreRestore( this.testProject.ToString() );
        context.Information( "Restoring... Done!" );

        context.Information( "Running Tests..." );
        context.DotNetCoreTest( this.testProject.ToString(), settings );
        context.Information( "Running Tests... Done!" );
    }

    public void RunCodeCoverage( string filter )
    {
        CreateResultsDir();
        DirectoryPath codeCoveragePath = this.resultsDir.Combine( new DirectoryPath( "CodeCoverage" ) );
        FilePath outputPath = codeCoveragePath.CombineWithFilePath( new FilePath( "coverage.xml" ) );
        this.cakeContext.EnsureDirectoryExists( codeCoveragePath );
        this.cakeContext.CleanDirectory( codeCoveragePath );

        OpenCoverSettings settings = new OpenCoverSettings
        {
            Register = "user",
            ReturnTargetCodeOffset = 0,
            OldStyle = true // This is needed or MissingMethodExceptions get thrown everywhere for some reason.
        }
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
