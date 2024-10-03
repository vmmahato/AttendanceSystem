using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
  public static  class UserAccessTypes
    {
        /// <summary>
        /// Add after creating new modules
        /// </summary>

        #region User
        public const string UserRead = "User_" + AccessTypes.Read;
        public const string UserReadWrite = "User_" + AccessTypes.ReadWrite;
        public const string UserExport = "User_" + AccessTypes.Export;
        public const string UserPrint = "User_" + AccessTypes.Print;
        #endregion
    }
}
