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
    public class KajService : IKajService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<Kaj> _kajRepository;
        public KajService(IDapperRepository dapperRepository, IGenericRepository<Kaj> kajRepository)
        {
            _dapperRepository = dapperRepository;
            _kajRepository = kajRepository;
        }
        public async Task<AccountResult> DeleteKajAsync(int KajID)
        {
            var result = new AccountResult();
            var ExistedKaj = GetKajByID(KajID);
            if (ExistedKaj != null)
            {
                _kajRepository.Delete(ExistedKaj);
                await _kajRepository.SaveChangesAsync();
            }
            else
            {
                result.Errors = new List<string> { "Kaj does not exist." };
                return result;
            }
            return result;
        }

        public async Task<IPagedList<KajModel>> KajListAsync(KajSearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                 ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex
                                 ,K.[KajID]
                                 ,K.[EmployeeID]
								 ,E.FirstName+' '+E.LastName EmployeeName
                                 ,cast(Year(K.[FromDate]) as varchar(4))+'-'+
							      cast(FORMAT(K.[FromDate],'MM') as varchar(2))+'-'+
							      cast(Right('00' + Convert(VarChar(2), Day(K.[FromDate])), 2) as varchar(2))
							      [FromDate]
                                  ,K.FromTime
                                  ,cast(Year(K.ToDate) as varchar(4))+'-'+
								  cast(FORMAT(K.ToDate,'MM') as varchar(2))+'-'+
								  cast(Right('00' + Convert(VarChar(2), Day(K.ToDate)), 2) as varchar(2))
								  ToDate
                                 ,K.ToTime
                                 ,K.[Remarks]
                                 ,K.Status
                                 ,K.[CreatedTS]
                                 ,K.[CreatedBy]
                                 ,K.[ModifiedTS]
                                 ,K.[ModifiedBy]
                             FROM [dbo].[Kaj] K LEFT OUTER JOIN Employee E ON K.EmployeeID=E.EmployeeID ");
            #region Filter
            DynamicParameters _parameters = new DynamicParameters();
            #endregion
            return await _dapperRepository.ExecuteQueryWithPagedListAsync<KajModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CreatedTS");
        }

        public Kaj GetKajByID(int KajID)
        {
            return _kajRepository.Table.FirstOrDefault(x => x.KajID == KajID);
        }

        public async Task<KajModel> GetKajByIDAsync(int KajID)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT K.[KajID]
                                 ,K.[EmployeeID]
								 ,E.FirstName+' '+E.LastName EmployeeName
                                 ,cast(Year(K.[FromDate]) as varchar(4))+'-'+
							      cast(FORMAT(K.[FromDate],'MM') as varchar(2))+'-'+
							      cast(Right('00' + Convert(VarChar(2), Day(K.[FromDate])), 2) as varchar(2))
							      [FromDate]
                                  ,K.FromTime
                                  ,cast(Year(K.ToDate) as varchar(4))+'-'+
								  cast(FORMAT(K.ToDate,'MM') as varchar(2))+'-'+
								  cast(Right('00' + Convert(VarChar(2), Day(K.ToDate)), 2) as varchar(2))
								  ToDate
                                 ,K.ToTime
                                 ,K.[Remarks]
                                 ,K.Status
                                 ,K.[CreatedTS]
                                 ,K.[CreatedBy]
                                 ,K.[ModifiedTS]
                                 ,K.[ModifiedBy]
                                 FROM [dbo].[Kaj] K 
                                 LEFT OUTER JOIN Employee E ON K.EmployeeID=E.EmployeeID
                                 WHERE KajID=@KajID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@KajID", KajID);
            return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<KajModel>(strSQL.ToString(), _parameters);
        }

        public async Task<AccountResult> InsertIntoKajAsync(KajViewModel model)
        {
            var result = new AccountResult();
            var Kaj = new Kaj()
            {
                EmployeeID=model.EmployeeID,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                FromTime = SharedServices.ConvertStringToTimeSpan(model.FromTime),
                ToTime = SharedServices.ConvertStringToTimeSpan(model.ToTime),
                Remarks = model.Remarks,
                Status = "Pending",
                CreatedBy = model.CreatedBy,
                CreatedTS = DateTime.UtcNow
            };
            _kajRepository.Insert(Kaj);
            await _kajRepository.SaveChangesAsync();
            return result;
        }

        public async Task<AccountResult> UpdateKajAsync(KajViewModel model)
        {
            var result = new AccountResult();
            var Kaj = GetKajByID(model.KajID);
            if (Kaj != null)
            {
                Kaj.EmployeeID = model.EmployeeID;
                Kaj.FromDate = model.FromDate;
                Kaj.ToDate = model.ToDate;
                Kaj.FromTime = SharedServices.ConvertStringToTimeSpan(model.FromTime);
                Kaj.ToTime = SharedServices.ConvertStringToTimeSpan(model.ToTime);
                Kaj.Status = model.Status;
                Kaj.Remarks = model.Remarks;
                Kaj.ModifiedBy = model.ModifiedBy;
                Kaj.ModifiedTS = DateTime.UtcNow;
                await _kajRepository.SaveChangesAsync();
            }
            else
            {
                result.Errors = new List<string> { "Kaj does not exist." };
                return result;
            }
            return result;
        }
        #region Kaj Approval
        public async Task<IPagedList<KajModel>> GetPendingKajListAsync(KajSearchViewModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                 ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex
                                 ,K.[KajID]
                                 ,K.[EmployeeID]
								 ,E.FirstName+' '+E.LastName EmployeeName
                                 ,cast(Year(K.[FromDate]) as varchar(4))+'-'+
							      cast(FORMAT(K.[FromDate],'MM') as varchar(2))+'-'+
							      cast(Right('00' + Convert(VarChar(2), Day(K.[FromDate])), 2) as varchar(2))
							      [FromDate]
                                  ,K.FromTime
                                  ,cast(Year(K.ToDate) as varchar(4))+'-'+
								  cast(FORMAT(K.ToDate,'MM') as varchar(2))+'-'+
								  cast(Right('00' + Convert(VarChar(2), Day(K.ToDate)), 2) as varchar(2))
								  ToDate
                                 ,K.ToTime
                                 ,K.[Remarks]
                                 ,K.Status
                                 ,K.[CreatedTS]
                                 ,K.[CreatedBy]
                                 ,K.[ModifiedTS]
                                 ,K.[ModifiedBy]
                                 FROM [dbo].[Kaj] K LEFT OUTER JOIN Employee E ON K.EmployeeID=E.EmployeeID
                                 WHERE 1=1 AND K.Status='Pending' ");
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

                return await _dapperRepository.ExecuteQueryWithPagedListAsync<KajModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CountIndex");
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<AccountResult> UpdatePendingKajAsync(KajViewModel model)
        {
            try
            {
                var result = new AccountResult();
                var ExistedKaj = GetKajByID(model.KajID);
                if (ExistedKaj != null)
                {
                    ExistedKaj.Status = model.Status;
                    ExistedKaj.ModifiedBy = model.ModifiedBy;
                    ExistedKaj.ModifiedTS = DateTime.UtcNow;
                    _kajRepository.Update(ExistedKaj);
                    await _kajRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Kaj does not exist." };
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
