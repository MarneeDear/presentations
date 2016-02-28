using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace graphs_demo.Models
{
    public class LikesModel
    {
        public int Person { get; set; }
        public int Thing { get; set; }
        public IEnumerable<SelectListItem> Persons =
            new List<SelectListItem>
            {
                new SelectListItem {Text = "Mike", Value="1" },
                new SelectListItem {Text = "Julian", Value="2" },
                new SelectListItem {Text="Megan", Value="3" },
                new SelectListItem {Text = "Cayne", Value="4" },
                new SelectListItem {Text = "Marnee", Value="5" }
            };
        public IEnumerable<SelectListItem> Things =
                    new List<SelectListItem>
                    {
                new SelectListItem {Text = "Graphs", Value="6" },
                new SelectListItem {Text = "Motorbikes", Value="7" },
                new SelectListItem {Text="Fractals", Value="8" },
                new SelectListItem {Text = "Chocolate", Value="9" },
                new SelectListItem {Text = "Beer", Value="10" }
                    };
    }


    public class AlchemyDemoModel
    {
        public string comment { get; set; }
        
        public IEnumerable<AlchemyNode> nodes { get; set; }
        public IEnumerable<AlchemyEdge> edges { get; set; }
    }

    public class AlchemyNode
    {
        public int id { get; set; }
        public string caption { get; set; }
        public string role { get; set; }
        public string fun_fact { get; set; }
        public bool root { get; set; }
    }

    public class AlchemyEdge
    {
        public int source { get; set; }
        public int target { get; set; }
        public string caption { get; set; }
    }

    public class Person
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Thing
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}