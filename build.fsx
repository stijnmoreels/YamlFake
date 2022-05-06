#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target
nuget Fake.DotNet.Testing.Expecto
nuget FSharp.Configuration //"
#load ".fake/build.fsx/intellisense.fsx"
open System
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open FSharp.Configuration

type Yaml = YamlConfig<"build/build.yml">
let yaml = Yaml()
let project = yaml.variables.Project

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
  let result = DotNet.exec id "run" $"--project .\src\{project}.Tests.Unit\{project}.Tests.Unit.fsproj --nunit-summary TestResults.xml"
  if not result.OK then failwith (String.Join (", ", result.Messages))

Target.create "All" ignore

"Clean"
  ==> "Cleans"
  ==> "Compile"
  ==> "Tests"
  ==> "All"

Target.runOrDefault "All"
