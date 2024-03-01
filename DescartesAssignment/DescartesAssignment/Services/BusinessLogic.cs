using DataLayer;
using DataLayer.Models;
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

        public async Task<DifferencesResponse> GetDifferences(byte[] firstArray, byte[] secondArray)
        {
            //check if data length do not match
            if(firstArray.Length != secondArray.Length)
            {
                return new DifferencesResponse
                {
                    DiffResultType = DifferenceTypeEnums.SizeDoNotMatch.ToString(),
                };
            }
            //check if data length and data content match
            if (AreElementsEqual(firstArray, secondArray))
            {
                return new DifferencesResponse
                {
                    DiffResultType = DifferenceTypeEnums.Equals.ToString(),
                };
            }
            //at this point we are certain that data are the same size but are not of similar content
            return new DifferencesResponse
            {
                DiffResultType = DifferenceTypeEnums.ContentDoNotMatch.ToString(),
                Diffs = GetDifferencesSpecifications(firstArray, secondArray)
            };
        }

        public async Task PutValuesToDb(DataForComparison dataForComparison)
        {
            
            await _dataAccess.SaveOrUpdateAsync(dataForComparison);
        }

        private static bool AreElementsEqual(byte[] firstArray, byte[] secondArray)
        {
            for (int i = 0; i < firstArray.Length; i++)
            {
                if (firstArray[i] != secondArray[i])
                    return false;
            }

            return true;
        }

        private static List<DifferencesSpecified> GetDifferencesSpecifications(byte[] firstArray, byte[] secondArray)
        {
            List<DifferencesSpecified> differences = new List<DifferencesSpecified>();

            // We will store first index where values don't align. 
            int startIndex = -1;
            for (int i = 0; i < firstArray.Length; i++)
            {
                if (firstArray[i] != secondArray[i])
                {
                    // Found difference. If this is difference the first difference then we memorize the index.
                    if (startIndex == -1)
                        startIndex = i;
                }
                else
                    if (startIndex != -1)
                    {
                        differences.Add(new DifferencesSpecified 
                        { 
                            Offset = startIndex, Length = i - startIndex 
                        });
                        startIndex = -1; // we set this difference index to default value so we know that we didn't find any differences from the last one.
                    }
            }
            // We finished the loop and now we just need to check if difference index is different than default value (-1). If it's different then this means that there are differences from that index until the end
            if (startIndex != -1)
                differences.Add(new DifferencesSpecified 
                { 
                    Offset = startIndex, Length = firstArray.Length - startIndex 
                });
            return differences;
        }

    }
}
