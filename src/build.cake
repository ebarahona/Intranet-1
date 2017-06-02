//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
#addin nuget:https://www.myget.org/f/righthand-test/?package=Cake.Docker

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
var e2eTestsDir = Directory("./Intranet.SeleniumTests");

Func<String, String> GetBuildDirectory = (dir) => Directory(dir) + Directory("bin") + Directory(configuration);

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
    CleanDirectory(GetBuildDirectory(apiDir));
    CleanDirectory(GetBuildDirectory(apiTestsDir));
});

Task("Web:Clean")
    .Does(() =>
{
    CleanDirectory(GetBuildDirectory(webDir));
    CleanDirectory(GetBuildDirectory(webTestsDir));
});

Task("E2E:Clean")
    .Does(() =>
{
    CleanDirectory(GetBuildDirectory(e2eTestsDir));
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

Task("E2E:Restore-NuGet-Packages")
    .IsDependentOn("E2E:Clean")
    .Does(() =>
{
    DotNetCoreRestore(e2eTestsDir);
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
    DotNetCoreBuild(webTestsDir, buildSettings);
});

Task("E2E:Build")
    .IsDependentOn("E2E:Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild(e2eTestsDir, buildSettings);
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
});

Task("API:Run")
     .IsDependentOn("API:Run-Unit-Tests")
     .Does(() =>
 {
     DotNetCoreRun("./Intranet.API/Intranet.API");
 });

Task("Web:Run")
    .IsDependentOn("Web:Run-Unit-Tests")
    .Does(() =>
{
    DotNetCoreRun("./Intranet.Web/Intranet.Web");
});


Task("E2E:Run-End2End-Tests")
    .IsDependentOn("E2E:Build")
    .Does(() =>
{
    DotNetCoreTest("./Intranet.SeleniumTests/Intranet.SeleniumTests.csproj", testSettings);
});

//Task("Web:TearDown")
//    .Does(() =>
//{
//    ...
//});

//Task("API:TearDown")
//    .Does(() =>
//{
//    ...
//});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("API:Run")
    .IsDependentOn("Web:Run")
    .IsDependentOn("E2E:Run-End2End-Tests");
//    .IsDependentOn("Web.TearDown")
//    .IsDependentOn("API.TearDown")

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);