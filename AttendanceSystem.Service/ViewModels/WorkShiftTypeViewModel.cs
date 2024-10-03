using AttendanceSystem.Helpers;
using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
   public class WorkShiftTypeViewModel
    {
        public int ShiftTypeID { get; set; }
        public int ShiftID { get; set; }
        public string Name { get; set; }
        public string  StartTime { get; set; }
        public string EndTime { get; set; }
        public int Count { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class WorkShiftTypeSearchViewModel : BaseOrderSearch
    {
        public int ShiftTypeID { get; set; }
        public int ShiftID { get; set; }
    }

    public class GetWorkShiftTypeViewModel
    {
        public int ShiftTypeID { get; set; }
        public int ShiftID { get; set; }
        public string Name { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int Count { get; set; }
        public int? Duration { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public string StartTimeString
        {
            get
            {
                if (StartTime == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(StartTime); }
            }
        }

        public string EndTimeString
        {
            get
            {
                if (EndTime == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(EndTime); }
            }
        }
    }
}
