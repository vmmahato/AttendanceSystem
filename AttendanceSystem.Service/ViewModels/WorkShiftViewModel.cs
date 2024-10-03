using AttendanceSystem.Helpers;
using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class WorkShiftViewModel
    {
        public int ShiftID { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }
        public string BeginingIn { get; set; }
        public string EndingIn { get; set; }
        public string BeginingOut { get; set; }
        public string EndingOut { get; set; }
        public string LateIn { get; set; }
        public string LeaveEarly { get; set; }
        public bool IsMustCheckIn { get; set; }
        public bool IsMustCheckOut { get; set; }
        public bool IsLateIn { get; set; }
        public bool IsEarlyLeave { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public List<WorkShiftTypeViewModel> ShiftList { get; set; }
    }

    public class WorkShiftSearchViewModel : BaseOrderSearch
    {
        public int ShiftID { get; set; }
    }
    public class ShiftWithTypes
    {
        public WorkShiftViewModel ShiftDetails { get; set; }
    }

    public class WorkShiftListViewModel
    {
        public int ShiftID { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan? ShiftStart { get; set; }
        public TimeSpan? ShiftEnd { get; set; }
        public TimeSpan? BeginingIn { get; set; }
        public TimeSpan? EndingIn { get; set; }
        public TimeSpan? BeginingOut { get; set; }
        public TimeSpan? EndingOut { get; set; }
        public TimeSpan? LateIn { get; set; }
        public string LeaveEarly { get; set; }
        public bool IsMustCheckIn { get; set; }
        public bool IsMustCheckOut { get; set; }
        public bool IsLateIn { get; set; }
        public bool IsEarlyLeave { get; set; }

        public string ShiftStartString
        {
            get
            {
                if (ShiftStart == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(ShiftStart); }
            }
        }
        public string ShiftEndString
        {
            get
            {
                if (ShiftEnd == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(ShiftEnd); }
            }
        }
        public string BeginingInString
        {
            get
            {
                if (BeginingOut == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(BeginingIn); }
            }
        }
        public string EndingInString
        {
            get
            {
                if (EndingIn == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(EndingIn); }
            }
        }
        public string BeginingOutString
        {
            get
            {
                if (BeginingOut == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(BeginingOut); }
            }
        }
        public string EndingOutString
        {
            get
            {
                if (EndingOut == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(EndingOut); }
            }
        }
        public string LateInString
        {
            get
            {
                if (LateIn == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(LateIn); }
            }
        }
    }
}
