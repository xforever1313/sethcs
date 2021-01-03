//
//          Copyright Seth Hendrick 2019-2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

#addin nuget:?package=Cake.ArgumentBinder&version=0.2.3

// For unit tests
#tool nuget:?package=NUnit.ConsoleRunner&version=3.9.0
#tool nuget:?package=OpenCover&version=4.6.519
#tool nuget:?package=ReportGenerator&version=4.0.10

#load "DeleteHelpers.cake"
#load "MSBuild.cake"
#load "SvnHelpers.cake"
#load "TestRunner.cake"
#load "UnitTestRunner.cake"
