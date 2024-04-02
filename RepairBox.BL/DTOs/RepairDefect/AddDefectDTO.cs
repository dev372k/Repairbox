using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.RepairDefect
{
    public class AddDefectDTO
    {
        public string Title { get; set; }
        public string Price { get; set; }
        public string Cost { get; set; }
        public string Time { get; set; }
        public int ModelId { get; set; }
    }
}
