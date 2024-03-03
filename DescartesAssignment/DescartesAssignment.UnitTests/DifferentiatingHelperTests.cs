using DescartesAssignment.Helpers;
using DescartesAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DescartesAssignment.UnitTests
{
    public class DifferentiatingHelperTests
    {
        [Fact]
        public void GetDifferencesSpecifications_EqualArrays_ReturnsEmptyList()
        {
            byte[] firstArray = new byte[] { 1, 2, 3 };
            byte[] secondArray = new byte[] { 1, 2, 3 };

            List<DifferencesSpecified> result = DifferentiatingHelper.GetDifferencesSpecifications(firstArray, secondArray);

            Assert.Empty(result);
        }

        [Theory]
        [InlineData(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 4 }, 1, 2)]
        [InlineData(new byte[] { 9, 8, 1, 2, 3 }, new byte[] { 1, 2, 3, 4, 5 }, 5, 0)]
        public void GetDifferencesSpecifications_ContentDoNotMatch_ReturnsDifferencesSpecified(byte[] firstArray, byte[] secondArray, int length, int offset)
        {
            List<DifferencesSpecified> result = DifferentiatingHelper.GetDifferencesSpecifications(firstArray, secondArray);

            Assert.Equal(result[0].Length, length);
            Assert.Equal(result[0].Offset, offset);
        }
    }
}
