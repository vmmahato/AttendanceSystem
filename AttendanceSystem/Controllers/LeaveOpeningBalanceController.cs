using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.BaseController;
using AttendanceSystem.Service;
using AttendanceSystem.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LeaveOpeningBalanceController : BaseApiController
    {
        protected ILeaveOpeningBalanceService _leaveOpeningBalanceService;
        public LeaveOpeningBalanceController(ILeaveOpeningBalanceService leaveOpeningBalanceService)
        {
            _leaveOpeningBalanceService = leaveOpeningBalanceService;
        }
        [HttpPost("GetEmployeeLeaveTypeListAsync")]
        public async Task<IActionResult> GetEmployeeLeaveTypeListAsync([FromBody]EmployeeLeaveList model)
        {
            model.FiscalYearID = CurrentUserDetails.FiscalYearID;
            var result = new List<EmployeeCodeValueViewModel>();
            var LeaveOpeningBalance = await _leaveOpeningBalanceService.GetEmployeeLeaveTypeListAsync(model);
            if (LeaveOpeningBalance == null) return Ok();
            var GroupedLeaveOpeningBalance = LeaveOpeningBalance.GroupBy(x => new { x.EmployeeID, x.FirstName, x.LastName,x.Gender })
                .Select(x => new EmployeeWithLeaveTypeList()
                {
                    EmployeeID = x.Key.EmployeeID,
                    FirstName = x.Key.FirstName,
                    LastName = x.Key.LastName,
                    Gender=x.Key.Gender,
                    LeaveCodeDetails = x.Select(y => new CodeValueViewModel()
                    {
                        LeaveCode = y.LeaveCode,
                        Value = y.Value,
                        ApplicableGender=y.ApplicableGender,
                        Gender=y.Gender
                    }).ToList()
                }).ToList();
            var CodeList = await _leaveOpeningBalanceService.GetLeaveCodeList(model.FiscalYearID);

           if (GroupedLeaveOpeningBalance.Count() > 0)
               {
               foreach(var employee in GroupedLeaveOpeningBalance)
                    {
                    string CurrentName = string.Empty;
                    CurrentName = employee.FirstName +" "+ employee.LastName;
                    if (CodeList.Count() > 0)
                    {
                        foreach (var code in CodeList)
                        {
                            bool Show = false;
                            if(code.Value==employee.Gender || code.Value == "A")
                            {
                                Show = true;
                            }
                            else
                            {
                                Show = false;
                            }
                            if (employee.LeaveCodeDetails.Count() > 0)
                            {
                                var currentCodeValue = employee.LeaveCodeDetails.FirstOrDefault(x => x.LeaveCode == code.ID);
                                if (currentCodeValue != null)
                                {
                                    result.Add(new EmployeeCodeValueViewModel()
                                    {
                                        EmployeeID= employee.EmployeeID,
                                        Name = CurrentName,
                                        LeaveCode = code.ID,
                                        Value = currentCodeValue.Value,
                                        Show = Show,
                                        ApplicableGender=code.Value,
                                        Gender=employee.Gender
                                    });
                                }
                                else
                                {
                                    result.Add(new EmployeeCodeValueViewModel()
                                    {
                                        EmployeeID = employee.EmployeeID,
                                        Name = CurrentName,
                                        LeaveCode = code.ID,
                                        Value = null,
                                        Show = Show,
                                        ApplicableGender = code.Value,
                                        Gender = employee.Gender
                                    });
                                }
                            }
                            else
                            {
                                result.Add(new EmployeeCodeValueViewModel()
                                {
                                    EmployeeID = employee.EmployeeID,
                                    Name = CurrentName,
                                    LeaveCode = code.ID,
                                    Value = null,
                                    Show= Show,
                                    ApplicableGender = code.Value,
                                    Gender = employee.Gender
                                });
                            }
                        }
                    }
                
                }
            }
            result = result.GroupBy(x => new { x.EmployeeID, x.Name })
                .Select(x => new EmployeeCodeValueViewModel()
                {
                    EmployeeID = x.Key.EmployeeID,
                    Name = x.Key.Name,
                    List = x.Select(y=>new CodeValueViewModel()
                    {
                        LeaveCode=y.LeaveCode,
                        Value=y.Value,
                        Show=y.Show,
                        ApplicableGender=y.ApplicableGender,
                        Gender=y.Gender
                    }).ToList()
                }).ToList();
            return Ok(new { result = result, CodeList = CodeList });
        }

        [HttpPost("UpdateLeaveOpeningBalanceAsync")]
        public async Task<IActionResult> UpdateLeaveOpeningBalanceAsync([FromBody]EmployeeWithYearLeaveList model)
        {
            model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            var result = await _leaveOpeningBalanceService.UpdateLeaveOpeningBalanceAsync(model,model.CreatedBy);
            return Ok(result);
        }
    }
}