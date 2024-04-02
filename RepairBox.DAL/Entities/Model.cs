using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class Model : Base
    {
        public string Name { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public ICollection<Order> Orders { get; set; }
        public ICollection<RepairableDefect> RepairableDefects { get; set; }
        [ForeignKey("Brand")]
        public int BrandId { get; set; }
    }
}
