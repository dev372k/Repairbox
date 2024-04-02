using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class RepairPriority : Base
    {
        public string Name { get; set; } = string.Empty;
        public decimal ProcessCharges { get; set; }
    }
}
