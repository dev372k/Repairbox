using RepairBox.BL.DTOs.RepairDefect;
using RepairBox.BL.ServiceModels.Brand;
using RepairBox.Common.Commons;
using RepairBox.Common.Helpers;
using RepairBox.DAL;
using RepairBox.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RepairBox.BL.Services
{
    public interface IRepairDefectServiceRepo
    {
        Task AddDefect(AddDefectDTO data);
        Task AddDefects(List<AddDefectDTO> datum, int modelId);
        Task DeleteDefect(int defectId);
        Task UpdateDefect(UpdateDefectDTO data);
        List<GetDefectDTO>? GetDefects();
        List<GetDefectDTO>? GetDefects(int modelId);
        GetDefectDTO? GetDefect(int defectId);
        PaginationModel GetDefects(string query, int pageNo);
        PaginationModel GetDefectsByModelId(string query, int pageNo, int modelId);
    }

    public class RepairDefectServiceRepo : IRepairDefectServiceRepo
    {
        public ApplicationDBContext _context;
        private int pageSize = DeveloperConstants.PAGE_SIZE;
        public RepairDefectServiceRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task AddDefects(List<AddDefectDTO> datum, int modelId)
        {
            List<RepairableDefect> defects = new List<RepairableDefect>();
            foreach (var data in datum)
            {
                defects.Add(new RepairableDefect
                {
                    DefectName = data.Title,
                    Cost = ConversionHelper.ConvertToDecimal(data.Cost),
                    Price = ConversionHelper.ConvertToDecimal(data.Price),
                    RepairTime = data.Time,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    ModelId = modelId
                });
            }

            var defectNames = defects.Select(x => x.DefectName.ToLower().Trim()).ToList();
            int index = 0;
            foreach (var defectName in defectNames)
            {
                var d = IGetDefects().FirstOrDefault(d => d.DefectName.ToLower().Trim().Contains(defectName));
                if (d != null)
                {
                    defects.RemoveAt(index);
                    index = 0;
                }
                else
                    index++;
            }

            if (defects.Count > 0)
            {
                await _context.RepairableDefects.AddRangeAsync(defects);
                await _context.SaveChangesAsync();
            }
        }

        private IQueryable<RepairableDefect> IGetDefects()
        {
            return _context.RepairableDefects.AsQueryable();
        }

        public async Task DeleteDefect(int defectId)
        {
            var defect = _context.RepairableDefects.FirstOrDefault(d => d.Id == defectId);
            if (defect != null)
            {
                _context.RepairableDefects.Remove(defect);
                await _context.SaveChangesAsync();
            }
        }

        public GetDefectDTO? GetDefect(int defectId)
        {
            var defect = _context.RepairableDefects.FirstOrDefault(rd => rd.Id == defectId);
            if (defect != null)
                return Omu.ValueInjecter.Mapper.Map<GetDefectDTO>(defect);
            return null;
        }

        public List<GetDefectDTO>? GetDefects()
        {
            var defects = new List<GetDefectDTO>();
            var defectList = _context.RepairableDefects.ToList();

            if (defectList != null)
            {
                defectList.ForEach(defect => defects.Add(Omu.ValueInjecter.Mapper.Map<GetDefectDTO>(defect)));
                return defects;
            }
            return null;
        }

        public List<GetDefectDTO>? GetDefects(int modelId)
        {
            var defects = new List<GetDefectDTO>();
            var defectList = _context.RepairableDefects.Where(b => b.ModelId == modelId).ToList();

            if (defectList != null)
            {
                defectList.ForEach(defect => defects.Add(Omu.ValueInjecter.Mapper.Map<GetDefectDTO>(defect)));
                return defects;
            }
            return null;
        }

        public PaginationModel GetDefects(string query, int pageNo)
        {
            List<GetDefectDTO> defectList = new List<GetDefectDTO>();
            var defectQuery = _context.RepairableDefects.AsQueryable();
            var defects = defectQuery.Where(d => query != null ? d.DefectName.ToLower().Contains(query.ToLower()) : true).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            defects.ForEach(defect => defectList.Add(Omu.ValueInjecter.Mapper.Map<GetDefectDTO>(defect)));

            return new PaginationModel
            {
                TotalPages = CommonHelper.TotalPagesforPagination(defectQuery.Count(), pageSize),
                CurrentPage = pageNo,
                Data = defectList
            };
        }

        public PaginationModel GetDefectsByModelId(string query, int pageNo, int modelId)
        {
            List<GetDefectDTO> defectList = new List<GetDefectDTO>();
            var defectQuery = _context.RepairableDefects.AsQueryable();
            var defects = defectQuery.Where(d => query != null ? d.DefectName.ToLower().Contains(query.ToLower()) : true && d.ModelId == modelId).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            defects.ForEach(defect => defectList.Add(Omu.ValueInjecter.Mapper.Map<GetDefectDTO>(defect)));

            return new PaginationModel
            {
                TotalPages = CommonHelper.TotalPagesforPagination(defectQuery.Count(), pageSize),
                CurrentPage = pageNo,
                Data = defectList
            };
        }

        public async Task UpdateDefect(UpdateDefectDTO data)
        {
            var defect = _context.RepairableDefects.FirstOrDefault(d => d.Id == data.Id);
            defect.DefectName = data.DefectName;
            defect.RepairTime = data.RepairTime;
            defect.Cost = data.Cost;
            defect.Price = data.Price;
            await _context.SaveChangesAsync();
        }

        public async Task AddDefect(AddDefectDTO data)
        {

            await _context.RepairableDefects.AddAsync(new RepairableDefect
            {
                DefectName = data.Title,
                Cost = ConversionHelper.ConvertToDecimal(data.Cost),
                Price = ConversionHelper.ConvertToDecimal(data.Price),
                RepairTime = data.Time,
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                ModelId = data.ModelId
            });

            await _context.SaveChangesAsync();
        }
    }
}
