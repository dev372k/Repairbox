using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using RepairBox.API.Models;
using RepairBox.BL.DTOs.Brand;
using RepairBox.BL.DTOs.Model;
using RepairBox.BL.DTOs.RepairDefect;
using RepairBox.BL.Services;
using RepairBox.Common.Commons;
using System.Text;

namespace RepairBox.API.Controllers
{
    [Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private IBrandServiceRepo _brandRepo;
        private IModelServiceRepo _modelRepo;
        private IRepairDefectServiceRepo _defectRepo;
        public BrandController(IBrandServiceRepo brandRepo, IModelServiceRepo modelRepo, IRepairDefectServiceRepo defectRepo)
        {
            _brandRepo = brandRepo;
            _modelRepo = modelRepo;
            _defectRepo = defectRepo;
        }

        #region Brand
        [HttpPost("AddBrand")]
        public async Task<IActionResult> AddBrand(string Name)
        {
            try
            {
                await _brandRepo.AddBrand(Name);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Brand") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
        
        [HttpPost("AddBrands")]
        public async Task<IActionResult> AddBrands(IFormFile file)
        {
            try
            {
                var datum = (List<AddBrandDTO>)readCSV(file, new AddBrandDTO());
                await _brandRepo.AddBrands(datum);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Brand(s)") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand(int Id, string Name)
        {
            try
            {
                await _brandRepo.UpdateBrand(Id, Name);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Brand") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("DeleteBrand")]
        public async Task<IActionResult> DeleteBrand(int Id)
        {
            try
            {
                await _brandRepo.DeleteBrand(Id);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.DELETED_SUCCESSFULLY, "Brand") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetBrand")]
        public IActionResult GetBrand(int brandId)
        {
            try
            {
                var data = _brandRepo.GetBrand(brandId);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetBrands")]
        public IActionResult GetBrands(string? query, int pageNo = 1)
        {
            try
            {
                var data = _brandRepo.GetBrands(query, pageNo);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetBrandsforDropdown")]
        public IActionResult GetBrandsforDropdown()
        {
            try
            {
                var data = _brandRepo.GetBrands();
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
        #endregion

        #region Model

        [HttpPost("AddModel")]
        public async Task<IActionResult> AddModel(AddModelDTO model)
        {
            try
            {
                await _modelRepo.AddModel(model);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Model") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("AddModels")]
        public async Task<IActionResult> AddModels(IFormFile file, int brandId)
        {
            try
            {
                var datum = (List<AddModelDTO>)readCSV(file, new AddModelDTO());
                await _modelRepo.AddModels(datum, brandId);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Model(s)") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdateModel")]
        public async Task<IActionResult> UpdateModel(UpdateModelDTO model)
        {
            try
            {
                await _modelRepo.UpdateModel(model);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Model") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("DeleteModel")]
        public async Task<IActionResult> DeleteModel(int Id)
        {
            try
            {
                await _modelRepo.DeleteModel(Id);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.DELETED_SUCCESSFULLY, "Model") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetModel")]
        public IActionResult GetModel(int modelId)
        {
            try
            {
                var data = _modelRepo.GetModel(modelId);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetModels")]
        public IActionResult GetModels(string? query, int pageNo = 1)
        {
            try
            {
                var data = _modelRepo.GetModels(query, pageNo);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        //[HttpGet("GetBrandModels")]
        //public IActionResult GetBrandModels(string? query, int brandId, int pageNo = 1)
        //{
        //    try
        //    {
        //        var data = _modelRepo.GetModelsByBrandId(query, pageNo, brandId);
        //        return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
        //    }
        //}

        [HttpGet("GetModelsforDropdown")]
        public IActionResult GetModelsforDropdown()
        {
            try
            {
                var data = _modelRepo.GetModels();
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
        
        [HttpGet("GetBrandModelsforDropdown")]
        public IActionResult GetBrandModelsforDropdown(int brandId)
        {
            try
            {
                var data = _modelRepo.GetModels(brandId);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
        #endregion

        #region Defect
        [HttpPost("AddDefect")]
        public async Task<IActionResult> AddDefect(AddDefectDTO model)
        {
            try
            {
                await _defectRepo.AddDefect(model);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Defect(s)") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
        
        [HttpPost("AddDefects")]
        public async Task<IActionResult> AddDefects(IFormFile file, int modelId)
        {
            try
            {
                var datum = (List<AddDefectDTO>)readCSV(file, new AddDefectDTO());
                await _defectRepo.AddDefects(datum, modelId);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Defect(s)") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("UpdateDefect")]
        public async Task<IActionResult> UpdateDefect(UpdateDefectDTO model)
        {
            try
            {
                await _defectRepo.UpdateDefect(model);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.UPDATED_SUCCESSFULLY, "Defect") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpPost("DeleteDefect")]
        public async Task<IActionResult> DeleteDefect(int Id)
        {
            try
            {
                await _defectRepo.DeleteDefect(Id);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.DELETED_SUCCESSFULLY, "Defect") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetDefect")]
        public IActionResult GetDefect(int defectId)
        {
            try
            {
                var data = _defectRepo.GetDefect(defectId);
                if(data != null)
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
                else
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = String.Format(CustomMessage.NOT_FOUND, "Defect") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        [HttpGet("GetDefects")]
        public IActionResult GetDefects(string? query, int pageNo = 1)
        {
            try
            {
                var data = _defectRepo.GetDefects(query, pageNo);
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }

        //[HttpGet("GetModelDefects")]
        //public IActionResult GetModelDefects(string? query, int modelId, int pageNo = 1)
        //{
        //    try
        //    {
        //        var data = _defectRepo.GetDefectsByModelId(query, pageNo, modelId);
        //        return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
        //    }
        //}

        [HttpGet("GetDefectsforDropdown")]
        public IActionResult GetDefectsforDropdown()
        {
            try
            {
                var data = _defectRepo.GetDefects();
                if( data == null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = new List<GetDefectDTO>() });
                }
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
        
        [HttpGet("GetModelDefectsforDropdown")]
        public IActionResult GetModelDefectsforDropdown(int modelId)
        {
            try
            {
                var data = _defectRepo.GetDefects(modelId);
                if (data == null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = new List<GetDefectDTO>() });
                }
                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
        #endregion

        // Read CSV
        private object? readCSV(IFormFile csvFile, object Data)
        {
            List<AddBrandDTO> brands = new List<AddBrandDTO>();
            List<AddModelDTO> models = new List<AddModelDTO>();
            List<AddDefectDTO> defects = new List<AddDefectDTO>();
            try
            {
                using (TextFieldParser parser = new TextFieldParser(csvFile.OpenReadStream(), Encoding.UTF8))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (Data.GetType() == typeof(AddBrandDTO))
                            brands.Add(new AddBrandDTO
                            {
                                Name = fields[0]
                            });
                        else if (Data.GetType() == typeof(AddModelDTO))
                            models.Add(new AddModelDTO
                            {
                                Name = fields[0],
                                Model = fields[1]
                            });
                        else if (Data.GetType() == typeof(AddDefectDTO))
                            defects.Add(new AddDefectDTO
                            {
                                Title = fields[0],
                                Price = fields[1],
                                Cost = fields[2],
                                Time = fields[3]
                            });
                    }
                }
                if (Data.GetType() == typeof(AddBrandDTO))
                {
                    brands.Remove(brands.FirstOrDefault());
                    return brands;
                }
                else if (Data.GetType() == typeof(AddModelDTO))
                {
                    models.Remove(models.FirstOrDefault());
                    return models;
                }
                else if (Data.GetType() == typeof(AddDefectDTO))
                {
                    defects.Remove(defects.FirstOrDefault());
                    return defects;
                }
                return null;
            }
            catch (Exception)
            {
                throw new Exception("There is a problem in csv file.");
            }
        }
    }
}
