using Neo4jClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using graphs_demo.Models;
using Neo4jClient.Cypher;

namespace graphs_demo.Controllers
{
    public class DemoController : Controller
    {
        private IGraphClient _neo4jClient = MvcApplication._neo4jClient;
        // GET: Demo
        public ActionResult Index()
        {
            AlchemyDemoModel alchemy = JsonConvert.DeserializeObject<AlchemyDemoModel>(System.IO.File.ReadAllText(@"C:\Users\Marnee Dearman\Dropbox\presentations2\presentations\dotnet\Graphs\graphs-demo\demo\data\contrib.json"));
            ViewBag.Comment = alchemy.comment;

            return View(alchemy);
        }

        [HttpGet]
        public ActionResult Neo4j(bool setup = false)
        {
            LikesModel model = new LikesModel();
            if (setup)
                SetupGraphNodes();

            return View(model);
        }

        [HttpPost]
        public ActionResult Neo4j(LikesModel model)
        {
            //create relationships
            _neo4jClient.Cypher
                .Match("(person:PERSON)", "(thing:THING)")
                .Where((Person person) => person.id == model.Person)
                .AndWhere((Thing thing) => thing.id == model.Thing)
                .CreateUnique("person-[:LIKES]->thing")
                .ExecuteWithoutResults();
            return View(model);
        }

        [HttpGet]
        public JsonResult PersonLikesThingsModel()
        {
            AlchemyDemoModel model = new AlchemyDemoModel();
            IList<AlchemyNode> nodeList = new List<AlchemyNode>();
            IList<AlchemyEdge> edgeList = new List<AlchemyEdge>();
            //get relationships and setup alchemy json model
            model.comment = "Julian Likes";
            _neo4jClient.Connect();
            var queryResults = _neo4jClient.Cypher
                .OptionalMatch("(person:PERSON)-[LIKES]-(thing:THING)")
                .Return((person, thing) =>
                new
                {
                    Person = person.As<Person>(),
                    Thing = thing.As<Thing>()
                })
                .Results;

            foreach(var rel in queryResults)
            {
                nodeList.Add(new AlchemyNode { id = rel.Person.id, caption = rel.Person.name });
                nodeList.Add(new AlchemyNode { id = rel.Thing.id, caption = rel.Thing.name });
                edgeList.Add(new AlchemyEdge { source = rel.Person.id, target = rel.Thing.id, caption = "LIKES" });
            }

            model.edges = edgeList;
            model.nodes = nodeList;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ContribModel()
        {
            //term = term.ToLower();
            //IList<string> list = _salesAreaRepository.GetList().Select(s => s.SalesAreaName).ToList();
            //var result = list.Where(s => s.ToLower().Contains(term)).Take(10);
            //return Json(result, JsonRequestBehavior.AllowGet);
            AlchemyDemoModel alchemy = JsonConvert.DeserializeObject<AlchemyDemoModel>(System.IO.File.ReadAllText(@"C:\Users\Marnee Dearman\Dropbox\presentations2\presentations\dotnet\Graphs\graphs-demo\demo\data\contrib.json"));
            return Json(alchemy, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LikesModel()
        {
            _neo4jClient.Connect();
            var queryResults = _neo4jClient.Cypher
                .OptionalMatch("(person:PERSON)-[LIKES]-(thing:THING)")
                .Return((person, thing) =>
                new
                {
                    Person = person.As<Person>(),
                    Thing = thing.As<Thing>()
                })
                .Results;

            return Json(queryResults, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PersonsModel()
        {
            _neo4jClient.Connect();
            var queryResults = _neo4jClient.Cypher
                .Match("(person:PERSON)")
                .Return(person => person.As<Person>())
                .Results;            

            return Json(queryResults, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ThingsModel()
        {
            _neo4jClient.Connect();
            var queryResults = _neo4jClient.Cypher
                .Match("(thing:THING)")
                .Return(thing => thing.As<Thing>())
                .Results;

            return Json(queryResults, JsonRequestBehavior.AllowGet);
        }

        private void SetupGraphNodes()
        {
            ClearGraph();
            _neo4jClient.Connect();
            int id = 0;
            LikesModel model = new LikesModel();
            foreach(var person in model.Persons)
            {                
                int.TryParse(person.Value, out id);
                var personNode = new Person { id = id, name = person.Text};
                _neo4jClient.Cypher
                    .Merge("(person:PERSON {id: {id}, name: {name} })")
                    .OnCreate()
                    .Set("person = {personNode}")
                    .WithParams(new
                    {
                        id = id,
                        name = person.Text,
                        personNode
                    }).ExecuteWithoutResults();
            }

            foreach (var thing in model.Things)
            {
                int.TryParse(thing.Value, out id);
                var thingNode = new Thing { id = id, name = thing.Text };
                _neo4jClient.Cypher
                    .Merge("(thing:THING {id: {id}, name: {name} })")
                    .OnCreate()
                    .Set("thing = {thingNode}")
                    .WithParams(new
                    {
                        id = id,
                        name = thing.Text,
                        thingNode
                    }).ExecuteWithoutResults();
            }
        }

        private void ClearGraph()
        {
            _neo4jClient.Connect();
            _neo4jClient.Cypher
                .Start(new { n = All.Nodes })
                .OptionalMatch("(n)-[r]-(x)")
                .With("n, r")
                .Delete("n, r")
                .ExecuteWithoutResults();
        }
    }
}