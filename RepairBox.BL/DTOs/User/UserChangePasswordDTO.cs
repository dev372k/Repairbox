using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.User
{
    public class UserChangePasswordDTO
    {
        public int Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword1 { get; set; }
        public string NewPassword2 { get; set; }
    }
}
