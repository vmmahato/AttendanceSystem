using AttendanceSystem.DapperServices;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.PageExtension;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Service
{
    public class EmployeeService : IEmployeeService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<Employee> _employeeRepository;
        private ICommonService _commonService;
        private IUnitOfWorkManager _unitOfWork;
        public EmployeeService(IDapperRepository dapperRepository,
                               IGenericRepository<Employee> employeeRepository,
                               ICommonService commonService,
                               IUnitOfWorkManager unitOfWork)
        {
            _dapperRepository = dapperRepository;
            _employeeRepository = employeeRepository;
            _commonService = commonService;
            _unitOfWork = unitOfWork;
        }
        public async Task<AccountResult> DeleteEmployeeAsync(int EmployeeID, string ImagePath)
        {
            var result = new AccountResult();
            var ExistedEmployee = GetEmployeeByID(EmployeeID);
            if (ExistedEmployee != null)
            {
                _employeeRepository.Delete(ExistedEmployee);
                await _employeeRepository.SaveChangesAsync();

                string DirectoryPath = _commonService.GetFullPath(ImagePath + "\\" + ExistedEmployee.EmployeeID);
                _commonService.DeleteFilesFromDirectoryPath(DirectoryPath);
            }
            else
            {
                result.Errors = new List<string> { "FiscalYear does not exist." };
                return result;
            }
            return result;
        }

        public async Task<IPagedList<EmployeeModel>> EmployeeListAsync(EmployeeSearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT E.[EmployeeID]
                                          ,E.[EnrollID]
                                          ,E.[FirstName]
                                          ,E.[LastName]
                                          ,E.[DesignationID]
										  ,DG.DesignationName
                                          ,E.[Gender]
                                          ,cast(Year(E.[DateOfBirth]) as varchar(4))+'-'+
										  cast(FORMAT(E.[DateOfBirth],'MM') as varchar(2))+'-'+
										  cast(Right('00' + Convert(VarChar(2), Day(E.[DateOfBirth])), 2) as varchar(2))
										  [DateOfBirth]
                                          ,E.[DepartmentID]
										  ,D.DepartmentName
                                          ,E.[SectionID]
										  ,S.SectionName
                                          ,E.[Nationality]
                                          ,E.[PhoneNumber]
                                          ,cast(Year(E.[JoiningDate]) as varchar(4))+'-'+
										  cast(FORMAT(E.[JoiningDate],'MM') as varchar(2))+'-'+
										  cast(Right('00' + Convert(VarChar(2), Day(E.[JoiningDate])), 2) as varchar(2))
										  [JoiningDate]
                                          ,E.[Status]
                                          ,E.[TemporaryAddress]
                                          ,E.[PermanentAddress]
                                          ,E.[ProfileImageURL]
                                          ,E.[ActiveAccount]
                                          ,E.[CountOT]
                                          ,E.[RestOnHolidays]
                                          ,E.[CheckClockIn]
                                          ,E.[CheckClockOut]
                                          ,E.IsOTAllowed
                                          ,E.DeviceNumber
                                          ,E.Religion
                                          ,E.[CreatedTS]
                                          ,E.[CreatedBy]
                                          ,E.[ModifiedTS]
                                          ,E.[ModifiedBy]
                                          FROM [dbo].[Employee] E 
                                          LEFT OUTER JOIN Department D ON E.DepartmentID=D.DepartmentID
									      LEFT OUTER JOIN Designation DG ON E.DesignationID=DG.DesignationID
									      LEFT OUTER JOIN Section S ON E.SectionID=S.SectionID ");
            #region Filter
            DynamicParameters _parameters = new DynamicParameters();
            #endregion
            return await _dapperRepository.ExecuteQueryWithPagedListAsync<EmployeeModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CreatedTS");
        }

        public Employee GetEmployeeByID(int EmployeeID)
        {
            return _employeeRepository.Table.FirstOrDefault(x => x.EmployeeID == EmployeeID);
        }

        public async Task<EmployeeModel> GetEmployeeByIDAsync(int EmployeeID, string ImagePath)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT [EmployeeID]
                                          ,[EnrollID]
                                          ,[FirstName]
                                          ,[LastName]
                                          ,[DesignationID]
                                          ,[Gender]
                                          ,cast(Year([DateOfBirth]) as varchar(4))+'-'+
										  cast(FORMAT([DateOfBirth],'MM') as varchar(2))+'-'+
										  cast(Right('00' + Convert(VarChar(2), Day([DateOfBirth])), 2) as varchar(2))
										  [DateOfBirth]
                                          ,[DepartmentID]
                                          ,[SectionID]
                                          ,[Nationality]
                                          ,[PhoneNumber]
                                          ,cast(Year([JoiningDate]) as varchar(4))+'-'+
										  cast(FORMAT([JoiningDate],'MM') as varchar(2))+'-'+
										  cast(Right('00' + Convert(VarChar(2), Day([JoiningDate])), 2) as varchar(2))
										  [JoiningDate]
                                          ,[Status]
                                          ,[TemporaryAddress]
                                          ,[PermanentAddress]
                                          ,[ProfileImageURL]
                                          ,[ActiveAccount]
                                          ,[CountOT]
                                          ,[RestOnHolidays]
                                          ,[CheckClockIn]
                                          ,[CheckClockOut]
                                          ,IsOTAllowed
                                          ,DeviceNumber
                                          ,Religion
                                          ,[CreatedTS]
                                          ,[CreatedBy]
                                          ,[ModifiedTS]
                                          ,[ModifiedBy]
                                           FROM [dbo].[Employee] WHERE EmployeeID=@EmployeeID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@EmployeeID", EmployeeID);
            var response= await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<EmployeeModel>(strSQL.ToString(), _parameters);
            if (response != null)
            {
                //string DirectoryPath = _commonService.GetFullPath(ImagePath + "\\" + response.EmployeeID);
                response.ProfileImageURL =Path.Combine(ImagePath, response.EmployeeID.ToString(),response.ProfileImageURL);
            }

            return response;
        }

        public async Task<AccountResult> InsertIntoEmployeeAsync(EmployeeViewModel model)
        {
            var result = new AccountResult();
            if (_employeeRepository.TableNoTracking.Any(x => x.DeviceNumber == model.DeviceNumber && x.EnrollID==model.EnrollID))
            {
                result.Errors = new List<string> { "EnrollID already exist for selected device" };
                return result;
            }
            string EmployeeProfileImage = string.Empty;
            if (model.EmployeeImage != null)
            {
                var errorList = _commonService.CheckFiles(model.EmployeeImage);
                if (errorList.Any())
                {
                    result.Errors = errorList;
                    return result;
                }
            }
            var Employee = new Employee()
            {
                EnrollID = model.EnrollID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DesignationID = model.DesignationID,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                DepartmentID = model.DepartmentID,
                SectionID = model.SectionID,
                Nationality = model.Nationality,
                PhoneNumber = model.PhoneNumber,
                JoiningDate = model.JoiningDate,
                Status = model.Status,
                TemporaryAddress = model.TemporaryAddress,
                PermanentAddress = model.PermanentAddress,
                ProfileImageURL = EmployeeProfileImage,
                ActiveAccount = model.ActiveAccount,
                CountOT = model.CountOT,
                RestOnHolidays = model.RestOnHolidays,
                CheckClockIn = model.CheckClockIn,
                CheckClockOut = model.CheckClockOut,
                IsOTAllowed=model.IsOTAllowed,
                DeviceNumber=model.DeviceNumber,
                Religion=model.Religion,
                CreatedBy = model.CreatedBy,
                CreatedTS = DateTime.UtcNow
            };
            using (var uow = _unitOfWork.NewUnitOfWork())
            {
                try
                {
                    _employeeRepository.Insert(Employee);
                    await _employeeRepository.SaveChangesAsync();

                    string DirectoryPath = _commonService.GetFullPath(model.EmployeeImageFolerURL+"\\"+Employee.EmployeeID);
                    var FileList = _commonService.GetFiles(model.EmployeeImage, "Employee");
                    var EmployeeImageDetails = await _commonService.SaveFiles(FileList, DirectoryPath);
                    if (EmployeeImageDetails != null)
                    {
                        EmployeeProfileImage = EmployeeImageDetails.Employee;
                    }
                    Employee.ProfileImageURL = EmployeeProfileImage;
                  await  _employeeRepository.SaveChangesAsync();
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

        public async Task<AccountResult> UpdateEmployeeAsync(EmployeeViewModel model)
        {
            var result = new AccountResult();
            try
            {
                if (_employeeRepository.TableNoTracking.Any(x => x.DeviceNumber == model.DeviceNumber && x.EnrollID == model.EnrollID && x.EmployeeID != model.EmployeeID))
                {
                    result.Errors = new List<string> { "EnrollID already exist for selected device" };
                    return result;
                }
                string EmployeeProfileImage = string.Empty;
                var ExistedEmployee = GetEmployeeByID(model.EmployeeID);
                if (model.EmployeeImage != null)
                {
                    var errorList = _commonService.CheckFiles(model.EmployeeImage);
                    if (errorList.Any())
                    {
                        result.Errors = errorList;
                        return result;
                    }
                    string DirectoryPath = _commonService.GetFullPath(model.EmployeeImageFolerURL + "\\" + model.EmployeeID);
                    var FileList = _commonService.GetFiles(model.EmployeeImage, "Employee");
                    var EmployeeImageDetails = await _commonService.SaveFiles(FileList, DirectoryPath);
                    if (EmployeeImageDetails != null)
                    {
                        EmployeeProfileImage = EmployeeImageDetails.Employee;
                    }
                }
                else
                {
                    EmployeeProfileImage = ExistedEmployee.ProfileImageURL;
                }

                if (ExistedEmployee != null)
                {
                    ExistedEmployee.EnrollID = model.EnrollID;
                    ExistedEmployee.FirstName = model.FirstName;
                    ExistedEmployee.LastName = model.LastName;
                    ExistedEmployee.DesignationID = model.DesignationID;
                    ExistedEmployee.Gender = model.Gender;
                    ExistedEmployee.DateOfBirth = model.DateOfBirth;
                    ExistedEmployee.DepartmentID = model.DepartmentID;
                    ExistedEmployee.SectionID = model.SectionID;
                    ExistedEmployee.Nationality = model.Nationality;
                    ExistedEmployee.PhoneNumber = model.PhoneNumber;
                    ExistedEmployee.JoiningDate = model.JoiningDate;
                    ExistedEmployee.Status = model.Status;
                    ExistedEmployee.TemporaryAddress = model.TemporaryAddress;
                    ExistedEmployee.PermanentAddress = model.PermanentAddress;
                    ExistedEmployee.ProfileImageURL = EmployeeProfileImage;
                    ExistedEmployee.ActiveAccount = model.ActiveAccount;
                    ExistedEmployee.CountOT = model.CountOT;
                    ExistedEmployee.RestOnHolidays = model.RestOnHolidays;
                    ExistedEmployee.CheckClockIn = model.CheckClockIn;
                    ExistedEmployee.CheckClockOut = model.CheckClockOut;
                    ExistedEmployee.IsOTAllowed = model.IsOTAllowed;
                    ExistedEmployee.DeviceNumber = model.DeviceNumber;
                    ExistedEmployee.Religion = model.Religion;
                    ExistedEmployee.ModifiedBy = model.ModifiedBy;
                    ExistedEmployee.ModifiedTS = DateTime.UtcNow;
                    await _employeeRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Employee does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public async Task<IList<SelectItemIntViewModel>> DDLEmployeeListAsync()
        {
            return await _employeeRepository.Table
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.EmployeeID,
                              Value = x.FirstName+" "+x.LastName
                          }).ToListAsync();
        }
        public async Task<IList<SelectItemIntViewModel>> DDLDesignationWiseEmployeeListAsync(int DesignationID)
        {
            return await _employeeRepository.Table.Where(x=>x.DesignationID== DesignationID)
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.EmployeeID,
                              Value = x.FirstName + " " + x.LastName
                          }).ToListAsync();
        }
        public async Task<IList<SelectItemIntViewModel>> DepartmentWiseEmployeeListAsync(DepartmentAndSectionIdModel model)
        {
            var strSQL = new StringBuilder();
            string DepartmentIDs = string.Empty;
            DepartmentIDs = string.Join(",", model.DepartmentID.ToList());
            if (DepartmentIDs == "")
            {
                DepartmentIDs = "0";
            }
            strSQL.AppendFormat(@"SELECT 
                                EmployeeID ID,
                                FirstName+' '+LastName Value
                                FROM Employee WHERE 
                                DepartmentID IN (" + DepartmentIDs + ")");
            #region Filter
            DynamicParameters _parameters = new DynamicParameters();
            #endregion
            var result = await _dapperRepository.ExecuteQueryAsync<SelectItemIntViewModel>(strSQL.ToString(), _parameters);
            return result.ToList();
        }
        public async Task<IList<SelectItemIntViewModel>> SectionWiseEmployeeListAsync(DepartmentAndSectionIdModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                string SectionIDs = string.Empty;
                SectionIDs = string.Join(",", model.SectionID.ToList());
                if (SectionIDs=="")
                {
                    SectionIDs = "0";
                }
                strSQL.AppendFormat(@"SELECT 
                                EmployeeID ID,
                                FirstName+' '+LastName Value
                                FROM Employee WHERE 
                                SectionID IN (" + SectionIDs + ")");
                #region Filter
                DynamicParameters _parameters = new DynamicParameters();
                #endregion
                var result = await _dapperRepository.ExecuteQueryAsync<SelectItemIntViewModel>(strSQL.ToString(), _parameters);
                return result.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<IList<SelectItemsIntViewModel>> DepartmentAndSectionWiseEmployeeListAsync(DepartmentAndSectionIdModel model)
        {
            var strSQL = new StringBuilder();
            string SectionIDs = string.Empty;
            SectionIDs = string.Join(",", model.SectionID.ToList());
            if (string.IsNullOrEmpty(SectionIDs)) return new List<SelectItemsIntViewModel>();
            strSQL.AppendFormat(@"SELECT 
								DepartmentID,
                                EmployeeID ID,
                                FirstName+' '+LastName Value
                                FROM Employee WHERE SectionID IN (" + SectionIDs + ")");
          
            #region Filter
            DynamicParameters _parameters = new DynamicParameters();
            #endregion
            var result = await _dapperRepository.ExecuteQueryAsync<SelectItemsIntViewModel>(strSQL.ToString(), _parameters);
            return result.ToList();
        }
        public async Task<IList<SelectItemIntViewModel>> LeaveApplicantEmployeeListAsync(SectionDesignationIdModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                EmployeeID ID,
                                FirstName+' '+LastName Value
                                FROM Employee WHERE 1=1 ");
                #region Filter
                if (model.SectionID > 0)
                {
                    strSQL.AppendFormat(@" AND SectionID=@SectionID ");
                }
                if (model.DesignationID > 0)
                {
                    strSQL.AppendFormat(@" AND DesignationID=@DesignationID ");
                }
                if (model.DepartmentID > 0)
                {
                    strSQL.AppendFormat(@" AND DepartmentID=@DepartmentID ");
                }
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@SectionID", model.SectionID);
                _parameters.Add("@DesignationID", model.DesignationID);
                _parameters.Add("@DepartmentID", model.DepartmentID);
                #endregion
                var result = await _dapperRepository.ExecuteQueryAsync<SelectItemIntViewModel>(strSQL.ToString(), _parameters);
                return result.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<IList<SelectItemIntViewModel>> ManagerRoleEmployeeListAsync(SectionDesignationIdModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                    E.EmployeeID ID,
                                    E.FirstName+' '+E.LastName Value
                                    FROM Employee E JOIN Designation D 
								    ON E.DesignationID=D.DesignationID WHERE D.DesignationName='Manager' ");
                DynamicParameters _parameters = new DynamicParameters();
                var result = await _dapperRepository.ExecuteQueryAsync<SelectItemIntViewModel>(strSQL.ToString(), _parameters);
                return result.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
