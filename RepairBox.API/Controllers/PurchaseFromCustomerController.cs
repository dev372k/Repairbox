using Microsoft.AspNetCore.Mvc;
using RepairBox.API.Models;
using RepairBox.BL.DTOs.CustomerIdentities;
using RepairBox.BL.DTOs.CustomerInfo;
using RepairBox.BL.DTOs.DeviceInfo;
using RepairBox.BL.DTOs.CustomerProductPurchase;
using RepairBox.BL.DTOs.PurchaseFromCustomerInvoiceDTOs;
using RepairBox.BL.Services;
using RepairBox.Common.Commons;
using System.Web;
using System.Text;
using RepairBox.BL.DTOs.PurchaseFromCustomer;
using RepairBox.Common.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using RepairBox.BL.DTOs.Order;
using RepairBox.BL.DTOs.Company;

namespace RepairBox.API.Controllers
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class PurchaseFromCustomerController : ControllerBase
    {
        private IPurchaseFromCustomerServiceRepo _purchaseFromCustomerRepo;
        private ICompanyServiceRepo _companyServiceRepo;
        public PurchaseFromCustomerController(IPurchaseFromCustomerServiceRepo purchaseFromCustomerRepo, ICompanyServiceRepo companyServiceRepo)
        {
            _purchaseFromCustomerRepo = purchaseFromCustomerRepo;
            _companyServiceRepo = companyServiceRepo;
        }

        [HttpGet("GetPurchaseFromCustomerFilterSortDropdown")]
        public IActionResult GetPurchaseFromCustomerFilterSortDropdown()
        {
            try
            {
                var FilterSortDropdown = new List<string>
                {
                    "Order Number", "Invoice ID", "Full name", "Phone", "Email", "IMEI", "Serial number", "Created at"
                };

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = FilterSortDropdown });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetPurchaseFromCustomerInvoice")]
        public IActionResult GetPurchaseFromCustomerInvoice(string invoiceId)
        {
            try
            {
                long id = ConversionHelper.ConvertToInt64(invoiceId);
                var data = _purchaseFromCustomerRepo.GetPurchaseFromCustomerInvoice(id);

                if(data == null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_FOUND, "Invoice") });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetPurchasesFromCustomersList")]
        public IActionResult GetPurchasesFromCustomersList(int pageNo)
        {
            try
            {
                var data = _purchaseFromCustomerRepo.GetPurchaseFromCustomerListings(pageNo, null);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("GetPurchasesFromCustomersListWithFilters")]
        public IActionResult GetPurchasesFromCustomersListWithFilters([FromQuery] int pageNo, [FromBody] DisplayPurchaseFromCustomerFiltersDTO pfcFilters)
        {
            try
            {
                var data = _purchaseFromCustomerRepo.GetPurchaseFromCustomerListings(pageNo, pfcFilters);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("AddPurchaseFromCustomer")]
        public IActionResult AddPurchaseFromCustomer(AddPurchaseFromCustomerDTO addPurchaseFromCustomer)
        {
            AddCustomerInfoDTO customerInfo = addPurchaseFromCustomer.CustomerInfo;
            AddCustomerIdentitiesDTO customerIdentities = addPurchaseFromCustomer.CustomerIdentities;
            AddDeviceInfoDTO deviceInfo = addPurchaseFromCustomer.DeviceInfo;

            try
            {
                int customerInfoId = _purchaseFromCustomerRepo.AddPurchaseFromCustomer(customerInfo, customerIdentities, deviceInfo);

                DateTime currentDate = DateTime.Now;
                string formattedDate = ConversionHelper.DateTimeFormatting(currentDate);

                long invoiceId;

                do
                {
                    invoiceId = CommonHelper.RandomLongValueGenerator(1000000000, 9999999999);
                } while (_purchaseFromCustomerRepo.GetPurchaseFromCustomerInvoice(invoiceId) != null);

                var company = _companyServiceRepo.GetCompany();

                if (company == null) { company = new GetCompanyDTO(); }

                var data = String.Format("Company Name: {0}\nCompany Phone: {1}\nCompany Address: {2}\nInvoice Number: #INV-{3}\nInvoice Date: {4}\nClient Name: {5}\nClient Address: {6}\nDevice Model: {7}\nDevice IMEI: {8}\nDevice Serial Number: {9}\nDevice Quantity: {10}\nTotal Amount: {11}", company.Name, company.Phone, company.Address, invoiceId, formattedDate, customerInfo.Name, customerInfo.Address, deviceInfo.DeviceNameModel, deviceInfo.IMEI, deviceInfo.SerialNumber, 1, deviceInfo.Price);

                var QRCodePath = String.Format("wwwroot\\QRCode\\{0}-QR.png", invoiceId);

                CommonHelper.GenerateQRCode(data, QRCodePath);

                AddPurchaseFromCustomerInvoiceDTO purchaseFromCustomerInvoiceDTO = new AddPurchaseFromCustomerInvoiceDTO();
                purchaseFromCustomerInvoiceDTO.invoiceId = invoiceId;
                purchaseFromCustomerInvoiceDTO.CustomerInfoId = customerInfoId;
                purchaseFromCustomerInvoiceDTO.Date = currentDate;
                purchaseFromCustomerInvoiceDTO.QRCodePath = QRCodePath;

                _purchaseFromCustomerRepo.AddPurchaseFromCustomerInvoice(purchaseFromCustomerInvoiceDTO);

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = invoiceId.ToString(), Message = String.Format(CustomMessage.ADDED_SUCCESSFULLY, "Purchase from Customer") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("DeletePurchaseFromCustomer")]
        public IActionResult DeletePurchaseFromCustomer(string invoiceId)
        {
            try
            {
                long id = ConversionHelper.ConvertToInt64(invoiceId);
                var deleted = _purchaseFromCustomerRepo.DeletePurchaseFromCustomer(id);
                if (deleted)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = String.Format(CustomMessage.DELETED_SUCCESSFULLY, "Purchase from Customer") });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = String.Format(CustomMessage.NOT_FOUND, "Purchase from Customer") });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdatePurchaseFromCustomer")]
        public IActionResult UpdatePurchaseFromCustomer(UpdatePurchaseFromCustomerDTO updatePurchaseFromCustomer)
        {
            UpdateCustomerInfoDTO customerInfo = updatePurchaseFromCustomer.CustomerInfo;
            UpdateCustomerIdentitiesDTO customerIdentities = updatePurchaseFromCustomer.CustomerIdentities;
            UpdateDeviceInfoDTO deviceInfo = updatePurchaseFromCustomer.DeviceInfo;
            long invoiceId = ConversionHelper.ConvertToInt64(updatePurchaseFromCustomer.invoiceId);

            try
            {
                var purchaseFromCustomerInvoice = _purchaseFromCustomerRepo.UpdatePurchaseFromCustomer(customerInfo, customerIdentities, deviceInfo, invoiceId);

                if (purchaseFromCustomerInvoice != null)
                {
                    string formattedDate = ConversionHelper.DateTimeFormatting(purchaseFromCustomerInvoice.Date);

                    var company = _companyServiceRepo.GetCompany();

                    var data = string.Empty;
                    if (company != null)
                        String.Format("Company Name: {0}\nCompany Phone: {1}\nCompany Address: {2}\nInvoice Number: #INV-{3}\nInvoice Date: {4}\nClient Name: {5}\nClient Address: {6}\nDevice Model: {7}\nDevice IMEI: {8}\nDevice Serial Number: {9}\nDevice Quantity: {10}\nTotal Amount: {11}", company.Name, company.Phone, company.Address, purchaseFromCustomerInvoice.invoiceId, formattedDate, customerInfo.Name, customerInfo.Address, deviceInfo.DeviceNameModel, deviceInfo.IMEI, deviceInfo.SerialNumber, 1, deviceInfo.Price);
                    else
                        String.Format("Company Name: {0}\nCompany Phone: {1}\nCompany Address: {2}\nInvoice Number: #INV-{3}\nInvoice Date: {4}\nClient Name: {5}\nClient Address: {6}\nDevice Model: {7}\nDevice IMEI: {8}\nDevice Serial Number: {9}\nDevice Quantity: {10}\nTotal Amount: {11}", "", "", "", purchaseFromCustomerInvoice.invoiceId, formattedDate, customerInfo.Name, customerInfo.Address, deviceInfo.DeviceNameModel, deviceInfo.IMEI, deviceInfo.SerialNumber, 1, deviceInfo.Price);

                    var QRCodePath = String.Format("wwwroot\\QRCode\\{0}-QR.png", purchaseFromCustomerInvoice.invoiceId);

                    CommonHelper.GenerateQRCode(data, QRCodePath);

                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = String.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Purchase from Customer") });
                }
                else
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = String.Format(CustomMessage.NOT_FOUND, "Invoice") });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
