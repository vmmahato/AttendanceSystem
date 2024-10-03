using Core.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace AttendanceSystem.Services.Helpers.Excel
{
    public interface IExcelHelper : IService
    {
        MemoryStream ListToExcelStream(IDictionary<string, dynamic> sheetList);
        MemoryStream ListToExcelStream<T>(IList<T> records);
        void ListToExcel<T>(List<T> list, string filePath);
    }
}
