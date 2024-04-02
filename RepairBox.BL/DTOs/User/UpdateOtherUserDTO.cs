using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.User
{
    public class UpdateOtherUserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int UserRoleId { get; set; }
        public bool Status { get; set; }
    }
}
