using Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.ViewModels;

namespace AttendanceSystem.Services
{
   public interface ITokenService:IService
    {
        Task<TokenModels> GenerateAccessTokenAsync(UserRolesViewModel user, string RefreshToken);
        string GenerateRefreshToken();
        Task<string> SaveRefreshTokenAsync(string ClientID, int UserID);
        Task<AccountResult> DeleteTokenAsync(Token token);
        Task<GenericResult<TokenModels>> RefreshTokenAsync(TokenRequestViewModel model);
        Task<AccountResult> DeleteAllTokenByUserIDAsync(int UserID);
    }
}
