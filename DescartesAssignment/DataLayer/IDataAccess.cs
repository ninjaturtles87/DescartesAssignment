using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDataAccess
    {
        Task<List<DataForComparison>> GetDataById(int id);
        Task SaveOrUpdate(DataForComparison dataForComparisons);
    }
}
