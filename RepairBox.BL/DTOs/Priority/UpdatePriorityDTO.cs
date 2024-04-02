using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Priority
{
    public class UpdatePriorityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ProcessCharges { get; set; }
    }
}
