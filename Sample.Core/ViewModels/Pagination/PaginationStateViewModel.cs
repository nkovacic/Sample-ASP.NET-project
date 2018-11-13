using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.ViewModels
{
    public class PaginationStateViewModel
    {
        public int Count { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
    }
}
