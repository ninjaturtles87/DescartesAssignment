using DataLayer;
using DataLayer.Models;
using DescartesAssignment.Helpers;
using DescartesAssignment.Interfaces;
using DescartesAssignment.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Drawing;
using static DescartesAssignment.Enums;
using Microsoft.Extensions.Logging;

namespace DescartesAssignment.Services
{
    public class BusinessLogic : IBusinessLogic
    {
        private readonly IDataAccess _dataAccess;
        private readonly ILogger<BusinessLogic> _logger;

        public BusinessLogic(IDataAccess dataAccess, ILogger<BusinessLogic> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        public async Task<DifferenceResponseProperties> GetDifferences(int id)
        {
            
            DifferenceResponseProperties responseProperties = new DifferenceResponseProperties { HasData = false };
            List<DataForComparison> dataForComparisonList = new List<DataForComparison>();
            try
            {
                //getting elements from database for comparison
                dataForComparisonList = await Task.Run(() => _dataAccess.GetDataById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured while getting data from database: " + ex.Message);
                throw;
            }
            //check if values for comparison on both sides exist
            var leftElement = dataForComparisonList.Where(x => x.Side == DataSide.Left.ToString().ToLower()).FirstOrDefault();
            if (leftElement is null)
                return responseProperties;

            var rightElement = dataForComparisonList.Where(x => x.Side == DataSide.Right.ToString().ToLower()).FirstOrDefault();
            if (rightElement is null)
                return responseProperties;

            responseProperties.HasData = true;
            _logger.LogInformation("Differences information is gathered");

            //check if data length do not match
            if (leftElement.Data.Length != rightElement.Data.Length)
            {
                responseProperties.DiffResultType = DifferenceTypeEnums.SizeDoNotMatch.ToString();
                return responseProperties;
            }

            //check if data length and data content match and, if not, return differencies specified.
            var differencesSpecified = DifferentiatingHelper.GetDifferencesSpecifications(leftElement.Data, rightElement.Data);

            if (differencesSpecified.Count == 0)
                responseProperties.DiffResultType = DifferenceTypeEnums.Equals.ToString();
            else
                responseProperties.DiffResultType = DifferenceTypeEnums.ContentDoNotMatch.ToString();

            responseProperties.Diffs = differencesSpecified;
            return responseProperties;

        }

        public async Task PutValuesToDb(DataForComparison dataForComparison)
        {
            try
            {
               await Task.Run(() => _dataAccess.SaveOrUpdate(dataForComparison));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add or update data to Database");
                throw new Exception(ex.Message);
            }
        }
    }
}
