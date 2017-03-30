module GithubClient

open System
open FSharp.Data
open Newtonsoft.Json
open FSharp.Data.HttpRequestHeaders
open FSharp.Configuration

type Settings = AppSettings<"App.config">

let githubOrgURL orgLogin = sprintf "https://api.github.com/orgs/%s?per_page=100" orgLogin
let githubUserURL userLogin = sprintf "https://api.github.com/users/%s?per_page=100" userLogin
let githubOrgMembersURL orgLogin page = sprintf "https://api.github.com/orgs/%s/members?per_page=100&page=%i" orgLogin page 
let githubUserOrgsURL userLogin page = sprintf "https://api.github.com/users/%s/orgs?per_page=100&page=%i" userLogin page
let githubUserStarredURL userLogin page = sprintf "https://api.github.com/users/%s/starred?per_page=100&page=%i" userLogin page
let githubOrgReposURL orgLogin page = sprintf "https://api.github.com/orgs/%s/repos?per_page=100&page=%i" orgLogin page

type OrgResponse = JsonProvider<"org.json">
type UserReponse = JsonProvider<"user.json">
type OrgMembersReponse = JsonProvider<"orgmembers.json">
type UserOrgsResponse = JsonProvider<"userorgs.json">
type StarredProjectsResponse = JsonProvider<"starredlist.json">
type OrgReposResponse = JsonProvider<"orgrepos.json">

let getGithubData url =
    Http.RequestString(url = url, httpMethod = HttpMethod.Get, 
        headers = [BasicAuth "MarneeDear" Settings.GithubToken; Accept HttpContentTypes.Json; UserAgent "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"]
    )

let getGithubDataPage url =
    Http.Request(url = url, httpMethod = HttpMethod.Head, 
        headers = [BasicAuth "MarneeDear" Settings.GithubToken; Accept HttpContentTypes.Json; UserAgent "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"]
    )

let getGithubOrganization orgLogin =
    getGithubData (githubOrgURL orgLogin)

let getGithubUser userLogin =
    getGithubData (githubUserURL userLogin)

let getGithubStarred userLogin page =
    getGithubData (githubUserStarredURL userLogin page)

let getGithubOrgMembers orgLogin page =
    getGithubData (githubOrgMembersURL orgLogin page)

let getGithubUserOrgs userLogin page = 
    getGithubData (githubUserOrgsURL userLogin page)

let getGithubOrgRepos orgLogin page =
    getGithubData (githubOrgReposURL orgLogin page)

let rando = Random().Next(100, 10000).ToString()

let getRequestPageNumbersList url =
    let response = getGithubDataPage url
    let lastLink = new System.Uri((response.Headers.Item "Link").Split([| ',' |]).[1].Split([| ';' |]).[0].Replace(">", "").Replace("<", ""))
    let lastPage = int(lastLink.Query.Split('&').[1].Split('=').[1]) //.Split('=').[1])
    [1 .. lastPage]

//let starredPageNumbersList userLogin = 
//    let response = getGithubDataPage (githubUserStarredURL userLogin 4908)
//    let lastLink = new System.Uri((response.Headers.Item "Link").Split([| ',' |]).[1].Split([| ';' |]).[0].Replace(">", "").Replace("<", ""))
//    let lastPage = int(lastLink.Query.Split('&').[1].Split('=').[1]) //.Split('=').[1])
//    [1 .. lastPage]
//
//let orgMembersPageNumbersList orgLogin =
//    let response = getGithubDataPage (githubOrgMembersURL orgLogin 4908)
//    let lastLink = new System.Uri((response.Headers.Item "Link").Split([| ',' |]).[1].Split([| ';' |]).[0].Replace(">", "").Replace("<", ""))
//    let lastPage = int(lastLink.Query.Split('&').[1].Split('=').[1]) //.Split('=').[1])
//    [1 .. lastPage]


let getUserStarred userLogin page : BuildGraph.GitHubEntity seq = 
    let response = (getGithubStarred userLogin page) |> StarredProjectsResponse.Parse
    response 
    |> Seq.map  (fun resp -> {
                                Label = BuildGraph.PROJECT
                                Properties = {
                                                Id = uint32(resp.Id)
                                                Login = resp.Owner.Login
                                                Name = resp.Name
                                                Location = String.Empty
                                            }
                            }
                )

let getOrgRecord orgLogin : BuildGraph.GitHubEntity =
    let response = getGithubOrganization orgLogin |> OrgResponse.Parse
    let properties : BuildGraph.GitHubEntityProperties = 
        {
            Id = uint32(response.Id)
            Login = response.Login
            Name = response.Name
            Location = response.Location
        }
    {
        Label = BuildGraph.ORGANIZATION
        Properties = properties
    }

let getUserRecord userLogin : BuildGraph.GitHubEntity =
    let response = getGithubUser userLogin |> UserReponse.Parse
    let properties : BuildGraph.GitHubEntityProperties = 
        {
            Id = uint32(response.Id)
            Login = response.Login
            Name = response.Name
            Location = response.Location
        }
    {
        Label = BuildGraph.USER
        Properties = properties
    }

let getOrgMembersList orgLogin page : BuildGraph.GitHubEntity seq = 
    let response = (getGithubOrgMembers orgLogin page) |> OrgMembersReponse.Parse
    response |> Seq.map (fun u -> getUserRecord u.Login) //|> Seq.map getUserRecord

let getOrgProjectsList orgLogin : BuildGraph.GitHubEntity seq =
    let pages = getRequestPageNumbersList (githubOrgReposURL orgLogin 4908)
    let response page = (getGithubOrgRepos orgLogin page) |> OrgReposResponse.Parse
    response 1
    |> Seq.map (fun resp -> {
                                Label = BuildGraph.PROJECT
                                Properties = {
                                                Id = uint32(resp.Id)
                                                Login = resp.Owner.Login
                                                Name = resp.Name
                                                Location = String.Empty
                                            }
                            }
                )


//    
//    let entityconvert response : BuildGraph.GitHubEntity seq =
//        response |> Seq.map (fun g -> g) |> Seq.map (fun repo -> {
//                                            Label = BuildGraph.PROJECT
//                                            Properties = {
//                                                            Id = uint32(repo.Id)
//                                                            Login = repo.Owner.Login
//                                                            Name = repo.Name
//                                                            Location = String.Empty
//                                            }
//                                            })
//    
//    let test =
//    pages 
//    |> Seq.map response 
//    |> Seq.map entityconvert
        //|> Seq.map (fun repos -> repos)
        //|> Seq.map (fun repo -> repo)
        //{
//                                    Label = BuildGraph.PROJECT
//                                    Properties = {
//                                                    Id = uint32(repo.Id)
//                                                    Login = repo.Owner.Login
//                                                    Name = repo.Name
//                                                    Location = String.Empty
//                                    }
//                                })


//                )
    //{ 
//                                        Label = BuildGraph.PROJECT
//                                        Properties = {
//                                                        Id = uint32(repo.Id)
//                                                        Login = repo.Owner.Login
//                                                        Name = repo.Name
//                                                        Location = String.Empty
//                                        }
//                                    })

//let getUserOrgs
//60ab7b0a6bcc5912ebd2663588325a43785bbe86

