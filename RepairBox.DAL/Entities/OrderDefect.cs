using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class OrderDefect : Base
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [ForeignKey("RepairableDefect")]
        public int RepairableDefectId { get; set; }
    }
}
