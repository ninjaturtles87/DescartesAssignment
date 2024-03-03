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
            try
            {
                DifferenceResponseProperties responseProperties = new DifferenceResponseProperties { HasData = false };

                //here is getting elements from database for comparison
                List<DataForComparison> dataForComparisonList = await Task.Run(() => _dataAccess.GetDataById(id));

                //check if values for comparison on both sides exist
                var leftElement = dataForComparisonList.Where(x => x.Side == DataSide.Left.ToString().ToLower()).FirstOrDefault();
                if (leftElement is null)
                    return responseProperties;

                var rightElement = dataForComparisonList.Where(x => x.Side == DataSide.Right.ToString().ToLower()).FirstOrDefault();
                if (rightElement is null)
                    return responseProperties;

                responseProperties.HasData = true;

                //check if data length do not match
                if (leftElement.Data.Length != rightElement.Data.Length)
                {
                    responseProperties.DiffResultType = DifferenceTypeEnums.SizeDoNotMatch.ToString();
                    _logger.LogInformation("Differences information is gathered");
                    return responseProperties;
                }

                //check if data length and data content match and if not, return differencies specified.
                var differencesSpecified = DifferentiatingHelper.GetDifferencesSpecifications(leftElement.Data, rightElement.Data);

                if (differencesSpecified.Count == 0)
                    responseProperties.DiffResultType = DifferenceTypeEnums.Equals.ToString();
                else
                    responseProperties.DiffResultType = DifferenceTypeEnums.ContentDoNotMatch.ToString();

                responseProperties.Diffs = differencesSpecified;
                _logger.LogInformation("Differences information is gathered");
                return responseProperties;
            }
            catch (Exception ex)
            {

                _logger.LogError("An error occured while getting data " + ex.Message);
                return null;
            }
            

        }

        public async Task PutValuesToDb(DataForComparison dataForComparison)
        {
            try
            {
               await Task.Run(() => _dataAccess.SaveOrUpdate(dataForComparison));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to insert data to Database");
                throw new Exception(ex.Message);
            }
        }
    }
}
