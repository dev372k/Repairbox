using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.DAL.Entities
{
    public class DeviceInfo : Base
    {
        [ForeignKey("CustomerInfo")]
        public int CustomerInfoId { get; set; }
        public string DeviceNameModel { get; set; } = string.Empty;
        public string IMEI { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
    }
}
