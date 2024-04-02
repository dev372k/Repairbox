using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class Permission : Base
    {
        public string Name { get; set; }
        public ICollection<UserRole_Permission> UserRole_Permissions { get; set; }
        public ICollection<Resource> Resources { get; set; }
    }
}
