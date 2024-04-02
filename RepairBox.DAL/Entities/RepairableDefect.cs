using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class RepairableDefect : Base
    {
        public string DefectName { get; set; }
        public string RepairTime { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("Model")]
        public int ModelId { get; set; }
    }
}
 