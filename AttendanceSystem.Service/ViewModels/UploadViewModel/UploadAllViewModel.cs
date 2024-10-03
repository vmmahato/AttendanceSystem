using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.ViewModels
{
  public  class UploadAllViewModel
    {
        public int ID { get; set; }
        public int Year { get; set; }
    }

    public class EmployeeProposedDarwandiViewModel
    {
        public string Designation { get; set; }
        public string Position { get; set; }
        public decimal? ApprovedDarwandi { get; set; }
        public decimal? AdvertisedPosts { get; set; }
        public decimal? WorkingEmployees { get; set; }
        public decimal? VacantDarwandi { get; set; }
        public decimal? ProposedDarwandi { get; set; }
    }

    public class TotalEmployeeProposedDarwandiViewModel
    {
        public decimal? TotalApprovedDarwandi { get; set; }
        public decimal? TotalAdvertisedPosts { get; set; }
        public decimal? TotalWorkingEmployees { get; set; }
        public decimal? TotalVacantDarwandi { get; set; }
        public decimal? TotalProposedDarwandi { get; set; }
    }

    public class StateFoodMobilizationViewModel
    {
        public string Type { get; set; }
        public decimal? InitialInventory { get; set; }
        public decimal? Paddy { get; set; }
        public decimal? PaddyRice { get; set; }
        public decimal? Rice { get; set; }
        public decimal? DepositAvailable { get; set; }
        public decimal? Sale { get; set; }
        public decimal? LastStock { get; set; }
    }

    public class TotalStateFoodMobilizationViewModel
    {
        public decimal? TotalInitialInventory { get; set; }
        public decimal? TotalPaddy { get; set; }
        public decimal? TotalPaddyRice { get; set; }
        public decimal? TotalRice { get; set; }
        public decimal? TotalDepositAvailable { get; set; }
        public decimal? TotalSale { get; set; }
        public decimal? TotalLastStock { get; set; }
    }
    public class MeatIncomeExpensesModel
    {
        public string Name { get; set; }
        public decimal? TargetQuantity { get; set; }
        public decimal? TargetPerAmount { get; set; }
        public decimal? TargetAmount { get; set; }
        public decimal? EstimatedQuantity { get; set; }
        public decimal? EstimatedPerAmount { get; set; }
        public decimal? EstimatedAmount { get; set; }
        public decimal? ProposedQuantity { get; set; }
        public decimal? ProposedPerAmount { get; set; }
        public decimal? ProposedAmount { get; set; }
    }

    public class MeatIncomeExpensesViewModel
    {
        public string Name { get; set; }
        public decimal? TargetQuantity { get; set; }
        public decimal? TargetPerAmount { get; set; }
        public decimal? TargetAmount { get; set; }
        public decimal? EstimatedQuantity { get; set; }
        public decimal? EstimatedPerAmount { get; set; }
        public decimal? EstimatedAmount { get; set; }
        public decimal? ProposedQuantity { get; set; }
        public decimal? ProposedPerAmount { get; set; }
        public decimal? ProposedAmount { get; set; }
        public string Type { get; set; }
    }

    public class TotaLMeatIncomeExpensesViewModel
    {
        public decimal? TotalTargetQuantity { get; set; }
        public decimal? TotalTargetPerAmount { get; set; }
        public decimal? TotalTargetAmount { get; set; }
        public decimal? TotalEstimatedQuantity { get; set; }
        public decimal? TotalEstimatedPerAmount { get; set; }
        public decimal? TotalEstimatedAmount { get; set; }
        public decimal? TotalProposedQuantity { get; set; }
        public decimal? TotalProposedPerAmount { get; set; }
        public decimal? TotalProposedAmount { get; set; }
    }
    public class GroupMeatIncomeExpensesViewModel
    {
        public string Type { get; set; }
        public List<MeatIncomeExpensesViewModel> List { get; set; }
        public TotaLMeatIncomeExpensesViewModel Totals { get; set; }
    }

    public class TransportationLeaseExpensesViewModel
    {
        public string District { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public decimal? FirstQuarterQuantity { get; set; }
        public decimal? FirstQuarterAmount { get; set; }
        public decimal? SecondQuarterQuantity { get; set; }
        public decimal? SecondQuarterAmount { get; set; }
        public decimal? ThirdQuarterQuantity { get; set; }
        public decimal? ThirdQuarterAmount { get; set; }
    }

    public class TotalTransportationLeaseExpensesViewModel
    {
        public decimal? TotalQuantity { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalFirstQuarterQuantity { get; set; }
        public decimal? TotalFirstQuarterAmount { get; set; }
        public decimal? TotalSecondQuarterQuantity { get; set; }
        public decimal? TotalSecondQuarterAmount { get; set; }
        public decimal? TotalThirdQuarterQuantity { get; set; }
        public decimal? TotalThirdQuarterAmount { get; set; }
    }

    public class RequestUploadViewModel
    {
        public IFormFile Excel { get; set; }
        public int Year { get; set; }
        public int UploadedBy { get; set; }
    }

}
