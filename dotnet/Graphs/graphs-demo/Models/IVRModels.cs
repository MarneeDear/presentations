using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace graphs_demo.Models
{
    public class IVRMessage
    {
        public Guid id { get; set; }
        public string message { get; set; }
    }

    public class AlchemyIVRModel
    {
        public string comment { get; set; }

        public IEnumerable<AlchemyIVRNode> nodes { get; set; }
        public IEnumerable<AlchemyIVREdge> edges { get; set; }
    }

    public class AlchemyIVRNode
    {
        public Guid id { get; set; }
        public string caption { get; set; }
        public string type { get; set; }
        public bool root { get; set; }
    }

    public class AlchemyIVREdge
    {
        public Guid source { get; set; }
        public Guid target { get; set; }
        public string caption { get; set; }
    }

}