// Learn more about F# at http://fsharp.org

open System
open Argu
open workshop.domain

//This is where we difine what options we accept on the command line
type CLIArguments =
    | DepartmentCode of dept:int
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | DepartmentCode _ -> "specify a course code."

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)

    let parser = ArgumentParser.Create<CLIArguments>(programName = "workshop", errorHandler = errorHandler)

    let cmd = parser.ParseCommandLine(inputs = argv, raiseOnUsage = true)

    printfn "I'm doing all the things!"
    
    match cmd.TryGetResult(CLIArguments.DepartmentCode) with
    | Some code -> printfn "The department name is [%s]" ((Workshop.getDepartment code).ToString())
    | None      -> printfn "I could not understand the department code. Please see the usage."


    0 // return an integer exit code
