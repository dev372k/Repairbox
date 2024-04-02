using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.User
{
    public class GetUserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AvatarPath { get; set; }
        public bool IsActive { get; set; }
        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; }
    }
}
