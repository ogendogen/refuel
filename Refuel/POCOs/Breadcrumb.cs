using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refuel.POCOs
{
    public class Breadcrumb
    {
        public string Page { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }
}
