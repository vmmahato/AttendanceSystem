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
using System.IO;
using AttendanceSystem.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Services
{
    public class LeaveSetupService : ILeaveSetupService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<LeaveSetup> _leaveSetupRepository;
        private IGenericRepository<Employee> _employeeRepository;
        private IGenericRepository<DefaultLeave> _defaultLeaveRepository;
        public LeaveSetupService
            (
                            IGenericRepository<LeaveSetup> leaveSetupRepository,
                            IDapperRepository dapperRepository,
                            IGenericRepository<Employee> employeeRepository,
                            IGenericRepository<DefaultLeave> defaultLeaveRepository
            )
        {
            _leaveSetupRepository = leaveSetupRepository;
            _dapperRepository = dapperRepository;
            _employeeRepository = employeeRepository;
            _defaultLeaveRepository = defaultLeaveRepository;
        }

        public async Task<IPagedList<LeaveSetupViewModel>> LeaveSetupListAsync(LeaveSetupSearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                LeaveID,
	                                LeaveCode,
	                                LeaveName,
	                                LeaveDays,
	                                LeaveIncrement,
	                                CASE WHEN ApplicableGender='M' THEN 'MALE' 
									WHEN ApplicableGender='FE' THEN 'FEMALE'
									WHEN ApplicableGender='A' THEN 'ALL'
									WHEN ApplicableGender='O' THEN 'OTHER' END
									ApplicableGender,
	                                Description,
	                                IsReplacementLeave,
	                                IsPaidLeave,
	                                IsLeaveCarryable,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM LeaveSetup
                                    WHERE 1=1 ");
            #region Filters
            
            if (!string.IsNullOrEmpty(model.LeaveCode))
            {
                strSQL.AppendFormat(@" AND LeaveCode=@LeaveCode ");
            }
            if (!string.IsNullOrEmpty(model.LeaveName))
            {
                strSQL.AppendFormat(@" AND LeaveName=@LeaveName ");
            }
            #endregion

            #region Parameters
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@LeaveCode", model.LeaveCode);
            _parameters.Add("@LeaveName", model.LeaveName);
            #endregion

            return await _dapperRepository.ExecuteQueryWithPagedListAsync<LeaveSetupViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CountIndex");
        }

        public async Task<AccountResult> InsertIntoLeaveSetupAsync(LeaveSetupViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_leaveSetupRepository.TableNoTracking.Any(x => x.LeaveName == model.LeaveName && x.FiscalYear == model.FiscalYear))
                {
                    result.Errors = new List<string> { model.LeaveName + " is already Exist" };
                    return result;
                }
                if (_leaveSetupRepository.TableNoTracking.Any(x => x.LeaveCode == model.LeaveCode && x.FiscalYear == model.FiscalYear))
                {
                    result.Errors = new List<string> { "LeaveCode " + model.LeaveCode + " is already Exist" };
                    return result;
                }
                var newLeaveSetup = new LeaveSetup()
                {
                    LeaveCode = model.LeaveCode,
                    LeaveName = model.LeaveName,
                    LeaveDays = model.LeaveDays,
                    LeaveIncrement = model.LeaveIncrement,
                    ApplicableGender = model.ApplicableGender,
                    Description = model.Description,
                    IsReplacementLeave = model.IsReplacementLeave,
                    IsPaidLeave = model.IsPaidLeave,
                    IsLeaveCarryable = model.IsLeaveCarryable,
                    FiscalYear=model.FiscalYear,
                    CreatedBy = model.CreatedBy,
                    CreatedTS = DateTime.UtcNow
                };
                _leaveSetupRepository.Insert(newLeaveSetup);
                await _leaveSetupRepository.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public LeaveSetup GetLeaveSetupByID(int LeaveID)
        {
            return _leaveSetupRepository.Table.FirstOrDefault(x =>x.LeaveID == LeaveID);
        }

        public async Task<AccountResult> UpdateLeaveSetupAsync(LeaveSetupViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if(_leaveSetupRepository.TableNoTracking.Any(x => x.LeaveName == model.LeaveName && x.FiscalYear == model.FiscalYear && x.LeaveID!=model.LeaveID))
                {
                    result.Errors = new List<string> { model.LeaveName + " is already Exist" };
                    return result;
                }
                if(_leaveSetupRepository.TableNoTracking.Any(x => x.LeaveCode == model.LeaveCode && x.FiscalYear == model.FiscalYear && x.LeaveID != model.LeaveID))
                {
                    result.Errors = new List<string> { "LeaveCode " + model.LeaveCode + " is already Exist" };
                    return result;
                }
                var ExistedLeave = GetLeaveSetupByID(model.LeaveID);
                if (ExistedLeave != null)
                {
                    ExistedLeave.LeaveCode = model.LeaveCode;
                    ExistedLeave.LeaveName = model.LeaveName;
                    ExistedLeave.LeaveDays = model.LeaveDays;
                    ExistedLeave.LeaveIncrement = model.LeaveIncrement;
                    ExistedLeave.ApplicableGender = model.ApplicableGender;
                    ExistedLeave.Description = model.Description;
                    ExistedLeave.IsReplacementLeave = model.IsReplacementLeave;
                    ExistedLeave.IsPaidLeave = model.IsPaidLeave;
                    ExistedLeave.IsLeaveCarryable = model.IsLeaveCarryable;
                    ExistedLeave.FiscalYear = model.FiscalYear;
                    ExistedLeave.ModifiedBy = model.ModifiedBy;
                    ExistedLeave.ModifiedTS = DateTime.UtcNow;

                     _leaveSetupRepository.Update(ExistedLeave);
                     await _leaveSetupRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Leave does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteLeaveSetupAsync(int LeaveID)
        {
            try
            {
                var result = new AccountResult();
                var ExistedLeave = GetLeaveSetupByID(LeaveID);
                if (ExistedLeave != null)
                {
                    _leaveSetupRepository.Delete(ExistedLeave);
                    await _leaveSetupRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Leave does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<LeaveSetupViewModel> GetLeaveSetupByIDAsync(int LeaveID)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                LeaveID,
	                                LeaveCode,
	                                LeaveName,
	                                LeaveDays,
	                                LeaveIncrement,
	                                ApplicableGender,
	                                Description,
	                                IsReplacementLeave,
	                                IsPaidLeave,
	                                IsLeaveCarryable,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM LeaveSetup
                                    WHERE LeaveID=@LeaveID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@LeaveID", LeaveID);
            return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<LeaveSetupViewModel>(strSQL.ToString(), _parameters);
        }
        public async Task<IList<SelectItemIntViewModel>> DDLLeaveTypeListAsync()
        {
            return await _leaveSetupRepository.Table
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.LeaveID,
                              Value = x.LeaveName
                          }).ToListAsync();
        }
        public async Task<IList<SelectItemIntViewModel>> DDLGenderWiseLeaveTypeListAsync(int EmployeeID)
        {
            try
            {
                var result = await _employeeRepository.TableNoTracking.FirstOrDefaultAsync(x => x.EmployeeID == EmployeeID);
                return await _leaveSetupRepository.Table.Where(x=>x.ApplicableGender== result.Gender || x.ApplicableGender=="ALL")
                              .Select(x => new SelectItemIntViewModel()
                              {
                                  ID = x.LeaveID,
                                  Value = x.LeaveName
                              }).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<AccountResult> PullDefaultLeaveAsync(int FiscalYear)
        {
            try
            {
                var result = new AccountResult();
                var defaultLeave = _defaultLeaveRepository.Table.ToList();
                var list = new List<LeaveSetup>();
                if (defaultLeave.Count>0)
                {
                    foreach(var Defaultleaves in defaultLeave)
                    {
                        if (!_leaveSetupRepository.TableNoTracking.Any(x => (x.LeaveName == Defaultleaves.LeaveName || x.LeaveCode == Defaultleaves.LeaveCode) && x.FiscalYear == FiscalYear))
                        {
                            var newLeaveSetup = new LeaveSetup()
                            {
                                LeaveCode = Defaultleaves.LeaveCode,
                                LeaveName = Defaultleaves.LeaveName,
                                LeaveDays = Defaultleaves.LeaveDays,
                                LeaveIncrement = Defaultleaves.LeaveIncrement,
                                ApplicableGender = Defaultleaves.ApplicableGender,
                                Description = Defaultleaves.Description,
                                IsReplacementLeave = Defaultleaves.IsReplacementLeave,
                                IsPaidLeave = Defaultleaves.IsPaidLeave,
                                IsLeaveCarryable = Defaultleaves.IsLeaveCarryable,
                                FiscalYear = FiscalYear,
                                CreatedBy = Defaultleaves.CreatedBy,
                                CreatedTS = DateTime.UtcNow
                            };
                        list.Add(newLeaveSetup);
                        }
                    }
                }
                
                _leaveSetupRepository.Insert(list);
                await _leaveSetupRepository.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}
