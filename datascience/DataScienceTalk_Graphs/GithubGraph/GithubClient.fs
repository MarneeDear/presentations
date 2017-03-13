module GithubClient

open System
open FSharp.Data
open Newtonsoft.Json
open FSharp.Data.HttpRequestHeaders

let githubOrgURL orgLogin = sprintf "https://api.github.com/orgs/%s?per_page=100" orgLogin
let githubUserURL userLogin = sprintf "https://api.github.com/users/%s?per_page=100" userLogin
let githubOrgMembersURL orgLogin = sprintf "https://api.github.com/orgs/%s/members?per_page=100" orgLogin
let githubUserOrgsURL userLogin = sprintf "https://api.github.com/users/%s/orgs?per_page=100" userLogin

type OrgResponse = JsonProvider<"org.json">
type UserReponse = JsonProvider<"user.json">
type OrgMembersReponse = JsonProvider<"orgmembers.json">
type UserOrgsResponse = JsonProvider<"userorgs.json">

let test = Http.RequestString("https://google.com")

let getGithubData url =
    Http.RequestString(url = url, httpMethod = HttpMethod.Get, 
        headers = [BasicAuth "marneedear" "60ab7b0a6bcc5912ebd2663588325a43785bbe86";Accept HttpContentTypes.Json; UserAgent "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"]
    )

let getGithubOrganization orgLogin =
    getGithubData (githubOrgURL orgLogin)

let getGithubUser userLogin =
    getGithubData (githubUserURL userLogin)

let getGithubOrgMembers orgLogin =
    getGithubData (githubOrgMembersURL orgLogin)

let getGithubUserOrgs userLogin = 
    getGithubData (githubUserOrgsURL userLogin)


let getOrgRecord orgLogin : BuildGraph.GitHubEntity =
    let response = getGithubOrganization orgLogin |> OrgResponse.Parse
    let properties : BuildGraph.GitHubEntityProperties = 
        {
            Id = uint32(response.Id)
            Login = response.Login
            Name = response.Name
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
        }
    {
        Label = BuildGraph.GITHUB_USER
        Properties = properties
    }

let getOrgMembersList orgLogin : BuildGraph.GitHubEntity seq = 
    let response = getGithubOrgMembers orgLogin |> OrgMembersReponse.Parse
    response |> Seq.map (fun u -> getUserRecord u.Login) //|> Seq.map getUserRecord

//let getUserOrgs
//60ab7b0a6bcc5912ebd2663588325a43785bbe86