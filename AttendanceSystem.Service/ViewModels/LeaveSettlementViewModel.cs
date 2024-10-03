using AttendanceSystem.PageList;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class LeaveSettlementSearchViewModel : BaseOrderSearch
    {
        public string Year { get; set; }
        public string EmployeeName { get; set; }
    }
    public class LeaveSettlementViewModel
    {
        public int CountIndex { get; set; }
        public int SettlementID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int LeaveID { get; set; }
        public string LeaveName { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? LeaveTaken { get; set; }
        public decimal? RemainingLeave { get; set; }
        public decimal? CarryToNext { get; set; }
        public decimal? Pay { get; set; }
        public decimal? SettlingLeave { get; set; }
        public int FiscalYear { get; set; }
        public string FiscalYears { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class SettlementViewModel
    {
        public int OpeningBalance { get; set; }
        public int LeaveTaken { get; set; }
        public int RemainingLeave { get; set; } 
        public bool IsLeaveCarryable { get; set; }
    }
    public class SettlementIDsViewModel
    {
        public int EmployeeID { get; set; }
        public int LeaveID { get; set; }
    }
}
