//using RepairBox.BL.DTOs.Permission;
//using RepairBox.DAL;
//using RepairBox.DAL.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RepairBox.BL.Services
//{
//    public interface IPermissionServiceRepo
//    {
//        List<GetPermissionDTO?> GetPermissions();
//        GetPermissionDTO? GetPermissionById(int id);
//        void AddPermission(AddPermissionDTO permissionDTO);
//        void UpdatePermission(UpdatePermissionDTO permissionDTO);
//        bool DeletePermission(int id);
//    }
//    public class PermissionServiceRepo : IPermissionServiceRepo
//    {
//        public ApplicationDBContext _context;
//        public PermissionServiceRepo(ApplicationDBContext context)
//        {
//            _context = context;
//        }
//        public List<GetPermissionDTO?> GetPermissions()
//        {
//            List<GetPermissionDTO> permissions = new List<GetPermissionDTO>();
//            var permissionList = _context.Permissions.ToList();
//            if (permissionList != null)
//            {
//                permissionList.ForEach(permission => permissions.Add(Omu.ValueInjecter.Mapper.Map<GetPermissionDTO>(permission)));
//                return permissions;
//            }
//            return null;
//        }
//        public GetPermissionDTO? GetPermissionById(int id)
//        {
//            var permission = _context.Permissions.FirstOrDefault(p => p.Id == id);
//            if (permission != null)
//            {
//                var dbRequest = Omu.ValueInjecter.Mapper.Map<GetPermissionDTO>(permission);
//                return dbRequest;
//            }
//            return null;
//        }
//        public void AddPermission(AddPermissionDTO permissionDTO)
//        {
//            var permission = new Permission
//            {
//                Name = permissionDTO.Name
//            };

//            _context.Permissions.Add(permission);
//            _context.SaveChanges();

//            return;
//        }
//        public void UpdatePermission(UpdatePermissionDTO permissionDTO)
//        {
//            var permission = _context.Permissions.FirstOrDefault(p => p.Id == permissionDTO.Id);
//            if (permission != null)
//            {
//                permission.Name = permissionDTO.Name;

//                _context.SaveChanges();
//            }

//            return;
//        }
//        public bool DeletePermission(int id)
//        {
//            var permission = _context.Permissions.FirstOrDefault(p => p.Id == id);
//            if (permission != null)
//            {
//                _context.Permissions.Remove(permission);
//                _context.SaveChanges();

//                return true;
//            }

//            return false;
//        }
//    }
//}
