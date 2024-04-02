using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class Brand : Base
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Model> Models { get; set; }
    }
}
