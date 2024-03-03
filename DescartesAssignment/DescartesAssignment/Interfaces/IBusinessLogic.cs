using DataLayer.Models;
using DescartesAssignment.Models;

namespace DescartesAssignment.Interfaces
{
    public interface IBusinessLogic
    {
        public Task PutValuesToDb(DataForComparison dataForComparison);

        public Task<DifferenceResponseProperties> GetDifferences(int id);
    }
}
