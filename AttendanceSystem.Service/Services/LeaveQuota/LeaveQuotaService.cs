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

namespace AttendanceSystem.Services
{
    public class LeaveQuotaService : ILeaveQuotaService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<LeaveQuota> _leaveQuotaRepository;
        public LeaveQuotaService(IGenericRepository<LeaveQuota> leaveQuotaRepository,
                            IDapperRepository dapperRepository)
        {
            _leaveQuotaRepository = leaveQuotaRepository;
            _dapperRepository = dapperRepository;
        }

        public async Task<IEnumerable<LeaveQuotaViewModel>> LeaveQuotaListAsync(int DesignationID)
        {
            try
            {
                #region Parameters
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@DesignationID", DesignationID);
                #endregion
                return await _dapperRepository.ExecuteStoredProcAsync<LeaveQuotaViewModel>("Spa_LeaveQuota", _parameters);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> InsertIntoLeaveQuotaAsync(List<LeaveQuotaViewModel> model, int UserID,int FiscalYear)
        {
            try
            {
                var result = new AccountResult();
                var ListOfLeaveQuota = new List<LeaveQuota>();
                foreach(var Leave in model)
                {
                    var ExistedLeaveQuota = GetLeaveQuotaByID(Leave.LeaveID, Leave.DesignationID);
                    if (ExistedLeaveQuota==null)
                    {
                        var LeaveQuota = new LeaveQuota()
                        {
                            DesignationID = Leave.DesignationID,
                            LeaveID = Leave.LeaveID,
                            LeaveType = Leave.LeaveType,
                            LeaveBalance = Leave.LeaveBalance,
                            LeaveIncrementPeroid = Leave.LeaveIncrementPeroid,
                            ApplicableGender = Leave.ApplicableGender,
                            IsPaidLeave = Leave.IsPaidLeave,
                            IsLeaveCarryable = Leave.IsLeaveCarryable,
                            IsReplacementLeave = Leave.IsReplacementLeave,
                            FiscalYear=Leave.FiscalYear,
                            CreatedBy = UserID,
                            CreatedTS = DateTime.UtcNow
                        };
                        _leaveQuotaRepository.Insert(LeaveQuota);
                        await _leaveQuotaRepository.SaveChangesAsync();
                    }
                    else
                    {
                        ExistedLeaveQuota.DesignationID = Leave.DesignationID;
                        ExistedLeaveQuota.LeaveID = Leave.LeaveID;
                        ExistedLeaveQuota.LeaveType = Leave.LeaveType;
                        ExistedLeaveQuota.LeaveBalance = Leave.LeaveBalance;
                        ExistedLeaveQuota.LeaveIncrementPeroid = Leave.LeaveIncrementPeroid;
                        ExistedLeaveQuota.ApplicableGender = Leave.ApplicableGender;
                        ExistedLeaveQuota.IsPaidLeave = Leave.IsPaidLeave;
                        ExistedLeaveQuota.IsLeaveCarryable = Leave.IsLeaveCarryable;
                        ExistedLeaveQuota.IsReplacementLeave = Leave.IsReplacementLeave;
                        ExistedLeaveQuota.FiscalYear = Leave.FiscalYear;
                        ExistedLeaveQuota.ModifiedBy = UserID;
                        ExistedLeaveQuota.ModifiedTS = DateTime.UtcNow;
                        _leaveQuotaRepository.Update(ExistedLeaveQuota);
                        await _leaveQuotaRepository.SaveChangesAsync();
                    }
                    
                }
               
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public LeaveQuota GetLeaveQuotaByID(int LeaveID, int DesignationID)
        {
            return _leaveQuotaRepository.Table.FirstOrDefault(x =>x.LeaveID == LeaveID && x.DesignationID == DesignationID);
        }
    }
}
