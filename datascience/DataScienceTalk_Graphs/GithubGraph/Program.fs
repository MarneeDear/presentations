open System

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

let buildOrgGraph orgLogin = 
    BuildGraph.createEntity (GithubClient.getOrgRecord orgLogin) |> ignore
        
    GithubClient.getOrgMembersList orgLogin    
    |> Seq.map BuildGraph.createEntity
    |> Seq.iter (fun userLogin -> BuildGraph.createOrganizationMembership userLogin orgLogin)
    ()

[<EntryPoint>]
let main argv = 
    let projects = if argv.Length.Equals 0 then [|"dotnet"|] else argv
    printfn "%s" "Why hello there. Let's build a graph."
    Environment.NewLine |> printfn "%s"
    printfn "%s" "O-[R]-> O"
    Environment.NewLine |> printfn "%s"
//    printfn "%s %s" "I am going to look for these Github Organizations for you." Environment.NewLine
//    projects |> Array.iter (printfn "%s")
//    projects |> Array.item 0 |> (printfn "Trying %s%s" Environment.NewLine)
//    projects |> Array.item 0 |> GithubClient.githubOrgURL |> printfn "%s"
//    Environment.NewLine |> printfn "%s"
//    printfn "%s" GithubClient.test
//    printfn "Organization JSON Response: %s %s" Environment.NewLine (projects |> Array.item 0 |> GithubClient.getGithubOrganization)
//    Environment.NewLine |> printfn "%s"
//    projects |> Array.item 0 |> GithubClient.getOrgRecord |> printfn "RECORD %A"
//    Environment.NewLine |> printfn "%s"
    printfn "%s" "Please wait while I build the graph. This is going to be awesome."
    projects |> Array.iter buildOrgGraph
    Environment.NewLine |> printfn "%s"
    printfn "%s" "All done. Check Neo4J."
    Environment.NewLine |> printfn "%s"
    printfn "%s" "Press any key to continue ... ."
    Console.ReadKey() |> ignore
    0 // return an integer exit code
