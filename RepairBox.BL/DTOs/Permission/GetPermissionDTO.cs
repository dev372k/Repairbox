using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Permission
{
    public class GetPermissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> ResourceNames { get; set; }
    }
}
