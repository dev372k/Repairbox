using RepairBox.BL.DTOs.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.User
{
    public class UserCreateTokenDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Dictionary<string, bool>? Resources { get; set; }
        public List<string> Permissions { get; set; }
        public string? Token { get; set; }
    }
}
