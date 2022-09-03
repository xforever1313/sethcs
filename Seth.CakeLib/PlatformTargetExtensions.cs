//
//          Copyright Seth Hendrick 2015-2021.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.Common.Tools.MSBuild;

namespace Seth.CakeLib
{
    public static class PlatformTargetExtensions
    {
        /// <summary>
        /// Gets the Dotnet RID for the given CPU architecture and
        /// platform ID.
        /// 
        /// This returns the most generic RID for the given combination.
        /// To get specific (e.g. directly target Windows 10), this method
        /// isn't the thing for you to use.
        /// </summary>
        /// <remarks>
        /// RID catalog is located here:
        /// https://docs.microsoft.com/en-us/dotnet/core/rid-catalog.
        /// </remarks>
        /// <param name="platformId">
        /// Which platform to target.  This only supports Windows and Linux,
        /// Mac OS and mobile devices not supported!
        /// </param>
        /// <returns>
        /// Empty string if our target <see cref="PlatformTarget.MSIL"/>,
        /// to represent Any CPU, as there are no RIDs that target an OS
        /// but for Any CPU.
        /// 
        /// Otherwise, returns the RID that can be passed into the "--runtime"
        /// parameter of "dotnet publish".
        /// </returns>
        public static string ToDotnetRid( this PlatformTarget target, PlatformID platformId )
        {
            if( target == PlatformTarget.MSIL )
            {
                // MSIL, or Any CPU, means there is no specific RID.
                // return empty string since nothing should be passed into
                // the "--runtime" argument.
                return "";
            }
            else if( platformId == PlatformID.Win32NT )
            {
                if( target == PlatformTarget.x64 )
                {
                    return "win-x64";
                }
                else if( target == PlatformTarget.x86 )
                {
                    return "win-x86";
                }
                else if( target == PlatformTarget.ARM )
                {
                    return "win-arm";
                }
                else if( target == PlatformTarget.ARM64 )
                {
                    return "win-arm64";
                }
                else
                {
                    throw new ArgumentException(
                        $"{nameof( PlatformTarget )}.{target} is not compatible with {nameof( PlatformID )}.{platformId}"
                    );
                }
            }
            else if( platformId == PlatformID.Unix )
            {
                if( target == PlatformTarget.x64 )
                {
                    return "linux-x64";
                }
                else if( target == PlatformTarget.ARM )
                {
                    return "linux-arm";
                }
                else if( target == PlatformTarget.ARM64 )
                {
                    return "linux-arm64";
                }
                // There does not appear to be a linux-x86 RID according to
                // Microsoft's catalog.
                else
                {
                    throw new ArgumentException(
                        $"{nameof( PlatformTarget )}.{target} is not compatible with {nameof( PlatformID )}.{platformId}"
                    );
                }
            }
            else
            {
                throw new ArgumentException(
                    $"This method does not support {nameof( PlatformID )}.{platformId}",
                    nameof( platformId )
                );
            }
        }

        /// <summary>
        /// Takes in a given <see cref="PlatformTarget"/> and converts
        /// it to the equivalent .deb package's architecture string.
        /// </summary>
        /// <remarks>
        /// Strings can be gotten by running "dpkg-architecture --list-known"
        /// on an Ubuntu machine.  You may need to install the
        /// "dpkg-dev" package in order to use this command first, however.
        /// </remarks>
        public static string ToDebPackageArchitecture( this PlatformTarget target )
        {
            if( target == PlatformTarget.x64 )
            {
                return "amd64";
            }
            else if( target == PlatformTarget.x86 )
            {
                return "i386";
            }
            else if( target == PlatformTarget.ARM )
            {
                // Per this GitHub issue, ARM in Dotnet
                // is armhf:
                // https://github.com/dotnet/core/issues/1758
                return "armhf";
            }
            else if( target == PlatformTarget.ARM64 )
            {
                return "arm64";
            }
            else
            {
                throw new ArgumentException(
                    $"This method does not support {nameof( PlatformTarget )}.{target}",
                    nameof( target )
                );
            }
        }
    }
}
