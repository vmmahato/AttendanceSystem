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
    public class LeaveApplicationService : ILeaveApplicationService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<LeaveApplication> _leaveApplicationRepository;
        private IGenericRepository<DynamicRoster> _dynamicRosterRepository;
        private IUnitOfWorkManager _unitOfWork;
        public LeaveApplicationService(IGenericRepository<LeaveApplication> leaveApplicationRepository,
                            IDapperRepository dapperRepository,
                            IGenericRepository<DynamicRoster> dynamicRosterRepository,
                            IUnitOfWorkManager unitOfWork)
        {
            _leaveApplicationRepository = leaveApplicationRepository;
            _dapperRepository = dapperRepository;
            _dynamicRosterRepository = dynamicRosterRepository;
            _unitOfWork = unitOfWork;
        }
        #region LeaveApplication
        public async Task<IPagedList<LeaveApplicationViewModel>> LeaveApplicationListAsync(LeaveApplicationSearchViewModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                L.LeaveApplicationID,
	                                L.EmployeeID,
									E.FirstName+' '+E.LastName EmployeeName,
	                                L.Code,
	                                L.DesignationID,
	                                L.DepartmentID,
	                                L.SectionID,
	                                L.LeaveID,
									LS.LeaveName LeaveType,
	                                L.LeaveDay,
									L.[Days],
	                                L.FromDate,
	                                L.ToDate,
	                                L.ApprovedBy,
									AE.FirstName+' '+AE.LastName ApprovedByName,
	                                L.RemainingLeave,
                                    L.ReplacementLeaveType,
                                    L.ReplacementEmployeeID,
									RE.FirstName+' '+RE.LastName ReplacementEmployeeName,
                                    L.IsOTLeave,
                                    L.IsReplacementLeave,
	                                L.Remarks,
	                                L.Status,
	                                L.CreatedTS,
	                                L.CreatedBy,
	                                L.ModifiedTS,
	                                L.ModifiedBy
	                                FROM LeaveApplication L LEFT OUTER JOIN Employee E 
                                    ON L.EmployeeID=E.EmployeeID 
									LEFT OUTER JOIN LeaveSetup LS ON LS.LeaveID=L.LeaveID
									LEFT OUTER JOIN Employee RE ON L.ReplacementEmployeeID=RE.EmployeeID
									LEFT OUTER JOIN Employee AE ON L.ApprovedBy=AE.EmployeeID
									WHERE 1=1 ");
                #region Filters

                if (!string.IsNullOrEmpty(model.LastName))
                {
                    strSQL.AppendFormat(@" AND E.LastName Like '%@LastName%' ");
                }
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    strSQL.AppendFormat(@" AND E.FirstName Like '%@FirstName%' ");
                }
                #endregion

                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FirstName", model.FirstName);
                _parameters.Add("@LastName", model.LastName);
                #endregion

                return await _dapperRepository.ExecuteQueryWithPagedListAsync<LeaveApplicationViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CountIndex");
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> InsertIntoLeaveApplicationAsync(LeaveApplicationViewModel model)
        {
            try
            {
                TimeSpan diffDate = model.ToDate.Subtract(model.FromDate);
                int days = diffDate.Days+1;
                var result = new AccountResult();
                var newLeaveApplication = new LeaveApplication()
                {
                    EmployeeID = model.EmployeeID,
                    Code = model.Code,
                    DesignationID = model.DesignationID,
                    DepartmentID = model.DepartmentID,
                    SectionID = model.SectionID,
                    LeaveID = model.LeaveID??0,
                    LeaveDay = model.LeaveDay,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    Days = days,
                    ApprovedBy = model.ApprovedBy,
                    RemainingLeave = model.RemainingLeave??0,
                    ReplacementEmployeeID=model.ReplacementEmployeeID,
                    IsOTLeave=model.IsOTLeave,
                    IsReplacementLeave=model.IsReplacementLeave,
                    Remarks = model.Remarks,
                    Status = "Pending",
                    ReplacementLeaveType=model.ReplacementLeaveType,
                    FiscalYear = model.FiscalYear,
                    CreatedBy = model.CreatedBy,
                    CreatedTS = DateTime.UtcNow
                };
                _leaveApplicationRepository.Insert(newLeaveApplication);

                //if replacement leave
                if (model.ReplacementLeaveType == "Replacement")
                {
                    for (var StartDate = model.FromDate.Date; StartDate <= model.ToDate.Date; StartDate = StartDate.AddDays(1))
                    {
                        int CurrentMonth = StartDate.Month;
                        int CurrentDay = StartDate.Day;

                        //Current Employee
                        var CurrentEmployeeDynamicRoster = await _dynamicRosterRepository.Table
                            .FirstOrDefaultAsync(x => x.FiscalYear == model.FiscalYear && x.EmployeeID == model.EmployeeID
                            && x.Month == CurrentMonth && x.Day == CurrentDay);
                        if (CurrentEmployeeDynamicRoster != null)
                        {
                            CurrentEmployeeDynamicRoster.ReplacementEmployeeID = model.ReplacementEmployeeID;
                            CurrentEmployeeDynamicRoster.Type = "OnLeave";

                            //Replacement Employee
                            var ReplacementEmployeeDynamicRoster = await _dynamicRosterRepository.Table
                               .FirstOrDefaultAsync(x => x.FiscalYear == model.FiscalYear && x.EmployeeID == model.ReplacementEmployeeID
                               && x.Month == CurrentMonth && x.Day == CurrentDay);
                            if (ReplacementEmployeeDynamicRoster != null)
                            {
                                ReplacementEmployeeDynamicRoster.ReplacementEmployeeID = model.ReplacementEmployeeID;
                                ReplacementEmployeeDynamicRoster.Type = "TakeOver";
                                ReplacementEmployeeDynamicRoster.ReplacementShiftID = CurrentEmployeeDynamicRoster.ShiftID;
                            }
                        }

                    }
                }
                using (var uow = _unitOfWork.NewUnitOfWork())
                {
                    try
                    {

                        await _leaveApplicationRepository.SaveChangesAsync();
                        if (model.ReplacementLeaveType == "Replacement")
                        {
                            await _dynamicRosterRepository.SaveChangesAsync();
                        } 
                        uow.Commit();
                    }
                    catch (Exception e)
                    {
                        uow.Rollback();
                        throw e;
                    }
                }
                  
               
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public LeaveApplication GetLeaveApplicationByID(int LeaveApplicationID)
        {
            return _leaveApplicationRepository.Table.FirstOrDefault(x =>x.LeaveApplicationID == LeaveApplicationID);
        }

        public async Task<AccountResult> UpdateLeaveApplicationAsync(LeaveApplicationViewModel model)
        {
            try
            {
                var result = new AccountResult();
                var ExistedLeaveApplication = GetLeaveApplicationByID(model.LeaveApplicationID);
                if (ExistedLeaveApplication != null)
                {
                    TimeSpan diffDate = model.ToDate.Subtract(model.FromDate);
                    int days = diffDate.Days+1;
                    ExistedLeaveApplication.EmployeeID = model.EmployeeID;
                    ExistedLeaveApplication.Code = model.Code;
                    ExistedLeaveApplication.DesignationID = model.DesignationID;
                    ExistedLeaveApplication.DepartmentID = model.DepartmentID;
                    ExistedLeaveApplication.SectionID = model.SectionID;
                    ExistedLeaveApplication.LeaveID = model.LeaveID??0;
                    ExistedLeaveApplication.LeaveDay = model.LeaveDay;
                    ExistedLeaveApplication.FromDate = model.FromDate;
                    ExistedLeaveApplication.ToDate = model.ToDate;
                    ExistedLeaveApplication.Days = days;
                    ExistedLeaveApplication.ApprovedBy = model.ApprovedBy;
                    ExistedLeaveApplication.RemainingLeave = model.RemainingLeave??0;
                    ExistedLeaveApplication.ReplacementEmployeeID = model.ReplacementEmployeeID;
                    ExistedLeaveApplication.IsOTLeave = model.IsOTLeave;
                    ExistedLeaveApplication.IsReplacementLeave = model.IsReplacementLeave;
                    ExistedLeaveApplication.Remarks = model.Remarks;
                    ExistedLeaveApplication.FiscalYear = model.FiscalYear;
                    ExistedLeaveApplication.ReplacementLeaveType = model.ReplacementLeaveType;
                    ExistedLeaveApplication.ModifiedBy = model.ModifiedBy;
                    ExistedLeaveApplication.ModifiedTS = DateTime.UtcNow;
                    _leaveApplicationRepository.Update(ExistedLeaveApplication);
                    await _leaveApplicationRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Leave Application does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteLeaveApplicationAsync(int LeaveApplicationID)
        {
            var result = new AccountResult();
            var ExistedLeaveApplication = GetLeaveApplicationByID(LeaveApplicationID);
            if (ExistedLeaveApplication != null)
            {
                   _leaveApplicationRepository.Delete(ExistedLeaveApplication);
             await _leaveApplicationRepository.SaveChangesAsync();
            }
            else
               {
                    result.Errors = new List<string> { "LeaveApplication does not exist." };
                    return result;
               }
            return result;
        }
        public async Task<LeaveApplicationViewModel> GetLeaveApplicationByIDAsync(int LeaveApplicationID)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                L.LeaveApplicationID,
	                                L.EmployeeID,
									E.FirstName+' '+E.LastName EmployeeName,
	                                L.Code,
	                                L.DesignationID,
	                                L.DepartmentID,
	                                L.SectionID,
	                                L.LeaveID,
									LS.LeaveName LeaveType,
	                                L.LeaveDay,
									L.[Days],
	                                L.FromDate,
	                                L.ToDate,
	                                L.ApprovedBy,
	                                L.RemainingLeave,
                                    L.ReplacementEmployeeID,
                                    L.IsOTLeave,
                                    L.IsReplacementLeave,
									RE.FirstName+' '+RE.LastName ReplacementEmployeeName,
	                                L.Remarks,
                                    L.ReplacementLeaveType,
	                                L.Status,
	                                L.CreatedTS,
	                                L.CreatedBy,
	                                L.ModifiedTS,
	                                L.ModifiedBy
	                                FROM LeaveApplication L LEFT OUTER JOIN Employee E 
                                    ON L.EmployeeID=E.EmployeeID 
									LEFT OUTER JOIN LeaveSetup LS ON LS.LeaveID=L.LeaveID
									LEFT OUTER JOIN Employee RE ON L.ReplacementEmployeeID=RE.EmployeeID
                                    WHERE L.LeaveApplicationID=@LeaveApplicationID");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@LeaveApplicationID", LeaveApplicationID);
                return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<LeaveApplicationViewModel>(strSQL.ToString(), _parameters);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion
        #region Leave Approval
        public async Task<IPagedList<LeaveApplicationViewModel>> GetPendingLeaveApplicationListAsync(LeaveApplicationSearchViewModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                L.LeaveApplicationID,
	                                L.EmployeeID,
									E.FirstName+' '+E.LastName EmployeeName,
	                                L.Code,
	                                L.DesignationID,
	                                L.DepartmentID,
	                                L.SectionID,
	                                L.LeaveID,
									LS.LeaveName LeaveType,
	                                L.LeaveDay,
									L.[Days],
	                                L.FromDate,
	                                L.ToDate,
	                                L.ApprovedBy,
	                                L.RemainingLeave,
                                    L.ReplacementEmployeeID,
                                    L.IsOTLeave,
                                    L.IsReplacementLeave,
									RE.FirstName+' '+RE.LastName ReplacementEmployeeName,
	                                L.Remarks,
                                    L.ReplacementLeaveType,
	                                L.Status,
	                                L.CreatedTS,
	                                L.CreatedBy,
	                                L.ModifiedTS,
	                                L.ModifiedBy
	                                FROM LeaveApplication L LEFT OUTER JOIN Employee E 
                                    ON L.EmployeeID=E.EmployeeID 
									LEFT OUTER JOIN LeaveSetup LS ON LS.LeaveID=L.LeaveID
									LEFT OUTER JOIN Employee RE ON L.ReplacementEmployeeID=RE.EmployeeID
                                    WHERE 1=1 AND L.Status='Pending' ");
                #region Filters

                if (!string.IsNullOrEmpty(model.LastName))
                {
                    strSQL.AppendFormat(@" AND E.LastName Like '%@LastName%' ");
                }
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    strSQL.AppendFormat(@" AND E.FirstName Like '%@FirstName%' ");
                }
                #endregion

                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FirstName", model.FirstName);
                _parameters.Add("@LastName", model.LastName);
                #endregion

                return await _dapperRepository.ExecuteQueryWithPagedListAsync<LeaveApplicationViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CountIndex");
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<AccountResult> UpdatePendingLeaveApplicationAsync(LeaveApplicationViewModel model)
        {
            try
            {
                var result = new AccountResult();
                var ExistedLeaveApplication = GetLeaveApplicationByID(model.LeaveApplicationID);
                if (ExistedLeaveApplication != null)
                {
                    ExistedLeaveApplication.Status = model.Status;
                    ExistedLeaveApplication.ModifiedBy = model.ModifiedBy;
                    ExistedLeaveApplication.ModifiedTS = DateTime.UtcNow;
                    _leaveApplicationRepository.Update(ExistedLeaveApplication);
                    await _leaveApplicationRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Leave Application does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<GenericResult<IEnumerable<GroupEmployeeWithShiftByEmployeeIDViewModel>>> GetReplacementEmployeeList(ReplacementEmployeeViewModel model)
        {
            var result = new GenericResult<IEnumerable<GroupEmployeeWithShiftByEmployeeIDViewModel>>();
            var AvailableEmployeeList = new List<EmployeeWithShiftIDViewModel>();
            int DaysDifference = (model.ToDate.AddDays(1).Date - model.FromDate.Date).Days;
            var EndDate = model.ToDate.Date;
            List<int> ShiftIDList = new List<int>();
            for (var StartDate = model.FromDate.Date; StartDate <= EndDate; StartDate=StartDate.AddDays(1)) 
            {
                int CurrentMonth = StartDate.Month;
                int CurrentDay = StartDate.Day;

                var strSQLCurrentEmployee = new StringBuilder();
                strSQLCurrentEmployee.AppendFormat(@"    SELECT		
                                                         WS.ShiftID
	                                                     FROM DynamicRoster DR
	                                                     LEFT JOIN WorkShift WS ON DR.ShiftID=WS.ShiftID
	                                                     WHERE DR.FiscalYear=@FiscalYear AND DR.EmployeeID=@EmployeeID 
	                                                     AND DR.Month=@Month
	                                                     AND DR.Day=@Day");
                #region Current Employee Parameters
                DynamicParameters _currentEmployeeParameters = new DynamicParameters();
                _currentEmployeeParameters.Add("@EmployeeID", model.EmployeeID);
                _currentEmployeeParameters.Add("@FiscalYear", model.FiscalYearID);
                _currentEmployeeParameters.Add("@Month", CurrentMonth);
                _currentEmployeeParameters.Add("@Day", CurrentDay);
                #endregion
                int CurrentEmployeeShiftID = await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<int>(strSQLCurrentEmployee.ToString(), _currentEmployeeParameters);

                if (CurrentEmployeeShiftID>0)
                {
                    ShiftIDList.Add(CurrentEmployeeShiftID);
                    var ShiftIDs = ShiftIDList.Distinct().ToList();
                    if(ShiftIDs.Count()>1)
                    {
                        result.Errors = new List<string> { "Multiple Shift Existed." };
                        return result;
                    }

                    #region Available Employee List

                    var strSQLEmployeeList = new StringBuilder();
                    strSQLEmployeeList.AppendFormat(@"SELECT E.FirstName+' '+E.LastName EmployeeName, E.EmployeeID,
                                                         DR.ShiftID EmployeeShiftID FROM Employee E
	                                                     LEFT JOIN DynamicRoster DR
	                                                     ON E.EmployeeID=DR.EmployeeID
	                                                     WHERE
	                                                       E.DesignationID=@DesignationID
	                                                       AND DR.FiscalYear=@FiscalYear 
	                                                       AND (DR.ShiftID<>@ShiftID OR DR.ShiftID IS NULL)
	                                                       AND DR.Month=@Month
	                                                       AND DR.Day=@Day");
                    #region Current Employee Parameters
                    DynamicParameters _employeeListParameters = new DynamicParameters();
                    _employeeListParameters.Add("@DesignationID", model.DesignationID);
                    _employeeListParameters.Add("@FiscalYear", model.FiscalYearID);
                    _employeeListParameters.Add("@ShiftID", CurrentEmployeeShiftID);
                    _employeeListParameters.Add("@Month", CurrentMonth);
                    _employeeListParameters.Add("@Day", CurrentDay);
                    #endregion
                    var EmployeeListWithShift = await _dapperRepository.ExecuteQueryAsync<EmployeeWithShiftIDViewModel>(strSQLEmployeeList.ToString(), _employeeListParameters);
                    if (EmployeeListWithShift.Count() > 0)
                    {
                        AvailableEmployeeList.AddRange(EmployeeListWithShift);
                    }

                    #endregion

                }
            }
            if (ShiftIDList.Count() > 0)
            {

                var ShiftIDs = ShiftIDList.Distinct().ToList();
                if (ShiftIDs.Count() > 1)
                {
                    result.Errors = new List<string> { "Multiple Shift Existed." };
                    return result;
                }
                else
                {
                    var GroupedShift = AvailableEmployeeList.GroupBy(x => x.EmployeeID)
                        .Select(x => new GroupEmployeeWithShiftByEmployeeIDViewModel()
                        {
                            EmployeeID = x.Key,
                            Count = x.Count(),
                            ShiftDetails = x.FirstOrDefault()
                        }).ToList();
                    GroupedShift = GroupedShift.Where(x => x.Count == DaysDifference).ToList();
                    result.Data = GroupedShift;
                    return result;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion
        public async Task<RemainingLeaveViewModel> GetEmplyeeOTAsync(LeaveAppicantViewModel model)
        {
            try
            {
                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@Fiscalyear", model.FiscalYear);
                _parameters.Add("@EmployeeID", model.EmployeeID);
                _parameters.Add("@StartDate", model.CurrentStartDate);
                #endregion
                return await _dapperRepository.ExecuteStoredFirstOrDefaultAsync<RemainingLeaveViewModel>("Spa_OTCalculation", _parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<RemainingLeaveViewModel> GetEmplyeeRemainingLeaveAsync(LeaveAppicantViewModel model)
        {
            try
            {
                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@Fiscalyear", model.FiscalYear);
                _parameters.Add("@EmployeeID", model.EmployeeID);
                _parameters.Add("@LeaveID", model.LeaveID);
                #endregion
                return await _dapperRepository.ExecuteStoredFirstOrDefaultAsync<RemainingLeaveViewModel>("Spa_RemainingLeaveCalculation", _parameters);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<LeaveApplicationViewModel> GetLeaveApplicationViewByIDAsync(int LeaveApplicationID)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
	                                L.LeaveApplicationID,
	                                L.EmployeeID,
									E.FirstName+' '+E.LastName EmployeeName,
	                                L.Code,
	                                L.DesignationID,
	                                L.DepartmentID,
	                                L.SectionID,
	                                L.LeaveID,
									LS.LeaveName LeaveType,
	                                L.LeaveDay,
									L.[Days],
	                                L.FromDate,
	                                L.ToDate,
	                                L.ApprovedBy,
	                                L.RemainingLeave,
                                    L.ReplacementEmployeeID,
                                    L.IsOTLeave,
                                    L.IsReplacementLeave,
									RE.FirstName+' '+RE.LastName ReplacementEmployeeName,
	                                L.Remarks,
                                    L.ReplacementLeaveType,
	                                L.Status,
	                                L.CreatedTS,
	                                L.CreatedBy,
	                                L.ModifiedTS,
	                                L.ModifiedBy,
									S.SectionName,
									D.DepartmentName,
									DE.DesignationName,
									Approve.FirstName+' '+Approve.LastName ApprovedByEmployee
	                                FROM LeaveApplication L 
									LEFT JOIN Employee E ON L.EmployeeID=E.EmployeeID 
									LEFT JOIN LeaveSetup LS ON LS.LeaveID=L.LeaveID
									LEFT JOIN Employee RE ON L.ReplacementEmployeeID=RE.EmployeeID
									LEFT JOIN Employee Approve ON L.ApprovedBy=Approve.EmployeeID
									LEFT JOIN Designation DE ON L.DesignationID=DE.DesignationID
									LEFT JOIN Department D ON L.DepartmentID=D.DepartmentID
									LEFT JOIN Section S ON L.SectionID=S.SectionID
                                    WHERE L.LeaveApplicationID=@LeaveApplicationID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@LeaveApplicationID", LeaveApplicationID);
            return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<LeaveApplicationViewModel>(strSQL.ToString(), _parameters);
        }
    }
}
