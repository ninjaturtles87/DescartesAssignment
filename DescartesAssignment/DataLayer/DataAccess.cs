using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DataLayer
{
    public class DataAccess : IDataAccess
    {
        private readonly Database _database;
        private readonly ILogger<DataAccess> _logger;

        public DataAccess(Database database, ILogger<DataAccess> logger)
        {
            _database = database;
            _logger = logger;
        }
        public async Task<List<DataForComparison>> GetDataById(int id)
        {
            try
            {
                _logger.LogInformation("Requested data is delivered from database");
                return await Task.Run(() => _database.DataForComparisonList.Where(x => x.Id == id).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error occured");
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveOrUpdate(DataForComparison dataForComparison)
        {
            try
            {
                //check if there are existing elements in list with the received Id and type(left/right) - add new element if not and update it if the element exists
                DataForComparison? listElement = await Task.Run(() => _database.DataForComparisonList.Where(x => x.Id == dataForComparison.Id && x.Side == dataForComparison.Side).FirstOrDefault());
                if (listElement is null) {
                    _logger.LogInformation("Data from request is added to database");
                    _database.DataForComparisonList.Add(dataForComparison);
                }
                else
                {
                    _logger.LogInformation("Existing data from database is updated with data from request");
                    listElement.Data = dataForComparison.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveOrUpdateAsync failed with message: " + ex.Message);
                throw;
            }
        }
    }
}
