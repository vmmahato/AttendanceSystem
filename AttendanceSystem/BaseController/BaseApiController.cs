using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.UserClaimModel;
using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.Helpers;

namespace AttendanceSystem.BaseController
{
    [Route("api/[controller]/[action]")]
    public abstract class BaseApiController : Controller
    {
        private UserClaimsInformation _currentUserDetails;
        protected virtual UserClaimsInformation CurrentUserDetails
        {
            get
            {
                if (_currentUserDetails == null)
                {
                    _currentUserDetails = User.ClaimsInformation();
                }
                return _currentUserDetails;
            }
        }
    }
}