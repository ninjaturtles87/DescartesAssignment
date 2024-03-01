using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DescartesAssignment.Models
{
    public class ReceivedData
    {
        [Required]
        public string Data { get; set; }
        // Data is valid if it is not null and is of type Byte[]
        public bool IsValid()
        {
            return Data is not null && Regex.IsMatch(Data, @"^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$");
        }
    }
}
