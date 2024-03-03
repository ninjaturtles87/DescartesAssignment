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
        private readonly ILogger<DifferencesController> _logger;

        public DifferencesController(IBusinessLogic businessLogic, ILogger<DifferencesController> logger)
        {
            _businessLogic = businessLogic;
            _logger = logger;
        }
        [HttpPut("{id}/{side:regex(left|right)}")]
        public async Task<IActionResult> SaveData(int id, string side, [FromBody] ReceivedData receivedData)
        {
            //If data received is not valid return bad request
            if (!receivedData.IsValid())
            {
                _logger.LogInformation("Data received from request is either null or not of proper type");
                return BadRequest();
            }

            DataForComparison data = new DataForComparison()
            {
                Id = id,
                Side = side,
                Data = Convert.FromBase64String(receivedData.Data)
            };
            try
            {
                await _businessLogic.PutValuesToDb(data);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Something went wrong with the request");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDifferencesAsync(int id)
        {
            try
            {
                var response = await _businessLogic.GetDifferences(id);

                if (!response.HasData)
                {
                    _logger.LogInformation("Requested data does not exist in database");
                    return NotFound();
                }  

                return Ok(new DifferencesResponse
                {
                    DiffResultType = response.DiffResultType,
                    Diffs = response.Diffs
                });

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Something went wrong with the request");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
