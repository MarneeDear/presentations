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

type GitHubRelationship =
    | BELONGS_TO //user belongs to an organization
    | FOLLOWS //user follows another user

type GitHubEntityProperties =
    {
        Id : uint32
        Login: string
        Name: string

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

let getRelationshipType rel =
    match rel with
    | BELONGS_TO -> "BELONGS_TO"
    | FOLLOWS -> "FOLLOWS"


//MERGE : CREATE or UPDATE a node based on the properties passed in
let createEntity (entity:GitHubEntity) =
    neo4jClient.Connect()

    let inline (=>) a b = a, box b
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
    entity.Properties.Login


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
        .CreateUnique(sprintf "(u)-[:%s]->(o)" (getRelationshipType GitHubRelationship.BELONGS_TO))
        .ExecuteWithoutResults()
//    userLogin, orgLogin

//DELETE AN ENTITY AND ALL OF ITS RELATIONSHIPS
let deleteNode (entity:GitHubEntity) =
    neo4jClient.Connect()

    neo4jClient.Cypher
        .OptionalMatch(sprintf "(n:%s)<-[r]-()" (getNodeLabel entity.Label)) 
        .Where(fun (n:GitHubEntityProperties ) -> n.Id = entity.Properties.Id)
        .Delete("r, n") //deletes all incoming relationships
        .ExecuteWithoutResults()
