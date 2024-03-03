using DescartesAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DescartesAssignment.UnitTests
{
    public class ReceivedDataTests
    {
        [Theory]
        [InlineData("AA=", false)]
        [InlineData("ABA=", true)]
        [InlineData("AA@BDA==", false)]
        [InlineData("ABAQAB==", true)]
        [InlineData("AATBad?=", false)]
        [InlineData("A/==", true)]
        [InlineData("AA$BDA=D", false)]
        public void IsReceivedDataValid(string data, bool expectedResult)
        {
            var receivedData = new ReceivedData();
            {
                receivedData.Data = data;
            }
            
            Assert.Equal(receivedData.IsValid(), expectedResult);
        }
    }
}
