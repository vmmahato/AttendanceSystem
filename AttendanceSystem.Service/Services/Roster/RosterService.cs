using AttendanceSystem.DapperServices;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public class RosterService : IRosterService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<FixedRoster> _fixedRosterRepository;
        private IUnitOfWorkManager _unitOfWork;
        private IGenericRepository<WorkShift> _workShiftRepository;
        private IGenericRepository<WeeklyRoster> _weeklyRosterRepository;
        private IGenericRepository<DynamicRoster> _dynamicRosterRepository;
        private IGenericRepository<FiscalYears> _fiscalYearsRepository;
        private IGenericRepository<Employee> _employeeRepository;
        private IGenericRepository<WeekDays> _weekdaysRepository;
        public RosterService(IDapperRepository dapperRepository,
                            IGenericRepository<FixedRoster> fixedRosterRepository,
                             IUnitOfWorkManager unitOfWork,
                             IGenericRepository<WorkShift> workShiftRepository,
                             IGenericRepository<WeeklyRoster> weeklyRosterRepository,
                             IGenericRepository<DynamicRoster> dynamicRosterRepository,
                             IGenericRepository<FiscalYears> fiscalYearsRepository,
                             IGenericRepository<Employee> employeeRepository,
                             IGenericRepository<WeekDays> weekdaysRepository)
        {
            _dapperRepository = dapperRepository;
            _fixedRosterRepository = fixedRosterRepository;
            _unitOfWork = unitOfWork;
            _workShiftRepository = workShiftRepository;
            _weeklyRosterRepository = weeklyRosterRepository;
            _dynamicRosterRepository = dynamicRosterRepository;
            _fiscalYearsRepository = fiscalYearsRepository;
            _employeeRepository = employeeRepository;
            _weekdaysRepository = weekdaysRepository;
        }

        #region Fixed Roster

        public async Task<IEnumerable<GroupedFixedRosterViewModel>> GetFixedRosterListAsync(FiscalWithEmployeeListViewModel model)
        {
            var result = new List<GroupedFixedRosterViewModel>();
            var strSQL = new StringBuilder();
            if (model.EmployeeList.Count() == 0) return null;
            string EmployeeIDList = string.Join(",", model.EmployeeList.ToList());
            strSQL.AppendFormat(@"SELECT E.FirstName+' '+E.LastName AS Name,
                                	   E.EmployeeID,
                                	   FR.ShiftID,REPLACE(CONVERT(VARCHAR, FR.ApplicableDate,111), '/','-') AS ApplicableDateString 
                                FROM Employee E
                                LEFT JOIN  FixedRoster FR ON E.EmployeeID=FR.EmployeeID AND FR.FiscalYear=@FiscalYear
                                WHERE E.EmployeeID IN (" + EmployeeIDList+") ");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@FiscalYear", model.FiscalYearID);
            var list= await _dapperRepository.ExecuteQueryAsync<RosterViewModel>(strSQL.ToString(), _parameters);
            if (list.Count() > 0)
            {
                var shiftList = await GetShiftListAsync();
                foreach (var data in list)
                {
                    data.ShiftList = shiftList;
                }
                result= list.GroupBy(x => new { x.EmployeeID, x.Name,x.ApplicableDateString })
                .Select(x => new GroupedFixedRosterViewModel()
                {
                    EmployeeID = x.Key.EmployeeID,
                    Name = x.Key.Name,
                    ApplicableDateString=x.Key.ApplicableDateString,
                    ShiftList = x.ToList()
                }).ToList();
            }

            return result;
        }
        public AccountResult UpdateFixedRosterAsync(UpdateGroupedFixedRosterViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (model.EmployeeList.Count() == 0) return result;

               // model.EmployeeList = model.EmployeeList.Where(x =>Convert.ToDateTime(x.ApplicableDateString) >= DateTime.UtcNow.AddDays(1)).ToList();
                foreach (var employee in model.EmployeeList)
                {
                    var applicabledate = Convert.ToDateTime(employee.ApplicableDateString);
                    if (applicabledate <= DateTime.UtcNow) continue;

                    foreach (var shift in employee.ShiftList)
                    {
                        #region Parameters
                        if (string.IsNullOrEmpty(employee.ApplicableDateString)) continue;
                        DynamicParameters _parameters = new DynamicParameters();
                        _parameters.Add("@FiscalYear",model.FiscalYear);
                        _parameters.Add("@EmployeeId", employee.EmployeeID);
                        _parameters.Add("@ShiftID", shift.ShiftID);
                        _parameters.Add("@applicableDate",DateTime.Parse(employee.ApplicableDateString));
                        _parameters.Add("@UserID", model.CreatedBy);
                        #endregion
                        var response = _dapperRepository.ExecuteStoredFirstOrDefault<MessageViewModel>("Spa_InsertFixedRosterInToDynamicRoster", _parameters);
                        if (response.Code != "SUCCESS")
                        {
                            result.Errors = new List<string> { "Error Occured: " + response.Message };
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion


        #region Weekly Roster

        public async Task<WeekWithEmployeeShiftList> GetWeeklyRosterListAsync(FiscalWithEmployeeListViewModel model)
        {
            var result = new WeekWithEmployeeShiftList();
            result.EmployeeShiftList = new List<GroupedWeeklyRosterViewModel>();
            result.WeekDays = new List<string>();
            var WeeklyRosterList = new List<WeeklyRosterViewModel>();
            var strSQL = new StringBuilder();
            if (model.EmployeeList.Count() == 0) return null;
            string EmployeeIDList = string.Join(",", model.EmployeeList.ToList());
            strSQL.AppendFormat(@"SELECT E.FirstName+' '+E.LastName AS Name
                                 ,E.EmployeeID
                                 ,WR.[DayID]
                                 ,WR.[DayName]
                                 ,WR.[ShiftID]
                                 ,REPLACE(CONVERT(VARCHAR, WR.ApplicableDate,111), '/','-') AS ApplicableDateString
                            	 ,WR.FiscalYear
                             FROM Employee E 
                             LEFT JOIN [dbo].[WeeklyRoster] WR ON E.EmployeeID=WR.EmployeeID AND WR.FiscalYear=@FiscalYear
                             WHERE E.EmployeeID IN (" + EmployeeIDList + ")");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@FiscalYear", model.FiscalYearID);
            var EmployeeShiftList = await _dapperRepository.ExecuteQueryAsync<WeeklyRosterViewModel>(strSQL.ToString(), _parameters);
            var weeks = await GetDaysInWeek();

            result.WeekDays = weeks.OrderBy(x => x.DayID).Select(x => x.Day).ToList();
            if (EmployeeShiftList.Count() > 0)
            {
                var GroupedEmployeeList = EmployeeShiftList.GroupBy(x => new { x.EmployeeID, x.Name, x.ApplicableDateString, x.FiscalYear })
                .Select(x => new GroupedWeeklyRosterViewModel()
                {
                    EmployeeID = x.Key.EmployeeID,
                    Name = x.Key.Name,
                    ApplicableDateString = x.Key.ApplicableDateString,
                    FiscalYear = x.Key.FiscalYear,
                    EmployeeShiftList = x.ToList()
                }).ToList();
                if (GroupedEmployeeList.Count() > 0)
                {
                    var shiftList = await GetShiftListAsync();
                    foreach (var employee in GroupedEmployeeList)
                    {
                        foreach (var day in weeks)
                        {
                            var CurrentShift = employee.EmployeeShiftList
                                .FirstOrDefault(x => x.DayID == day.DayID && x.DayName == day.Day);
                            int CurrentShiftID = 0;
                            if (CurrentShift != null)
                            {
                                CurrentShiftID = CurrentShift.ShiftID;
                            }
                            WeeklyRosterList.Add(new WeeklyRosterViewModel()
                            {
                                ApplicableDateString =employee.ApplicableDateString,
                                DayID = day.DayID,
                                DayName = day.Day,
                                EmployeeID = employee.EmployeeID,
                                ShiftID = CurrentShiftID,
                                FiscalYear = employee.FiscalYear,
                                ShiftList = shiftList,
                                Name=employee.Name
                            });
                        }
                    }
                }
                result.EmployeeShiftList = WeeklyRosterList.GroupBy(x => new { x.EmployeeID, x.Name, x.ApplicableDateString, x.FiscalYear })
                  .Select(x => new GroupedWeeklyRosterViewModel()
                  {
                      EmployeeID = x.Key.EmployeeID,
                      Name = x.Key.Name,
                      ApplicableDateString = x.Key.ApplicableDateString,
                      FiscalYear = x.Key.FiscalYear,
                      EmployeeShiftList = x.OrderBy(x => x.DayID).ToList()
                  }).ToList();
            }
                return result;

        }

        #endregion
        /// <summary>
        /// Get All Shift 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectItemIntViewModel>> GetShiftListAsync()
        {
            var result = new List<SelectItemIntViewModel>();
            var list= await _workShiftRepository.TableNoTracking
                 .Select(x => new SelectItemIntViewModel()
                 {
                     ID = x.ShiftID,
                     Value = x.ShiftName
                 }).ToListAsync();
            if (list.Count() > 0)
            {
                //result.Add(new SelectItemIntViewModel()
                //{
                //    ID = 0,
                //    Value = ""
                //});
                result.AddRange(list);
            }
            return result;
        }

        /// <summary>
        /// Get Days in Week
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<WeekDaysViewModel>> GetDaysInWeek()
        {
            return await _weekdaysRepository.TableNoTracking
                 .Select(x => new WeekDaysViewModel()
                 {
                     DayID = x.DayID,
                     Day = x.Day
                 }).OrderBy(x => x.DayID).ToListAsync();
        }

        public AccountResult UpdateWeeklyRosterAsync(UpdateGroupedWeeklyRosterViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (model.EmployeeList.Count() == 0) return result;
                //model.EmployeeList = model.EmployeeList.Where(x =>Convert.ToDateTime(x.ApplicableDateString) > DateTime.UtcNow.AddDays(1)).ToList();
                foreach (var employee in model.EmployeeList)
                {
                    var applicabledate = Convert.ToDateTime(employee.ApplicableDateString);
                    if (applicabledate <= DateTime.UtcNow) continue;
                    foreach (var shift in employee.EmployeeShiftList)
                    {
                        var data = new WeeklyRoster()
                        {
                            ShiftID=shift.ShiftID,
                            ApplicableDate=Convert.ToDateTime(employee.ApplicableDateString),
                            FiscalYear=model.FiscalYear,
                            CreatedBy=model.CreatedBy,
                            CreatedTS=DateTime.UtcNow,
                            DayID=shift.DayID,
                            DayName=shift.DayName,
                            EmployeeID=employee.EmployeeID
                        };
                        _weeklyRosterRepository.Insert(data);
                        if (string.IsNullOrEmpty(employee.ApplicableDateString)) continue;
                        //var strSQL = new StringBuilder();
                        //strSQL.AppendFormat(@"
                        //                    DELETE FROM WeeklyRoster WHERE FiscalYear=@FiscalYear AND EmployeeID =@EmployeeID

                        //                      DELETE FROM [dbo].DynamicRoster
                        //                    WHERE FiscalYear=@FiscalYear AND EmployeeID =@EmployeeID
                        //                    AND (([Month]>Month(@applicableDate) and DayName=@DayName) OR 
                        //                    ([Month]=MONTH(@applicableDate) AND [Day]>=DAY(@applicableDate) and  [DayName]=@DayName))");
                        DynamicParameters _parameters1 = new DynamicParameters();
                        _parameters1.Add("@applicableDate", DateTime.Parse(employee.ApplicableDateString));
                        _parameters1.Add("@FiscalYear", model.FiscalYear);
                        _parameters1.Add("@EmployeeID", employee.EmployeeID);
                        _parameters1.Add("@DayName", shift.DayName);
                        _dapperRepository.ExecuteStoredProc("DeleteWeeklyRoster", _parameters1);

                        #region Parameters
                        DynamicParameters _parameters = new DynamicParameters();
                        _parameters.Add("@FiscalYear", model.FiscalYear);
                        _parameters.Add("@EmployeeId", employee.EmployeeID);
                        _parameters.Add("@ShiftID", shift.ShiftID);
                        _parameters.Add("@DayName", shift.DayName);
                        _parameters.Add("@applicableDate",DateTime.Parse(employee.ApplicableDateString));
                        _parameters.Add("@UserID", model.CreatedBy);
                        #endregion
                        var response = _dapperRepository.ExecuteStoredFirstOrDefault<MessageViewModel>("Spa_InsertWeeklyRosterInToDynamicRoster", _parameters);
                        if(response.Code!= "SUCCESS")
                        {
                            result.Errors = new List<string> {"Error Occured: "+response.Message};
                        }
                    }
                }
                _weeklyRosterRepository.SaveChanges();

                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public FiscalYears GetFiscalYear(int FiscalYearID)
        {
            return  _fiscalYearsRepository.TableNoTracking.FirstOrDefault(x => x.FiscalID == FiscalYearID);
        }
        public async Task<DaysInMonthWithEmployeeShiftList> GetDynamicRosterListAsync(FiscalWithEmployeeDynamicListViewModel model)
        {
            var DynamicRosterViewModelList = new List<DynamicRosterViewModel>();
            var CurrentFiscalYear =  GetFiscalYear(model.FiscalYearID);
            var shiftList = await GetShiftListAsync();
            int StartDay = 1;

            var result = new DaysInMonthWithEmployeeShiftList();
            if (model.Year== CurrentFiscalYear.StartYear.Year && model.Month == CurrentFiscalYear.StartYear.Month)
            {
                StartDay = CurrentFiscalYear.StartYear.Day;
                int TotalDays= DateTime.DaysInMonth(model.Year, model.Month);
                result.Days = Enumerable.Range(StartDay, (TotalDays-StartDay)+1);
                model.EnglishFromDate = new DateTime(model.Year, model.Month, StartDay);
                model.EnglishToDate= new DateTime(model.Year, model.Month, TotalDays);
            }
            else if (model.Year == CurrentFiscalYear.EndYear.Year && model.Month == CurrentFiscalYear.EndYear.Month)
            {
                result.Days = Enumerable.Range(StartDay, CurrentFiscalYear.EndYear.Day);
                model.EnglishFromDate = new DateTime(model.Year, model.Month, StartDay);
                model.EnglishToDate = new DateTime(model.Year, model.Month, CurrentFiscalYear.EndYear.Day);
            }
            else
            {
                int TotalDays = DateTime.DaysInMonth(model.Year, model.Month);
                result.Days = Enumerable.Range(StartDay, TotalDays);
                model.EnglishFromDate = new DateTime(model.Year, model.Month, StartDay);
                model.EnglishToDate = new DateTime(model.Year, model.Month, TotalDays);
            }


           
            var strSQL = new StringBuilder();
            if (model.EmployeeList.Count() == 0) return null;

            string EmployeeIDList = string.Join(",", model.EmployeeList.ToList());
            strSQL.AppendFormat(@"SELECT E.FirstName+' '+E.LastName AS Name,
                                	   E.EmployeeID,
									   DR.Month,
									   DR.Day,
									   DR.DayName,
                                	   DR.ShiftID,
									   REPLACE(CONVERT(VARCHAR, DR.ApplicableDate,111), '/','-') AS ApplicableDateString
									   FROM Employee E
                                LEFT JOIN  DynamicRoster DR 
                ON E.EmployeeID=DR.EmployeeID AND DR.FiscalYear=@FiscalYear
            	AND DR.ActualDate <=@EnglishToDate AND DR.ActualDate>=@EnglishFromDate 
                                 WHERE E.EmployeeID IN (" + EmployeeIDList + ")");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@FiscalYear", model.FiscalYearID);
            _parameters.Add("@EnglishFromDate", model.EnglishFromDate);
            _parameters.Add("@EnglishToDate", model.EnglishToDate);
            var EmployeeShiftList = await _dapperRepository.ExecuteQueryAsync<DynamicRosterViewModel>(strSQL.ToString(), _parameters);

            if (EmployeeShiftList.Count() > 0)
            {
                var GroupedEmployeeList = EmployeeShiftList.GroupBy(x => new { x.EmployeeID, x.Name, x.FiscalYear,x.Month })
                .Select(x => new GroupedDynamicRosterViewModel()
                {
                    EmployeeID = x.Key.EmployeeID,
                    Name = x.Key.Name,
                    FiscalYear = x.Key.FiscalYear,
                    Month=x.Key.Month,
                    EmployeeShiftList = x.ToList()
                }).ToList();

                string DayName  = DateTime.UtcNow.DayOfWeek.ToString();
                var CurrentDateNow = DateTime.UtcNow;

                foreach (var employee in GroupedEmployeeList)
                {
                    for (DateTime Initial = model.EnglishFromDate; Initial <= model.EnglishToDate; Initial = Initial.AddDays(1))
                    {

                        bool IsOldDate = false;
                        if ((Initial < CurrentDateNow))
                        {
                            IsOldDate = true;
                        }
                        if ((Initial.Year == CurrentDateNow.Year && Initial.Month == CurrentDateNow.Month) && Initial.Day <= CurrentDateNow.Day)
                        {
                            IsOldDate = true;
                        }
                        var CurrentShift = employee.EmployeeShiftList
                                           .FirstOrDefault(x => x.Month == Initial.Month && x.Day == Initial.Day);
                        int CurrentShiftID = 0;
                        string SetDate = string.Empty;
                        if (CurrentShift != null)
                        {
                            CurrentShiftID = CurrentShift.ShiftID;
                            DayName = CurrentShift.DayName;
                            SetDate = CurrentShift.ApplicableDateString;
                        }
                        else
                        {
                            employee.Month = Initial.Month;
                            employee.FiscalYear = model.FiscalYearID;

                        }
                        int CurrentMonth = Initial.Month;
                        DynamicRosterViewModelList.Add(new DynamicRosterViewModel()
                        {
                            ApplicableDateString = SetDate,
                            Day = Initial.Day,
                            Month = CurrentMonth,
                            DayName = DayName,
                            EmployeeID = employee.EmployeeID,
                            ShiftID = CurrentShiftID,
                            FiscalYear = model.FiscalYearID,
                            ShiftList = shiftList,
                            Name = employee.Name,
                            IsOldDate = IsOldDate
                        });
                    }
                }
               result.EmployeeShiftList = DynamicRosterViewModelList.GroupBy(x => new { x.EmployeeID, x.Name, x.FiscalYear,x.Month })
               .Select(x => new GroupedDynamicRosterViewModel()
               {
                   EmployeeID = x.Key.EmployeeID,
                   Name = x.Key.Name,
                   FiscalYear = x.Key.FiscalYear,
                   IsOldMonth=x.Key.Month < CurrentDateNow.Month?true:false,
                   EmployeeShiftList = x.OrderBy(x=>x.Day).ToList()
               }).ToList();
            }

            result.IsOldMonth=result.EmployeeShiftList.Any(x=>x.IsOldMonth==true)?true:false;
          
            result.Month = model.Month;
            result.FiscalYear = model.FiscalYearID;
            result.EnglishFromDate = model.EnglishFromDate;
            result.EnglishToDate = model.EnglishToDate;
            return result;

        }

        public AccountResult UpdateDynamicRosterAsync(DaysInMonthWithEmployeeShiftList model)
        {
            var result = new AccountResult();
            var CurrentFiscalYear =  GetFiscalYear(model.FiscalYear);
            int Year = 0;
            var CurrentDateNow = DateTime.UtcNow;
            var CheckList = new List<DynamicRoster>();
            if (model.EmployeeShiftList.Count() > 0)
            {
                foreach(var employee in model.EmployeeShiftList)
                {
                    if (employee.EmployeeShiftList.Count() > 0)
                    {
                        var currentYear = (DateTime)model.EnglishFromDate;
                        if (string.IsNullOrEmpty(employee.ApplicableDateString)) continue;
                        foreach (var shift in employee.EmployeeShiftList)
                        {
                            currentYear.AddDays(1);
                            if (currentYear < employee.ApplicableDate) continue;
                            Year = currentYear.Year;
                            DateTime ConvertToDate = Convert
                              .ToDateTime(Year.ToString() + "-" + model.Month.ToString() + "-" + shift.Day.ToString());
                            string DayName = ConvertToDate.DayOfWeek.ToString();


                            if (Year == CurrentDateNow.Year && shift.Month < CurrentDateNow.Month
                                || (Year == CurrentDateNow.Year && shift.Month == CurrentDateNow.Month && shift.Day <= CurrentDateNow.Day)
                                )
                            { continue; }
                            var CheckDate = Convert.ToDateTime(employee.ApplicableDateString);
                            if (ConvertToDate < CheckDate) continue;
                            var strSQL = new StringBuilder();
                            strSQL.AppendFormat(@"SELECT  
                                                    ApplicableGender,
                                                    ApplicableReligion,
                                                    FromDate,
                                                    ToDate,
                                                    WeekendDay,
                                                    IsWeekendLeave FROM   
													(SELECT A.DepartmentID,
                                                    Split.a.value('.', 'VARCHAR(100)') AS ActualDepartmentID,
                                                    ApplicableGender,
                                                    ApplicableReligion,
                                                    FromDate,
                                                    ToDate,
                                                    WeekendDay,
                                                    IsWeekendLeave,
													FiscalYear
                                                    FROM
                                                    (
                                                    SELECT DepartmentID,
                                                    CAST ('<M>' + REPLACE(DepartmentID, ',', '</M><M>') + '</M>' AS XML) AS ActualDepartmentID,
                                                    ApplicableGender,
                                                    ApplicableReligion,
                                                    FromDate,
                                                    ToDate,
                                                    WeekendDay,
                                                    IsWeekendLeave,
													FiscalYear
                                                    FROM Holiday
                                                    ) AS A CROSS APPLY ActualDepartmentID.nodes ('/M') AS Split(a)) H
													LEFT JOIN Employee E
													ON (ISNULL(CAST(H.ActualDepartmentID AS INT),0)=E.DepartmentID)
											     	AND (E.Gender=H.ApplicableGender OR H.ApplicableGender='A' OR H.ApplicableGender IS NULL)
													AND (E.Religion=H.ApplicableReligion OR (E.Religion IS NULL OR H.ApplicableReligion IS NULL))

													WHERE FiscalYear=@FiscalYear AND (E.EmployeeID=@EmployeeID OR E.EmployeeID IS NULL) AND
													((WeekendDay IS NULL AND @Date<=H.ToDate AND @Date>=H.FromDate)
                                                    OR (WeekendDay IS NOT NULL AND WeekendDay=DATENAME(DW,@Date)))");
                            DynamicParameters _parameters = new DynamicParameters();
                            _parameters.Add("@EmployeeID", shift.EmployeeID);
                            _parameters.Add("@Date", ConvertToDate);
                            _parameters.Add("@FiscalYear", model.FiscalYear);
                            var EmployeeHolidayList = _dapperRepository.ExecuteQuery<EmployeeHolidayViewModel>(strSQL.ToString(), _parameters);
                            if (EmployeeHolidayList.Count() > 0)
                            {
                                var holiday = new DynamicRoster()
                                {
                                    ShiftID = 0,
                                    EmployeeID = employee.EmployeeID,
                                    Month = model.Month,
                                    Day = shift.Day,
                                    DayName = DayName,
                                    ApplicableDate = DateTime.Parse(employee.ApplicableDateString),
                                    FiscalYear = model.FiscalYear,
                                    CreatedBy = model.CreatedBy,
                                    CreatedTS = DateTime.UtcNow,
                                    ActualDate=ConvertToDate,
                                    Year=Year
                                };
                                CheckList.Add(holiday);
                                _dynamicRosterRepository.Insert(holiday);
                                continue;

                            }
                            else
                            {

                                var data = new DynamicRoster()
                                {
                                    ShiftID = shift.ShiftID,
                                    EmployeeID = employee.EmployeeID,
                                    Month = model.Month,
                                    Day = shift.Day,
                                    DayName = DayName,
                                    ApplicableDate = DateTime.Parse(employee.ApplicableDateString),
                                    FiscalYear = model.FiscalYear,
                                    CreatedBy = model.CreatedBy,
                                    CreatedTS = DateTime.UtcNow,
                                    ActualDate = ConvertToDate,
                                    Year = Year
                                };
                                CheckList.Add(data);
                                _dynamicRosterRepository.Insert(data);
                            }

                       }
                        using (var uow = _unitOfWork.NewUnitOfWork())
                        {
                            try
                            {
                                if (CheckList.Count() == 0) return result;
                                var IsDeleted = DeleteDynamicRoster(employee, CurrentFiscalYear,model.Month);
                                if (IsDeleted.Success)
                                {
                                    _dynamicRosterRepository.SaveChanges();
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
                }
            }

            return result;
        }

        public AccountResult DeleteDynamicRoster(GroupedDynamicRosterViewModel employee, FiscalYears CurrentFiscalYear,int Month)
        {
            var result = new AccountResult();
            int StartYear = CurrentFiscalYear.StartYear.Year;
            int EndYear = CurrentFiscalYear.EndYear.Year;

            List<int> YearList = new List<int>()
                            {
                                StartYear,EndYear
                            };
            var Current = DateTime.UtcNow;
                var ShiftList = _dynamicRosterRepository.Table
               .Where(x => x.FiscalYear == CurrentFiscalYear.FiscalID && YearList.Contains(x.Year) && x.Month == Month && x.EmployeeID== employee.EmployeeID)
               .ToList();
                if (ShiftList.Count() > 0)
                {
                    foreach(var shift in ShiftList)
                    {
                        DateTime ConvertToDate = Convert
                          .ToDateTime(shift.Year.ToString() + "-" + shift.Month.ToString() + "-" + shift.Day.ToString());
                        if (ConvertToDate < Convert.ToDateTime(employee.ApplicableDateString))
                        {
                            continue;
                        }
                        else
                        {
                            _dynamicRosterRepository.Delete(shift);
                        }
                    }
                }

             _dynamicRosterRepository.SaveChanges();
            return result;
        }

        public async Task<DaysInMonthWithEmployeeShiftList> GetDynamicRosterListNPAsync(FiscalWithEmployeeDynamicListViewModel model)
        {
            var result = new DaysInMonthWithEmployeeShiftList();
            var DynamicRosterViewModelList = new List<DynamicRosterViewModel>();

            result.NepaliFromDate = model.NepaliFromDate;
            result.NepaliToDate = model.NepaliToDate;

            var shiftList = await GetShiftListAsync();
            var strSQL = new StringBuilder();
            if (model.EmployeeList.Count() == 0) return null;
            string EmployeeIDList = string.Join(",", model.EmployeeList.ToList());
            strSQL.AppendFormat(@"SELECT E.FirstName+' '+E.LastName AS Name,
                                	   E.EmployeeID,
									   DR.Month,
									   DR.Day,
									   DR.DayName,
                                	   DR.ShiftID,
									   REPLACE(CONVERT(VARCHAR, DR.ApplicableDate,111), '/','-') AS ApplicableDateString
									   FROM Employee E
                                LEFT JOIN  DynamicRoster DR 
            ON E.EmployeeID=DR.EmployeeID AND DR.FiscalYear=@FiscalYear
            	AND DR.ActualDate <=@NepaliToDate AND DR.ActualDate>=@NepaliFromDate ");

            strSQL.AppendFormat(@" WHERE E.EmployeeID IN (" + EmployeeIDList + ")");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@FiscalYear", model.FiscalYearID);
            _parameters.Add("@NepaliFromDate", model.NepaliFromDate);
            _parameters.Add("@NepaliToDate", model.NepaliToDate);
            var EmployeeShiftList = await _dapperRepository.ExecuteQueryAsync<DynamicRosterViewModel>(strSQL.ToString(), _parameters);

            if (EmployeeShiftList.Count() > 0)
            {
                var GroupedEmployeeList = EmployeeShiftList.GroupBy(x => new { x.EmployeeID, x.Name, x.FiscalYear })
                .Select(x => new GroupedDynamicRosterViewModel()
                {
                    EmployeeID = x.Key.EmployeeID,
                    Name = x.Key.Name,
                    FiscalYear = x.Key.FiscalYear,
                    EmployeeShiftList = x.ToList()
                }).ToList();

                string DayName = DateTime.UtcNow.DayOfWeek.ToString();
                var CurrentDateNow = DateTime.UtcNow;
                foreach (var employee in GroupedEmployeeList)
                {
                for (DateTime Initial = model.NepaliFromDate; Initial <= model.NepaliToDate; Initial= Initial.AddDays(1))
                        {
                           
                            bool IsOldDate = false;
                            if ((Initial< CurrentDateNow))
                            {
                                IsOldDate = true;
                            }
                            if ((Initial.Year==CurrentDateNow.Year && Initial.Month == CurrentDateNow.Month) && Initial.Day <= CurrentDateNow.Day)
                            {
                                IsOldDate = true;
                            }
                            var CurrentShift = employee.EmployeeShiftList
                                               .FirstOrDefault(x => x.Month == Initial.Month && x.Day == Initial.Day);
                            int CurrentShiftID = 0;
                            string SetDate = string.Empty; 
                            if (CurrentShift != null)
                            {
                                CurrentShiftID = CurrentShift.ShiftID;
                                DayName = CurrentShift.DayName;
                                SetDate = CurrentShift.ApplicableDateString;
                            }
                            else
                            {
                                employee.Month = Initial.Month;
                                employee.FiscalYear = model.FiscalYearID;

                            }
                            int CurrentMonth = Initial.Month;
                            DynamicRosterViewModelList.Add(new DynamicRosterViewModel()
                            {
                                ApplicableDateString = SetDate,
                                Day = Initial.Day,
                                Month = CurrentMonth,
                                DayName = DayName,
                                EmployeeID = employee.EmployeeID,
                                ShiftID = CurrentShiftID,
                                FiscalYear = model.FiscalYearID,
                                ShiftList = shiftList,
                                Name = employee.Name,
                                IsOldDate = IsOldDate
                            });
                        }
                }
                result.EmployeeShiftList = DynamicRosterViewModelList.GroupBy(x => new { x.EmployeeID, x.Name, x.FiscalYear })
                .Select(x => new GroupedDynamicRosterViewModel()
                {
                    EmployeeID = x.Key.EmployeeID,
                    Name = x.Key.Name,
                    FiscalYear = x.Key.FiscalYear,
                    IsOldMonth = x.Where(y=>y.Month < CurrentDateNow.Month).Count()>0 ? true : false,
                    EmployeeShiftList = x.ToList()
                }).ToList();
            }
            result.NepaliFromDate = model.NepaliFromDate;
            result.NepaliToDate = model.NepaliToDate;
            result.FiscalYear = model.FiscalYearID;
            return result;

        }

        public AccountResult UpdateDynamicRosterNPAsync(DaysInMonthWithEmployeeShiftList model)
        {
            var result = new AccountResult();
            var CurrentFiscalYear = GetFiscalYear(model.FiscalYear);
            List<DynamicRoster> DynamicRosterList = new List<DynamicRoster>();
        var CurrentDateNow = DateTime.UtcNow;
            int Year = 0;
            if (model.EmployeeShiftList.Count() > 0)
            {
                foreach (var employee in model.EmployeeShiftList)
                {
                    if (employee.EmployeeShiftList.Count() > 0)
                    {
                        var currentYear = (DateTime)model.NepaliFromDate;
                        if (string.IsNullOrEmpty(employee.ApplicableDateString)) continue;
                        foreach (var shift in employee.EmployeeShiftList)
                        {
                            currentYear = currentYear.AddDays(1);
                            if (currentYear < employee.ApplicableDate) continue;
                            Year = currentYear.Year;
                            DateTime ConvertToDate = Convert
                              .ToDateTime(Year.ToString() + "-" + shift.Month.ToString() + "-" + shift.Day.ToString());
                            string DayName = ConvertToDate.DayOfWeek.ToString();


                            if (Year==CurrentDateNow.Year && shift.Month < CurrentDateNow.Month
                                || (Year == CurrentDateNow.Year && shift.Month == CurrentDateNow.Month && shift.Day <= CurrentDateNow.Day)
                                )
                            { continue; }
                            var CheckDate = Convert.ToDateTime(employee.ApplicableDateString);
                            if (ConvertToDate < CheckDate) continue;
                            var strSQL = new StringBuilder();
                            strSQL.AppendFormat(@"SELECT  
                                                    ApplicableGender,
                                                    ApplicableReligion,
                                                    FromDate,
                                                    ToDate,
                                                    WeekendDay,
                                                    IsWeekendLeave FROM   
													(SELECT A.DepartmentID,
                                                    Split.a.value('.', 'VARCHAR(100)') AS ActualDepartmentID,
                                                    ApplicableGender,
                                                    ApplicableReligion,
                                                    FromDate,
                                                    ToDate,
                                                    WeekendDay,
                                                    IsWeekendLeave,
													FiscalYear
                                                    FROM
                                                    (
                                                    SELECT DepartmentID,
                                                    CAST ('<M>' + REPLACE(DepartmentID, ',', '</M><M>') + '</M>' AS XML) AS ActualDepartmentID,
                                                    ApplicableGender,
                                                    ApplicableReligion,
                                                    FromDate,
                                                    ToDate,
                                                    WeekendDay,
                                                    IsWeekendLeave,
													FiscalYear
                                                    FROM Holiday
                                                    ) AS A CROSS APPLY ActualDepartmentID.nodes ('/M') AS Split(a)) H
													LEFT JOIN Employee E
													ON (ISNULL(CAST(H.ActualDepartmentID AS INT),0)=E.DepartmentID)
											     	AND (E.Gender=H.ApplicableGender OR H.ApplicableGender='A' OR H.ApplicableGender IS NULL)
													AND (E.Religion=H.ApplicableReligion OR (E.Religion IS NULL OR H.ApplicableReligion IS NULL))

													WHERE FiscalYear=@FiscalYear AND (E.EmployeeID=@EmployeeID OR E.EmployeeID IS NULL) AND
													((WeekendDay IS NULL AND @Date<=H.ToDate AND @Date>=H.FromDate)
                                                    OR (WeekendDay IS NOT NULL AND WeekendDay=DATENAME(DW,@Date)))");
                            DynamicParameters _parameters = new DynamicParameters();
                            _parameters.Add("@EmployeeID", shift.EmployeeID);
                            _parameters.Add("@Date", ConvertToDate);
                            _parameters.Add("@FiscalYear", model.FiscalYear);
                            var EmployeeHolidayList = _dapperRepository.ExecuteQuery<EmployeeHolidayViewModel>(strSQL.ToString(), _parameters);
                            if (EmployeeHolidayList.Count() > 0)
                            {
                                var holiday = new DynamicRoster()
                                {
                                    ShiftID = 0,
                                    EmployeeID = employee.EmployeeID,
                                    Month = shift.Month,
                                    Day = shift.Day,
                                    DayName = DayName,
                                    ApplicableDate = DateTime.Parse(employee.ApplicableDateString),
                                    FiscalYear = model.FiscalYear,
                                    CreatedBy = model.CreatedBy,
                                    CreatedTS = DateTime.UtcNow,
                                    ActualDate= ConvertToDate,
                                    Year=Year
                                };
                                DynamicRosterList.Add(holiday);
                                continue;

                            }
                            else
                            {

                                var data = new DynamicRoster()
                                {
                                    ShiftID = shift.ShiftID,
                                    EmployeeID = employee.EmployeeID,
                                    Month = shift.Month,
                                    Day = shift.Day,
                                    DayName = DayName,
                                    ApplicableDate = DateTime.Parse(employee.ApplicableDateString),
                                    FiscalYear = model.FiscalYear,
                                    CreatedBy = model.CreatedBy,
                                    CreatedTS = DateTime.UtcNow,
                                    ActualDate = ConvertToDate,
                                    Year = Year
                                };
                                DynamicRosterList.Add(data);
                              
                            }

                        }
                    }
                }


                using (var uow = _unitOfWork.NewUnitOfWork())
                {
                    try
                    {

                        if (DynamicRosterList.Count() > 0)
                        {
                            int StartYear=CurrentFiscalYear.StartYear.Year;
                            int EndYear = CurrentFiscalYear.EndYear.Year;

                            List<int> YearList = new List<int>()
                            {
                                StartYear,EndYear
                            };
                            List<DynamicRoster> Delete = new List<DynamicRoster>();
                            foreach (var roster in DynamicRosterList)
                            {
                                var itemsToDelete = _dynamicRosterRepository
                                    .Table
                                    .FirstOrDefault(x => x.FiscalYear == model.FiscalYear && x.EmployeeID == roster.EmployeeID
                                    && YearList.Contains(x.Year) && x.Month == roster.Month && x.Day == roster.Day);
                                if (itemsToDelete != null)
                                {
                                    Delete.Add(itemsToDelete);
                                }
                            }
                            _dynamicRosterRepository.Delete(Delete);
                            _dynamicRosterRepository.Insert(DynamicRosterList);
                            _dynamicRosterRepository.SaveChanges();

                        }
                        else
                        {
                            return result;
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
    }
}

