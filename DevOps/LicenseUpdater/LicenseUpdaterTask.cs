//
//          Copyright Seth Hendrick 2019-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cake.Common.IO;
using Cake.Common.Solution;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.LicenseHeaderUpdater;

namespace DevOps.LicenseUpdater
{
    [TaskName( "update_licenses" )]
    [TaskDescription( "Updates all the license headers in all .cs projects" )]
    public class LicenseUpdaterTask : DevOpsTask
    {
        // ---------------- Fields ----------------

        const string currentLicense =
@"//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

";

        // Used to be boost before Feb 28th 2021 before switching to GPL.
        const string oldLicenseRegex1 =
@"//
//\s+Copyright\s+Seth\s+Hendrick\s+\d+-?\d*\.?
//\s+Distributed\s+under\s+the\s+Boost\s+Software\s+License,\s+Version\s+1\.0\.?
//\s+\(See\s+accompanying\s+file\s+[./]*LICENSE_1_0\.txt\s+or\s+copy\s+at
//\s+http://www.boost.org/LICENSE_1_0\.txt\)
//[\n\r\s]*";

        const string oldLicenseRegex2 =
@"(//)?
//\s+Copyright\s+Seth\s+Hendrick\s+[^\n\r]+\.?
//\s+Distributed\s+under\s+the\s+Boost\s+Software\s+License,\s+Version\s+1\.0\.?
//\s+\(See\s+accompanying\s+file\s+[./]*LICENSE_1_0\.txt\s+or\s+copy\s+at
//\s+http://www.boost.org/LICENSE_1_0\.txt\)
(//)?[\n\r\s]*";

        // ---------------- Functions ----------------

        public override void Run( BuildContext context )
        {
            CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings
            {
                LicenseString = currentLicense,
                Threads = 0,
            };

            settings.OldHeaderRegexPatterns.Add( oldLicenseRegex1 );
            settings.OldHeaderRegexPatterns.Add( oldLicenseRegex2 );

            settings.FileFilter = delegate ( FilePath path )
            {
                if( Regex.IsMatch( path.ToString(), @"[/\\]obj[/\\]" ) )
                {
                    return false;
                }
                if( Regex.IsMatch( path.ToString(), @"[/\\]bin[/\\]" ) )
                {
                    return false;
                }
                if( Regex.IsMatch( path.ToString(), "[Ss]eth[Cc][Ss]" ) )
                {
                    return false;
                }
                else
                {
                    return true;
                }
            };

            List<FilePath> files = new List<FilePath>();

            SolutionParserResult slnResult = context.ParseSolution( context.Solution );
            foreach( SolutionProject proj in slnResult.Projects )
            {
                if( proj.Path.ToString().EndsWith( ".csproj" ) )
                {
                    string glob = proj.Path.GetDirectory() + "/**/*.cs";
                    files.AddRange( context.GetFiles( glob ) );
                }
            }

            context.UpdateLicenseHeaders( files, settings );
        }
    }
}
