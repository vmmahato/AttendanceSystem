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
    public class DeviceService : IDeviceService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<Device> _deviceRepository;
        private IGenericRepository<DeviceModel> _deviceModelRepository;
        private IGenericRepository<Employee> _employeeRepository;
        public DeviceService(IGenericRepository<Device> deviceRepository,
                            IDapperRepository dapperRepository,
                            IGenericRepository<DeviceModel> deviceModelRepository,
                            IGenericRepository<Employee> employeeRepository)
        {
            _deviceRepository = deviceRepository;
            _dapperRepository = dapperRepository;
            _deviceModelRepository = deviceModelRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IPagedList<DeviceViewModel>> DeviceListAsync(DeviceSearchViewModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                DeviceID,
                                    Branch,
	                                DeviceModelID,
	                                DeviceName,
	                                DeviceNumber,
	                                IPAddress,
	                                CommunicationPort,
	                                SerialPort,
	                                SerialNumber,
	                                IsActive,
	                                IsRegister,
	                                TerminalIP,
	                                CreatedBy,
	                                ModifiedBy,
	                                CreatedTS,
	                                ModifiedTS,
	                                CommunicationPassword,
	                                CommunicationMode
	                                FROM Device WHERE 1=1 AND IsDelete<>1 ");
                #region Filters

                if (!string.IsNullOrEmpty(model.DeviceName))
                {
                    strSQL.AppendFormat(@" AND DeviceName=@DeviceName");
                }
                if (!string.IsNullOrEmpty(model.DeviceNumber))
                {
                    strSQL.AppendFormat(@" AND DeviceNumber=@DeviceNumber");
                }
                if (!string.IsNullOrEmpty(model.DeviceModelID))
                {
                    strSQL.AppendFormat(@" AND DeviceModelID=@DeviceModelID");
                }

                #endregion

                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@DeviceName", model.DeviceName);
                _parameters.Add("@DeviceNumber", model.DeviceNumber);
                _parameters.Add("@DeviceModelID", model.DeviceModelID);
                #endregion

                return await _dapperRepository.ExecuteQueryWithPagedListAsync<DeviceViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CreatedTS");
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> InsertIntoDeviceAsync(DeviceViewModel model)
        {
            var result = new AccountResult();
            if (_deviceRepository.TableNoTracking.Any(x => x.DeviceNumber == model.DeviceNumber && x.IsDelete == false))
            {
                result.Errors = new List<string> { "Device/Number " + model.DeviceName + " is already taken" };
                return result;
            }
            if (_deviceRepository.TableNoTracking.Any(x => x.IPAddress == model.IPAddress && x.IsDelete == false))
            {
                result.Errors = new List<string> { "IP address " + model.IPAddress + " is already taken" };
                return result;
            }
            if (_deviceRepository.TableNoTracking.Any(x => x.DeviceName == model.DeviceName && x.IsDelete == false))
            {
                result.Errors = new List<string> { "DeviceName " + model.DeviceName + " is already taken" };
                return result;
            }
            var newDevice = new Device()
            {
                DeviceName = model.DeviceName,
                DeviceModelID = model.DeviceModelID,
                DeviceNumber = model.DeviceNumber,
                IPAddress = model.IPAddress,
                CommunicationPort = model.CommunicationPort,
                SerialPort = model.SerialPort,
                SerialNumber = model.SerialNumber,
                IsActive = model.IsActive,
                IsRegister = model.IsRegister,
                TerminalIP = ":",
                CommunicationPassword = model.CommunicationPassword,
                CommunicationMode = model.CommunicationMode,
                Branch=model.Branch,
                CreatedBy = model.CreatedBy,
                CreatedTS = DateTime.UtcNow
            };
            _deviceRepository.Insert(newDevice);
            await _deviceRepository.SaveChangesAsync();
            return result;
        }

        public Device GetDeviceByID(int DeviceID)
        {
            return _deviceRepository.Table.FirstOrDefault(x =>x.DeviceID == DeviceID);
        }

        public async Task<AccountResult> UpdateDeviceAsync(DeviceViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_deviceRepository.TableNoTracking.Any(x => x.DeviceNumber == model.DeviceNumber && x.DeviceID!=model.DeviceID && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "Device/Number " + model.DeviceName + " is already taken" };
                    return result;
                }
                if (_deviceRepository.TableNoTracking.Any(x => x.IPAddress == model.IPAddress && x.DeviceID!=model.DeviceID && x.IsDelete == false))
                {
                    result.Errors = new List<string> { "IP address " + model.IPAddress + " is already taken" };
                    return result;
                }
                var ExistedDevice = GetDeviceByID(model.DeviceID);
                if (ExistedDevice != null)
                {
                    ExistedDevice.DeviceName = model.DeviceName;
                    ExistedDevice.DeviceModelID = Convert.ToInt32(model.DeviceModelID);
                    ExistedDevice.DeviceNumber =Convert.ToInt32(model.DeviceNumber);
                    ExistedDevice.IPAddress = model.IPAddress;
                    ExistedDevice.CommunicationPort = model.CommunicationPort;
                    ExistedDevice.SerialPort = model.SerialPort;
                    ExistedDevice.SerialNumber = model.SerialNumber;
                    ExistedDevice.IsActive = model.IsActive;
                    ExistedDevice.IsRegister = model.IsRegister;
                    ExistedDevice.TerminalIP = model.TerminalIP;
                    ExistedDevice.CommunicationPassword = model.CommunicationPassword;
                    ExistedDevice.CommunicationMode = model.CommunicationMode;
                    ExistedDevice.Branch = model.Branch;
                    ExistedDevice.ModifiedBy = model.ModifiedBy;
                    ExistedDevice.ModifiedTS = DateTime.UtcNow;
                    _deviceRepository.Update(ExistedDevice);
                    await _deviceRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Device does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteDeviceAsync(int DeviceID)
        {
            var result = new AccountResult();
            var ExistedDevice = GetDeviceByID(DeviceID);
            if (ExistedDevice != null)
            {
                var DeviceList = await _employeeRepository.TableNoTracking.Where(x => x.DeviceNumber == ExistedDevice.DeviceNumber.ToString()).ToListAsync();
                if (DeviceList.Count() > 0)
                {
                    result.Errors = new List<string> { "Device existed on Employee." };
                }else
                {
                    _deviceRepository.Delete(ExistedDevice);
                }
             await _deviceRepository.SaveChangesAsync();
            }
            else
               {
                    result.Errors = new List<string> { "Device does not exist." };
                    return result;
               }
            return result;
        }
        public async Task<DeviceViewModel> GetDeviceByIDAsync(int DeviceID)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                DeviceID,
                                    Branch,
	                                DeviceModelID,
	                                DeviceName,
	                                DeviceNumber,
	                                IPAddress,
	                                CommunicationPort,
	                                SerialPort,
	                                SerialNumber,
	                                IsActive,
	                                IsRegister,
	                                TerminalIP,
	                                CreatedBy,
	                                ModifiedBy,
	                                CreatedTS,
	                                ModifiedTS,
	                                CommunicationPassword,
	                                CommunicationMode
	                                FROM Device
                                    WHERE DeviceID=@DeviceID");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@DeviceID", DeviceID);
                return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<DeviceViewModel>(strSQL.ToString(), _parameters);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private bool TelnetCommand(string IP, int PortNumber)
        {
            bool status = false;
            TcpClient tcpClient = null;

            try
            {
                IPAddress iPAddress = IPAddress.Parse(IP);
                tcpClient = new TcpClient();
                tcpClient.Connect(iPAddress, PortNumber);
                if (tcpClient.Connected)
                    status= true;
                tcpClient.Close();
            }
            catch
            {
                status = false;
            }
            finally
            {
                tcpClient.Close();
            }
            return status;
        
    }

        public async Task<List<DeviceViewModel>> GetDeviceDetailsWithStatus()
        {
            var result = new List<DeviceViewModel>();
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT [DeviceID]
                                     ,[DeviceModelID]
                                     ,[DeviceName]
                                     ,[DeviceNumber]
                                     ,[IPAddress]
                                     ,[CommunicationPort]
                                     ,[SerialPort]
                                     ,[SerialNumber]
                                     ,[IsActive]
                                     ,[IsRegister]
                                     ,[TerminalIP]
                                     ,[CommunicationPassword]
                                     ,[CommunicationMode]
                                 FROM [dbo].[Device]");
            var List = await _dapperRepository.ExecuteQueryAsync<DeviceViewModel>(strSQL.ToString(), null);
            if (List.Count() > 0)
            {
                foreach(var device in List)
                {
                    bool Status = TelnetCommand(device.IPAddress, device.CommunicationPort);
                    var data = new DeviceViewModel()
                    {
                        DeviceID = device.DeviceID,
                        DeviceModelID = device.DeviceModelID,
                        DeviceNumber = device.DeviceNumber,
                        DeviceName = device.DeviceName,
                        CommunicationPassword = device.CommunicationPassword,
                        CommunicationPort = device.CommunicationPort,
                        IPAddress = device.IPAddress,
                        DeviceStatus = Status
                    };
                    result.Add(data);
                }
            }
            return result;
        }
        public async Task<IList<SelectItemIntViewModel>> DDLDeviceListAsync()
        {
            return await _deviceRepository.Table.Where(x => x.IsActive == true)
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.DeviceID,
                              Value = x.DeviceNumber.ToString()
                          }).ToListAsync();
        }
        public async Task<IList<SelectItemIntViewModel>> DDLDeviceModelListAsync()
        {
            return await _deviceModelRepository.Table.Where(x => x.IsActive == true)
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.DeviceModelID,
                              Value = x.DeviceModelName.ToString()
                          }).ToListAsync();
        }
    }
}
