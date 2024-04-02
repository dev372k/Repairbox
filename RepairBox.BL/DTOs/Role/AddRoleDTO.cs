using RepairBox.BL.DTOs.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Role
{
    public class AddRoleDTO
    {
        public string Name { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}
