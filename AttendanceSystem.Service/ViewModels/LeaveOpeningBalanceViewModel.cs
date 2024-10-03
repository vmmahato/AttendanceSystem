using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class LeaveOpeningBalanceViewModel
    {
        public int LeaveOpeningBalanceID { get; set; }
        public int EmployeeID { get; set; }
        public string Type { get; set; }
        public decimal? Value { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class LeaveOpeningBalanceSearchViewModel : BaseOrderSearch
    {
        public int DepartmentID { get; set; }
        public int SectionID { get; set; }
        public int EmployeeID { get; set; }
    }

    public class EmployeeWithLeaveTypeList
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LeaveCode { get; set; }
        public decimal? Value { get; set; }
        public string ApplicableGender { get; set; }
        public string Gender { get; set; }
        public List<CodeValueViewModel> List { get; set; }
        public List<CodeValueViewModel> LeaveCodeDetails { get; set; }
    }

    public class CodeValueViewModel
    {
        public string LeaveCode { get; set; }
        public decimal? Value { get; set; }
        public string ApplicableGender { get; set; }
        public string Gender { get; set; }
        public bool Show { get; set; }
    }


    public class EmployeeCodeValueViewModel
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string LeaveCode { get; set; }
        public decimal? Value { get; set; }
        public bool Show { get; set; }
        public string ApplicableGender { get; set; }
        public string Gender { get; set; }
        public List<CodeValueViewModel> List { get; set; }
    }
    public class EmployeeWithYearLeaveList
    {
        public int CreatedBy { get; set; }
        public int FiscalYear { get; set; }
        public List<EmployeeWithLeaveTypeList> EmployeeList { get; set; }
    }

    public class EmployeeLeaveList
    {
        public int FiscalYearID { get; set; }
        public List<int> EmployeeIDs { get; set; }
    }

}
