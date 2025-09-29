//
//          Copyright Seth Hendrick 2015-2025.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using Cake.Common.Tools.MSBuild;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Seth.CakeLib;

namespace Tests.CakeLib
{
    [TestClass]
    public sealed class PlatformTargetExtensionsTests
    {
        // ---------------- Tests ----------------

        [TestMethod]
        public void ToDotnetWindowsRidTest()
        {
            // Setup
            const PlatformID platform = PlatformID.Win32NT;

            // Act / Check
            Assert.AreEqual( "win-x64", PlatformTarget.x64.ToDotnetRid( platform ) );
            Assert.AreEqual( "win-x86", PlatformTarget.x86.ToDotnetRid( platform ) );
            Assert.AreEqual( "win-arm", PlatformTarget.ARM.ToDotnetRid( platform ) );
            Assert.AreEqual( "win-arm64", PlatformTarget.ARM64.ToDotnetRid( platform ) );
        }

        [TestMethod]
        public void ToDotnetLinuxRidTest()
        {
            // Setup
            const PlatformID platform = PlatformID.Unix;

            // Act / Check
            Assert.AreEqual( "linux-x64", PlatformTarget.x64.ToDotnetRid( platform ) );
            Assert.AreEqual( "linux-arm", PlatformTarget.ARM.ToDotnetRid( platform ) );
            Assert.AreEqual( "linux-arm64", PlatformTarget.ARM64.ToDotnetRid( platform ) );
        }

        [TestMethod]
        public void ToDebPackageArchitectureTests()
        {
            Assert.AreEqual( "all", PlatformTarget.MSIL.ToDebPackageArchitecture() );
            Assert.AreEqual( "amd64", PlatformTarget.x64.ToDebPackageArchitecture() );
            Assert.AreEqual( "i386", PlatformTarget.x86.ToDebPackageArchitecture() );
            Assert.AreEqual( "armhf", PlatformTarget.ARM.ToDebPackageArchitecture() );
            Assert.AreEqual( "arm64", PlatformTarget.ARM64.ToDebPackageArchitecture() );
        }
    }
}
