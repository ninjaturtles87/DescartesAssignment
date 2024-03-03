using DataLayer;
using DataLayer.Models;
using DescartesAssignment.Helpers;
using DescartesAssignment.Interfaces;
using DescartesAssignment.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Drawing;
using static DescartesAssignment.Enums;

namespace DescartesAssignment.Services
{
    public class BusinessLogic : IBusinessLogic
    {
        private readonly IDataAccess _dataAccess;

        public BusinessLogic(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<DifferenceResponseProperties> GetDifferences(int id)
        {

            DifferenceResponseProperties responseProperties = new DifferenceResponseProperties { HasData = false };
            
            //here is getting elements from database for comparison
            List<DataForComparison> dataForComparisonList = await _dataAccess.GetDataByIdAsync(id);

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
                return responseProperties;
            }

            //check if data length and data content match and if not, return differencies specified.
            var differencesSpecified = DifferentiatingHelper.GetDifferencesSpecifications(leftElement.Data, rightElement.Data);

            if(differencesSpecified.Count == 0)
                responseProperties.DiffResultType = DifferenceTypeEnums.Equals.ToString();
            else
                responseProperties.DiffResultType = DifferenceTypeEnums.ContentDoNotMatch.ToString();

            responseProperties.Diffs = differencesSpecified;
            return responseProperties;

        }

        public async Task PutValuesToDb(DataForComparison dataForComparison)
        {
            await _dataAccess.SaveOrUpdateAsync(dataForComparison);
        }
    }
}
