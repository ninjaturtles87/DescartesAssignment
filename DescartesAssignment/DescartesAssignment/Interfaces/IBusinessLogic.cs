using DataLayer.Models;
using DescartesAssignment.Models;

namespace DescartesAssignment.Interfaces
{
    public interface IBusinessLogic
    {
        public Task PutValuesToDb(DataForComparison dataForComparison);

        public Task<DifferencesResponse> GetDifferences(byte[] firstArray, byte[] secondArray);
    }
}
