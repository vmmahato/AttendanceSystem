using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.DapperServices;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Services
{
    public class DepartmentService : IDepartmentService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<Department> _departmentRepository;
        private IGenericRepository<Section> _sectionRepository;
        private IGenericRepository<Employee> _employeeRepository;
        public DepartmentService(IGenericRepository<Department> departmentRepository,
                            IDapperRepository dapperRepository,
                            IGenericRepository<Employee> employeeRepository,
                            IGenericRepository<Section> sectionRepository)
        {
            _departmentRepository = departmentRepository;
            _dapperRepository = dapperRepository;
            _employeeRepository = employeeRepository;
            _sectionRepository = sectionRepository;
        }

        public async Task<IPagedList<DepartmentViewModel>> DepartmentListAsync(DepartmentSearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                DepartmentID,
	                                DepartmentCode ,
	                                DepartmentName,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM Department WHERE 1=1 AND IsDelete<>1 ");
            #region Filters
            
            if (!string.IsNullOrEmpty(model.DepartmentName))
            {
                strSQL.AppendFormat(@" AND DepartmentName=@DepartmentName");
            }
            #endregion

            #region Parameters
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@DepartmentName", model.DepartmentName);
            #endregion

            return await _dapperRepository.ExecuteQueryWithPagedListAsync<DepartmentViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CreatedTS");
        }

        public async Task<AccountResult> InsertIntoDepartmentAsync(DepartmentViewModel model)
        {
            var result = new AccountResult();
            if (_departmentRepository.TableNoTracking.Any(x =>x.DepartmentName == model.DepartmentName && x.IsDelete == false))
            {
                result.Errors = new List<string> { "Department " + model.DepartmentName + " is already taken" };
                return result;
            }
            if (_departmentRepository.TableNoTracking.Any(x => x.DepartmentCode == model.DepartmentCode && x.IsDelete == false))
            {
                result.Errors = new List<string> { "Department code " + model.DepartmentCode + " is already taken" };
                return result;
            }
            var newDepartment = new Department()
            {
                DepartmentCode = model.DepartmentCode,
                DepartmentName = model.DepartmentName,
                CreatedBy = model.CreatedBy,
                CreatedTS = DateTime.UtcNow
            };
            _departmentRepository.Insert(newDepartment);
            await _departmentRepository.SaveChangesAsync();
            return result;
        }

        public Department GetDepartmentByID(int DepartmentID)
        {
            return _departmentRepository.Table.FirstOrDefault(x =>x.DepartmentID == DepartmentID);
        }

        public async Task<AccountResult> UpdateDepartmentAsync(DepartmentViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_departmentRepository.TableNoTracking.Any(x => x.DepartmentName == model.DepartmentName && x.DepartmentID!=model.DepartmentID && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "Department " + model.DepartmentName + " is already taken" };
                    return result;
                }
                if (_departmentRepository.TableNoTracking.Any(x => x.DepartmentCode == model.DepartmentCode && x.DepartmentID != model.DepartmentID && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "Department code " + model.DepartmentCode + " is already taken" };
                    return result;
                }
                var ExistedDepartment = GetDepartmentByID(model.DepartmentID);
                if (ExistedDepartment != null)
                {
                    ExistedDepartment.DepartmentCode = model.DepartmentCode;
                    ExistedDepartment.DepartmentName = model.DepartmentName;
                    ExistedDepartment.ModifiedBy = model.ModifiedBy;
                    ExistedDepartment.ModifiedTS = DateTime.UtcNow;
                    _departmentRepository.Update(ExistedDepartment);
                    await _departmentRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Department does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteDepartmentAsync(int DepartmentID)
        {
            var result = new AccountResult();
            var ExistedDepartment = GetDepartmentByID(DepartmentID);
            if (ExistedDepartment != null)
            {
                   var EmployeeList = await _employeeRepository.TableNoTracking.Where(x => x.DepartmentID == ExistedDepartment.DepartmentID).ToListAsync();
                   var SectionList = await _sectionRepository.TableNoTracking.Where(x => x.DepartmentID == ExistedDepartment.DepartmentID).ToListAsync();
                if (EmployeeList.Count() > 0)
                {
                    result.Errors = new List<string> { "Department existed on Employee." };
                }
                else if (SectionList.Count() > 0)
                {
                    result.Errors = new List<string> { "Department existed on Section." };
                }
                else
                {
                    _departmentRepository.Delete(ExistedDepartment);
                }
             await _departmentRepository.SaveChangesAsync();
            }
            else
               {
                    result.Errors = new List<string> { "Department does not exist." };
                    return result;
               }
            return result;
        }
        public async Task<DepartmentViewModel> GetDepartmentByIDAsync(int DepartmentID)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                DepartmentID,
	                                DepartmentCode ,
	                                DepartmentName,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM Department 
                              WHERE DepartmentID=@DepartmentID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@DepartmentID", DepartmentID);
            return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<DepartmentViewModel>(strSQL.ToString(), _parameters);
        }
        public async Task<IList<SelectItemIntViewModel>> DDLDepartmentListAsync()
        {
            return await _departmentRepository.Table
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.DepartmentID,
                              Value = x.DepartmentName
                          }).ToListAsync();
        }
    }
}
