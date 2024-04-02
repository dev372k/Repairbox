using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL
{
    public class PaginationModel
    {
        public double TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public object Data { get; set; }
    }
}
