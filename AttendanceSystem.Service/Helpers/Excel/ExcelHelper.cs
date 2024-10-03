using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using DataTable = System.Data.DataTable;

namespace AttendanceSystem.Services.Helpers.Excel
{
    public class ExcelHelper : IExcelHelper
    {
        private readonly ILogger<ExcelHelper> _logger;
        private Type[] numberTypes = new[] { typeof(int), typeof(Int16), typeof(long), typeof(int?), typeof(Int16?),
            typeof(long?), typeof(double), typeof(decimal), typeof(double?), typeof(decimal?), typeof(float), typeof(float?) };
        public ExcelHelper(ILogger<ExcelHelper> logger)
        {
            _logger = logger;
        
        }
        #region " Private Heavyweight functions "
        private void WriteExcel<T>(IList<T> records, SpreadsheetDocument spreadsheet, string ExcludeColumn)
        {
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            Stylesheet stylesheet = new Stylesheet();
            workbookStylesPart.Stylesheet = stylesheet;
            //  For each worksheet you want to create
            string workSheetID = "rId" + 1;
            string worksheetName = "Worksheet" + 1;

            WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();

            // create sheet data
            newWorksheetPart.Worksheet.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.SheetData());

            // save worksheet
            WriteToWorkSheet(records, newWorksheetPart, ExcludeColumn);
            newWorksheetPart.Worksheet.Save();

            // create the worksheet to workbook relation
            // if (worksheetNumber == 1)
            spreadsheet.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

            spreadsheet.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>().AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
            {
                Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                SheetId = 1,
                Name = worksheetName
            });

            spreadsheet.WorkbookPart.Workbook.Save();
        }
        private void WriteToWorkSheet<T>(IList<T> records, WorksheetPart worksheetPart, string ExcludeColumn)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();


            var props = typeof(T).GetProperties();//GetMembers().Where(x => x.MemberType == MemberTypes.Property);
                                                  //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
                                                  //
                                                  //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
                                                  //  cells of data, we'll know if to write Text values or Numeric cell values.

            //change made not to display unnecessary columns on excel sheet. Pass the columnname in comma separated form in ExcludeColumn parameter.

            //var ExcludeColumn = "OrganizationID,SalesPersonID";
            if (ExcludeColumn != null)
            {
                var ExcludeColumnList = ExcludeColumn.Split(',');
                foreach (var item in ExcludeColumnList)
                {
                    props = props.Where(x => x.Name != item).ToArray();

                }
            }


            int numberOfColumns = props.Count();
            bool[] IsNumericColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            var colOptions = new List<ColOption>();
            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;

            var headerRow = new Row { RowIndex = rowIndex, };  // add a row at the top of spreadsheet
            sheetData.Append(headerRow);
            int colIndex = 0;
            foreach (var prop in props)
            {
                var colOption = new ColOption();
                colOption.Type = GetCellValueType(prop);
                colOption.ExcelColumnName = excelColumnNames[colIndex];
                var browsableProp = prop.GetCustomAttribute(typeof(BrowsableAttribute), true) as BrowsableAttribute;
                // If the property is browsable then only show it in excel
                if (browsableProp == null || browsableProp.Browsable == true)
                {
                    string columnName = "";
                    // Check for different column Name
                    var displayNameProp = prop.GetCustomAttribute(typeof(DisplayNameAttribute), true) as DisplayNameAttribute;
                    if (displayNameProp != null)
                    {
                        columnName = displayNameProp.DisplayName;
                    }
                    else
                    {
                        columnName = prop.Name;
                    }
                    // Check for formatting
                    var formatStringProp = prop.GetCustomAttribute(typeof(DisplayFormatAttribute), true) as DisplayFormatAttribute;
                    if (formatStringProp != null)
                    {
                        colOption.DataFormatString = formatStringProp.DataFormatString;
                    }
                    AppendTextCell(excelColumnNames[colIndex] + "1", columnName, headerRow);
                    colIndex++;
                    colOption.Browsable = true;
                }
                else
                {
                    colOption.Browsable = false;
                }
                colOptions.Add(colOption);
            }
            //
            //  Now, step through each row of data in our DataTable...
            //
            foreach (var record in records)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);
                colIndex = 0;
                foreach (var prop in props)
                {
                    var colOption = colOptions[colIndex];

                    if (colOption.Browsable)
                    {
                        object valueObj = prop.GetValue(record);
                        string cellValue = valueObj == null ? "" : (string.IsNullOrEmpty(colOption.DataFormatString) == true ? valueObj.ToString() : string.Format(colOption.DataFormatString, valueObj));
                        //string cellValue = "";
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendCell(colOption.ExcelColumnName + rowIndex.ToString(), cellValue, newExcelRow, colOption.Type);
                    }
                    colIndex++;
                }
            }
        }
        #endregion


        #region " Private Helper functions "
        private string hexvaluesToRemove = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
        private void AppendCell(string cellReference, string cellStringValue, Row excelRow, CellValues dataType)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference, DataType = dataType };
            CellValue cellValue = new CellValue();

            if (string.IsNullOrEmpty(cellStringValue) == false)
            {
                cellStringValue = Regex.Replace(cellStringValue, hexvaluesToRemove, "");
            }
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private void AppendTextCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String };
            CellValue cellValue = new CellValue();
            if (string.IsNullOrEmpty(cellStringValue) == false)
            {
                cellStringValue = Regex.Replace(cellStringValue, hexvaluesToRemove, "");
            }
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private CellValues GetCellValueType(PropertyInfo property)
        {
            if (property.PropertyType == typeof(string))
            {
                return CellValues.String;
            }
            else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
            {
                //  return CellValues.Boolean; // Shows error when opening excel file
                return CellValues.String;
            }
            else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
            {
                return CellValues.String;
            }
            else if (numberTypes.Contains(property.PropertyType))
            {
                return CellValues.Number;
            }
            // If not found anything

            return CellValues.String;
        }
        private string GetExcelColumnName(int columnIndex, int offset = 0)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if ((columnIndex - offset) < 26)
                return ((char)('A' + (columnIndex - offset))).ToString();

            char firstChar = (char)('A' + ((columnIndex - offset) / 26) - 1);
            char secondChar = (char)('A' + ((columnIndex - offset) % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }
        private class ColOption
        {
            public CellValues Type = CellValues.String;
            public bool Browsable = true;
            public string ExcelColumnName = "";
            public string DataFormatString = "";
        }
        #endregion

        public MemoryStream ListToExcelStream(IDictionary<string, dynamic> sheetList)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                UInt32Value sheetCount = 1; // Mimimum value is 1

                foreach (var data in sheetList)
                {

                    // Add a WorksheetPart to the WorkbookPart
                    WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    // Append a new worksheet and associate it with the workbook.
                    Sheet sheet = new Sheet()
                    {
                        Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = sheetCount,
                        Name = data.Key
                    };
                    sheets.Append(sheet);

                    WriteToWorkSheet(data.Value, worksheetPart, null);
                    sheetCount++;
                }

                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();
            }
            return stream;
        }

        public MemoryStream ListToExcelStream<T>(IList<T> records)
        {
            MemoryStream stream = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
            {
                WriteExcel(records, document, null);
            }
            return stream;
        }

        public void ListToExcel<T>(List<T> list, string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
