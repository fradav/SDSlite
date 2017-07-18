// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake

// Directories
let buildDir  = "./build/"

// Filesets
let appReferences  =
    !! "/SDSLite/*.csproj"
    // !! "/**/*.csproj"
    // ++ "/**/*.fsproj"

let nuspecFile =
    "./SDSLite/package.nuspec"
// version info
let version = "1.4.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir]
)

Target "Build" (fun _ ->
    // compile all projects below src/app/
    MSBuildReleaseExt buildDir [("Plaform","x64")] "Build" appReferences
    |> Log "AppBuild-Output: "
)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target "NuGet" (fun _ ->
    NuGetPack(fun p ->
        { p with
            OutputPath = "bin"
            ToolPath = "packages/NuGet.CommandLine/tools/NuGet.exe"
            WorkingDir = "build"
            Version = version
        }) nuspecFile
)

// Build order
"Clean"
  ==> "Build"
  ==> "NuGet"

// start build
RunTargetOrDefault "NuGet"
