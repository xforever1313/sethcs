//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Text.RegularExpressions;
using Cake.Common.IO;
using Cake.Common.Solution;
using Cake.Core;
using Cake.Core.IO;
using SethCS.Exceptions;
using SethCS.Extensions;

namespace Seth.CakeLib
{
    public static class SolutionProjectHelpers
    {
        // ---------------- Functions ----------------

        /// <summary>
        /// Performs an action on all of the project files contained within a solution.
        /// </summary>
        /// <param name="filter">
        /// Use this to filter out which <see cref="SolutionProject"/> you do not
        /// want to perform the action on.  Have this return false to not
        /// perform the action on the passed in <see cref="SolutionProject"/>
        /// </param>
        public static void PerformActionOnSolutionProjectFiles(
            this ICakeContext context,
            FilePath solutionPath,
            Action<SolutionProject> action,
            Func<SolutionProject, bool> filter = null
        )
        {
            ArgumentChecker.IsNotNull( action, nameof( action ) );

            SolutionParserResult slnResult = context.ParseSolution( solutionPath );
            slnResult.Projects.SerialPerformActionOnList(
                delegate ( SolutionProject project )
                {
                    if( filter != null && filter( project ) )
                    {
                        action( project );
                    }
                },
                $"Errors when parsing a {nameof( SolutionProject )}"
            );
        }

        /// <summary>
        /// Performs an action on all of the .csproj files contained within a solution.
        /// </summary>
        /// <param name="filter">
        /// Use this to filter out which <see cref="SolutionProject"/> you do not
        /// want to perform the action on.  Have this return false to not
        /// perform the action on the passed in <see cref="SolutionProject"/>
        /// </param>
        public static void PerformActionOnSolutionCsProjectFiles(
            this ICakeContext context,
            FilePath solutionPath,
            Action<SolutionProject> action,
            Func<SolutionProject, bool> filter = null
        )
        {
            bool csProjFilter( SolutionProject project )
            {
                if( project.Path.ToString().EndsWithIgnoreCase( ".csproj" ) )
                {
                    if( filter != null )
                    {
                        return filter( project );
                    }
                    else
                    {
                        return true;
                    }
                }

                return false;
            }

            PerformActionOnSolutionProjectFiles( context, solutionPath, action, csProjFilter );
        }

        /// <summary>
        /// Performs an action on all c# source files (.cs)
        /// in the solution.
        /// </summary>
        /// <param name="csFilefilter">
        /// Use this to filter out which <see cref="FilePath"/> you do not
        /// want to perform the action on.  Have this return false to not
        /// perform the action on the passed in <see cref="FilePath"/>
        /// 
        /// The default fitler (if this is null) will filter out any .cs projects in a 'bin' or 'obj'
        /// directory.  This must not be null if you don't want that to happen.
        /// </param>
        /// <param name="slnProjectFilter">
        /// Use this to filter out which <see cref="SolutionProject"/> you do not
        /// want to perform the action on.  Have this return false to not
        /// perform the action on the passed in <see cref="SolutionProject"/>
        /// </param>
        public static void PerformActionOnSolutionCsFiles(
            this ICakeContext context,
            FilePath solutionPath,
            Action<FilePath> action,
            Func<FilePath, bool> csFilefilter = null,
            Func<SolutionProject, bool> slnProjectFilter = null
        )
        {
            if( csFilefilter == null )
            {
                csFilefilter = DefaultCsFileFilter;
            }

            PerformActionOnSolutionCsProjectFiles(
                context,
                solutionPath,
                delegate ( SolutionProject project )
                {
                    string glob = project.Path.GetDirectory() + "/**/*.cs";
                    context.GetFiles( glob ).SerialPerformActionOnList(
                        delegate ( FilePath csFile )
                        {
                            if( csFilefilter( csFile ) )
                            {
                                action( csFile );
                            }
                        },
                        "Error when processing a .cs file"
                    );
                },
                slnProjectFilter
            );
        }

        public static bool DefaultCsFileFilter( FilePath path )
        {
            if( Regex.IsMatch( path.ToString(), @"[/\\]obj[/\\]" ) )
            {
                return false;
            }
            if( Regex.IsMatch( path.ToString(), @"[/\\]bin[/\\]" ) )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
