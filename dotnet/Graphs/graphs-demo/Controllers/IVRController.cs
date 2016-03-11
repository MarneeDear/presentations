using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using graphs_demo.Models;

namespace graphs_demo.Controllers
{
    public class IVRController : Controller
    {
        private IGraphClient _neo4jClient = MvcApplication._neo4jClient;

        // GET: IVR
        public ActionResult Index(bool clear = false)
        {
            if (clear)
                ClearGraph();

            return View();
        }

        public JsonResult FullGraph()
        {
            return Json("");
        }

        public JsonResult Welcome()
        {
            //TODO get IVR Graph from database
            AlchemyIVRModel model = new AlchemyIVRModel();
            IList<AlchemyIVRNode> nodeList = new List<AlchemyIVRNode>();
            IList<AlchemyIVREdge> edgeList = new List<AlchemyIVREdge>();

            _neo4jClient.Connect();
            var queryResults = _neo4jClient.Cypher
                .Match("(welcome:WELCOME)-[HAS_OPTION]-(message:MESSAGE)")
                .Return((welcome, message) =>
                new
                {
                    Welcome = welcome.As<IVRMessage>(),
                    Message = message.As<IVRMessage>()
                }).Results;

            //BUILD the JSON for Alchemy
            foreach (var rel in queryResults)
            {
                if (!nodeList.Where(n => n.id == rel.Message.id).Any())
                {
                    nodeList.Add(new AlchemyIVRNode { id = rel.Message.id,
                        type = "message",
                        caption = rel.Message.message });
                }
                if (!nodeList.Where(n => n.id == rel.Welcome.id).Any())
                {
                    nodeList.Add(new AlchemyIVRNode { id = rel.Welcome.id,
                        type = "welcome", root = true,
                        caption = rel.Welcome.message });
                }
                edgeList.Add(new AlchemyIVREdge
                {
                    source = rel.Welcome.id,
                    target = rel.Message.id,
                    caption = "OPTION"
                });
            }
            model.edges = edgeList;
            model.nodes = nodeList;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private void AddMessageToMessage(Guid message_id, IVRMessage message_model)
        {
            CreateIVRMessage(message_model);
            _neo4jClient.Connect();
            _neo4jClient.Cypher
                .Match("(welcome:WELCOME)", "(message:MESSAGE)")
                .Where((IVRMessage start_message) => start_message.id == message_id)
                .AndWhere((IVRMessage next_message) => next_message.id == message_model.id)
                .CreateUnique("welcome-[:HAS_OPTION]->message")
                .ExecuteWithoutResults();
        }

        private void AddMessageToWelcome(Guid welcome_id, IVRMessage message_model)
        {
            CreateIVRMessage(message_model);
            _neo4jClient.Connect();
            _neo4jClient.Cypher
                .Match("(welcome:WELCOME)", "(message:MESSAGE)")
                .Where((IVRMessage welcome) => welcome.id == welcome_id)
                .AndWhere((IVRMessage message) => message.id == message_model.id)
                .CreateUnique("welcome-[:HAS_OPTION]->message")
                .ExecuteWithoutResults();
        }

        private void CreateIVRMessage(IVRMessage message)
        {
            message.id = Guid.NewGuid();
            var messageNode = new IVRMessage { id = message.id, message = message.message };
            _neo4jClient.Cypher
                .Merge("(message:MESSAGE {id: {id}, message:{message} })")
                .OnCreate()
                .Set("message = {messageNode}")
                .WithParams(new
                {
                    id = message.id,
                    message = message.message
                }).ExecuteWithoutResults();
        }

        private void CreateIVRWelcomeMessage(IVRMessage message)
        {
            _neo4jClient.Connect();
            var messageNode = new IVRMessage { id=Guid.NewGuid(), message = message.message };
            _neo4jClient.Cypher
                .Merge("(message:WELCOME {message: {message}}")
                .OnCreate()
                .Set("message = {messageNode}")
                .WithParams(new
                {
                    id = message.id,
                    message = message.message
                }).ExecuteWithoutResults();
            
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