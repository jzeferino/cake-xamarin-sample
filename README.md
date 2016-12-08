# Cake Xamarin sample

A simple example of using [Cake](http://cakebuild.net/) to build Xamarin.Android and Xamarin.iOS projects.
Plus, the script runs [xUnit](http://cakebuild.net/dsl/xunit-v2) tests, and [StyleCop](https://github.com/Ashthos/Cake.StyleCop).

What the script does:

- Clean the solution and artifacts folder.
- Restore NuGet packages and Xamarin Components.
- Build Android project.
- Build iOS project.
- Run StyleCop.
- Run xUnit tests.
- Write the xUnit tests result and StyleCop output to the artifacts folder.

What's part of the solution:

- Cake.Xamarin.Sample.Shared => A Portable Class Library (PCL) with shared code between Xamarin.Android and Xamarin.iOS.
- Cake.Xamarin.Sample.Tests => A PCL with tests over shared code PCL.
- Cake.Xamarin.Sample.Android => A Android project that uses the shared code PCL.
- Cake.Xamarin.Sample.iOS => A iOS project that uses the shared code PCL.

## Requirements

1. xbuild (Linux and OS X) or msbuild (Windows) installed.
1. .NET Core, .NET 4.5 and Mono installed.
1. Terminal or Powershell.

## How to run it 

1. Clone the repo and open terminal inside it.
3. Run `./build.sh` in OS X or `.\build.ps1` on Windows.
4. Get the outputs on the `artifacts` folder.

### Aditional notes 

- To use a specific Xamarin account to restore Xamarin Components `./build.sh -ScriptArgs -xamarin_username="youremail@example.com" -xamarin_password="password"`.
- To run a specific Task like build only android or ios: `./build.sh -t Build-Android` or `./build.sh -t Build-iOS`.