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

        public DifferencesController(IBusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
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
        public async Task<IActionResult> GetDifferencesAsync(int id)
        {
            var response = await _businessLogic.GetDifferences(id);

            if(!response.HasData)
                return NotFound();

            return Ok(new DifferencesResponse
            {
                DiffResultType= response.DiffResultType,
                Diffs= response.Diffs
            });
        }
    }
}
