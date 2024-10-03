using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.AppSettings;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.Helpers;
using AttendanceSystem.UserClaimTypesModel;
using AttendanceSystem.ViewModels;

namespace AttendanceSystem.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTConfig _jwtSettings;
        private readonly IGenericRepository<Token> _tokenRepository;
        private readonly IGenericRepository<FiscalYears> _fiscalYearRepository;
        private readonly IAccountService _accountService;
        public TokenService(IOptions<JWTConfig> jwtSettings,
                            IGenericRepository<Token> tokenRepository,
                            IAccountService accountService,
                            IGenericRepository<FiscalYears> fiscalYearRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _tokenRepository = tokenRepository;
            _accountService = accountService;
            _fiscalYearRepository = fiscalYearRepository;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<TokenModels> GenerateAccessTokenAsync(UserRolesViewModel user,string RefreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            double tokenExpiryTime = Convert.ToDouble(_jwtSettings.ExpireTime);

            //current Fiscal Year
            var CurrentFiscalYear = ReturnCurrentFiscalYear();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = SetClaims.CreateClaims(user, CurrentFiscalYear),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddHours(10)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var strToken=  tokenHandler.WriteToken(token);
            var strRefreshToken = await SaveRefreshTokenAsync(_jwtSettings.ClientId,user.UserRolesID);
            return  new TokenModels()
            {
                token = strToken,
                expiration = token.ValidTo,
                refresh_token = strRefreshToken
            };
        }

        public async Task<string> SaveRefreshTokenAsync(string ClientID, int UserID)
        {
                var Token = new Token()
                {
                    UserID =UserID,
                    ClientID = ClientID,
                    RefreshToken = GenerateRefreshToken(),
                    CreatedTS = DateTime.Now,
                    ExpiryTime=DateTime.UtcNow.AddHours(10)
                };
                _tokenRepository.Insert(Token);
           await _tokenRepository.SaveChangesAsync();
            return Token.RefreshToken;
        }

        public async Task<AccountResult> DeleteTokenAsync(Token token)
        {
            var result = new AccountResult();
            _tokenRepository.Delete(token);
            await _tokenRepository.SaveChangesAsync();
            return result;
        }
        public async Task<GenericResult<TokenModels>> RefreshTokenAsync(TokenRequestViewModel model)
        {
            var result = new GenericResult<TokenModels>();
            var refreshToken = _tokenRepository.Table
                        .FirstOrDefault(x =>x.ClientID == _jwtSettings.ClientId && x.RefreshToken == model.RefreshToken
                        .ToString());


            if (refreshToken == null)
            {
                // refresh token not found or invalid (or invalid clientId)
                result.Errors=new string[] {"Invalid Refresh Token"};
                return result;
            }

            // check if refresh token is expired
            if (refreshToken.ExpiryTime < DateTime.UtcNow)
            {
                result.Errors = new string[] { "Refresh Token is expired" };
                result.TokenExpired = true;
                return result;
            }

            var user = await _accountService.GetUserAsync(new TokenRequestViewModel()
            {
                UserID = refreshToken.UserID
            });
            if (user == null)
            {
                result.Errors = new string[] { "User doesnot exist." };
                return result;
            }
            // generate a new refresh token 

            var newRefreshToken =await SaveRefreshTokenAsync(refreshToken.ClientID, refreshToken.UserID);

            // invalidate the old refresh token (by deleting it)
           var IsDeletedToken= await DeleteTokenAsync(refreshToken);

            if (IsDeletedToken.Success)
            {
                var response = await GenerateAccessTokenAsync(user, newRefreshToken);
                result.Data = response;
            }
            return result;

        }

        public async Task<AccountResult> DeleteAllTokenByUserIDAsync(int UserID)
        {
            var TokenList = _tokenRepository.Table.Where(x => x.UserID == UserID).ToList();
            var result = new AccountResult();
            _tokenRepository.Delete(TokenList);
            await _tokenRepository.SaveChangesAsync();
            return result;
        }
        private CurrentFiscalYearDetails ReturnCurrentFiscalYear()
        {
            var currentDate = DateTime.UtcNow;
            return _fiscalYearRepository.TableNoTracking
                .Where(x => x.IsCurrentFiscalYear==true && x.StartYear <= currentDate && x.EndYear >= currentDate)
                .Select(x => new CurrentFiscalYearDetails()
                {
                    FiscalYearID = x.FiscalID,
                    FromDate = x.StartYear,
                    ToDate = x.EndYear
                }).FirstOrDefault();
        }
    }
}
