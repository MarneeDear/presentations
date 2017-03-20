﻿open System

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

let ``create the organization`` orgLogin =
    BuildGraph.createEntity (GithubClient.getOrgRecord orgLogin)

let ``get the organization members list`` orgLogin page =
    GithubClient.getOrgMembersList orgLogin page
    
let ``create the organization member and location graph`` user orgLogin =
    BuildGraph.createEntity user
    BuildGraph.createOrganizationMembership user.Properties.Login orgLogin
    BuildGraph.createLocation user.Properties.Location
    BuildGraph.createMemberLocation user.Properties.Login user.Properties.Location

let ``process each page of the members`` orgLogin page =
    ``get the organization members list`` orgLogin page
    |> Seq.iter (fun user -> ``create the organization member and location graph`` user orgLogin)

let ``feed the graph`` orgLogin = 
    let pagesList = GithubClient.pageNumbersList orgLogin
    ``create the organization`` orgLogin
    //pagesList |> List.iter
    pagesList |> List.iter (``process each page of the members`` orgLogin)

//    ``get the organization members list`` orgLogin 1
//    |> Seq.iter (fun user -> ``create the organization member and location graph`` user orgLogin)
    ()

//    |> Seq.map (fun user -> 
//                    BuildGraph.createEntity user
//                    user)
//    |> Seq.iter (fun user -> 
//                    BuildGraph.createOrganizationMembership user.Properties.Login orgLogin
//                    )


[<EntryPoint>]
let main argv = 
    let projects = if argv.Length.Equals 0 then [|"fsprojects"|] else argv
    printfn "%s" "*************************************"
    printfn "%s" "Why hello there. Let's build a graph."
//    Environment.NewLine |> printfn "%s"
    printfn "%s" "O-[R]->O"
    projects |> Array.iter (printfn "%s")
    Environment.NewLine |> printfn "%s"
    printfn "%s" "*************************************"
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
    projects |> Array.iter ``feed the graph`` //builds members and their locations
    //TODO build locations graph
    //TODO do some analysis
    Environment.NewLine |> printfn "%s"
    printfn "%s" "All done. You can now display your beautiful graph in the Neo4j Browser."
    Environment.NewLine |> printfn "%s"
    printfn "%s" "Press any key to end ... ."
    Console.ReadKey() |> ignore
    0 // return an integer exit code
