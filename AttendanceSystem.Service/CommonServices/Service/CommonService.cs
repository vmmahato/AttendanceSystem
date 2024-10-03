using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AttendanceSystem.Helpers;
using Microsoft.AspNetCore.Http;
using AttendanceSystem.DapperServices;
using Dapper;

namespace AttendanceSystem.Services
{
    public class CommonService : ICommonService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private IGenericRepository<CodeTable> _codeTableRepository;
        private IDapperRepository _dapperRepository;
        private IGenericRepository<Department> _departmentRepository;
        private IGenericRepository<Employee> _employeeRepository;
        public CommonService(IGenericRepository<CodeTable> codeTableRepository,
                                                IHostingEnvironment hostingEnvironment,
                                                IDapperRepository dapperRepository,
                                                IGenericRepository<Department> departmentRepository,
                                                IGenericRepository<Employee> employeeRepository)
        {
            _codeTableRepository = codeTableRepository;
            _hostingEnvironment = hostingEnvironment;
            _dapperRepository = dapperRepository;
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task<IEnumerable<SelectItemViewModel>> GetGenderList()
        {
            return await _codeTableRepository.TableNoTracking
                 .Where(x => x.TableName == "GenderTable").OrderBy(x=>x.DisplayOrder)
                 .Select(x => new SelectItemViewModel()
                 {
                     ID = x.Description,
                     Value = x.Value
                 }).ToListAsync();
        }

        public string GetFullPath(string URL, string BranchIDFromServer, string Branch)
        {
            return Path.Combine(_hostingEnvironment.WebRootPath, URL, Branch, BranchIDFromServer);
        }

        public async Task<ImageTypesViewModel> SaveFiles(List<FileUploadViewModel> files, string DirectoryPath)
        {
            var result = new ImageTypesViewModel();
            bool folderExists = Directory.Exists(DirectoryPath);
            if (folderExists)
            {
                Directory.GetFiles(DirectoryPath).ToList().ForEach(File.Delete);
            }
            else
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            foreach (var file in files)
            {
                var FullPath = Path.Combine(DirectoryPath, file.FileName);
                if (!string.IsNullOrEmpty(FullPath))
                {
                    using (var fileSteam = new FileStream(FullPath, FileMode.Create))
                    {
                        await file.File.CopyToAsync(fileSteam);
                        if (file.FileType == "Employee")
                        {
                            result.Employee = file.FileName;
                        }
                        if (file.FileType == "CompanyImage")
                        {
                            result.Company = file.FileName;
                        }
                    }
                }
            }
            return result;

        }

        public List<FileUploadViewModel> GetLogo(LogoViewModel model)
        {
            var FileList = new List<FileUploadViewModel>();

            //Logo
            string LogoFileName = Guid.NewGuid() + "_" + model.Logo.FileName;
            FileList.Add(new FileUploadViewModel()
            {
                File = model.Logo,
                FileName = LogoFileName,
                FileType = "Logo"
            });
            return FileList;
        }

        public List<string> CheckFiles(IFormFile File)
        {
            var Errors = new List<string>();
            if (SharedServices.CheckFileLength(File) == false)
            {
                Errors.Add("Register Document exceeds length of 1 mb.");
            }
            return Errors;
        }

        public List<string> CheckLogo(LogoViewModel model)
        {
            var Errors = new List<string>();
            if (SharedServices.CheckFileLength(model.Logo) == false)
            {
                Errors.Add("Logo exceeds length of 1 mb.");
            }
            return Errors;
        }

        public void DeleteFiles(int ID,string Type)
        {
            var DirectoryPath = GetFullPath("Images", ID.ToString(), Type);
            if (!string.IsNullOrEmpty(DirectoryPath))
            {
                if (Directory.Exists(DirectoryPath))
                {
                    Directory.GetFiles(DirectoryPath).ToList().ForEach(File.Delete);
                }
            }

        }

        public async Task<IEnumerable<SelectItemViewModel>> GetReportYearList()
        {
            return await _codeTableRepository.TableNoTracking
                .Where(x => x.TableName == "MultipleYear")
                .Select(x => new SelectItemViewModel()
                {
                    ID = x.Description,
                    Value = x.Value
                }).ToListAsync();
        }

        public async Task<SelectItemViewModel> GetReportYearList(int Year)
        {
            return await _codeTableRepository.TableNoTracking
                .Where(x => x.TableName == "MultipleYear" && x.Value==Year.ToString())
                .Select(x => new SelectItemViewModel()
                {
                    ID = x.Description,
                    Value = x.Value
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SelectItemViewModel>> GetDesignationList()
        {
            return await _codeTableRepository.TableNoTracking
                 .Where(x => x.TableName == "Designation")
                 .Select(x => new SelectItemViewModel()
                 {
                     ID = x.Description,
                     Value = x.Value
                 }).ToListAsync();
        }

        public async Task<IEnumerable<SelectItemViewModel>> TestCodeTable()
        {
            return await _codeTableRepository.TableNoTracking
                .Select(x => new SelectItemViewModel()
                {
                    ID = x.Description,
                    Value = x.Value
                }).ToListAsync();
        }

        public string GetFullPath(string URL)
        {
            return Path.Combine(_hostingEnvironment.WebRootPath, URL);
        }

        public List<FileUploadViewModel> GetFiles(IFormFile File,string ImageType)
        {
            var FileList = new List<FileUploadViewModel>();
            string ImageName = Guid.NewGuid() + "_" + File.FileName;
            FileList.Add(new FileUploadViewModel()
            {
                File =File,
                FileName = ImageName,
                FileType = ImageType
            });
            return FileList;
        }

        public void DeleteFilesFromDirectoryPath(string DirectoryPath)
        {
            if (!string.IsNullOrEmpty(DirectoryPath))
            {
                if (Directory.Exists(DirectoryPath))
                {
                    Directory.GetFiles(DirectoryPath).ToList().ForEach(File.Delete);
                }
            }

        }

        public async Task<IEnumerable<SelectItemViewModel>> GetCheckClockStatusList()
        {
            return await _codeTableRepository.TableNoTracking
                .Where(x => x.TableName == "CheckClock")
                .Select(x => new SelectItemViewModel()
                {
                    ID = x.Description,
                    Value = x.Value
                }).ToListAsync();
        }

        public async Task<IEnumerable<SelectItemViewModel>> GetHolidayTypeList()
        {
            return await _codeTableRepository.TableNoTracking
                  .Where(x => x.TableName == "HolidayTypeTable").OrderBy(x => x.DisplayOrder)
                  .Select(x => new SelectItemViewModel()
                  {
                      ID = x.Description,
                      Value = x.Value
                  }).ToListAsync();
        }

        public async Task<IEnumerable<SelectItemViewModel>> GetApplicableReligionList()
        {
            return await _codeTableRepository.TableNoTracking
                  .Where(x => x.TableName == "ReligionTypeTable").OrderBy(x => x.DisplayOrder)
                  .Select(x => new SelectItemViewModel()
                  {
                      ID = x.Description,
                      Value = x.Value
                  }).ToListAsync();
        }

        public async Task<IEnumerable<DashboardLeaveViewModel>> GetLeaveCount()
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT
                            E.FirstName+' '+E.LastName [Name],
                            D.DepartmentName Department
                            FROM LeaveApplication LA
                            LEFT JOIN Employee E ON LA.EmployeeID=E.EmployeeID
                            LEFT JOIN Department D ON E.DepartmentID=D.DepartmentID
                            WHERE LA.Status='Approved' AND CONVERT(VARCHAR, GETDATE(), 101)<=CONVERT(VARCHAR, LA.ToDate, 101)
                            AND CONVERT(VARCHAR, GETDATE(), 101)>=CONVERT(VARCHAR, LA.FromDate, 101) ");
            return await _dapperRepository.ExecuteQueryAsync<DashboardLeaveViewModel>(strSQL.ToString(), null);
        }

        public async Task<DashboardAbsentCountWithViewModel> GetAbsentCount(int FiscalYear)
        {
            var result = new DashboardAbsentCountWithViewModel();
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"
                                    SELECT 
                                    A.Name,
                                    A.Department,A.EmployeeID,
                                    CASE WHEN DATEPART(HOUR, MIN(A.PunchDate))<=DATEPART(HOUR, MIN(A.ShiftEnd)) 
                                    AND DATEPART(HOUR, MIN(A.PunchDate))>=DATEPART(HOUR, MIN(A.ShiftStart))
                                    THEN 0 ELSE 1 END Absent
                                    FROM
                                    (SELECT
                                    E.FirstName+' '+E.LastName [Name],E.EmployeeID,
                                    D.DepartmentName Department,
                                    WS.ShiftStart,
                                    WS.ShiftEnd,
                                    DL.PunchDate
                                    FROM Employee E 
                                    LEFT JOIN Department D ON E.DepartmentID=D.DepartmentID
                                    LEFT JOIN DynamicRoster DR ON E.EmployeeID=DR.EmployeeID 
                                    AND DR.FiscalYear=@FiscalYear AND DR.Month=MONTH(GETDATE()) AND DR.Day=DATENAME(DAY,GETDATE())
                                    LEFT JOIN WorkShift WS ON DR.ShiftID=WS.ShiftID
                                    LEFT JOIN DeviceLogs DL ON DL.DeviceNumber=E.DeviceNUmber 
                                    AND DL.EnrollID=E.EnrollID AND CONVERT(VARCHAR, DL.PunchDate, 101)=CONVERT(VARCHAR, GETDATE(), 101)) AS A
                                    GROUP BY A.Name,A.Department,A.EmployeeID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@FiscalYear", FiscalYear);
            result.List= await _dapperRepository.ExecuteQueryAsync<DashboardAbsentViewModel>(strSQL.ToString(), _parameters);

            result.TotalAbsentCount = result.List.Count() == 0 ? 0 : result.List.Count();
            return result;
        }

        public async Task<IEnumerable<DashboardHolidayViewModel>> GetHolidayCount()
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    H.HolidayName Name,
                                    H.HolidayType Type,
                                    H.FromDate,
                                    H.ToDate
                                    FROM
                                    Holiday H
                                    WHERE  ((MONTH(H.FromDate)=MONTH(GETDATE()) OR MONTH(H.ToDate)=MONTH(GETDATE())))
                                    AND DATENAME(DAY, GETDATE())<=DATENAME(DAY,H.ToDate)
                                    ");
            return await _dapperRepository.ExecuteQueryAsync<DashboardHolidayViewModel>(strSQL.ToString(), null);
        }

        public async Task<IEnumerable<DashboardVisitorViewModel>> GetVisitorCount()
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT
                                        OV.VisitorName Visitor,
                                        E.FirstName+' '+E.LastName Employee,
                                        OV.FromDate,
                                        OV.ToDate
                                        FROM OfficeVisit OV
                                        LEFT JOIN Employee E ON OV.EmployeeID=E.EmployeeID
                                        WHERE CONVERT(DATE,OV.FromDate,102)=CONVERT(DATE,GETDATE(),102)");
            return await _dapperRepository.ExecuteQueryAsync<DashboardVisitorViewModel>(strSQL.ToString(), null);
        }

        public async Task<DashboardCounts> GetCounts()
        {
            var result = new DashboardCounts();
            //Department
           result.DepartmentCount = await _departmentRepository.TableNoTracking.CountAsync();


            //Employee
           result.EmployeesCount = await _employeeRepository.TableNoTracking.CountAsync();

            //Active
        result.ActiveUserCount= await _employeeRepository.TableNoTracking.Where(x => x.ActiveAccount).CountAsync();

            return result;
        }
        public async Task<IEnumerable<DashboardAttendanceLogViewModel>> GetAttendanceLogCount()
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"select 
                                        Count(*) as PuntchCount,Test.EmployeeName,Test.EnrollID,Test.DeviceNumber,min(PunchDate)as Puntchdate,
                                            CASE WHEN min(PunchDate) is not null 
                                            THEN 'CheckIN'
                                            ELSE 'Not CheckIn' END AS DateStatus
                                            from (select  CONCAT(E.FirstName, ' ', E.LastName)as EmployeeName,
                                        D.EnrollID,D.DeviceNumber,D.PunchDate
                                        FROM DeviceLogs D
                                        Inner JOIN Employee E
                                        ON (E.EnrollID=D.EnrollID AND E.DeviceNumber=D.DeviceNumber) 
                                        where CONVERT(DATE,D.PunchDate,102)=CONVERT(date, getdate()))Test
                                        group by EmployeeName,DeviceNumber,EnrollID
                                        order by MIN(PunchDate)");
            return await _dapperRepository.ExecuteQueryAsync<DashboardAttendanceLogViewModel>(strSQL.ToString(), null);
        }

        public async Task<IEnumerable<DashboardAttendanceStatusViewModel>> GetAttendanceStatusCount()
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"select 
                                        Count(*) as PuntchCount,Test.EmployeeID,Test.EmployeeName,
                                        min(PunchDate)as Puntchdate ,
                                        CASE WHEN min(PunchDate) is not null 
                                            THEN 'CheckIN'
                                            ELSE 'Not CheckIn' END AS StartDateStatus
                                        from 
                                        (select E.EmployeeID,CONCAT(E.FirstName, ' ', E.LastName)as EmployeeName,
                                        D.PunchDate
                                        FROM  Employee E
                                        left JOIN DeviceLogs D
                                        ON (D.EnrollID=E.EnrollID AND D.DeviceNumber=E.DeviceNumber AND 
                                        cast(D.PunchDate as date)=CONVERT(date, getdate())) 
                                        )Test
                                        group by EmployeeID,EmployeeName
                                        order by MIN(PunchDate)");
            return await _dapperRepository.ExecuteQueryAsync<DashboardAttendanceStatusViewModel>(strSQL.ToString(), null);
        }

        public async Task<StatusViewModel> GetStatus(int FiscalYear)
        {
            var result = new StatusViewModel();
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    A.Name,
                                    A.Department,
									A.EmployeeID,
								    CASE WHEN 	DATEPART(MINUTE, MIN(A.PunchDate))>DATEPART(MINUTE, MIN(A.ShiftStart))
									THEN 1 ELSE 0 END AS LateCount,
									CASE WHEN DATEPART(HOUR, MIN(A.PunchDate))<=DATEPART(HOUR, MIN(A.ShiftEnd)) 
                                    AND DATEPART(HOUR, MIN(A.PunchDate))>=DATEPART(HOUR, MIN(A.ShiftStart))
                                    THEN 0 ELSE 1 END AbsentCount
                                    FROM
                                    (SELECT
									E.EmployeeID,
                                    E.FirstName+' '+E.LastName [Name],
                                    D.DepartmentName Department,
                                    WS.ShiftStart,
                                    WS.ShiftEnd,
                                    DL.PunchDate
                                    FROM Employee E 
                                    LEFT JOIN Department D ON E.DepartmentID=D.DepartmentID
                                    LEFT JOIN DynamicRoster DR ON E.EmployeeID=DR.EmployeeID 
                                    AND DR.FiscalYear=@FiscalYear AND DR.Month=MONTH(GETDATE()) AND DR.Day=DATENAME(DAY,GETDATE())
                                    LEFT JOIN WorkShift WS ON DR.ShiftID=WS.ShiftID
                                    LEFT JOIN DeviceLogs DL ON DL.DeviceNumber=E.DeviceNUmber 
                                    AND DL.EnrollID=E.EnrollID AND CONVERT(VARCHAR, DL.PunchDate, 101)=CONVERT(VARCHAR, GETDATE(), 101)) AS A
									GROUP BY   A.Name,
                                    A.Department,A.EmployeeID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@FiscalYear", FiscalYear);
            result.List = await _dapperRepository.ExecuteQueryAsync<EmployeeStatusViewModel>(strSQL.ToString(), _parameters);
            if (result.List.Count() > 0)
            {
                result.AbsentCount = result.List.Where(x => x.AbsentCount == true).Count();
                result.LateCount = result.List.Where(x => x.LateCount == true).Count();
                result.PresentCount = result.List.Where(x => x.AbsentCount == false).Count();
            }
            else
            {
                result.AbsentCount = 0;
                result.LateCount = 0;
                result.PresentCount = 0;
            }

            var strOfficeVisit = new StringBuilder();
            strOfficeVisit.AppendFormat(@"SELECT COUNT(OfficeVisitID) AS OfficeVisitCount FROM OfficeVisit WHERE CONVERT(DATE,FromDate,102)=CONVERT(DATE,GETDATE(),102)");
            result.OfficeVisitCount = await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<int>(strOfficeVisit.ToString(), null);

            var strKaj = new StringBuilder();
            strKaj.AppendFormat(@"	SELECT COUNT(KajID) AS KajCount FROM Kaj
									WHERE CONVERT(DATE,FromDate,102)=CONVERT(DATE,GETDATE(),102)");
            result.KajCount = await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<int>(strKaj.ToString(), null);

            var strLeave = new StringBuilder();
            strLeave.AppendFormat(@"SELECT COUNT(LA.LeaveID) AS LeaveCount
                            FROM LeaveApplication LA
                           WHERE LA.Status='Approved' AND CONVERT(VARCHAR, GETDATE(), 101)<=CONVERT(VARCHAR, LA.ToDate, 101)
                            AND CONVERT(VARCHAR, GETDATE(), 101)>=CONVERT(VARCHAR, LA.FromDate, 101)");
            result.LeaveCount = await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<int>(strLeave.ToString(), null);

            return result;
        }

        public async Task<IEnumerable<DashboardDeviceStatusViewModel>> GetDeviceStatusCount()
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"select 
                                        DeviceNumber,DeviceStatus,
                                        CASE 
                                        when DeviceStatus=0 then 'Deactive'
                                        else 'Active' 
                                        End AS Status
                                        from DeviceStatus");
            return await _dapperRepository.ExecuteQueryAsync<DashboardDeviceStatusViewModel>(strSQL.ToString(), null);
        }
    }
}
