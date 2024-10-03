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
    public class HolidayService : IHolidayService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<Holiday> _holidayRepository;
        public HolidayService(IGenericRepository<Holiday> holidayRepository,
                            IDapperRepository dapperRepository)
        {
            _holidayRepository = holidayRepository;
            _dapperRepository = dapperRepository;
        }

        public async Task<IPagedList<HolidayViewModel>> HolidayListAsync(HolidaySearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                H.HolidayID,
	                                H.HolidayName,
	                                H.HolidayType,
	                                H.ApplicableReligion,
	                                C.Description ApplicableGender,
	                                H.FromDate,
                                    H.ToDate,
	                                H.FiscalYear,
	                                H.Description,
	                                H.IsDepartmentWiseHoliday,
	                                H.DepartmentID DepartmentIDString,
	                                H.SectionID SectionIDString,
                                    H.WeekendDay,
                                    H.IsWeekendLeave,
	                                H.CreatedTS,
	                                H.CreatedBy,
	                                H.ModifiedTS,
	                                H.ModifiedBy
	                                FROM Holiday H LEFT OUTER JOIN CodeTable C 
                                    ON H.ApplicableGender=C.Value WHERE 1=1 ");
            #region Filters
            
            if (!string.IsNullOrEmpty(model.HolidayName))
            {
                strSQL.AppendFormat(@" AND HolidayName=@HolidayName");
            }
            #endregion

            #region Parameters
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@HolidayName", model.HolidayName);
            #endregion

            return await _dapperRepository.ExecuteQueryWithPagedListAsync<HolidayViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CreatedTS");
        }

        public async Task<AccountResult> InsertIntoHolidayAsync(HolidayViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_holidayRepository.TableNoTracking.Any(x => x.HolidayName == model.HolidayName))
                {
                    result.Errors = new List<string> { "Holiday " + model.HolidayName + " is already taken" };
                    return result;
                }
                var newHoliday = new Holiday()
                {
                    HolidayName = model.HolidayName,
                    HolidayType = model.HolidayType,
                    ApplicableReligion = model.ApplicableReligion,
                    ApplicableGender = model.ApplicableGender,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    FiscalYear = model.FiscalYear,
                    Description = model.Description,
                    IsDepartmentWiseHoliday = model.IsDepartmentWiseHoliday,
                    DepartmentID = string.Join(",", model.DepartmentID),
                    SectionID = string.Join(",", model.SectionID),
                    WeekendDay=model.WeekendDay,
                    IsWeekendLeave=model.IsWeekendLeave,
                    CreatedBy = model.CreatedBy,
                    CreatedTS = DateTime.UtcNow
                };
                _holidayRepository.Insert(newHoliday);
                await _holidayRepository.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Holiday GetHolidayByID(int HolidayID)
        {
            return _holidayRepository.Table.FirstOrDefault(x =>x.HolidayID == HolidayID);
        }

        public async Task<AccountResult> UpdateHolidayAsync(HolidayViewModel model)
        {
            try
            {
                var result = new AccountResult();
                var ExistedHoliday = GetHolidayByID(model.HolidayID);
                if (ExistedHoliday != null)
                {
                    ExistedHoliday.HolidayName = model.HolidayName;
                    ExistedHoliday.HolidayType = model.HolidayType;
                    ExistedHoliday.ApplicableReligion = model.ApplicableReligion;
                    ExistedHoliday.ApplicableGender = model.ApplicableGender;
                    ExistedHoliday.FromDate = model.FromDate;
                    ExistedHoliday.ToDate = model.ToDate;
                    ExistedHoliday.FiscalYear = model.FiscalYear;
                    ExistedHoliday.Description = model.Description;
                    ExistedHoliday.IsDepartmentWiseHoliday = model.IsDepartmentWiseHoliday;
                    ExistedHoliday.DepartmentID = model.DepartmentID != null ? String.Join(",", model.DepartmentID) : "";
                    ExistedHoliday.SectionID = model.SectionID != null ? String.Join(",", model.SectionID) : "";
                    ExistedHoliday.WeekendDay = model.WeekendDay;
                    ExistedHoliday.IsWeekendLeave = model.IsWeekendLeave;
                    ExistedHoliday.ModifiedBy = model.ModifiedBy;
                    ExistedHoliday.ModifiedTS = DateTime.UtcNow;
                    _holidayRepository.Update(ExistedHoliday);
                    await _holidayRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Holiday does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteHolidayAsync(int HolidayID)
        {
            var result = new AccountResult();
            var ExistedHoliday = GetHolidayByID(HolidayID);
            if (ExistedHoliday != null)
            {
                   _holidayRepository.Delete(ExistedHoliday);
             await _holidayRepository.SaveChangesAsync();
            }
            else
               {
                    result.Errors = new List<string> { "Holiday does not exist." };
                    return result;
               }
            return result;
        }
        public async Task<HolidayModel> GetHolidayByIDAsync(int HolidayID)
        {
            try
            {
                char[] delimiters = new char[] { ',' };
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                HolidayID,
	                                HolidayName,
	                                HolidayType,
	                                ApplicableReligion,
	                                ApplicableGender,
									cast(Year(ToDate) as varchar(4))+'-'+
									cast(FORMAT(ToDate,'MM') as varchar(2))+'-'+
									cast(Right('00' + Convert(VarChar(2), Day(ToDate)), 2) as varchar(2)) ToDate,
	                                cast(Year(FromDate) as varchar(4))+'-'+
									cast(FORMAT(FromDate,'MM') as varchar(2))+'-'+
									cast(Right('00' + Convert(VarChar(2), Day(FromDate)), 2) as varchar(2)) FromDate,
	                                FiscalYear,
	                                Description,
	                                IsDepartmentWiseHoliday,
	                                DepartmentID DepartmentIDString,
	                                SectionID SectionIDString,
                                    WeekendDay,
                                    IsWeekendLeave,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM Holiday
                                    WHERE HolidayID=@HolidayID");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@HolidayID", HolidayID);
                var result = await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<HolidayModel>(strSQL.ToString(), _parameters);
                result.DepartmentID = Array.ConvertAll(result.DepartmentIDString.Split(delimiters), s => int.Parse(s));
                result.SectionID = Array.ConvertAll(result.SectionIDString.Split(delimiters), s => int.Parse(s));
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
