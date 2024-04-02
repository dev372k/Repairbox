using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class CustomerIdentities : Base
    {
        [ForeignKey("CustomerInfo")]
        public int CustomerInfoId { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
