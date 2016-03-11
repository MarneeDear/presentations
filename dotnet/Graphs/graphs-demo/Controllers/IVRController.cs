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
        [HttpGet]
        public ActionResult Index(bool clear = false)
        {
            if (clear)
                ClearGraph();
            //TODO Get current welcome message for IVR
            return View();
        }

        [HttpPost]
        public ActionResult Index(string create_welcome, string create_option, IVRModel model)
        {
            //TODO GET LIST OF option messages from the graph 
            //Maybe do this as a tuple of previous and next so user knows what he is doing
            //MAYBE show path in the viz
            //USER will add a message to the "next" message in that tuple

            TryUpdateModel(model);
            //TryUpdateModel(model.OptionMessage);
            if (!string.IsNullOrEmpty(create_welcome))
                CreateIVRWelcomeMessage(model.WelcomeMessage);

            if (!string.IsNullOrEmpty(create_option))
            {
                CreateIVRMessage(model.OptionMessage);

                //START from the ROOT node (WELCOME)
                AddMessageToWelcome(model.WelcomeMessage.id.Value, model.OptionMessage.id.Value);
            }

            //TODO add next message to message

            return View(model);
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
                .OptionalMatch("(welcome:WELCOME)-[HAS_OPTION]-(message:MESSAGE)")
                .Return((welcome, message) =>
                new
                {
                    Welcome = welcome.As<IVRMessage>(),
                    Message = message.As<IVRMessage>()
                }).Results;

            //TODO if no next messages just get the welcome message or do an optional match?

            //BUILD the JSON for Alchemy
            foreach (var rel in queryResults)
            {
                //check that there is a next message first and then build the alchemy model
                if (rel.Message != null)
                {
                    if (!nodeList.Where(n => n.id == rel.Message.id).Any())
                    {
                        nodeList.Add(new AlchemyIVRNode
                        {
                            id = rel.Message.id.Value,
                            type = "message",
                            caption = rel.Message.message
                        });
                    }

                    edgeList.Add(new AlchemyIVREdge
                    {
                        source = rel.Welcome.id.Value,
                        target = rel.Message.id.Value,
                        caption = "OPTION"
                    });
                }
                if (!nodeList.Where(n => n.id == rel.Welcome.id).Any())
                {
                    nodeList.Add(new AlchemyIVRNode { id = rel.Welcome.id.Value,
                        type = "welcome", root = true,
                        caption = rel.Welcome.message });
                }
                
            }
            model.edges = edgeList;
            model.nodes = nodeList;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private void AddMessageToMessage(Guid start_message_id, Guid next_message_id)
        {
            //CreateIVRMessage(message_model);
            _neo4jClient.Connect();
            _neo4jClient.Cypher
                .Match("(message:MESSAGE)", "(message:MESSAGE)")
                .Where((IVRMessage start_message) => start_message.id == start_message_id)
                .AndWhere((IVRMessage next_message) => next_message.id == next_message_id)
                .CreateUnique("welcome-[:HAS_OPTION]->message")
                .ExecuteWithoutResults();
        }

        private void AddMessageToWelcome(Guid welcome_id, Guid message_id)
        {
            //CreateIVRMessage(message_model);
            _neo4jClient.Connect();
            _neo4jClient.Cypher
                .Match("(welcome:WELCOME)", "(message:MESSAGE)")
                .Where((IVRMessage welcome) => welcome.id == welcome_id)
                .AndWhere((IVRMessage message) => message.id == message_id)
                .CreateUnique("welcome-[:HAS_OPTION]->message")
                .ExecuteWithoutResults();
        }

        private void CreateIVRMessage(IVRMessage messageNode)
        {
            messageNode.id = Guid.NewGuid();
            //var messageNode = new IVRMessage { id = message.id, message = message.message };
            _neo4jClient.Cypher
                .Merge("(message:MESSAGE {id: {id}, message:{message} })")
                .OnCreate()
                .Set("message = {messageNode}")
                .WithParams(new
                {
                    id = messageNode.id,
                    message = messageNode.message,
                    messageNode
                }).ExecuteWithoutResults();
        }

        private void CreateIVRWelcomeMessage(IVRMessage messageNode)
        {
            _neo4jClient.Connect();
            messageNode.id = Guid.NewGuid();
            //var messageNode = new IVRMessage { id=Guid.NewGuid(), message = message.message };
            _neo4jClient.Cypher
                .Merge("(message:WELCOME {message: {message} })")
                .OnCreate()
                .Set("message = {messageNode}")
                .WithParams(new
                {
                    id = messageNode.id,
                    message = messageNode.message,
                    messageNode
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