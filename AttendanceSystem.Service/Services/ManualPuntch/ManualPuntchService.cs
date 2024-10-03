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
using System.Net;
using System.Net.Sockets;

namespace AttendanceSystem.Services
{
    public class ManualPuntchService : IManualPuntchService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<DeviceLogs> _manulapuntchRepository;
        private IGenericRepository<Employee> _employeeeRepository;
        public ManualPuntchService(IGenericRepository<DeviceLogs> manulapuntchRepository,
                            IDapperRepository dapperRepository, IGenericRepository<Employee> employeeeRepository)
        {
            _manulapuntchRepository = manulapuntchRepository;
            _employeeeRepository = employeeeRepository;
            _dapperRepository = dapperRepository;
        }

       
        public async Task<AccountResult> InsertIntoManualPuntchAsync(ManualPuntchViewModelResult model)
        {

            try
            {
                var result = new AccountResult();
                var employee = _employeeeRepository.Table.FirstOrDefault(x => x.EmployeeID == model.EmployeeId);

                DateTime dt = Convert.ToDateTime(model.Dateonly + " " + model.Timeonly);
                DateTime Puntch = DateTime.ParseExact(model.Dateonly + " " + model.Timeonly, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                var newDevice = new DeviceLogs()
                {
                    DeviceLogsID = new Guid(),
                    DeviceNumber = Convert.ToInt32(employee.DeviceNumber),
                    EnrollID = employee.EnrollID,
                    PunchDate = Puntch,
                    IsProcessed = false,
                    FetchedDate = DateTime.UtcNow,
                    CreatedBy = model.CreatedBy,
                    CreatedTS = DateTime.UtcNow
                };
                _manulapuntchRepository.Insert(newDevice);
                await _manulapuntchRepository.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<AccountResult> UpdateManualPuntchAsync(ManualPuntchViewModelResult model)
        {
            try
            {
                var result = new AccountResult();
                var ExistedDevice = GetDevicelogsByID(model.DeviceLogsID);
                var employee = _employeeeRepository.Table.FirstOrDefault(x => x.EmployeeID == model.EmployeeId);

                DateTime dt = Convert.ToDateTime(model.Dateonly + " " + model.Timeonly);
                //DateTime Puntch = DateTime.ParseExact(model.Dateonly + " " + model.Timeonly, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                if (ExistedDevice != null)
                {

                    ExistedDevice.DeviceNumber = Convert.ToInt32(employee.DeviceNumber) ;
                    ExistedDevice.EnrollID = employee.EnrollID;
                    ExistedDevice.PunchDate = dt;
                    ExistedDevice.FetchedDate = DateTime.UtcNow;
                    ExistedDevice.ModifiedBy = model.ModifiedBy;
                    ExistedDevice.ModifiedTS = DateTime.UtcNow;
                    _manulapuntchRepository.Update(ExistedDevice);
                    await _manulapuntchRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Attendance does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteManualPuntchAsync(Guid DeviceLogsID)
        {
            var result = new AccountResult();
            var ExistedDevice = GetDevicelogsByID(DeviceLogsID);
            if (ExistedDevice != null)
            {
                _manulapuntchRepository.Delete(ExistedDevice);
             await _manulapuntchRepository.SaveChangesAsync();
            }
            else
               {
                    result.Errors = new List<string> { "Attendace does not exist." };
                    return result;
               }
            return result;
        }
        public DeviceLogs GetDevicelogsByID(Guid DeviceLogsID)
        {
            return _manulapuntchRepository.Table.FirstOrDefault(x => x.DeviceLogsID == DeviceLogsID);
        }

        public async Task<IPagedList<ManualPuntchViewModel>> ManualPuntchListAsync(ManualPuntchSearchViewModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                          ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
                                             emp.FirstName+' '+emp.LastName [Name],
                                             emp.EmployeeId,
                                             device.DeviceLogsID,device.DeviceNumber,device.EnrollID,
                                             CONVERT(nvarchar(10),device.PunchDate,126) as PunchDate,
                                             CONVERT(nvarchar(10), device.PunchDate,126) as Dateonly,
                                             CONVERT(VARCHAR(20), device.PunchDate, 108) AS Timeonly,
                                             emp.SectionID as SectionID,
                                             emp.DepartmentID as DepartmentID
                                             FROM DeviceLogs device
                                             left join Employee emp
                                             on (device.EnrollID=emp.EnrollID and device.DeviceNumber=emp.DeviceNUmber)
                                             WHERE 1=1 and IsProcessed=0");
                #region Filters

                if (model.DeviceNumber>0)
                {
                    strSQL.AppendFormat(@" AND DeviceNumber=@DeviceNumber");
                } 
                if (model.EnrollID>0)
                {
                    strSQL.AppendFormat(@" AND EnrollID=@EnrollID");
                }

                #endregion

                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@EnrollID", model.EnrollID);
                _parameters.Add("@DeviceNumber", model.DeviceNumber);
                //_parameters.Add("@PuntchDate", model.PuntchDate);
                #endregion

                return await _dapperRepository.ExecuteQueryWithPagedListAsync<ManualPuntchViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "EnrollID");
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<ManualPuntchModel> GetManualPuntchByDevicelogsIDAsync(Guid DevicelogsID)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                             ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
                                             emp.FirstName+' '+emp.LastName [Name],
                                             emp.EmployeeId,
                                             device.DeviceLogsID,device.DeviceNumber,device.EnrollID,
                                             CONVERT(nvarchar(10),device.PunchDate,126) as PunchDate,
                                             CONVERT(nvarchar(10), device.PunchDate,126) as Dateonly,
                                             CONVERT(VARCHAR(20), device.PunchDate, 108) AS Timeonly,
                                             emp.SectionID as SectionID,
                                             emp.DepartmentID as DepartmentID
                                             FROM DeviceLogs device
                                             left join Employee emp
                                             on (device.EnrollID=emp.EnrollID and device.DeviceNumber=emp.DeviceNUmber)
                                             WHERE 1=1 and IsProcessed=0
                                             and device.DeviceLogsID=@DevicelogsID");

                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@DevicelogsID", DevicelogsID);
                #endregion

                var result= await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<ManualPuntchViewModel>(strSQL.ToString(), _parameters);
                var Finalresult = new ManualPuntchModel() {
                    DeviceLogsID=result.DeviceLogsID,
                    DeviceNumber=result.DeviceNumber,
                    EmployeeId=result.EmployeeId,
                    EnrollID=result.EnrollID,
                    PunchDate=result.PunchDate,
                    IsProcessed=result.IsProcessed,
                    FetchedDate=result.FetchedDate,
                    CreatedTS=result.CreatedTS,
                    CreatedBy=result.CreatedBy,
                    Dateonly=result.Dateonly,
                    Timeonly=result.Timeonly,
                    Name=result.Name,
                    DepartmentID = StringToIntArray(result.DepartmentID),
                    SectionID= StringToIntArray(result.SectionID),
                };

                return Finalresult;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        private static int[] StringToIntArray(string myNumbers)
        {
            List<int> myIntegers = new List<int>();
            Array.ForEach(myNumbers.Split(",".ToCharArray()), s =>
            {
                int currentInt;
                if (Int32.TryParse(s, out currentInt))
                    myIntegers.Add(currentInt);
            });
            return myIntegers.ToArray();
        }
    }
}
