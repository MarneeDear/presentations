open System

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

let ``create the organization`` orgLogin =
    BuildGraph.createEntity (GithubClient.getOrgRecord orgLogin)

let ``get the organization members list`` orgLogin page =
    GithubClient.getOrgMembersList orgLogin page

let ``get the organization repose list`` orgLogin page =
    GithubClient.getGithubOrgRepos orgLogin page
    
let ``create the organization member, projects, and location graph`` user orgLogin =
    BuildGraph.createEntity user
    BuildGraph.createOrganizationMembership user.Properties.Login orgLogin
    BuildGraph.createLocation user.Properties.Location
    BuildGraph.createMemberLocation user.Properties.Login user.Properties.Location

let ``process each page of the members`` orgLogin page =
    ``get the organization members list`` orgLogin page
    |> Seq.iter (fun user -> ``create the organization member, projects, and location graph`` user orgLogin)

let ``feed the org graph`` orgLogin = 
    let pagesList = GithubClient.getRequestPageNumbersList (GithubClient.githubOrgMembersURL orgLogin 4908)
    // GithubClient.orgMembersPageNumbersList orgLogin
    ``create the organization`` orgLogin
    //pagesList |> List.iter
    pagesList |> List.iter (``process each page of the members`` orgLogin)

let ``create the user`` userLogin =
    BuildGraph.createEntity (GithubClient.getUserRecord userLogin)

let ``get the starred projects list`` userLogin page =
    GithubClient.getUserStarred userLogin page

let ``create the starred project graph`` project userLogin =
    BuildGraph.createEntity project
    BuildGraph.createStarred project.Properties.Id userLogin

let ``process each page of the starred projects`` userLogin page =
    ``get the starred projects list`` userLogin page
    |> Seq.iter (fun project -> ``create the starred project graph`` project userLogin)

let ``feed the starred graph`` userLogin =
    let pagesList = GithubClient.getRequestPageNumbersList (GithubClient.githubUserStarredURL userLogin 4908) 
    ``create the user`` userLogin
    pagesList |> List.iter (``process each page of the starred projects`` userLogin)

//    ``get the organization members list`` orgLogin 1
//    |> Seq.iter (fun user -> ``create the organization member and location graph`` user orgLogin)
    ()

//    |> Seq.map (fun user -> 
//                    BuildGraph.createEntity user
//                    user)
//    |> Seq.iter (fun user -> 
//                    BuildGraph.createOrganizationMembership user.Properties.Login orgLogin
//                    )

let doOrgMembers organizations = 
    printfn "These are the organizations for which I will build a members graph:"
    organizations |> Seq.iter (printfn "%s")
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
    organizations |> Seq.iter ``feed the org graph`` //builds members and their locations

let doStarred userLogins =
    printfn "These are the users for which I will build a starred projects graph:"
    userLogins |> Seq.iter (printfn "%s")
    userLogins |> Seq.iter ``feed the starred graph``

let doHelp () =
    printfn "Use one of these options:"
    printfn "--organization [list of organization logins] : use to get an organization and its members"
    printfn "--starred-projects [list of user logins] : use to get the users's starred projects"

[<EntryPoint>]
let main argv = 

    let arguments = 
        if argv.Length.Equals 0 then 
            [|"--starred-projects"; "MarneeDear"|]
        else
            argv

    printfn "%s" "*************************************"
    printfn "%s" "Why hello there. Let's build a graph."
//    Environment.NewLine |> printfn "%s"
    printfn "%s" "O-[R]->O"
    printfn "%s" "Please wait while I build the graph. Did I tell you that graphs are everywhere?"
    printfn "%s" "*************************************"
    Environment.NewLine |> printfn "%s"

    //TODO get the organization projects
    match arguments.[0] with
    | "--organization" -> doOrgMembers (arguments |> Array.toSeq |> Seq.tail)
    | "--starred-projects" -> doStarred (arguments |> Array.toSeq |> Seq.tail)  
    | _ -> doHelp()

////    projects |> Array.iter (printfn "%s")
////    Environment.NewLine |> printfn "%s"
////    printfn "%s" "*************************************"
//////    printfn "%s %s" "I am going to look for these Github Organizations for you." Environment.NewLine
//////    projects |> Array.iter (printfn "%s")
//////    projects |> Array.item 0 |> (printfn "Trying %s%s" Environment.NewLine)
//////    projects |> Array.item 0 |> GithubClient.githubOrgURL |> printfn "%s"
//////    Environment.NewLine |> printfn "%s"
//////    printfn "%s" GithubClient.test
//////    printfn "Organization JSON Response: %s %s" Environment.NewLine (projects |> Array.item 0 |> GithubClient.getGithubOrganization)
//////    Environment.NewLine |> printfn "%s"
//////    projects |> Array.item 0 |> GithubClient.getOrgRecord |> printfn "RECORD %A"
//////    Environment.NewLine |> printfn "%s"
////    printfn "%s" "Please wait while I build the graph. This is going to be awesome."
////    projects |> Array.iter ``feed the graph`` //builds members and their locations
    //TODO build locations graph
    //TODO do some analysis
    Environment.NewLine |> printfn "%s"
    printfn "%s" "All done. You can now display your beautiful graph in the Neo4j Browser."
    printfn "%s" "Press any key to end ... ."
    Console.ReadKey() |> ignore
    0 // return an integer exit code
