using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    //TODO: komentar da objasnis sta je
    public class DataForComparison
    {
        public int Id { get; set; }
        public string? Side { get; set; }    
        public Byte[]? Data { get; set; }
    }
}
