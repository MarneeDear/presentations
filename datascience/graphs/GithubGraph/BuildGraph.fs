module BuildGraph 

open System
open Neo4jClient
//open Neo4jClient.Cypher
//open FSharp.Configuration


[<Literal>]
let Neo4JConnectionString =  @"http://localhost:7474/db/data"
let neo4jClient = new GraphClient(new Uri(Neo4JConnectionString))

//Let's define the domain with types
//This is an oversimplification but helpful for processing the results we get from the Github API

//This discriminated union will represent the LABELS we have in the graph

type GitHubLabel = 
    | GITHUB_USER
    | GITHUB_ORGANIZATION
    | GITHUB_LOCATION
    | GITHUB_PROJECT

type GitHubRelationship =
    | BELONGS_TO //user belongs to an organization
    | FOLLOWS //user follows another user
    | LOCATED_IN //user is located in this place
    | STARRED

type GitHubLocationProperties =
    {
        Name : string
    }

//type GitHubLocation =
//    {
//        Label : GitHubLabel
//        Properties : GitHubLocationProperties
//    }

type GitHubEntityProperties =
    {
        Id : uint32
        Login: string
        Name: string
        Location: string
    }

//The RECORD TYPE below will represent our NODE properties
//In this case both a user and an org will have the same properties so I just
//created one record type for both
type GitHubEntity =
    {
        Label : GitHubLabel
        Properties : GitHubEntityProperties
    }

//type GitHubMembership =
//    {
//        Path : GitHubEntity * GitHubRelationship * GitHubEntity
//    }


let getNodeLabel entityLabel =
    match entityLabel with
    | GITHUB_USER -> "GITHUB_USER"
    | GITHUB_ORGANIZATION -> "GITHUB_ORGANIZATION"
    | GITHUB_LOCATION -> "GITHUB_LOCATION"
    | GITHUB_PROJECT -> "GITHUB_PROJECT"

let getRelationshipType rel =
    match rel with
    | BELONGS_TO -> "BELONGS_TO"
    | FOLLOWS -> "FOLLOWS"
    | LOCATED_IN -> "LOCATED_IN"
    | STARRED -> "STARRED"

let inline (=>) a b = a, box b

let createLocation (location:string) =
//    let properties = location.Properties
//    let label = getNodeLabel location.Label
    
    let locationNode : GitHubLocationProperties = 
        {
            Name = match location with
                    | "" -> "No Location Given"
                    | _ -> location
        }
    neo4jClient.Connect()
    neo4jClient.Cypher
        .Merge(sprintf "(loc:%s {Name: {name} })" (getNodeLabel GITHUB_LOCATION))
        .OnCreate()
        .Set("loc = {properties}")
        .WithParams(dict [
                        "name" => locationNode.Name
                        "properties" => locationNode
                    ])
        .ExecuteWithoutResults()

let createMemberLocation userLogin location =
    neo4jClient.Connect()

    let locationNode  = 
        match location with
        | "" -> "No Location Given"
        | _ -> location

    neo4jClient.Cypher
        .Match(sprintf "(l:%s)" (getNodeLabel GITHUB_LOCATION), sprintf "(u:%s)" (getNodeLabel GITHUB_USER))
        .Where(fun (l:GitHubLocationProperties) -> l.Name = locationNode)
        .AndWhere(fun (u:GitHubEntityProperties) -> u.Login = userLogin)
        .CreateUnique(sprintf "(u)-[:%s]->(l)" (getRelationshipType LOCATED_IN))
        .ExecuteWithoutResults()


//MERGE : CREATE or UPDATE a node based on the properties passed in
let createEntity (entity:GitHubEntity) =
    neo4jClient.Connect()

    let properties = entity.Properties
    let label = getNodeLabel entity.Label

    neo4jClient.Cypher
        .Merge(sprintf "(entity:%s {Id: {id}, Login: {login}, Name: {name} })" label)
        .OnCreate()
        .Set("entity = {properties}")
        .WithParams(dict [
                        "id" => properties.Id
                        "login" => properties.Login
                        "name" => properties.Name
                        "properties" => properties
                    ])
        .ExecuteWithoutResults()

//CREATE A LINK BETWEEN AN ORG AND A USER
let createOrganizationMembership userLogin orgLogin =
    //CHECK THE ENTITIES HAVE THE RIGHT LABELS
//    let checkOrg =
//        match org.Label with
//        | GITHUB_ORGANIZATION -> org.Properties
//        | _ -> failwith "org argument must be a GITHUB_ORGANIZATION"
//
//    let checkUser =
//        match user.Label with
//        | GITHUB_USER -> user.Properties
//        | _ -> failwith "user argument must be a GITHUB_USER"
    neo4jClient.Connect()

    neo4jClient.Cypher
        .Match(sprintf "(o:%s)" (getNodeLabel GITHUB_ORGANIZATION), sprintf "(u:%s)" (getNodeLabel GITHUB_USER))
        .Where(fun (o:GitHubEntityProperties) -> o.Login = orgLogin)
        .AndWhere(fun (u:GitHubEntityProperties) -> u.Login = userLogin)
        .CreateUnique(sprintf "(u)-[:%s]->(o)" (getRelationshipType BELONGS_TO))
        .ExecuteWithoutResults()

let createStarred projectId userLogin =
    neo4jClient.Connect()

    neo4jClient.Cypher
        .Match(sprintf "(p:%s)" (getNodeLabel GITHUB_PROJECT), sprintf "(u:%s)" (getNodeLabel GITHUB_USER))
        .Where(fun (p:GitHubEntityProperties) -> p.Id = projectId)
        .AndWhere(fun (u:GitHubEntityProperties) -> u.Login = userLogin)
        .CreateUnique(sprintf "(u)-[:%s]->(p)" (getRelationshipType STARRED))
        .ExecuteWithoutResults()

//DELETE AN ENTITY AND ALL OF ITS RELATIONSHIPS
let deleteNode (entity:GitHubEntity) =
    neo4jClient.Connect()

    neo4jClient.Cypher
        .OptionalMatch(sprintf "(n:%s)<-[r]-()" (getNodeLabel entity.Label)) 
        .Where(fun (n:GitHubEntityProperties ) -> n.Id = entity.Properties.Id)
        .Delete("r, n") //deletes all incoming relationships
        .ExecuteWithoutResults()
