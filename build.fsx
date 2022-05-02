#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target
nuget Fake.DotNet.Testing.Expecto //"
#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.DotNet.Testing

Target.initEnvironment ()

Target.create "Clean" <| fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs 

Target.create "Cleans" <| fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs 

Target.create "Compile" <| fun _ ->
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build id)

Target.create "Tests" <| fun _ ->
  !! "src/Library.Tests.Unit/**/Library.Tests.Unit.exe"
  |> Expecto.run (fun ps -> { ps with Summary = true })

Target.create "All" ignore

"Clean"
  ==> "Cleans"
  ==> "Compile"
  ==> "Tests"
  ==> "All"

Target.runOrDefault "All"
