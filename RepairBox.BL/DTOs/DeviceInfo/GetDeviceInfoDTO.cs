using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairBox.BL.DTOs.DeviceInfo
{
    public class GetDeviceInfoDTO
    {
        public string DeviceNameModel { get; set; }
        public string IMEI { get; set; }
        public string SerialNumber { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
    }
}
