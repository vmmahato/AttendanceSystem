using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.DapperServices;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AttendanceSystem.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Services
{
    public class CompanyProfileService : ICompanyProfileService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<CompanyProfile> _companyProfileRepository;
        private ICommonService _commonService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private IUnitOfWorkManager _unitOfWork;
        public CompanyProfileService
            (
                            IGenericRepository<CompanyProfile> companyProfileRepository,
                            IDapperRepository dapperRepository,
                            ICommonService commonService,
                            IUnitOfWorkManager unitOfWork,
                            IHostingEnvironment hostingEnvironment
            )
        {
            _companyProfileRepository = companyProfileRepository;
            _dapperRepository = dapperRepository;
            _commonService = commonService;
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IPagedList<CompanyProfileViewModel>> CompanyProfileListAsync(CompanyProfileSearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                CompanyProfileID,
	                                CompanyCode,
	                                CompanyName,
	                                CompanyAddress,
	                                ContactPerson,
	                                BranchName,
	                                ContactNumber,
	                                Email,
	                                WebSite,
	                                PanNumber,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM CompanyProfile
                                    WHERE 1=1 ");
            #region Filters
            
            if (!string.IsNullOrEmpty(model.CompanyCode))
            {
                strSQL.AppendFormat(@" AND CompanyCode=@CompanyCode ");
            }
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                strSQL.AppendFormat(@" AND CompanyName=@CompanyName ");
            }
            if (!string.IsNullOrEmpty(model.BranchName))
            {
                strSQL.AppendFormat(@" AND BranchName=@BranchName ");
            }
            if (!string.IsNullOrEmpty(model.PanNumber))
            {
                strSQL.AppendFormat(@" AND PanNumber=@PanNumber ");
            }
            #endregion

            #region Parameters
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@CompanyCode", model.CompanyCode);
            _parameters.Add("@CompanyName", model.CompanyName);
            _parameters.Add("@BranchName", model.BranchName);
            _parameters.Add("@PanNumber", model.PanNumber);
            #endregion

            return await _dapperRepository.ExecuteQueryWithPagedListAsync<CompanyProfileViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CountIndex");
        }

        public async Task<AccountResult> InsertIntoCompanyProfileAsync(CompanyProfileViewModel model)
        {
                var result = new AccountResult();
            //var ExistedBranch = _companyProfileRepository.Table.FirstOrDefaultAsync(x => x.BranchName == model.BranchName);
            //if(ExistedBranch!=null)
            //{
            //    result.Errors = new List<string> { "Duplicate Branch Found" };
            //    return result;
            //}
                string CompanyProfileImage = string.Empty;
                if (model.CompanyImage != null)
                {
                    var errorlist = _commonService.CheckFiles(model.CompanyImage);
                    if (errorlist.Any())
                    {
                        result.Errors = errorlist;
                        return result;
                    }
                }
                var newCompanyProfile = new CompanyProfile()
                {
                    CompanyCode = model.CompanyCode,
                    CompanyName = model.CompanyName,
                    CompanyAddress = model.CompanyAddress,
                    ContactPerson = model.ContactPerson,
                    BranchName = model.BranchName,
                    ContactNumber = model.ContactNumber,
                    Email = model.Email,
                    WebSite = model.WebSite,
                    PanNumber = model.PanNumber,
                    CreatedBy = model.CreatedBy,
                    CreatedTS = DateTime.UtcNow
                };

                using (var uow = _unitOfWork.NewUnitOfWork())
                {
                    try
                    {
                        _companyProfileRepository.Insert(newCompanyProfile);
                        await _companyProfileRepository.SaveChangesAsync();

                        string DirectoryPath = _commonService.GetFullPath(model.CompanyFolderURL + "\\" + newCompanyProfile.CompanyProfileID);
                        var FileList = _commonService.GetFiles(model.CompanyImage, "CompanyImage");
                        var CompanyImageDetails = await _commonService.SaveFiles(FileList, DirectoryPath);
                        if (CompanyImageDetails != null)
                        {
                            CompanyProfileImage = CompanyImageDetails.Company;
                        }
                        newCompanyProfile.CompanyImageURL = CompanyProfileImage;
                        await _companyProfileRepository.SaveChangesAsync();
                        uow.Commit();
                    }
                    catch (Exception e)
                    {
                        uow.Rollback();
                        throw e;
                    }
                }
                return result;
         }

        public CompanyProfile GetCompanyProfileByID(int CompanyProfileID)
        {
            return _companyProfileRepository.Table.FirstOrDefault(x =>x.CompanyProfileID == CompanyProfileID);
        }

        public async Task<AccountResult> UpdateCompanyProfileAsync(CompanyProfileViewModel model)
        {
            try
            {
                var result = new AccountResult();
                string CompanyProfileImage = string.Empty;
                var ExistedCompanyProfile = GetCompanyProfileByID(model.CompanyProfileID);
                if (model.CompanyImage != null)
                {
                    var errorList = _commonService.CheckFiles(model.CompanyImage);
                    if (errorList.Any())
                    {
                        result.Errors = errorList;
                        return result;
                    }
                    string DirectoryPath = _commonService.GetFullPath(model.CompanyFolderURL + "\\" + model.CompanyProfileID);
                    var FileList = _commonService.GetFiles(model.CompanyImage, "CompanyImage");
                    var CompanyImageDetails = await _commonService.SaveFiles(FileList, DirectoryPath);
                    if (CompanyImageDetails != null)
                    {
                        CompanyProfileImage = CompanyImageDetails.Company;
                    }
                }
                else
                {
                    CompanyProfileImage = ExistedCompanyProfile.CompanyImageURL;
                }
               
                if (ExistedCompanyProfile != null)
                {
                    ExistedCompanyProfile.CompanyCode = model.CompanyCode;
                    ExistedCompanyProfile.CompanyName = model.CompanyName;
                    ExistedCompanyProfile.CompanyAddress = model.CompanyAddress;
                    ExistedCompanyProfile.ContactPerson = model.ContactPerson;
                    ExistedCompanyProfile.BranchName = model.BranchName;
                    ExistedCompanyProfile.ContactNumber = model.ContactNumber;
                    ExistedCompanyProfile.Email = model.Email;
                    ExistedCompanyProfile.WebSite = model.WebSite;
                    ExistedCompanyProfile.PanNumber = model.PanNumber;
                    ExistedCompanyProfile.CompanyImageURL = CompanyProfileImage;
                    ExistedCompanyProfile.ModifiedBy = model.ModifiedBy;
                    ExistedCompanyProfile.ModifiedTS = DateTime.UtcNow;
                    _companyProfileRepository.Update(ExistedCompanyProfile);
                    await _companyProfileRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Company does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteCompanyProfileAsync(int CompanyProfileID,string ImagePath)
        {
            var result = new AccountResult();
            var ExistedCompanyProfile = GetCompanyProfileByID(CompanyProfileID);
            if (ExistedCompanyProfile != null)
            {
                _companyProfileRepository.Delete(ExistedCompanyProfile);
                await _companyProfileRepository.SaveChangesAsync();
                string DirectoryPath = _commonService.GetFullPath(ImagePath + "\\" + ExistedCompanyProfile.CompanyProfileID);
                _commonService.DeleteFilesFromDirectoryPath(DirectoryPath);
            }
            else
               {
                    result.Errors = new List<string> { "Company does not exist." };
                    return result;
               }
            return result;
        }
        public async Task<CompanyProfileViewModel> GetCompanyProfileByIDAsync(int CompanyProfileID, string ImagePath)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                CompanyProfileID,
	                                CompanyCode,
	                                CompanyName,
	                                CompanyAddress,
	                                ContactPerson,
	                                BranchName,
	                                ContactNumber,
	                                Email,
	                                WebSite,
	                                PanNumber,
                                    CompanyImageURL,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM CompanyProfile
                                    WHERE CompanyProfileID=@CompanyProfileID");
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@CompanyProfileID", CompanyProfileID);
                var result = await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<CompanyProfileViewModel>(strSQL.ToString(), _parameters);
                if (result != null)
                {
                    //string DirectoryPath = _commonService.GetFullPath(ImagePath + "\\" + result.CompanyProfileID);
                    result.CompanyImageURL = Path.Combine(ImagePath, result.CompanyProfileID.ToString(),result.CompanyImageURL);
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<IList<SelectItemIntViewModel>> DDLCompanyListAsync()
        {
            return await _companyProfileRepository.Table
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.CompanyProfileID,
                              Value = x.BranchName
                          }).ToListAsync();
        }
    }
}
