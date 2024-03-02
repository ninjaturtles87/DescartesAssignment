using DataLayer;
using DataLayer.Models;
using DescartesAssignment.Services;
using Moq;
using System.Reflection;
using static DescartesAssignment.Enums;

namespace DescartesAssignment.UnitTests
{
    public class BusinessLogicTests
    {
        private readonly Mock<IDataAccess> _dataAccessMock;
        public BusinessLogicTests()
        {
            _dataAccessMock = new Mock<IDataAccess>();
        }
        [Fact]
        public async Task GetDifferences_SizeDoNotMatch_ReturnsCorrectResult()
        {
            var businessLogic = new BusinessLogic(_dataAccessMock.Object);
            byte[] firstArray = new byte[] { 1, 2, 3 };
            byte[] secondArray = new byte[] { 1, 2, 3, 4 };

            var result = await businessLogic.GetDifferences(firstArray, secondArray);

            Assert.Equal(DifferenceTypeEnums.SizeDoNotMatch.ToString(), result.DiffResultType);
        }

        [Fact]
        public async Task GetDifferences_Equals_ReturnsCorrectResult()
        {
            var businessLogic = new BusinessLogic(_dataAccessMock.Object);
            byte[] firstArray = new byte[] { 1, 2, 3 };
            byte[] secondArray = new byte[] { 1, 2, 3 };

            var result = await businessLogic.GetDifferences(firstArray, secondArray);

            Assert.Equal(DifferenceTypeEnums.Equals.ToString(), result.DiffResultType);
        }

        [Fact]
        public async Task GetDifferences_ContentDoNotMatch_ReturnsCorrectResult()
        {
            var businessLogic = new BusinessLogic(_dataAccessMock.Object);
            byte[] firstArray = new byte[] { 1, 2, 3 };
            byte[] secondArray = new byte[] { 1, 2, 4 };

            var result = await businessLogic.GetDifferences(firstArray, secondArray);

            Assert.Equal(DifferenceTypeEnums.ContentDoNotMatch.ToString(), result.DiffResultType);
            Assert.Equal(result.Diffs[0].Length, 1);
            Assert.Equal(result.Diffs[0].Offset, 2);
        }

        [Fact]
        public async Task PutValuesToDb_CallsSaveOrUpdateAsync()
        {
            
            var businessLogic = new BusinessLogic(_dataAccessMock.Object);
            var dataForComparison = new DataForComparison();

            await businessLogic.PutValuesToDb(dataForComparison);

            _dataAccessMock.Verify(x => x.SaveOrUpdateAsync(dataForComparison), Times.Once);
        }
    }
}