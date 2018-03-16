#addin "Cake.Xamarin&version=2.0.1"

#tool xunit.runner.console&version=2.3.1

// Arguments.
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Define directories.
var solutionFile = GetFiles("./*.sln").First();

// Android and iOS.
var androidProject = GetFiles("./src/android/Cake.Xamarin.Sample.Android/*.csproj").First();
var iOSProject = GetFiles("./src/ios/Cake.Xamarin.Sample.iOS/*.csproj").First();

// Tests.
var testsProject = GetFiles("./test/**/*.csproj").First();
// NOTE: (assumes Tests projects end with .Tests).
var testsDllPath = string.Format("./test/Cake.Xamarin.Sample.Tests/bin/{0}/*.Tests.dll", configuration);

// Output folders.
var artifactsDirectory = Directory("./artifacts");
var iOSOutputDirectory = "bin/iPhoneSimulator";

Task("Clean")
	.Does(() => 
	{
		CleanDirectory(artifactsDirectory);

		// There are some files created after iOSBuild that don't clean from the iOSOutputDirectory, force to clean here.
		CleanDirectories("./**/" + iOSOutputDirectory);

		MSBuild(solutionFile, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Clean")
			.SetVerbosity(Verbosity.Minimal));
	});

Task("Restore-Packages")
	.Does(() => 
	{
		NuGetRestore(solutionFile);
	});

Task("Run-Tests")
	// Allows the build process to continue even if there Tests aren't passing.
	.ContinueOnError()
	.IsDependentOn("Prepare-Build")
	.Does(() =>
	{		
		MSBuild(testsProject.FullPath, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Build")
			.SetVerbosity(Verbosity.Minimal));			

		XUnit2(testsDllPath, 
			new XUnit2Settings
			{
				XmlReport = true,
				OutputDirectory = artifactsDirectory
			});		
    });

Task("Prepare-Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore-Packages")
   	.Does (() => {});

Task("Build-Android")
	.IsDependentOn("Prepare-Build")
	.Does(() =>
	{ 		
		MSBuild(androidProject, settings =>
			settings.SetConfiguration(configuration)           
			.WithProperty("DebugSymbols", "false")
			.WithProperty("TreatWarningsAsErrors", "false")
			.SetVerbosity(Verbosity.Minimal));
    });

Task("Build-iOS")
	.IsDependentOn("Prepare-Build")
	.Does (() =>
	{
    		MSBuild(iOSProject, settings => 
			settings.SetConfiguration(configuration)   
			.WithTarget("Build")
			.WithProperty("Platform", "iPhoneSimulator")
			.WithProperty("OutputPath", iOSOutputDirectory)
			.WithProperty("TreatWarningsAsErrors", "false")
			.SetVerbosity(Verbosity.Minimal));
	});

Task("Default")
	.IsDependentOn("Build-Android")
	.IsDependentOn("Build-iOS")
	.IsDependentOn("Run-Tests");

RunTarget(target);
