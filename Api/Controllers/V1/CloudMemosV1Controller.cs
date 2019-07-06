using System;
using System.Threading.Tasks;
using CloudMemos.Dtos.V1;
using CloudMemos.Logic.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudMemos.Api.Controllers.V1
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Produces("application/json")]
    [Route("v1/[controller]")]
    [ApiController]
    public class CloudMemosV1Controller : ControllerBase
    {
        private readonly IMemoManagerV1 _facade;
        private readonly INewIdGenerator _newIdGenerator;

        public CloudMemosV1Controller(IMemoManagerV1 facade, INewIdGenerator newIdGenerator)
        {
            _facade = facade;
            _newIdGenerator = newIdGenerator;
        }

        [HttpGet("{id}")]
        //[Authorize(Policy = nameof(PolicyType.ReadCloudMemos))]
        [ProducesResponseType(typeof(TextPieceResponceV1), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var response = await _facade.Get(id);

                if (response == null)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                var errorId = _newIdGenerator.Generate();
                return StatusCode(500, $"Failed to process request. Error code {errorId}");
            }
        }

        [HttpPost]
        //[Authorize(Policy = nameof(PolicyType.WriteCloudMemos))]
        [ProducesResponseType(typeof(TextPieceResponceV1), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateMemoRequestV1 memo)
        {
            try
            {
                var response = await _facade.Create(memo);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (Exception exception)
            {
#if DEBUG
                return StatusCode(500, exception.ToString());
#else
                var errorId = _newIdGenerator.Generate();
                return StatusCode(500, $"Failed to process request. Error code {errorId}");
#endif
            }
        }
    }
}