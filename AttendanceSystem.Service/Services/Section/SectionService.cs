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
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Services
{
    public class SectionService : ISectionService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<Section> _sectionRepository;
        public SectionService(IGenericRepository<Section> sectionRepository,
                            IDapperRepository dapperRepository)
        {
            _sectionRepository = sectionRepository;
            _dapperRepository = dapperRepository;
        }

        public async Task<IPagedList<SectionViewModel>> SectionListAsync(SectionSearchViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                S.SectionID,
	                                S.SectionCode,
	                                S.SectionName,
	                                S.DepartmentID,
									D.DepartmentName,
	                                S.CreatedTS,
	                                S.CreatedBy,
	                                S.ModifiedTS,
	                                S.ModifiedBy
	                                FROM Section S JOIN Department D ON S.DepartmentID=D.DepartmentID
                                    WHERE 1=1 ");
            #region Filters
            
            if (!string.IsNullOrEmpty(model.SectionName))
            {
                strSQL.AppendFormat(@" AND SectionName=@SectionName");
            }
            if (!string.IsNullOrEmpty(model.DepartmentName))
            {
                strSQL.AppendFormat(@" AND DepartmentName=@DepartmentName");
            }
            #endregion

            #region Parameters
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@SectionName", model.SectionName);
            _parameters.Add("@DepartmentName", model.DepartmentName);
            #endregion

            return await _dapperRepository.ExecuteQueryWithPagedListAsync<SectionViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "CountIndex");
        }

        public async Task<AccountResult> InsertIntoSectionAsync(SectionViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_sectionRepository.TableNoTracking.Any(x => x.SectionName == model.SectionName))
                {
                    result.Errors = new List<string> { "Section " + model.SectionName + " is already taken" };
                    return result;
                }
                if (_sectionRepository.TableNoTracking.Any(x => x.SectionCode == model.SectionCode))
                {
                    result.Errors = new List<string> { "Section code " + model.SectionCode + " is already taken" };
                    return result;
                }
                var newSection = new Section()
                {
                    SectionCode = model.SectionCode,
                    SectionName = model.SectionName,
                    DepartmentID = model.DepartmentID,
                    CreatedBy = model.CreatedBy,
                    CreatedTS = DateTime.UtcNow
                };
                _sectionRepository.Insert(newSection);
                await _sectionRepository.SaveChangesAsync();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Section GetSectionByID(int SectionID)
        {
            return _sectionRepository.Table.FirstOrDefault(x =>x.SectionID == SectionID);
        }

        public async Task<AccountResult> UpdateSectionAsync(SectionViewModel model)
        {
            try
            {
                var result = new AccountResult();
                if (_sectionRepository.TableNoTracking.Any(x => x.SectionName == model.SectionName && x.SectionID!=model.SectionID))
                {
                    result.Errors = new List<string> { "Section " + model.SectionName + " is already taken" };
                    return result;
                }
                if (_sectionRepository.TableNoTracking.Any(x => x.SectionCode == model.SectionCode && x.SectionID!= model.SectionID))
                {
                    result.Errors = new List<string> { "Section code " + model.SectionCode + " is already taken" };
                    return result;
                }
                var ExistedSection = GetSectionByID(model.SectionID);
                if (ExistedSection != null)
                {
                    ExistedSection.SectionCode = model.SectionCode;
                    ExistedSection.SectionName = model.SectionName;
                    ExistedSection.DepartmentID = model.DepartmentID;
                    ExistedSection.ModifiedBy = model.ModifiedBy;
                    ExistedSection.ModifiedTS = DateTime.UtcNow;
                    _sectionRepository.Update(ExistedSection);
                    await _sectionRepository.SaveChangesAsync();
                }
                else
                {
                    result.Errors = new List<string> { "Section does not exist." };
                    return result;
                }
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<AccountResult> DeleteSectionAsync(int SectionID)
        {
            var result = new AccountResult();
            var ExistedSection = GetSectionByID(SectionID);
            if (ExistedSection != null)
            {
                   _sectionRepository.Delete(ExistedSection);
             await _sectionRepository.SaveChangesAsync();
            }
            else
               {
                    result.Errors = new List<string> { "Section does not exist." };
                    return result;
               }
            return result;
        }
        public async Task<SectionViewModel> GetSectionByIDAsync(int SectionID)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                    ROW_NUMBER() OVER(ORDER BY (SELECT 1) ) AS CountIndex,
	                                SectionID,
	                                SectionCode,
	                                SectionName,
	                                DepartmentID,
	                                CreatedTS,
	                                CreatedBy,
	                                ModifiedTS,
	                                ModifiedBy
	                                FROM Section
                                    WHERE SectionID=@SectionID");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@SectionID", SectionID);
            return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<SectionViewModel>(strSQL.ToString(), _parameters);
        }

        public async Task<IList<SelectItemIntViewModel>> DDLSectionListAsync(int DepartmentID)
        {
            return await _sectionRepository.Table.Where(x=>x.DepartmentID== DepartmentID)
                          .Select(x => new SelectItemIntViewModel()
                          {
                              ID = x.SectionID,
                              Value = x.SectionName
                          }).ToListAsync();
        }
        public async Task<IList<SelectItemIntViewModel>> DDLDepartmentWiseSectionListAsync(DepartmentAndSectionIdModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                string DepartmentIDs = string.Empty;
                DepartmentIDs = string.Join(",", model.DepartmentID.ToList());
                if (DepartmentIDs == "")
                {
                    DepartmentIDs = "0";
                }
                strSQL.AppendFormat(@"SELECT SectionID ID,SectionName Value from Section WHERE DepartmentID IN (" + DepartmentIDs + ")");
                #region Filter
                DynamicParameters _parameters = new DynamicParameters();
                #endregion
                var result = await _dapperRepository.ExecuteQueryAsync<SelectItemIntViewModel>(strSQL.ToString(), _parameters);
                return result.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
