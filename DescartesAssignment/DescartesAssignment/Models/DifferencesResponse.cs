namespace DescartesAssignment.Models
{
    public class DifferencesResponse
    {
        public DifferencesResponse()
        {
            Diffs = new List<DifferencesSpecified>();
        }
        public string DiffResultType { get; set; }
        public List<DifferencesSpecified> Diffs { get; set; }
    }
}
