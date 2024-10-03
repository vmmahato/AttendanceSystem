using AttendanceSystem.DapperServices;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Service
{
    public class LeaveOpeningBalanceService : ILeaveOpeningBalanceService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<LeaveOpeningBalance> _leaveOpeningBalanceRepository;
        private IUnitOfWorkManager _unitOfWork;
        private IGenericRepository<LeaveSetup> _leaveSetupRepository;
        public LeaveOpeningBalanceService(IDapperRepository dapperRepository,
                                          IGenericRepository<LeaveOpeningBalance> leaveOpeningBalanceRepository,
                                          IUnitOfWorkManager unitOfWork,
                                          IGenericRepository<LeaveSetup> leaveSetupRepository)
        {
            _dapperRepository = dapperRepository;
            _leaveOpeningBalanceRepository = leaveOpeningBalanceRepository;
            _unitOfWork = unitOfWork;
            _leaveSetupRepository = leaveSetupRepository;
        }
        public async Task<IEnumerable<EmployeeWithLeaveTypeList>> GetEmployeeLeaveTypeListAsync(EmployeeLeaveList model)
        {
            if (model.EmployeeIDs.Count == 0) return null;
            var strSQL = new StringBuilder();
            var EmployeeIDList = string.Join(",", model.EmployeeIDs);
            strSQL.AppendFormat(@"SELECT DISTINCT E.[EmployeeID],LS.ApplicableGender
										  ,E.Gender
                                          ,E.[FirstName]
                                          ,E.[LastName]
                                    	  ,LS.LeaveCode
										  ,CASE WHEN LOB.Value IS NOT NULL THEN LOB.Value
										   WHEN LQ.LeaveBalance IS NOT NULL THEN LQ.LeaveBalance
										   ELSE LS.LeaveDays END AS Value
                                      FROM  LeaveSetup LS 
									  LEFT JOIN LeaveQuota LQ ON LS.LeaveID=LQ.LeaveID
									  LEFT JOIN Designation D ON LQ.DesignationID=D.DesignationID
									  LEFT JOIN Employee E  ON 
									  (
									  (LS.ApplicableGender=E.Gender OR LS.ApplicableGender='A') 
									  AND
									  (D.DesignationID=E.DesignationID OR D.DesignationID IS NULL)
									   )
                                      LEFT JOIN LeaveOpeningBalance LOB ON E.EmployeeID=LOB.EmployeeID AND LS.LeaveCode=LOB.Type
                                      WHERE LS.FiscalYear=@FiscalYearID AND E.EmployeeID IN(" + EmployeeIDList + ")   ORDER BY LS.LeaveCode");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@FiscalYearID", model.FiscalYearID);
            return await _dapperRepository.ExecuteQueryAsync<EmployeeWithLeaveTypeList>(strSQL.ToString(), _parameters);
        }

        public async Task<AccountResult> UpdateLeaveOpeningBalanceAsync(EmployeeWithYearLeaveList model, int CreatedBy)
        {
            var result = new AccountResult();
            var DataToInsert = new List<LeaveOpeningBalance>();
            if (model.EmployeeList.Count > 0)
            {
                foreach(var employee in model.EmployeeList)
                {
                    if (employee.List.Count() > 0)
                    {
                        foreach(var leaveCode in employee.List)
                        {
                            if(leaveCode.ApplicableGender=="A" || leaveCode.ApplicableGender == leaveCode.Gender)
                            {
                                decimal codeValue = leaveCode.Value ?? 0;
                                var data = new LeaveOpeningBalance()
                                {
                                    EmployeeID = employee.EmployeeID,
                                    Type = leaveCode.LeaveCode,
                                    Value = codeValue,
                                    CreatedBy = CreatedBy,
                                    CreatedTS = DateTime.UtcNow,
                                    FiscalYear = model.FiscalYear
                                };
                                DataToInsert.Add(data);
                            }
                        }
                    }

                }
                using (var uow = _unitOfWork.NewUnitOfWork())
                {
                    try
                    {
                      var DeletedData=  await DeleteLeaveOpeningBalanceAsync(model.EmployeeList, model.FiscalYear);
                        if (DeletedData.Success)
                        {
                            await _leaveOpeningBalanceRepository.BulkInsertAsync(DataToInsert);
                            await _leaveOpeningBalanceRepository.SaveChangesAsync();
                        }
                        else
                        {
                            uow.Rollback();
                        }
                        uow.Commit();
                    }
                    catch (Exception e)
                    {
                        uow.Rollback();
                        throw e;
                    }
                }
                   
            }

            return result;
        }

        public async Task<AccountResult> DeleteLeaveOpeningBalanceAsync(List<EmployeeWithLeaveTypeList> List,int FiscalYear)
        {
            var result = new AccountResult();
            var strSQL = new StringBuilder();
            string EmployeeIDs = string.Empty;
            if (List.Count > 0)
            {
                EmployeeIDs = string.Join(",", List.Select(x => x.EmployeeID).Distinct().ToList());
                strSQL.AppendFormat(@"DELETE FROM [dbo].[LeaveOpeningBalance] WHERE FiscalYear=@Year AND EmployeeID IN (" + EmployeeIDs + ")");
                #region Filter
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@Year", FiscalYear);
                #endregion
                await _dapperRepository.ExecuteAsync(strSQL.ToString(), _parameters);
            }
            return result;
        }

        public async Task<List<SelectItemViewModel>> GetLeaveCodeList(int FiscalYear)
        {
            return await _leaveSetupRepository.TableNoTracking
                  .Select(x => new SelectItemViewModel()
                  {
                      ID = x.LeaveCode,
                      Value = x.ApplicableGender
                  }).ToListAsync();
        }
    }
}
