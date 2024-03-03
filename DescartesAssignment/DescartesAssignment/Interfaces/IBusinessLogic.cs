using DataLayer.Models;
using DescartesAssignment.Models;

namespace DescartesAssignment.Interfaces
{
    public interface IBusinessLogic
    {
        Task PutValuesToDb(DataForComparison dataForComparison);

        Task<DifferenceResponseProperties> GetDifferences(int id);
    }
}
