namespace DescartesAssignment.Models
{
    public class DifferencesResponse
    {
        public string DiffResultType { get; set; }
        public List<DifferencesSpecified> Diffs { get; set; }
    }
}
