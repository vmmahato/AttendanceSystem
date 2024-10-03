using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class GenericResult<T> : AccountResult
    {
        public T Data { get; set; }
        public new GenericResult<T> Failed(IEnumerable<string> errors)
        {
            Errors = errors;
            return this;
        }

        public new GenericResult<T> Failed(params string[] errors)
        {
            Errors = errors;
            return this;
        }
      
    }
}
