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

namespace AttendanceSystem.Services
{
    public class LeaveSettlementService : ILeaveSettlementService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<LeaveSettlement> _leaveSettlementRepository;
        public LeaveSettlementService(IGenericRepository<LeaveSettlement> leaveSettlementRepository,
                            IDapperRepository dapperRepository)
        {
            _leaveSettlementRepository = leaveSettlementRepository;
            _dapperRepository = dapperRepository;
        }

        public async Task<IPagedList<LeaveSettlementViewModel>> LeaveSettlementListAsync(LeaveSettlementSearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                L.SettlementID,
	                                L.EmployeeID,
									E.FirstName+' '+LastName EmployeeName,
	                                L.LeaveID,
									LS.LeaveName,
	                                L.OpeningBalance,
	                                L.LeaveTaken,
	                                L.RemainingLeave,
	                                L.CarryToNext,
	                                L.Pay,
	                                L.SettlingLeave,
                                    L.FiscalYear,
									F.FiscalYear FiscalYears,
	                                L.CreatedTS,
	                                L.CreatedBy,
	                                L.ModifiedTS,
	                                L.ModifiedBy
	                                FROM LeaveSettlement L JOIN  Employee E 
									ON L.EmployeeID=E.EmployeeID
									JOIN LeaveSetup LS ON L.LeaveID=LS.LeaveID 
									LEFT OUTER JOIN FiscalYears F ON F.FiscalID=L.FiscalYear
									WHERE 1=1 ");
            #region Filters

            if (!string.IsNullOrEmpty(model.Year))
            {
                strSQL.AppendFormat(@" AND Year=@Year");
            }
            if (!string.IsNullOrEmpty(model.EmployeeName))
            {
                strSQL.AppendFormat(@" AND EmployeeName=@EmployeeName");
            }
            #endregion

            #region Parameters
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@EmployeeName", model.EmployeeName);
            _parameters.Add("@Year", model.Year);
            #endregion

            return await _dapperRepository.ExecuteQueryWithPagedListAsync<LeaveSettlementViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CountIndex");
        }

        public async Task<AccountResult> InsertIntoLeaveSettlementAsync(LeaveSettlementViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_leaveSettlementRepository.TableNoTracking.Any(x => x.EmployeeID == model.EmployeeID && x.LeaveID==model.LeaveID))
                {
                    result.Errors = new List<string> {"Leave is already Settled for mention employee" };
                    return result;
                }
                var newLeaveSettlement = new LeaveSettlement()
                {
                    EmployeeID = model.EmployeeID,
                    LeaveID = model.LeaveID,
                    OpeningBalance = model.OpeningBalance,
                    LeaveTaken = model.LeaveTaken,
                    RemainingLeave = model.RemainingLeave,
                    CarryToNext = model.CarryToNext,
                    Pay = model.Pay,
                    SettlingLeave = model.SettlingLeave,
                    FiscalYear=model.FiscalYear,
                    CreatedBy = model.CreatedBy,
                    CreatedTS = DateTime.UtcNow
                };
                _leaveSettlementRepository.Insert(newLeaveSettlement);
                await _leaveSettlementRepository.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public LeaveSettlement GetLeaveSettlementByID(int SettlementID)
        {
            return _leaveSettlementRepository.Table.FirstOrDefault(x => x.SettlementID == SettlementID);
        }

        public async Task<AccountResult> UpdateLeaveSettlementAsync(LeaveSettlementViewModel model)
        {
            try
            {
                var result = new AccountResult();
                var ExistedLeaveSettlement = GetLeaveSettlementByID(model.SettlementID);
                if (ExistedLeaveSettlement != null)
                {
                    ExistedLeaveSettlement.EmployeeID = model.EmployeeID;
                    ExistedLeaveSettlement.LeaveID = model.LeaveID;
                    ExistedLeaveSettlement.OpeningBalance = model.OpeningBalance;
                    ExistedLeaveSettlement.LeaveTaken = model.LeaveTaken;
                    ExistedLeaveSettlement.RemainingLeave = model.RemainingLeave;
                    ExistedLeaveSettlement.CarryToNext = model.CarryToNext;
                    ExistedLeaveSettlement.Pay = model.Pay;
                    ExistedLeaveSettlement.SettlingLeave = model.SettlingLeave;
                    ExistedLeaveSettlement.FiscalYear = model.FiscalYear;
                    ExistedLeaveSettlement.ModifiedBy = model.ModifiedBy;
                    ExistedLeaveSettlement.ModifiedTS = DateTime.Now;
                    _leaveSettlementRepository.Update(ExistedLeaveSettlement);
                    await _leaveSettlementRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "LeaveSettlement does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteLeaveSettlementAsync(int LeaveSettlementID)
        {
            var result = new AccountResult();
            var ExistedLeaveSettlement = GetLeaveSettlementByID(LeaveSettlementID);
            if (ExistedLeaveSettlement != null)
            {
                _leaveSettlementRepository.Delete(ExistedLeaveSettlement);
                await _leaveSettlementRepository.SaveChangesAsync();
            }
            else
            {
                result.Errors = new List<string> { "LeaveSettlement does not exist." };
                return result;
            }
            return result;
        }
        public async Task<LeaveSettlementViewModel> GetLeaveSettlementByIDAsync(int SettlementID)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                SettlementID,
	                                EmployeeID,
	                                LeaveID,
	                                OpeningBalance,
	                                LeaveTaken,
	                                RemainingLeave,
	                                CarryToNext,
	                                Pay,
	                                SettlingLeave,
                                    FiscalYear,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM LeaveSettlement 
                                    WHERE SettlementID=@SettlementID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@SettlementID", SettlementID);
            return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<LeaveSettlementViewModel>(strSQL.ToString(), _parameters);
        }
        public async Task<SettlementViewModel> GetEmployeeWiseLeaveSettlementAsync(SettlementIDsViewModel model)
        {
            try
            {
                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@EmployeeID", model.EmployeeID);
                _parameters.Add("@LeaveID", model.LeaveID);
                #endregion
                return await _dapperRepository.ExecuteStoredFirstOrDefaultAsync<SettlementViewModel>("Spa_LeaveSettlement", _parameters);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
