using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataAccess : IDataAccess
    {
        private readonly Database _database;

        public DataAccess(Database database)
        {
            _database = database;
        }
        public Task<List<DataForComparison>> GetDataByIdAsync(int id)
        {
            return Task.FromResult(_database.DataForComparisonList.Where(x => x.Id == id).ToList());
        }

        public Task<bool> SaveOrUpdateAsync(DataForComparison dataForComparison)
        {
            try
            {
                //check if there are existing elements in list with the received Id and type(left/right) - add new element if not and update it if the element exists
                DataForComparison? listElement = _database.DataForComparisonList.Where(x => x.Id == dataForComparison.Id && x.Side == dataForComparison.Side).FirstOrDefault();
                if (listElement is null) {
                    _database.DataForComparisonList.Add(dataForComparison);
                }
                else
                {
                    listElement.Data = dataForComparison.Data;
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveOrUpdateAsync failed: " + ex.Message);
                return Task.FromResult(false);
            }
        }
    }
}
