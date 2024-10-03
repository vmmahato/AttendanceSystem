using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
  public  class FileUploadViewModel
    {
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
