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

type OrgResponse = JsonProvider<"org.json">
type UserReponse = JsonProvider<"user.json">
type OrgMembersReponse = JsonProvider<"orgmembers.json">
type UserOrgsResponse = JsonProvider<"userorgs.json">
type StarredProjects = JsonProvider<"starredlist.json">

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
    let response = (getGithubStarred userLogin page) |> StarredProjects.Parse
    response 
    |> Seq.map  (fun resp -> {
                                Label = BuildGraph.GITHUB_PROJECT
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
        Label = BuildGraph.GITHUB_ORGANIZATION
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
        Label = BuildGraph.GITHUB_USER
        Properties = properties
    }

let getOrgMembersList orgLogin page : BuildGraph.GitHubEntity seq = 
    let response = (getGithubOrgMembers orgLogin page) |> OrgMembersReponse.Parse
    response |> Seq.map (fun u -> getUserRecord u.Login) //|> Seq.map getUserRecord

//let getUserOrgs
//60ab7b0a6bcc5912ebd2663588325a43785bbe86

