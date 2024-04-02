using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class Setting : Base
    {
        public decimal Tax { get; set; } = 0;
    }
}
