namespace DescartesAssignment.Models
{
    public class DifferenceResponseProperties
    {
        public string DiffResultType { get; set; }
        public List<DifferencesSpecified> Diffs { get; set; }
        public bool HasData { get; set; }
        public DifferenceResponseProperties()
        {
            Diffs = new List<DifferencesSpecified>();
        }
    }
}
