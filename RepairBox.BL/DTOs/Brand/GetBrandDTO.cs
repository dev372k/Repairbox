using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.ServiceModels.Brand
{
    public class GetBrandDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
