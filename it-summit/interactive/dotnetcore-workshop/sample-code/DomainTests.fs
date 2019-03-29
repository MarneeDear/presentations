module DomainTests

open Expecto
open workshop.domain

[<Tests>]
let tests =
  testList "Course Tests" [
      testCase "Engineering convert to code 100" <| fun _ ->
        Expect.equal (Workshop.Engineering.ToCode()) 100 "Engineering course code should be 100"
  ]
