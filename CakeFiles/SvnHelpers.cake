//
//          Copyright Seth Hendrick 2019.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

#load "./ArgumentHelpers.cake"

public class SvnConfig
{
    // ---------------- Fields ----------------

    // ---------------- Constructor ----------------

    public SvnConfig()
    {

    }

    // ---------------- Properties ----------------

    [StringArgument( "svn_exe", Description = "Location of the SVN Executable", Required = true )]

    public string SvnExeLocation{ get; set; }

    [StringArgument( "svn_root", Description = "Location of the local SVN repo.  This usually contains a .SVN folder", Required = true )]
    public string SvnCheckoutRoot{ get; set; }
}

Task( "SVN" )
.Does(
    ( context ) =>
    {
        SvnConfig config = ArgumentHelpers.FromArguments<SvnConfig>( context );

        Information( "EXE: " + config.SvnExeLocation );
        Information( "Root: " + config.SvnCheckoutRoot );
    }
).Description( ArgumentHelpers.GetDescription<SvnConfig>( "Prints SVN stuff" ) );

RunTarget("SVN");