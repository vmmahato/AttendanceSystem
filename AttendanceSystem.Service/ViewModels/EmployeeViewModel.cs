using AttendanceSystem.PageList;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
   public class EmployeeViewModel
    {
        public int EmployeeID { get; set; }
        public long EnrollID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Status { get; set; }
        public string TemporaryAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string ProfileImageURL { get; set; }
        public bool ActiveAccount { get; set; }
        public bool CountOT { get; set; }
        public bool RestOnHolidays { get; set; }
        public string CheckClockIn { get; set; }
        public string CheckClockOut { get; set; }
        public bool IsOTAllowed { get; set; }
        public string DeviceNumber { get; set; }
        public string Religion { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public IFormFile EmployeeImage { get; set; }
        public string EmployeeImageFolerURL { get; set; }
    }
    public class EmployeeModel
    {
        public int EmployeeID { get; set; }
        public long EnrollID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public string JoiningDate { get; set; }
        public string Status { get; set; }
        public string TemporaryAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string ProfileImageURL { get; set; }
        public bool ActiveAccount { get; set; }
        public bool CountOT { get; set; }
        public bool RestOnHolidays { get; set; }
        public string CheckClockIn { get; set; }
        public string CheckClockOut { get; set; }
        public bool IsOTAllowed { get; set; }
        public string DeviceNumber { get; set; }
        public string Religion { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public IFormFile EmployeeImage { get; set; }
        public string EmployeeImageFolerURL { get; set; }
    }

    public class EmployeeSearchViewModel: BaseOrderSearch
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
