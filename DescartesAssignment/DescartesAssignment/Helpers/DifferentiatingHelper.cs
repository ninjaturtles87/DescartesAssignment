using DescartesAssignment.Models;

namespace DescartesAssignment.Helpers
{
    public class DifferentiatingHelper
    {
        public static List<DifferencesSpecified> GetDifferencesSpecifications(byte[] firstArray, byte[] secondArray)
        {
            List<DifferencesSpecified> differences = new List<DifferencesSpecified>();

            // First index where values don't align is stored 
            int startIndex = -1;
            for (int i = 0; i < firstArray.Length; i++)
            {
                if (firstArray[i] != secondArray[i])
                {
                    // Found difference. If this is difference the first difference then we memorize the index.
                    if (startIndex == -1)
                        startIndex = i;
                }
                else
                    if (startIndex != -1)
                {
                    differences.Add(new DifferencesSpecified
                    {
                        Offset = startIndex,
                        Length = i - startIndex
                    });
                    startIndex = -1; // This difference index is set to default value to assure that any differences from the last one were not found.
                }
            }
            // Loop is finished and now check if difference index is different than default value (-1) is done. If it's different then this means that there are differences from that index until the end
            if (startIndex != -1)
                differences.Add(new DifferencesSpecified
                {
                    Offset = startIndex,
                    Length = firstArray.Length - startIndex
                });

            return differences;
        }
    }
}
