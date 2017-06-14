//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
#addin nuget:https://www.myget.org/f/righthand-test/?package=Cake.Docker
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
    NpmInstall(settings => settings.FromPath("./Intranet.Web/Intranet.Web").WithLogLevel(NpmLogLevel.Warn));
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
    NpmRunScript("test", settings => settings.FromPath("./Intranet.Web/Intranet.Web"));
});

Task("API:Publish")
     .IsDependentOn("API:Run-Unit-Tests")
     .Does(() =>
 {
     DotNetCorePublish("./Intranet.API/Intranet.API");
 });

 Task("Web:Publish")
     .IsDependentOn("Web:Run-Unit-Tests")
     .Does(() =>
 {
     DotNetCorePublish("./Intranet.Web/Intranet.Web");
 });
 
Task("API:Run")
     //.IsDependentOn("API:Publish")
     .Does(() =>
 {
     //DotNetCoreExecute("./Intranet.API/Intranet.API/bin/Debug/netcoreapp1.1/publish/Intranet.API.dll");
     DotNetCoreRun("./Intranet.API/Intranet.API");
 });

Task("Web:Run")
    .IsDependentOn("Web:Run-Unit-Tests")
    .Does(() =>
{
    ...
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

Task("Default").IsDependentOn("All:DockerRun");
    .IsDependentOn("API:Build")
    .IsDependentOn("Web:Build");
    .IsDependentOn("E2E:Run-End2End-Tests");
//    .IsDependentOn("Web.TearDown")
//    .IsDependentOn("API.TearDown")

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);