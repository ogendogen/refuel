using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Refuel.POCOs
{
    public class Parameter
    {
        public string TranslatedKey { get; set; }
        public string RawKey { get; set; }
        public string TranslatedValue { get; set; }
        public string RawValue { get; set; }
    }
}
