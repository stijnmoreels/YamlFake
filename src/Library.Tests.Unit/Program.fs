module Library.Tests.Unit

open Library
open Expecto

[<Tests>]
let tests =
  test "A simple test" {
    let name = "Patrick"
    let response = Say.hello name
    Expect.equal response ("Hello " + name) "The strings should equal" }

[<EntryPoint>]
let main args =
  runTestsInAssemblyWithCLIArgs [] args