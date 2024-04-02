using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Permission
{
    public class AddPermissionDTO
    {
        public string Name { get; set; }
        public List<string> ResourceNames { get; set; }
    }
}
