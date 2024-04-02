using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class UserRole_Permission
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("UserRole")]
        public int RoleId { get; set; }
        public UserRole Role { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
