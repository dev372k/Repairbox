using RepairBox.BL.DTOs.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.Role
{
    public class GetRoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetAllPermissionsDTO> Permissions { get; set; }
    }
}
