using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakeProject.Server.Model
{
    public class Command
    {
        public string CommandText { get; set; }
        public int Parameters { get; set; }
        public string Description { get; set; }
    }
}
