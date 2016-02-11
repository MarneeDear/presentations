using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class Presenter
    {
        public string PresenterName { get; set; }

        public void GivePresentation(ref Group group)
        {
            group.WeLearnedAllTheThings = true;
        }
    }

    public class Group
    {
        public string GroupName { get; set; }
        public bool WeLearnedAllTheThings { get; set; }
    }
}
