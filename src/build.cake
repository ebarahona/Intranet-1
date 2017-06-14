﻿//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
#addin "Cake.Npm"

var target = Argument("target", "Default");
var configuration =
    HasArgument("Configuration") ? Argument<string>("Configuration") :
    EnvironmentVariable("Configuration") != null ? EnvironmentVariable("Configuration") :
    "Release";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var apiDir = Directory("./Intranet.API/Intranet.API");
var apiTestsDir = Directory("./Intranet.API/Intranet.API.UnitTests");

var webDir = Directory("./Intranet.Web/Intranet.Web");
var webTestsDir = Directory("./Intranet.Web/Intranet.Web.UnitTests");

// Define settings.
var buildSettings = new DotNetCoreBuildSettings
{
    Configuration = configuration,
};

var testSettings = new DotNetCoreTestSettings
{
    Configuration = configuration,
    NoBuild = true
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("API:Clean")
    .Does(() =>
{
    CleanDirectories(GetDirectories("./Intranet.API/**/bin"));
});

Task("Web:Clean")
    .Does(() =>
{
    CleanDirectories(GetDirectories("./Intranet.Web/**/bin"));
    CleanDirectory(Directory("./Intranet.Web/Intranet.Web/wwwroot/dist"), fileSystemInfo => !fileSystemInfo.Path.FullPath.Contains("_placeholder"));
    CleanDirectory(Directory("./Intranet.Web/Intranet.Web/node_modules"));
});

Task("API:Restore-NuGet-Packages")
    .IsDependentOn("API:Clean")
    .Does(() =>
{
      DotNetCoreRestore(apiDir);
      DotNetCoreRestore(apiTestsDir);
});

Task("Web:Restore-NuGet-Packages")
    .IsDependentOn("Web:Clean")
    .Does(() =>
{
    DotNetCoreRestore(webDir);
    DotNetCoreRestore(webTestsDir);
});

Task("API:Build")
    .IsDependentOn("API:Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild(apiDir, buildSettings);
    DotNetCoreBuild(apiTestsDir, buildSettings);
});

Task("Web:Build")
    .IsDependentOn("Web:Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild(webDir, buildSettings);
    NpmInstall(settings => settings.FromPath("./Intranet.Web/Intranet.Web").WithLogLevel(NpmLogLevel.Warn));
    DotNetCoreBuild(webTestsDir, buildSettings);
});

Task("API:Run-Unit-Tests")
    .IsDependentOn("API:Build")
    .Does(() =>
{
    DotNetCoreTest("./Intranet.API/Intranet.API.UnitTests/Intranet.API.UnitTests.csproj", testSettings);
});

Task("Web:Run-Unit-Tests")
    .IsDependentOn("Web:Build")
    .Does(() =>
{
    DotNetCoreTest("./Intranet.Web/Intranet.Web.UnitTests/Intranet.Web.UnitTests.csproj", testSettings);
    NpmRunScript("test", settings => settings.FromPath("./Intranet.Web/Intranet.Web"));
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("API:Run-Unit-Tests")
    .IsDependentOn("Web:Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);