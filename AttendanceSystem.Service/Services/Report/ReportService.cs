using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AttendanceSystem.DapperServices;
using AttendanceSystem.ViewModels;
using System.Linq;
using System.Text;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;

namespace AttendanceSystem.Services
{
    public class ReportService : IReportService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<FiscalYears> _fiscalYearsRepository;
        public ReportService(IDapperRepository dapperRepository, IGenericRepository<FiscalYears> fiscalYearsRepository)
        {
            _dapperRepository = dapperRepository;
            _fiscalYearsRepository = fiscalYearsRepository;
        }

        public async Task<IEnumerable<DailyReportViewModel>> DailyAttendanceReportAsync(ReportSearchViewModel model)
        {
            try
            {
                string EmployeeIDs = string.Empty;
                EmployeeIDs = string.Join(",", model.EmployeeID.ToList());
                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@ReportType", model.ReportType);
                _parameters.Add("@EmployeeID", EmployeeIDs);
                _parameters.Add("@Date", model.Date);
                #endregion
                return await _dapperRepository.ExecuteStoredProcAsync<DailyReportViewModel>("DailyAttendanceReport", _parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<IEnumerable<MonthlyReportViewModel>> MonthlyAttendanceReportAsync(ReportSearchViewModel model)
        {
            var result = new List<MonthlyReportViewModel>();
            string EmployeeIDs = string.Empty;
            EmployeeIDs = string.Join(",", model.EmployeeID.ToList());
            #region Parameters
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@ReportType", model.ReportType);
            _parameters.Add("@EmployeeID", EmployeeIDs);
            _parameters.Add("@FromDate", model.FromDate);
            _parameters.Add("@ToDate", model.ToDate);
            #endregion
            var ReportListRsult = await _dapperRepository.ExecuteStoredProcAsync<MonthlyReportList>("MonthlyAttendanceReport", _parameters);

            if (ReportListRsult.Count() > 0)
            {
                result = ReportListRsult.GroupBy(x => new { x.EmployeeID, x.Name, x.Department })
                    .Select(y => new MonthlyReportViewModel()
                    {
                        Department = y.Key.Department,
                        Name = y.Key.Name,
                        ReportList = y.Where(z => z.EmployeeID == y.Key.EmployeeID)
                        .Select(z => new MonthlyReport()
                        {
                            Date = z.Date,
                            Day = z.Day,
                            Estimated_IN = z.Estimated_IN,
                            Estimated_OUT = z.Estimated_OUT,
                            WorkHour = z.WorkHour,
                            Actual_IN = z.Actual_IN,
                            Actual_OUT = z.Actual_OUT,
                            BreakOut = z.BreakOut,
                            BreakIn = z.BreakIn,
                            TeaOut = z.TeaOut,
                            TeaIn = z.TeaIn,
                            workedhour = z.workedhour,
                            OT = z.OT,
                            EarlyIN = z.EarlyIN,
                            EarlyOUT = z.EarlyOUT,
                            LateIN = z.LateIN,
                            LateOUT = z.LateOUT,
                            Remarks = z.Remarks,
                        }).ToList()

                    }).ToList();
            }
            return result;
        }
        public async Task<RosterReportModel> GetRosterReportListAsync(ReportSearchViewModel model)
        {
            try
            {
                var reportlist = new RosterReportViewModel();
                var result = new RosterReportModel();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                strSQL.AppendFormat(@"SELECT E.FirstName+' '+E.LastName AS Name,
                                	   E.EmployeeID,
									   DR.Month,
									   DR.Day,
									   DR.DayName,
                                	   DR.ShiftID,
									   ISNULL(W.ShiftCode,'OFF') [ShiftName],
									   REPLACE(CONVERT(VARCHAR, DR.ApplicableDate,111), '/','-') AS ApplicableDate
									   FROM Employee E
									 LEFT JOIN  DynamicRoster DR ON E.EmployeeID=DR.EmployeeID 
                                     AND DR.FiscalYear=@FiscalYear AND DR.Month = @Month
									 LEFT OUTER JOIN WorkShift W ON DR.ShiftID=W.ShiftID
                                     WHERE E.EmployeeID IN (" + EmployeeIDList + ")");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FiscalYear", model.FiscalYearID);
                _parameters.Add("@Month", model.Month);
                var EmployeeShiftList = await _dapperRepository.ExecuteQueryAsync<RosterModel>(strSQL.ToString(), _parameters);

                if (EmployeeShiftList.Count() > 0)
                {
                    result.EmployeeShiftList = EmployeeShiftList.GroupBy(x => new { x.EmployeeID, x.Name })
                        .Select(y => new RosterReportViewModel()
                        {
                            Name = y.Key.Name,
                            ShiftList = y.Where(z => z.EmployeeID == y.Key.EmployeeID)
                            .Select(z => new RosterModel()
                            {
                                Name = z.Name,
                                Month = z.Month,
                                Day = z.Day,
                                DayName = z.DayName,
                                ShiftID = z.ShiftID,
                                ShiftName = z.ShiftName,
                                EmployeeID = z.EmployeeID,
                            }).OrderBy(d => d.Day).ToList()
                        }).ToList();
                }
                int DaysInCurrentMonth = 0;
                var CurrentFiscalYear = GetFiscalYear(model.FiscalYearID);
                List<int> YearList = new List<int>()
                        {
                            CurrentFiscalYear.StartYear.Year,CurrentFiscalYear.EndYear.Year
                        };
                int? currentYear = YearList.Distinct().Where(x => x == DateTime.UtcNow.Year).Select(x => x).FirstOrDefault();
                DaysInCurrentMonth = DateTime.DaysInMonth((int)currentYear, model.Month);
                result.Days = new List<HeaderModel>();
                for (int i = 1; i <= DaysInCurrentMonth; i++)
                {
                    DateTime date = new DateTime((int)currentYear, model.Month, i);
                    string dayname = date.ToString("ddd");
                    result.Days.Add(new HeaderModel()
                    {
                        Day = i,
                        DayName = dayname
                    });
                }
                result.Days = result.Days.OrderBy(x => x.Day).ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public async Task<RosterReportModel> GetRosterDateWiseReportListAsync(ReportSearchViewModel model)
        {
            try
            {
                var reportlist = new RosterReportViewModel();
                var result = new RosterReportModel();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                //      strSQL.AppendFormat(@"SELECT E.FirstName+' '+E.LastName AS Name,
                //                      	   E.EmployeeID,
                //  DR.Month,
                //  DR.Day,
                //  DR.DayName,
                //                      	   DR.ShiftID,
                //  ISNULL(W.ShiftCode,'OFF') [ShiftName],
                //  REPLACE(CONVERT(VARCHAR, DR.ApplicableDate,111), '/','-') AS ApplicableDate
                //  FROM Employee E
                //   LEFT JOIN  DynamicRoster DR ON E.EmployeeID=DR.EmployeeID 
                //                              AND DR.ActualDate <=@ToDate AND DR.ActualDate>=@FromDate 
                //LEFT OUTER JOIN WorkShift W ON DR.ShiftID=W.ShiftID
                //                           WHERE E.EmployeeID IN (" + EmployeeIDList + ")");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FromDate", model.FromDate);
                _parameters.Add("@ToDate", model.ToDate);
                _parameters.Add("@EmployeeID", EmployeeIDList);
                var EmployeeShiftList = await _dapperRepository.ExecuteStoredProcAsync<RosterModel>("Spa_RosterReport", _parameters);

                if (EmployeeShiftList.Count() > 0)
                {
                    result.EmployeeShiftList = EmployeeShiftList.GroupBy(x => new { x.EmployeeID, x.Name })
                        .Select(y => new RosterReportViewModel()
                        {
                            Name = y.Key.Name,
                            ShiftList = y.Where(z => z.EmployeeID == y.Key.EmployeeID)
                            .Select(z => new RosterModel()
                            {
                                Name = z.Name,
                                Month = z.Month,
                                Day = z.Day,
                                DayName = z.DayName,
                                ShiftID = z.ShiftID,
                                ShiftName = z.ShiftName,
                                EmployeeID = z.EmployeeID,
                            }).OrderBy(d => d.Day).ToList()
                        }).ToList();
                }
                var Shiftlist = EmployeeShiftList.GroupBy(x => new { x.Day, x.DayName })
                        .Select(y => new HeaderModel()
                        {
                            Day = y.Key.Day,
                            DayName = y.Key.DayName
                        }).ToList();
                var CurrentFiscalYear = GetFiscalYear(model.FiscalYearID);

                result.Days = new List<HeaderModel>();
                foreach (var Header in Shiftlist)
                {
                    if (Header.Day != 0 && Header.DayName != null)
                    {
                        result.Days.Add(new HeaderModel()
                        {
                            Day = Header.Day,
                            DayName = Header.DayName
                        });
                    }
                }
                result.Days = result.Days.OrderBy(x => x.Day).ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        private FiscalYears GetFiscalYear(int FiscalYearID)
        {
            return _fiscalYearsRepository.TableNoTracking.FirstOrDefault(x => x.FiscalID == FiscalYearID);
        }
        public async Task<IEnumerable<DailyLeaveReportViewModel>> DailyLeaveReportAsync(ReportSearchViewModel model)
        {
            try
            {
                var result = new List<DailyLeaveReportViewModel>();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                strSQL.AppendFormat(@"select
                                     emp.FirstName+' '+emp.LastName [Name],CONVERT(nvarchar(10), leave.FromDate,126) as FromDate ,
                                     CONVERT(nvarchar(10), leave.ToDate,126) as ToDate,leave.LeaveDay,leave.Status,
	                                 leaveSet.LeaveName,
	                                 leave.ReplacementLeaveType
	                                 from LeaveApplication as leave
	                                 left join Employee as emp
	                                 on leave.EmployeeID=emp.EmployeeID
	                                 left join LeaveSetup as leaveSet
	                                 on leaveSet.LeaveID=leave.leaveID
                                       WHERE
                                       @Date  BETWEEN leave.FromDate AND leave.ToDate
	                                   and leave.EmployeeID  IN (" + EmployeeIDList + ")");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@Date", model.Date);
                var LeaveList = await _dapperRepository.ExecuteQueryAsync<DailyLeaveReportViewModel>(strSQL.ToString(), _parameters);
                result = LeaveList.ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public async Task<IEnumerable<DailyManualReportViewModel>> DailyManualPuntchReportAsync(ReportSearchViewModel model)
        {
            try
            {
                var result = new List<DailyManualReportViewModel>();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                strSQL.AppendFormat(@"select 
                                      EmployeeId,EnrollID,Test.Name,Test.DeviceNumber,min(PuntchTime)as PuntchTime
                                            from 
											(select
											emp.EmployeeID,emp.FirstName+' '+emp.LastName [Name],de.EnrollID,de.DeviceNumber,CONVERT(VARCHAR(20), PunchDate, 108) AS PuntchTime
                                        FROM DeviceLogs de
                                        Inner JOIN Employee emp
                                        ON (Emp.EnrollID=De.EnrollID AND Emp.DeviceNumber=De.DeviceNumber) 
                                        where CONVERT(DATE,De.PunchDate,102)=@Date
                                        and IsProcessed=0
										and emp.EmployeeID  IN (" + EmployeeIDList + "))Test group by EmployeeId,Name,DeviceNumber,EnrollID order by MIN(PuntchTime)");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@Date", model.Date);
                var ManualPuntchList = await _dapperRepository.ExecuteQueryAsync<DailyManualReportViewModel>(strSQL.ToString(), _parameters);
                result = ManualPuntchList.ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public async Task<IEnumerable<DailyOfficeVisitReportViewModel>> DailyOfficeVisitReportAsync(ReportSearchViewModel model)
        {
            try
            {
                var result = new List<DailyOfficeVisitReportViewModel>();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                strSQL.AppendFormat(@"select emp.EmployeeID,emp.FirstName+' '+emp.LastName [Name],
                                        CONVERT(nvarchar(10), ov.FromDate,126) as FromDate,SUBSTRING( convert(varchar, ov.FromTime,108),1,5)as FromTime,
                                        CONVERT(nvarchar(10), ov.ToDate,126) as ToDate,SUBSTRING( convert(varchar, ov.ToTime,108),1,5) as ToTime,
                                        ov.Status,ov.VisitorName
                                        from OfficeVisit ov
                                        left join Employee emp
                                        on ov.EmployeeID=emp.EmployeeID
                                        where ov.FromDate<=@Date 
                                        and ov.ToDate>=@Date
                                        and ov.EmployeeID in(" + EmployeeIDList + ")");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@Date", model.Date);
                var OfficeVisitList = await _dapperRepository.ExecuteQueryAsync<DailyOfficeVisitReportViewModel>(strSQL.ToString(), _parameters);
                result = OfficeVisitList.ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public async Task<IEnumerable<MonthlyManualReportViewModel>> MonthlyManualPuntchReportAsync(ReportSearchViewModel model)
        {
            try
            {
                var result = new List<MonthlyManualReportViewModel>();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                strSQL.AppendFormat(@"select  
                                      EmployeeId,EnrollID,Test.Name,Test.DeviceNumber,
									  Test.AttendanceDate,
									  min(PuntchTime)as PuntchTime
                                            from 
											(select
											emp.EmployeeID,emp.FirstName+' '+emp.LastName [Name],
											de.EnrollID,de.DeviceNumber,
											CONVERT(nvarchar(10), de.PunchDate,126)  as AttendanceDate,
											CONVERT(VARCHAR(20), PunchDate, 108) AS PuntchTime
                                        FROM DeviceLogs de
                                        Inner JOIN Employee emp
                                        ON (Emp.EnrollID=De.EnrollID AND Emp.DeviceNumber=De.DeviceNumber) 
                                        where CONVERT(DATE,De.PunchDate,102) BETWEEN @FromDate AND @ToDate
                                        and IsProcessed=0
										and emp.EmployeeID  IN (" + EmployeeIDList + "))Test group by EmployeeId,Name,DeviceNumber,EnrollID,AttendanceDate order by EmployeeId ");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FromDate", model.FromDate);
                _parameters.Add("@ToDate", model.ToDate);
                var ManualPuntchList = await _dapperRepository.ExecuteQueryAsync<MonthlyManualReportViewModel>(strSQL.ToString(), _parameters);
                result = ManualPuntchList.ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public async Task<IEnumerable<DailyOfficeVisitReportViewModel>> MonthlyOfficeVisitReportAsync(ReportSearchViewModel model)
        {
            try
            {
                var result = new List<DailyOfficeVisitReportViewModel>();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                strSQL.AppendFormat(@"select emp.EmployeeID,emp.FirstName+' '+emp.LastName [Name],
                                        CONVERT(nvarchar(10), ov.FromDate,126) as FromDate,SUBSTRING( convert(varchar, ov.FromTime,108),1,5)as FromTime,
                                        CONVERT(nvarchar(10), ov.ToDate,126) as ToDate,SUBSTRING( convert(varchar, ov.ToTime,108),1,5) as ToTime,
                                        ov.Status,ov.VisitorName
                                        from OfficeVisit ov
                                        left join Employee emp
                                        on ov.EmployeeID=emp.EmployeeID
                                        where (ov.FromDate between @FromDate and @ToDate
										OR
										ov.ToDate between @FromDate and @ToDate
										)
                                        and ov.EmployeeID in(" + EmployeeIDList + ")");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FromDate", model.FromDate);
                _parameters.Add("@ToDate", model.ToDate);
                var OfficeVisitList = await _dapperRepository.ExecuteQueryAsync<DailyOfficeVisitReportViewModel>(strSQL.ToString(), _parameters);
                result = OfficeVisitList.ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public async Task<IEnumerable<DailyLeaveReportViewModel>> MonthlyLeaveReportAsync(ReportSearchViewModel model)
        {
            try
            {
                var result = new List<DailyLeaveReportViewModel>();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                strSQL.AppendFormat(@"select
                                     emp.FirstName+' '+emp.LastName [Name],CONVERT(nvarchar(10), leave.FromDate,126) as FromDate ,
                                     CONVERT(nvarchar(10), leave.ToDate,126) as ToDate,leave.LeaveDay,leave.Status,
	                                 leaveSet.LeaveName,
	                                 leave.ReplacementLeaveType
	                                 from LeaveApplication as leave
	                                 left join Employee as emp
	                                 on leave.EmployeeID=emp.EmployeeID
	                                 left join LeaveSetup as leaveSet
	                                 on leaveSet.LeaveID=leave.leaveID
                                       WHERE
                                        (leave.FromDate between @FromDate and @ToDate
										OR
										leave.ToDate between @FromDate and @ToDate
										)
	                                   and leave.EmployeeID  IN (" + EmployeeIDList + ")");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FromDate", model.FromDate);
                _parameters.Add("@ToDate", model.ToDate);
                var LeaveList = await _dapperRepository.ExecuteQueryAsync<DailyLeaveReportViewModel>(strSQL.ToString(), _parameters);
                result = LeaveList.ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public async Task<IEnumerable<MissingpunchReportViewModel>> DailyMissingpunchReportAsync(ReportSearchViewModel model)
        {
            try
            {
                var result = new List<MissingpunchReportViewModel>();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                var fiscalYear = _fiscalYearsRepository.Table.FirstOrDefault(x => x.IsCurrentFiscalYear == true && (model.Date >= x.StartYear && model.Date <= x.EndYear));
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                strSQL.AppendFormat(@"
                                    select 
                                        temptbl.Name,temptbl.EmployeeID as EmployeeID,
                                        CONVERT(nvarchar(10), dev.PunchDate,126) as punchDate,
                                            case 
                                            	when
                                            	dev.DeviceNumber 
                                            	IS NULL
                                            	then 'Missing punch'
                                            	else 'No Missing Punch'
                                             End as StatusPuntch,
                                             @Date as Searchdate
                                             from  		
                                                (select
                                                dy.ShiftID,emp.FirstName+' '+emp.LastName [Name],
                                                dy.FiscalYear,dy.Month, dy.Day ,dy.DayName,emp.DeviceNUmber,emp.EnrollID,emp.EmployeeID 
                                                from
                                                 DynamicRoster dy
                                                 left join  employee emp
                                                 on dy.EmployeeID=emp.EmployeeID
                                                
                                                where dy.ShiftID >0
                                                and emp.EmployeeID in(" + EmployeeIDList + ") " +
                                                "and dy.FiscalYear=@FiscalYearID and  dy.Month=cast(month(@Date) as varchar(2))" +
                                                " and dy.Day= cast(day(@Date) as varchar(2)) )temptbl left join DeviceLogs dev" +
                                                " on temptbl.DeviceNUmber=dev.DeviceNumber 	and temptbl.EnrollID= dev.EnrollID " +
                                                "and CONVERT(nvarchar(10), dev.PunchDate,126) =CONVERT(nvarchar(10),@Date,126) " +
                                                "group by EmployeeID,Name, CONVERT(nvarchar(10), dev.PunchDate,126),dev.DeviceNumber");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@Date", model.Date);
                _parameters.Add("@FiscalYearID", fiscalYear.FiscalID);
                var LeaveList = await _dapperRepository.ExecuteQueryAsync<MissingpunchReportViewModel>(strSQL.ToString(), _parameters);
                result = LeaveList.ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<MissingpunchReportViewModel>> MonthlyMissingpunchReportAsync(ReportSearchViewModel model)
        {
            try
            {
                var result = new List<MissingpunchReportViewModel>();
                var strSQL = new StringBuilder();
                if (model.EmployeeID.Count() == 0) return null;
                string EmployeeIDList = string.Join(",", model.EmployeeID.ToList());
                strSQL.AppendFormat(@"  select 
                                        temptbl.Name,temptbl.EmployeeID as EmployeeID,
                                        CONVERT(nvarchar(10), dev.PunchDate,126) as punchDate,
                                            case 
                                            	when
                                            	dev.DeviceNumber 
                                            	IS NULL
                                            	then 'Missing punch'
                                            	else 'No Missing Punch'
                                             End as StatusPuntch
                                             from  		
                                                (select
                                                dy.ShiftID,emp.FirstName+' '+emp.LastName [Name],
                                                dy.FiscalYear,dy.Month, dy.Day ,dy.DayName,emp.DeviceNUmber,emp.EnrollID,emp.EmployeeID
                                                from
                                                 DynamicRoster dy
                                                 left join  employee emp
                                                 on dy.EmployeeID=emp.EmployeeID
                                                
                                                where dy.ShiftID >0
                                                and emp.EmployeeID in(" + EmployeeIDList + ") " +
                                                " and dy.FiscalYear= @FiscalYearID and " +
                                                "(" +
                                                "dy.Month<=cast(month(@FromDate) as varchar(2)) " +
                                                "and " +
                                                " dy.Month>=cast(month(@ToDate) as varchar(2)) " +
                                                " and " +
                                                "dy.Day between cast(day(@FromDate) as varchar(2))" +
                                                " and " +
                                                "cast(day(@ToDate) as varchar(2))" +
                                                " ))temptbl " +
                                                " left join DeviceLogs dev" +
                                                " on temptbl.DeviceNUmber=dev.DeviceNumber 	and temptbl.EnrollID= dev.EnrollID " +
                                                "  and " +
                                                "CONVERT(nvarchar(10), dev.PunchDate,126)>=CONVERT(nvarchar(10),@FromDate, 126)" +
                                                "  and CONVERT(nvarchar(10), dev.PunchDate,126)<=CONVERT(nvarchar(10), @ToDate, 126)" +
                                                "group by EmployeeID,Name, CONVERT(nvarchar(10), dev.PunchDate,126),dev.DeviceNumber");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@FromDate", model.FromDate);
                _parameters.Add("@ToDate", model.ToDate);
                _parameters.Add("@FiscalYearID", model.FiscalYearID);
                var LeaveList = await _dapperRepository.ExecuteQueryAsync<MissingpunchReportViewModel>(strSQL.ToString(), _parameters);
                result = LeaveList.ToList();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
