using DataLayer;
using DataLayer.Models;
using DescartesAssignment.Interfaces;
using DescartesAssignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using static DescartesAssignment.Enums;

namespace DescartesAssignment.Controllers
{
    [Route("v1/diff")]
    [ApiController]
    public class DifferencesController : ControllerBase
    {
        private readonly IBusinessLogic _businessLogic;
        private readonly IDataAccess _dataAccess;

        public DifferencesController(IBusinessLogic businessLogic, IDataAccess dataAccess)
        {
            _businessLogic = businessLogic;
            _dataAccess = dataAccess;
        }
        [HttpPut("{id}/{side:regex(left|right)}")]
        public async Task<IActionResult> SaveData(int id, string side, [FromBody] ReceivedData receivedData)
        {
            //If data received is not valid return bad request
            if (!receivedData.IsValid())
            {
                return BadRequest();
            }

            DataForComparison data = new DataForComparison()
            {
                Id = id,
                Side = side,
                Data = Convert.FromBase64String(receivedData.Data)
            };

            await _businessLogic.PutValuesToDb(data);
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDifferences(int id)
        {
            //here we go to database and extrat elements for comparison
            List<DataForComparison> dataForComparisonList = await _dataAccess.GetDataByIdAsync(id);

            // check if values for comparison exist
            var leftElement = dataForComparisonList.Where(x => x.Side == DataSide.Left.ToString()).FirstOrDefault();

            if (leftElement is null )
                return NotFound();
            
            var rightElement = dataForComparisonList.Where(x => x.Side == DataSide.Right.ToString()).FirstOrDefault();
            
            if (rightElement is null)
                return NotFound();
            //here we are sure that values exist, so we compare them
            var response = await _businessLogic.GetDifferences(leftElement.Data, rightElement.Data);
            return Ok(response);
        }
    }
}
