using AttendanceSystem.DapperServices;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.Helpers;
using AttendanceSystem.PageExtension;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Service
{
    public class WorkShiftService : IWorkShiftService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<WorkShift> _workShiftRepository;
        private IUnitOfWorkManager _unitOfWork;
        private IGenericRepository<WorkShiftType> _workShiftTypeRepository;
        private IGenericRepository<FixedRoster> _fixedRoster;
        private IGenericRepository<WeeklyRoster> _weeklyRoster;
        private IGenericRepository<DynamicRoster> _dynamicRoster;
        public WorkShiftService(IDapperRepository dapperRepository,
                                IGenericRepository<WorkShift> workShiftRepository,
                                IUnitOfWorkManager unitOfWork,
                                IGenericRepository<WorkShiftType> workShiftTypeRepository,
                                IGenericRepository<FixedRoster> fixedRoster,
                                IGenericRepository<WeeklyRoster> weeklyRoster,
                                IGenericRepository<DynamicRoster> dynamicRoster)
        {
            _dapperRepository = dapperRepository;
            _workShiftRepository = workShiftRepository;
            _unitOfWork = unitOfWork;
            _workShiftTypeRepository = workShiftTypeRepository;
            _fixedRoster = fixedRoster;
            _weeklyRoster = weeklyRoster;
            _dynamicRoster = dynamicRoster;
        }
        public async Task<AccountResult> DeleteWorkShiftAsync(int WorkShiftID)
        {
            var result = new AccountResult();
            var ExistedWorkShift = GetWorkShiftByID(WorkShiftID);
            if (ExistedWorkShift != null)
            {
                var DynamicShiftList = await _dynamicRoster.TableNoTracking.Where(x => x.ShiftID == ExistedWorkShift.ShiftID).ToListAsync();
                var WeeklyShiftList = await _weeklyRoster.TableNoTracking.Where(x => x.ShiftID == ExistedWorkShift.ShiftID).ToListAsync();
                var FixedShiftList = await _fixedRoster.TableNoTracking.Where(x => x.ShiftID == ExistedWorkShift.ShiftID).ToListAsync();
                if (DynamicShiftList.Count() > 0 || WeeklyShiftList.Count() > 0 || FixedShiftList.Count() > 0)
                {
                    result.Errors = new List<string> { "Shift existed on Roster." };
                }
                else
                {
                    _workShiftRepository.Delete(ExistedWorkShift);
                }
                await _workShiftRepository.SaveChangesAsync();

                await DeleteShiftTypeList(WorkShiftID);
            }
            else
            {
                result.Errors = new List<string> { "Work Shift does not exist." };
                return result;
            }
            return result;
        }

        public WorkShift GetWorkShiftByID(int WorkShiftID)
        {
            return _workShiftRepository.Table.FirstOrDefault(x => x.ShiftID == WorkShiftID);
        }

        /// <summary>
        /// Call both at same time from controller GetWorkShiftByIDAsync, GetWorkShiftTypeByIDAsync
        /// </summary>
        /// <param name="WorkShiftID"></param>
        /// <returns></returns>
        public async Task<WorkShiftListViewModel> GetWorkShiftByIDAsync(int WorkShiftID)
        {
            var strSQl = new StringBuilder();
            strSQl.AppendFormat(@"SELECT [ShiftID]
                                  ,[ShiftCode]
                                  ,[ShiftName]
                                  ,[ShiftStart]
                                  ,[ShiftEnd]
                                  ,[BeginingIn]
                                  ,[EndingIn]
                                  ,[BeginingOut]
                                  ,[EndingOut]
                                  ,[LateIn]
                                  ,[LeaveEarly]
                                  ,[IsMustCheckIn]
                                  ,[IsMustCheckOut]
                                  ,[IsLateIn]
                                  ,[IsEarlyLeave]
                              FROM [dbo].[WorkShift] WHERE ShiftID=@ShiftID");
            #region Filter
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@ShiftID", WorkShiftID);
            #endregion
           return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<WorkShiftListViewModel>(strSQl.ToString(), _parameters);
        }

        public async Task<IEnumerable<GetWorkShiftTypeViewModel>> GetWorkShiftTypeByIDAsync(int WorkShiftID)
        {
            var strSQl = new StringBuilder();
            strSQl.AppendFormat(@"SELECT [ShiftTypeID]
                                        ,[ShiftID]
                                        ,[Name]
                                        ,[StartTime]
                                        ,[EndTime]
                                        ,[Count]
                                        ,[Duration]
                                        ,[CreatedTS]
                                        ,[CreatedBy]
                                        ,[ModifiedTS]
                                        ,[ModifiedBy]
                                    FROM [dbo].[WorkShiftType] WHERE ShiftID=@ShiftID");
            #region Filter
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@ShiftID", WorkShiftID);
            #endregion
            return await _dapperRepository.ExecuteQueryAsync<GetWorkShiftTypeViewModel>(strSQl.ToString(), _parameters);
        }

        public async Task<AccountResult> InsertIntoWorkShiftAsync(WorkShiftViewModel model)
        {
            var result = new AccountResult();
            if (_workShiftRepository.TableNoTracking.Any(x => x.ShiftCode == model.ShiftCode && x.ShiftName == model.ShiftName && x.IsDelete == false))
            {
                result.Errors = new List<string> { "ShiftName or Code " + model.ShiftName+","+ model.ShiftCode + " is already taken" };
                return result;
            }
            if (_workShiftRepository.TableNoTracking.Any(x => x.ShiftName == model.ShiftName && x.IsDelete == false))
            {
                result.Errors = new List<string> { "ShiftName " + model.ShiftName + " is already taken" };
                return result;
            }
            if (_workShiftRepository.TableNoTracking.Any(x => x.ShiftCode == model.ShiftCode && x.IsDelete == false))
            {
                result.Errors = new List<string> { "ShiftCode " + model.ShiftCode + " is already taken" };
                return result;
            }
            if (model.ShiftStart == null)
            {
                result.Errors = new List<string> { "ShiftStart is mandatory" };
                return result;
            }
            if (model.ShiftEnd == null)
            {
                result.Errors = new List<string> { "ShiftEnd is mandatory" };
                return result;
            }
            List<WorkShiftType> ShiftList = new List<WorkShiftType>();
            var data = new WorkShift()
            {
                ShiftCode = model.ShiftCode,
                BeginingIn = SharedServices.ConvertStringToTimeSpan(model.BeginingIn),
                BeginingOut = SharedServices.ConvertStringToTimeSpan(model.BeginingOut),
                EndingIn = SharedServices.ConvertStringToTimeSpan(model.EndingIn),
                EndingOut = SharedServices.ConvertStringToTimeSpan(model.EndingOut),
                ShiftStart = SharedServices.ConvertStringToTimeSpan(model.ShiftStart),
                ShiftEnd = SharedServices.ConvertStringToTimeSpan(model.ShiftEnd),
                IsEarlyLeave = model.IsEarlyLeave,
                IsLateIn = model.IsLateIn,
                IsMustCheckIn = model.IsMustCheckIn,
                IsMustCheckOut = model.IsMustCheckOut,
                LateIn = SharedServices.ConvertStringToTimeSpan(model.LateIn),
                LeaveEarly = model.LeaveEarly,
                ShiftName = model.ShiftName,
                CreatedBy = model.CreatedBy,
                CreatedTS = DateTime.UtcNow
            };
            using (var uow = _unitOfWork.NewUnitOfWork())
            {
                try
                {
                    _workShiftRepository.Insert(data);
                    await _workShiftRepository.SaveChangesAsync();
                    if (model.ShiftList.Count() > 0)
                    {
                        foreach (var shift in model.ShiftList)
                        {
                            var dataToInsert = new WorkShiftType()
                            {
                                ShiftID = data.ShiftID,
                                Name = shift.Name,
                                StartTime = SharedServices.ConvertStringToTimeSpan(shift.StartTime),
                                EndTime = SharedServices.ConvertStringToTimeSpan(shift.EndTime),
                                Count = shift.Count,
                                Duration = shift.Duration,
                                CreatedBy = model.CreatedBy,
                                CreatedTS = DateTime.UtcNow
                            };
                            ShiftList.Add(dataToInsert);
                        }
                    await    _workShiftTypeRepository.BulkInsertAsync(ShiftList);
                        await _workShiftTypeRepository.SaveChangesAsync();
                    }
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

        public async Task<AccountResult> UpdateWorkShiftAsync(WorkShiftViewModel model)
        {
            var result = new AccountResult();
            if (_workShiftRepository.TableNoTracking.Any(x => x.ShiftCode == model.ShiftCode && x.ShiftName == model.ShiftName && x.ShiftID!=model.ShiftID && x.IsDelete == false))
            {
                result.Errors = new List<string> { "ShiftName or Code " + model.ShiftName + "," + model.ShiftCode + " is already taken" };
                return result;
            }
            if (_workShiftRepository.TableNoTracking.Any(x => x.ShiftName == model.ShiftName && x.ShiftID != model.ShiftID && x.IsDelete == false))
            {
                result.Errors = new List<string> { "ShiftName " + model.ShiftName + " is already taken" };
                return result;
            }
            if (_workShiftRepository.TableNoTracking.Any(x => x.ShiftCode == model.ShiftCode && x.ShiftID != model.ShiftID && x.IsDelete == false))
            {
                result.Errors = new List<string> { "ShiftCode " + model.ShiftCode + " is already taken" };
                return result;
            }
            List<WorkShiftType> ShiftList = new List<WorkShiftType>();
            var ExistedData = GetWorkShiftByID(model.ShiftID);
            if (ExistedData != null)
            {
                ExistedData.ShiftCode = model.ShiftCode;
                ExistedData.BeginingIn = SharedServices.ConvertStringToTimeSpan(model.BeginingIn);
                ExistedData.BeginingOut = SharedServices.ConvertStringToTimeSpan(model.BeginingOut);
                ExistedData.EndingIn = SharedServices.ConvertStringToTimeSpan(model.EndingIn);
                ExistedData.EndingOut = SharedServices.ConvertStringToTimeSpan(model.EndingOut);
                ExistedData.ShiftStart = SharedServices.ConvertStringToTimeSpan(model.ShiftStart);
                ExistedData.ShiftEnd = SharedServices.ConvertStringToTimeSpan(model.ShiftEnd);
                ExistedData.IsEarlyLeave = model.IsEarlyLeave;
                ExistedData.IsLateIn = model.IsLateIn;
                ExistedData.IsMustCheckIn = model.IsMustCheckIn;
                ExistedData.IsMustCheckOut = model.IsMustCheckOut;
                ExistedData.LateIn = SharedServices.ConvertStringToTimeSpan(model.LateIn);
                ExistedData.LeaveEarly = model.LeaveEarly;
                ExistedData.ShiftName = model.ShiftName;
                ExistedData.ModifiedBy = model.CreatedBy;
                ExistedData.ModifiedTS = DateTime.UtcNow;
                using (var uow = _unitOfWork.NewUnitOfWork())
                {
                    try
                    {
                        await _workShiftRepository.SaveChangesAsync();
                        if (model.ShiftList.Count() > 0)
                        {
                            var response = await DeleteShiftTypeList(model.ShiftID);
                            if (response.Success)
                            {

                                foreach (var shift in model.ShiftList)
                                {
                                    var dataToInsert = new WorkShiftType()
                                    {
                                        ShiftID = ExistedData.ShiftID,
                                        Name = shift.Name,
                                        StartTime = SharedServices.ConvertStringToTimeSpan(shift.StartTime),
                                        EndTime = SharedServices.ConvertStringToTimeSpan(shift.EndTime),
                                        Count = shift.Count,
                                        Duration = shift.Duration,
                                        CreatedBy = model.CreatedBy,
                                        CreatedTS = DateTime.UtcNow
                                    };
                                    ShiftList.Add(dataToInsert);
                                }
                                await _workShiftTypeRepository.BulkInsertAsync(ShiftList);
                                await _workShiftTypeRepository.SaveChangesAsync();
                            }
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

        private async Task<AccountResult> DeleteShiftTypeList(int ShiftID)
        {
            var result = new AccountResult();
            var DynamicShiftList = await _dynamicRoster.TableNoTracking.Where(x => x.ShiftID == ShiftID).ToListAsync();
            var WeeklyShiftList = await _weeklyRoster.TableNoTracking.Where(x => x.ShiftID == ShiftID).ToListAsync();
            var FixedShiftList = await _fixedRoster.TableNoTracking.Where(x => x.ShiftID == ShiftID).ToListAsync();
            if (DynamicShiftList.Count() > 0 || WeeklyShiftList.Count() > 0 || FixedShiftList.Count() > 0)
            {
                result.Errors = new List<string> { "Shift existed on Roster." };
                return result;
            }
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"DELETE FROM [dbo].[WorkShiftType] WHERE ShiftID=@ShiftID");

            #region Filter
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@ShiftID", ShiftID);
            #endregion

          await  _dapperRepository.ExecuteAsync(strSQL.ToString(),_parameters);
            return result;
        }
        public async Task<IPagedList<WorkShiftListViewModel>> WorkShiftListAsync(WorkShiftSearchViewModel model)
        {
            var strSQl = new StringBuilder();
            strSQl.AppendFormat(@"SELECT [ShiftID]
                                  ,[ShiftCode]
                                  ,[ShiftName]
                                  ,[ShiftStart]
                                  ,[ShiftEnd]
                                  ,[BeginingIn]
                                  ,[EndingIn]
                                  ,[BeginingOut]
                                  ,[EndingOut]
                                  ,[LateIn]
                                  ,[LeaveEarly]
                                  ,[IsMustCheckIn]
                                  ,[IsMustCheckOut]
                                  ,[IsLateIn]
                                  ,[IsEarlyLeave]
                              FROM [dbo].[WorkShift] WHERE 1=1 AND IsDelete<>1 ");
            #region Filter
            if (model.ShiftID > 0)
            {
                strSQl.AppendFormat(@" AND ShiftID=@ShiftID");
            }
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@ShiftID", model.ShiftID);
            #endregion
            try
            {
                return await _dapperRepository.ExecuteQueryWithPagedListAsync<WorkShiftListViewModel>(strSQl.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "ShiftCode");
            }
            catch (Exception e)
            {

                throw e;
            }
          
        }
    }
}
