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
    public class DesignationService : IDesignationService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<Designation> _designationRepository;
        private IGenericRepository<Employee> _employeeRepository;
        public DesignationService(IGenericRepository<Designation> designationRepository,
                            IDapperRepository dapperRepository,
                            IGenericRepository<Employee> employeeRepository)
        {
            _designationRepository = designationRepository;
            _dapperRepository = dapperRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IPagedList<DesignationViewModel>> DesignationListAsync(DesignationSearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                DesignationID,
	                                DesignationName ,
	                                DesignationLevel,
                                    Salary,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM Designation WHERE 1=1 AND IsDelete<>1 ");
            #region Filters
            
            if (!string.IsNullOrEmpty(model.DesignationName))
            {
                strSQL.AppendFormat(@" AND DesignationName=@DesignationName");
            }
            if (!string.IsNullOrEmpty(model.DesignationLevel))
            {
                strSQL.AppendFormat(@" AND DesignationLevel=@DesignationLevel");
            }
            #endregion

            #region Parameters
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@DesignationName", model.DesignationName);
            _parameters.Add("@DesignationLevel", model.DesignationLevel);
            #endregion

            return await _dapperRepository.ExecuteQueryWithPagedListAsync<DesignationViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CreatedTS");
        }

        public async Task<AccountResult> InsertIntoDesignationAsync(DesignationViewModel model)
        {
            var result = new AccountResult();
            if (_designationRepository.TableNoTracking.Any(x =>x.DesignationName == model.DesignationName && x.IsDelete == false))
            {
                result.Errors = new List<string> { "Designation " + model.DesignationName + " is already taken" };
                return result;
            }
            var newDesignation = new Designation()
            {
                DesignationName = model.DesignationName,
                DesignationLevel = model.DesignationName,
                Salary=model.Salary,
                CreatedBy = model.CreatedBy,
                CreatedTS = DateTime.UtcNow
            };
            _designationRepository.Insert(newDesignation);
            await _designationRepository.SaveChangesAsync();
            return result;
        }

        public Designation GetDesignationByID(int DesignationID)
        {
            return _designationRepository.Table.FirstOrDefault(x =>x.DesignationID == DesignationID);
        }

        public async Task<AccountResult> UpdateDesignationAsync(DesignationViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_designationRepository.TableNoTracking.Any(x => x.DesignationName == model.DesignationName && x.DesignationID!=model.DesignationID && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "Designation " + model.DesignationName + " is already taken" };
                    return result;
                }
                var ExistedDesignation = GetDesignationByID(model.DesignationID);
                if (ExistedDesignation != null)
                {
                    ExistedDesignation.DesignationName = model.DesignationName;
                    ExistedDesignation.DesignationLevel = model.DesignationLevel;
                    ExistedDesignation.Salary = model.Salary;
                    ExistedDesignation.ModifiedBy = model.ModifiedBy;
                    ExistedDesignation.ModifiedTS = DateTime.UtcNow;
                    _designationRepository.Update(ExistedDesignation);
                    await _designationRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Designation does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteDesignationAsync(int DesignationID)
        {
            var result = new AccountResult();
            var ExistedDesignation = GetDesignationByID(DesignationID);
            if (ExistedDesignation != null)
            {
                var EmployeeList = await _employeeRepository.TableNoTracking.Where(x => x.DesignationID == ExistedDesignation.DesignationID).ToListAsync();
                if (EmployeeList.Count() > 0)
                {
                    result.Errors = new List<string> { "Designation existed on Employee." };
                }
                else
                {
                    _designationRepository.Delete(ExistedDesignation);
                }
               
             await _designationRepository.SaveChangesAsync();
            }
            else
               {
                    result.Errors = new List<string> { "Designation does not exist." };
                    return result;
               }
            return result;
        }
        public async Task<DesignationViewModel> GetDesignationByIDAsync(int DesignationID)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                DesignationID,
	                                DesignationName ,
	                                DesignationLevel,
                                    Salary,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM Designation
                                    WHERE DesignationID=@DesignationID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@DesignationID", DesignationID);
            return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<DesignationViewModel>(strSQL.ToString(), _parameters);
        }
        public async Task<IList<SelectItemIntViewModel>> DDLDesignationListAsync()
        {
            return await _designationRepository.Table
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.DesignationID,
                              Value = x.DesignationName
                          }).ToListAsync();
        }
    }
}
