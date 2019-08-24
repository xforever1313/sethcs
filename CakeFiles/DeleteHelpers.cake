//
//          Copyright Seth Hendrick 2019.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

// ---------------- Classes ----------------

public class DeleteHelpers
{
    // ---------------- Arguments ----------------

    public const string DeleteDirArg = "deletion_directory";
    public const string NumFilesToKeepArg = "num_to_keep";
    public const string PatternArg = "deletion_pattern";

    // ---------------- Fields ----------------

    private const string defaultPattern = "*";

    private readonly ICakeContext cakeContext;

    // ---------------- Constructor ----------------

    public DeleteHelpers( ICakeContext cakeContext )
    {
        this.cakeContext = cakeContext;
        
        string dir = cakeContext.Argument( DeleteHelpers.DeleteDirArg, string.Empty );
        this.Directory = new DirectoryPath( dir );
        this.NumberOfFilesToKeep = cakeContext.Argument<int>( DeleteHelpers.NumFilesToKeepArg, 0 );
        if( this.NumberOfFilesToKeep < 0 )
        {
            this.NumberOfFilesToKeep = 0;
        }
        this.DeletionPattern = cakeContext.Argument( DeleteHelpers.PatternArg, DeleteHelpers.defaultPattern );
    }

    // ---------------- Properties ----------------

    /// <summary>
    /// The directory to delete things from.
    /// </summary>
    public DirectoryPath Directory { get; private set; }

    /// <summary>
    /// The number of files or directories 
    /// to keep that match the given pattern.
    /// Defaulted to 0.
    /// Can not be negative.
    /// </summary>
    public int NumberOfFilesToKeep { get; private set; }

    /// <summary>
    /// The glob of the deletion pattern to use.
    /// </summary>
    public string DeletionPattern { get; private set; }

    // ---------------- Functions ----------------

    public void DeleteDirectories()
    {
        DirectoryPathCollection dirs = this.cakeContext.GetDirectories( this.DeletionPattern );
        List<DirectoryPath> orderedDirs = dirs.OrderBy( f => System.IO.Directory.GetCreationTime( f.ToString() ) ).ToList();
        
        while( orderedDirs.Count > this.NumberOfFilesToKeep )
        {
            DirectoryPath dir = orderedDirs[0];
            this.cakeContext.Information( $"Deleting '{dir.ToString()}'" );

            DeleteDirectorySettings dirSettings = new DeleteDirectorySettings
            {
                Force = true,
                Recursive = true
            };
            
            this.cakeContext.DeleteDirectory( dir, dirSettings );
            orderedDirs.RemoveAt( 0 );
        }
    }

    public void DeleteFiles()
    {
        FilePathCollection files = this.cakeContext.GetFiles( this.DeletionPattern );
        List<FilePath> orderedFiles = files.OrderBy( f => System.IO.File.GetCreationTime( f.ToString() ) ).ToList();
        
        while( orderedFiles.Count > this.NumberOfFilesToKeep )
        {
            FilePath file = orderedFiles[0];
            this.cakeContext.Information( $"Deleting '{file.ToString()}'" );
            this.cakeContext.DeleteFile( file );
            orderedFiles.RemoveAt( 0 );
        }
    }
}

// ---------------- Tasks ----------------

Task( "delete_files" )
.Does(
    ( context ) =>
    {
        DeleteHelpers helpers = new DeleteHelpers( context );
        helpers.DeleteFiles();
    }
)
.Description(
@"Deletes files within a specific directory.
Arguments:
    - " + $"{DeleteHelpers.DeleteDirArg}: The path to delete files from (required)." + @"
    - " + $"{DeleteHelpers.NumFilesToKeepArg}: The number of the most recent files" + @"
                                 to keep that match the pattern.
                                 Defaulted to 0.  Can not be negative.
    - " + $"{DeleteHelpers.PatternArg}: The glob pattern to delete files from." + @"
                        Defaulted to '*'
"
);

Task( "delete_dirs" )
.Does(
    ( context ) =>
    {
        DeleteHelpers helpers = new DeleteHelpers( context );
        helpers.DeleteDirectories();
    }
)
.Description(
@"Deletes directories within a specific directory.
Arguments:
    - " + $"{DeleteHelpers.DeleteDirArg}: The path to delete directories from (required)." + @"
    - " + $"{DeleteHelpers.NumFilesToKeepArg}: The number of the most recent files" + @"
                                 to keep that match the pattern.
                                 Defaulted to 0.  If negative, this becomes 0.
    - " + $"{DeleteHelpers.PatternArg}: The glob pattern to delete files from." + @"
                        Defaulted to '*'
"
);