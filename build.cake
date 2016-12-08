#addin "Cake.Xamarin&version=1.3.0.6"
#addin "nuget:?package=Cake.StyleCop&version=1.1.2"

#tool "xunit.runner.console&version=2.1.0"
#tool "XamarinComponent&version=1.1.0.39"

// Arguments.
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var xamarinUsername = Argument("xamarin_username", "");
var XamarinPassword = Argument("xamarin_password", "");

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

// Xamarin Component.
var xamarinComponetPath = "./tools/XamarinComponent/tools/xamarin-component.exe";
var XamarinComponetEmail = xamarinUsername;
var XamarinComponetPassword = XamarinPassword;

// StyleCop.
var styleCopPath = File("./settings.stylecop");
var styleCopResultFile = File("StylecopResults.xml");

Task("Clean")
	.Does(() => 
	{
		CleanDirectory(artifactsDirectory);

		// There are some files created after iOSBuild that don't clean from the iOSOutputDirectory, force to clean here.
		CleanDirectories("./**/" + iOSOutputDirectory);

		DotNetBuild(solutionFile, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Clean")
			.SetVerbosity(Verbosity.Minimal));
	});

Task("Restore-XamarinComponents")
	.Does(() => 
	{
		RestoreComponents(solutionFile, 
			new XamarinComponentRestoreSettings 
			{
				ToolPath = xamarinComponetPath,
				Email = XamarinComponetEmail,
				Password = XamarinComponetPassword
			});
	});

Task("Restore-Packages")
	.Does(() => 
	{
		NuGetRestore(solutionFile);
	});

Task("Run-StyleCop")	
	// Allows the build process to continue even if there is erros reported by StyleCop.
	.ContinueOnError()
    	.Does(() =>
	{        
		StyleCopAnalyse(settings => settings
			.WithSolution(solutionFile)
			.WithSettings(styleCopPath)
			.ToResultFile(artifactsDirectory + styleCopResultFile));	
    });

Task("Run-Tests")
	// Allows the build process to continue even if there Tests aren't passing.
	.ContinueOnError()
	.IsDependentOn("Prepare-Build")
    	.Does(() =>
	{		
		DotNetBuild(testsProject.FullPath, settings => settings
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
	.IsDependentOn("Restore-XamarinComponents")	
    	.Does (() => {});

Task("Build-Android")
	.IsDependentOn("Prepare-Build")
    	.Does(() =>
	{ 		
		DotNetBuild(androidProject, settings =>
			settings.SetConfiguration(configuration)           
			.WithProperty("DebugSymbols", "false")
			.WithProperty("TreatWarningsAsErrors", "false")
			.SetVerbosity(Verbosity.Minimal));
    });

Task("Build-iOS")
	.IsDependentOn("Prepare-Build")
    	.Does (() =>
	{
    		DotNetBuild(iOSProject, settings => 
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
	.IsDependentOn("Run-StyleCop")
	.IsDependentOn("Run-Tests");

RunTarget(target);
