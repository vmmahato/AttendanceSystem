using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
   public class TokenRequestViewModel
    {
            public string GrantType { get; set; } // password or refresh_token
            public string ClientId { get; set; }
            public string RefreshToken { get; set; }
             public string UserName { get; set; }
             public int? UserID { get; set; }
    }
}
