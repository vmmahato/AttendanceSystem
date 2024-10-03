using AttendanceSystem.PageList;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class CompanyProfileSearchViewModel : BaseOrderSearch
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string PanNumber { get; set; }
    }
    public class CompanyProfileViewModel
    {
        public int CountIndex { get; set; }
        public int CompanyProfileID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string ContactPerson { get; set; }
        public string BranchName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string PanNumber { get; set; }
        public IFormFile CompanyImage { get; set; }
        public string CompanyFolderURL { get; set; }
        public string CompanyImageURL { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
