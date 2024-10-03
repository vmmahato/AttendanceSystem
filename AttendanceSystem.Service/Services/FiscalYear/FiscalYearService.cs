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
    public class FiscalYearService : IFiscalYearService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<FiscalYears> _fiscalYearRepository;
        public FiscalYearService(IGenericRepository<FiscalYears> fiscalYearRepository,
                            IDapperRepository dapperRepository)
        {
            _fiscalYearRepository = fiscalYearRepository;
            _dapperRepository = dapperRepository;
        }

        public async Task<IPagedList<FiscalYearViewModel>> FiscalYearListAsync(FiscalYearSearchViewModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                FiscalID,
	                                FiscalYear ,
	                                StartYear,
	                                EndYear,
                                    StartDateBS,
                                    EndDateBS,
	                                IsCurrentFiscalYear,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM FiscalYears WHERE 1=1 AND IsDelete<>1 ");
                #region Filters

                if (!string.IsNullOrEmpty(model.FiscalYear))
                {
                    strSQL.AppendFormat(@" AND FiscalYear=@FiscalYear");
                }
                #endregion

                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FiscalYear", model.FiscalYear);
                #endregion

                return await _dapperRepository.ExecuteQueryWithPagedListAsync<FiscalYearViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CreatedTS");
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> InsertIntoFiscalYearAsync(FiscalYearViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_fiscalYearRepository.TableNoTracking.Any(x => x.StartYear == model.StartYear && x.EndYear == model.EndYear && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "FiscalYear " + model.StartYear + " to " + model.EndYear + " is already taken" };
                    return result;
                }
                if (_fiscalYearRepository.TableNoTracking.Any(x => x.StartYear == model.StartYear && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "StartDate " + model.StartYear + " is already taken" };
                    return result;
                }
                if (_fiscalYearRepository.TableNoTracking.Any(x => x.EndYear == model.EndYear && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "EndDate " + model.EndYear + " is already taken" };
                    return result;
                }
                if (_fiscalYearRepository.TableNoTracking.Any(x => x.FiscalYear == model.FiscalYear && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "FiscalYear " + model.FiscalYear + " is already taken" };
                    return result;
                }
                if (_fiscalYearRepository.Table.Any(x => x.IsCurrentFiscalYear == true && x.IsDelete == false) && model.IsCurrentFiscalYear == true)
                {
                    result.Errors = new List<string> { "More than one fiscal year is not allowed." };
                    return result;
                }
                var newFiscalYear = new FiscalYears()
                {
                    FiscalYear = model.FiscalYear,
                    StartYear = model.StartYear,
                    EndYear = model.EndYear,
                    StartDateBS = model.StartDateBS,
                    EndDateBS = model.EndDateBS,
                    IsCurrentFiscalYear = model.IsCurrentFiscalYear,
                    CreatedBy = model.CreatedBy,
                    CreatedTS = DateTime.UtcNow
                };
                _fiscalYearRepository.Insert(newFiscalYear);
                await _fiscalYearRepository.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public FiscalYears GetFiscalYearByID(int FiscalID)
        {
            try
            {
                return _fiscalYearRepository.Table.FirstOrDefault(x => x.FiscalID == FiscalID);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> UpdateFiscalYearAsync(FiscalYearViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_fiscalYearRepository.TableNoTracking.Any(x => x.FiscalYear == model.FiscalYear && x.FiscalID!=model.FiscalID && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "FiscalYear " + model.FiscalYear + " is already taken" };
                    return result;
                }
               // var ExistedFiscal = _fiscalYearRepository.Table.Where(x => x.IsCurrentFiscalYear == true && x.FiscalID!=model.FiscalID && x.IsDelete == false).ToList();
                if (_fiscalYearRepository.Table.Any(x => x.IsCurrentFiscalYear == true && x.FiscalID != model.FiscalID && x.IsDelete == false) && model.IsCurrentFiscalYear==true)
                {
                    result.Errors = new List<string> { "More than one fiscal year is not allowed." };
                    return result;
                }
                var ExistedFiscalYear = GetFiscalYearByID(model.FiscalID);
                if (ExistedFiscalYear != null)
                {
                    ExistedFiscalYear.FiscalYear = model.FiscalYear;
                    ExistedFiscalYear.StartYear = model.StartYear;
                    ExistedFiscalYear.EndYear = model.EndYear;
                    ExistedFiscalYear.StartDateBS = model.StartDateBS;
                    ExistedFiscalYear.EndDateBS = model.EndDateBS;
                    ExistedFiscalYear.IsCurrentFiscalYear = model.IsCurrentFiscalYear;
                    ExistedFiscalYear.ModifiedBy = model.ModifiedBy;
                    ExistedFiscalYear.ModifiedTS = DateTime.UtcNow;
                    _fiscalYearRepository.Update(ExistedFiscalYear);
                    await _fiscalYearRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "FiscalYear does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteFiscalYearAsync(int FiscalID)
        {
            var result = new AccountResult();
            var ExistedFiscalYear = GetFiscalYearByID(FiscalID);
            if (ExistedFiscalYear != null)
            {
                   _fiscalYearRepository.Delete(ExistedFiscalYear);
             await _fiscalYearRepository.SaveChangesAsync();
            }
            else
               {
                    result.Errors = new List<string> { "FiscalYear does not exist." };
                    return result;
               }
            return result;
        }
        public async Task<FiscalYearModel> GetFiscalYearByIDAsync(int FiscalID)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                FiscalID,
	                                FiscalYear ,
	                                cast(Year(StartYear) as varchar(4))+'-'+
									cast(FORMAT(StartYear,'MM') as varchar(2))+'-'+
									cast(Right('00' + Convert(VarChar(2), Day(StartYear)), 2) as varchar(2)) StartYear,
	                                cast(Year(EndYear) as varchar(4))+'-'+
									cast(FORMAT(EndYear,'MM') as varchar(2))+'-'+
									cast(Right('00' + Convert(VarChar(2), Day(EndYear)), 2) as varchar(2)) EndYear,
                                    cast(Year(StartDateBS) as varchar(4))+'-'+
									cast(FORMAT(StartDateBS,'MM') as varchar(2))+'-'+
									cast(Right('00' + Convert(VarChar(2), Day(StartDateBS)), 2) as varchar(2)) StartDateBS,
	                                cast(Year(EndDateBS) as varchar(4))+'-'+
									cast(FORMAT(EndDateBS,'MM') as varchar(2))+'-'+
									cast(Right('00' + Convert(VarChar(2), Day(EndDateBS)), 2) as varchar(2)) EndDateBS,
	                                IsCurrentFiscalYear,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM FiscalYears
                                    WHERE FiscalID=@FiscalID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@FiscalID", FiscalID);
            return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<FiscalYearModel>(strSQL.ToString(), _parameters);
        }
        public async Task<IList<SelectItemIntViewModel>> DDLFiscalYearListAsync()
        {
            return await _fiscalYearRepository.TableNoTracking
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.FiscalID,
                              Value = x.FiscalYear
                          }).ToListAsync();
        }
    }
}
