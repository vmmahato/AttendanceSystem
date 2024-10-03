using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class AccountResult
    {
        public AccountResult() : base()
        {
            Errors = new List<string>();
        }

        public IEnumerable<string> Errors { get; set; }
        public bool TokenExpired { get; set; }
        public bool DataChanged { get; set; }
        public int ID { get; set; }

        public AccountResult RefreshTokenExpired(bool tokenExpired)
        {
            TokenExpired = tokenExpired;
            return this;

        }

        public AccountResult ServerDataChanged(bool dataChanged)
        {
            DataChanged = dataChanged;
            return this;

        }
        public AccountResult(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public AccountResult(int id)
        {
            ID = id;
        }

        public AccountResult(params string[] errors)
        {
            Errors = errors;
        }

        public AccountResult Failed(IEnumerable<string> errors)
        {
            Errors = errors;
            return this;
        }

        public AccountResult Failed(params string[] errors)
        {
            Errors = errors;
            return this;
        }
        
        public bool Success
        {
            get { return !Errors.Any(); }
        }
    }
}
