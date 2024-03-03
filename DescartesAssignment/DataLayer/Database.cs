using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    
    public class Database
    {
        public List<DataForComparison> DataForComparisonList { get; set; }

        public Database()
        {
            DataForComparisonList = new List<DataForComparison>();
        }
    }
}
