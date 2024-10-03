using AttendanceSystem.DapperServices;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.Helpers;
using AttendanceSystem.PageExtension;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Service
{
    public class OfficeVisitService : IOfficeVisitService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<OfficeVisit> _officeVisitRepository;
        public OfficeVisitService(IDapperRepository dapperRepository, IGenericRepository<OfficeVisit> officeVisitRepository)
        {
            _dapperRepository = dapperRepository;
            _officeVisitRepository = officeVisitRepository;
        }
        public async Task<AccountResult> DeleteOfficeVisitAsync(int OfficeVisitID)
        {
            var result = new AccountResult();
            var ExistedOfficeVisit = GetOfficeVisitByID(OfficeVisitID);
            if (ExistedOfficeVisit != null)
            {
                _officeVisitRepository.Delete(ExistedOfficeVisit);
                await _officeVisitRepository.SaveChangesAsync();
            }
            else
            {
                result.Errors = new List<string> { "OfficeVisit does not exist." };
                return result;
            }
            return result;
        }

        public async Task<IPagedList<OfficeVisitModel>> OfficeVisitListAsync(OfficeVisitSearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                     ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex
                                     ,O.[OfficeVisitID]
                                     ,O.[EmployeeID]
                                     ,O.VisitorName
									 ,E.FirstName+' '+E.LastName EmployeeName
									 ,cast(Year(O.[FromDate]) as varchar(4))+'-'+
									  cast(FORMAT(O.[FromDate],'MM') as varchar(2))+'-'+
									  cast(Right('00' + Convert(VarChar(2), Day(O.[FromDate])), 2) as varchar(2))
									  [FromDate]
                                     ,O.FromTime
                                     ,cast(Year(O.ToDate) as varchar(4))+'-'+
									  cast(FORMAT(O.ToDate,'MM') as varchar(2))+'-'+
									  cast(Right('00' + Convert(VarChar(2), Day(O.ToDate)), 2) as varchar(2))
									 ToDate
                                     ,O.ToTime
                                     ,O.[Remarks]
                                     ,O.Status
                                     ,O.[CreatedTS]
                                     ,O.[CreatedBy]
                                     ,O.[ModifiedTS]
                                     ,O.[ModifiedBy]
	                                 FROM OfficeVisit O LEFT OUTER JOIN Employee E ON 
									 O.EmployeeID=E.EmployeeID ");
            #region Filter
            DynamicParameters _parameters = new DynamicParameters();
            #endregion
            return await _dapperRepository.ExecuteQueryWithPagedListAsync<OfficeVisitModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CountIndex");
        }

        public OfficeVisit GetOfficeVisitByID(int OfficeVisitID)
        {
            return _officeVisitRepository.Table.FirstOrDefault(x => x.OfficeVisitID == OfficeVisitID);
        }

        public async Task<OfficeVisitModel> GetOfficeVisitByIDAsync(int OfficeVisitID)
        {
            try
            {

                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                      ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex
                                     ,O.[OfficeVisitID]
                                     ,O.[EmployeeID]
									 ,E.FirstName+' '+E.LastName EmployeeName
                                     ,O.VisitorName
									 ,cast(Year(O.[FromDate]) as varchar(4))+'-'+
									  cast(FORMAT(O.[FromDate],'MM') as varchar(2))+'-'+
									  cast(Right('00' + Convert(VarChar(2), Day(O.[FromDate])), 2) as varchar(2))
									  [FromDate]
                                     ,O.FromTime
                                     ,cast(Year(O.ToDate) as varchar(4))+'-'+
									  cast(FORMAT(O.ToDate,'MM') as varchar(2))+'-'+
									  cast(Right('00' + Convert(VarChar(2), Day(O.ToDate)), 2) as varchar(2))
									  ToDate
                                     ,O.ToTime
                                     ,O.[Remarks]
                                     ,O.Status
                                     ,O.[CreatedTS]
                                     ,O.[CreatedBy]
                                     ,O.[ModifiedTS]
                                     ,O.[ModifiedBy]
                                     FROM [dbo].[OfficeVisit] O 
                                     LEFT OUTER JOIN Employee E ON O.EmployeeID=E.EmployeeID
                                     WHERE OfficeVisitID=@OfficeVisitID");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@OfficeVisitID", OfficeVisitID);
                return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<OfficeVisitModel>(strSQL.ToString(), _parameters);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> InsertIntoOfficeVisitAsync(OfficeVisitViewModel model)
        {
            try
            {
                var result = new AccountResult();
                var OfficeVisit = new OfficeVisit()
                {
                    EmployeeID = model.EmployeeID,
                    VisitorName=model.VisitorName,
                    FromDate = model.FromDate,
                    FromTime = SharedServices.ConvertStringToTimeSpan(model.FromTime),
                    ToDate = model.ToDate,
                    ToTime = SharedServices.ConvertStringToTimeSpan(model.ToTime),
                    Remarks = model.Remarks,
                    Status="Pending",
                    CreatedBy = model.CreatedBy,
                    CreatedTS = DateTime.UtcNow
                };
                _officeVisitRepository.Insert(OfficeVisit);
                await _officeVisitRepository.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> UpdateOfficeVisitAsync(OfficeVisitViewModel model)
        {
            var result = new AccountResult();
            var OfficeVisit = GetOfficeVisitByID(model.OfficeVisitID);
            if (OfficeVisit != null)
            {
                OfficeVisit.EmployeeID = model.EmployeeID;
                OfficeVisit.VisitorName = model.VisitorName;
                OfficeVisit.FromDate = model.FromDate;
                OfficeVisit.FromTime = SharedServices.ConvertStringToTimeSpan(model.FromTime);
                OfficeVisit.ToDate = model.ToDate;
                OfficeVisit.ToTime = SharedServices.ConvertStringToTimeSpan(model.ToTime);
                OfficeVisit.Remarks = model.Remarks;
                OfficeVisit.Status = model.Status;
                OfficeVisit.ModifiedBy = model.ModifiedBy;
                OfficeVisit.ModifiedTS = DateTime.UtcNow;
                await _officeVisitRepository.SaveChangesAsync();
            }
            else
            {
                result.Errors = new List<string> { "OfficeVisit does not exist." };
                return result;
            }
            return result;
        }
        #region OfficeVisit Approval
        public async Task<IPagedList<OfficeVisitModel>> GetPendingOfficeVisitListAsync(OfficeVisitSearchViewModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                     ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex
                                     ,O.[OfficeVisitID]
                                     ,O.[EmployeeID]
                                     ,O.VisitorName
									 ,E.FirstName+' '+E.LastName EmployeeName
									 ,cast(Year(O.[FromDate]) as varchar(4))+'-'+
									  cast(FORMAT(O.[FromDate],'MM') as varchar(2))+'-'+
									  cast(Right('00' + Convert(VarChar(2), Day(O.[FromDate])), 2) as varchar(2))
									  [FromDate]
                                     ,O.FromTime
                                     ,cast(Year(O.ToDate) as varchar(4))+'-'+
									  cast(FORMAT(O.ToDate,'MM') as varchar(2))+'-'+
									  cast(Right('00' + Convert(VarChar(2), Day(O.ToDate)), 2) as varchar(2))
									 ToDate
                                     ,O.ToTime
                                     ,O.[Remarks]
                                     ,O.Status
                                     ,O.[CreatedTS]
                                     ,O.[CreatedBy]
                                     ,O.[ModifiedTS]
                                     ,O.[ModifiedBy]
	                                 FROM OfficeVisit O LEFT OUTER JOIN Employee E ON 
									 O.EmployeeID=E.EmployeeID
                                    WHERE 1=1 AND O.Status='Pending' ");
                #region Filters

                if (!string.IsNullOrEmpty(model.LastName))
                {
                    strSQL.AppendFormat(@" AND E.LastName Like '%@LastName%' ");
                }
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    strSQL.AppendFormat(@" AND E.FirstName Like '%@FirstName%' ");
                }
                if (!string.IsNullOrEmpty(model.VisitorName))
                {
                    strSQL.AppendFormat(@" AND O.VisitorName Like '%@VisitorName%' ");
                }
                #endregion

                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FirstName", model.FirstName);
                _parameters.Add("@LastName", model.LastName);
                _parameters.Add("@VisitorName", model.VisitorName);
                #endregion

                return await _dapperRepository.ExecuteQueryWithPagedListAsync<OfficeVisitModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CountIndex");
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<AccountResult> UpdatePendingOfficeVisitAsync(OfficeVisitModel model)
        {
            try
            {
                var result = new AccountResult();
                var ExistedOfficeVisit = GetOfficeVisitByID(model.OfficeVisitID);
                if (ExistedOfficeVisit!= null)
                {
                    ExistedOfficeVisit.Status = model.Status;
                    ExistedOfficeVisit.ModifiedBy = model.ModifiedBy;
                    ExistedOfficeVisit.ModifiedTS = DateTime.UtcNow;
                    _officeVisitRepository.Update(ExistedOfficeVisit);
                    await _officeVisitRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Application does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion
    }
}
