using DataLayer;
using DataLayer.Models;
using DescartesAssignment.Models;
using DescartesAssignment.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using static DescartesAssignment.Enums;

namespace DescartesAssignment.UnitTests
{
    public class BusinessLogicTests
    {
        private readonly Mock<IDataAccess> _dataAccessMock;
        private readonly BusinessLogic _businessLogic;
        private readonly Mock<ILogger<BusinessLogic>> _loggerMock;

        public BusinessLogicTests()
        {
            _dataAccessMock = new Mock<IDataAccess>();
            _loggerMock = new Mock<ILogger<BusinessLogic>>();
            _businessLogic = new BusinessLogic(_dataAccessMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetDifferences_WhenElementsExist_ReturnsOkDifferenceResponseProperties()
        {
            int id = 1;
            var dataForComparisonList = new List<DataForComparison>
            {
                new DataForComparison
                {
                    Id = id, Side = DataSide.Left.ToString().ToLower(), Data = new byte[] { 1, 2, 3 }
                },
                new DataForComparison
                {
                    Id = id, Side = DataSide.Right.ToString().ToLower(), Data = new byte[] { 1, 2, 4 }
                }
            };
            _dataAccessMock.Setup(x => x.GetDataById(id)).Returns(Task.FromResult(dataForComparisonList));

            var result = await _businessLogic.GetDifferences(id);

            Assert.True(result.HasData);
            Assert.Equal(DifferenceTypeEnums.ContentDoNotMatch.ToString(), result.DiffResultType);
            Assert.NotNull(result.Diffs);
            Assert.Equal(result.Diffs[0].Length, 1);
            Assert.Equal(result.Diffs[0].Offset, 2);
        }

        [Theory]
        [InlineData(DataSide.Left)]
        [InlineData(DataSide.Right)]
        public async Task GetDifferences_WhenOneElementMissing_ReturnsDifferenceResponsePropertiesWithNoData(DataSide dataSide)
        {
            int id = 1;
            byte[] data = new byte[] { 1, 2, 4 };
            var dataForComparisonList = new List<DataForComparison>();
            dataForComparisonList.Add(
                new DataForComparison
                {
                    Id = id,
                    Side = dataSide.ToString().ToLower(),
                    Data = data
                });
            _dataAccessMock.Setup(x => x.GetDataById(id)).Returns(Task.FromResult(dataForComparisonList));

            var result = await _businessLogic.GetDifferences(id);

            Assert.False(result.HasData);
        }

        [Fact]
        public async Task GetDifferences_WhenBothElementsMissing_ReturnsDifferenceResponsePropertiesWithNoData()
        {
            int id = 1;
            var dataForComparisonList = new List<DataForComparison>();
            _dataAccessMock.Setup(x => x.GetDataById(id)).Returns(Task.FromResult(dataForComparisonList));

            var result = await _businessLogic.GetDifferences(id);

            Assert.False(result.HasData);
        }

        [Fact]
        public async Task PutValuesToDb_CallsSaveOrUpdate()
        {
            var dataForComparison = new DataForComparison();

            await _businessLogic.PutValuesToDb(dataForComparison);

            _dataAccessMock.Verify(x => x.SaveOrUpdate(dataForComparison), Times.Once);
        }
    }
}