using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CloudMemos.Dtos.V2;
using CloudMemos.Logic.BusinessLogic;
using Microsoft.AspNetCore.Http;

namespace CloudMemos.Api.Controllers.V2
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Produces("application/json")]
    [Route("v2/[controller]")]
    [ApiController]
    public class CloudMemosV2Controller : ControllerBase
    {
        private readonly IMemoManagerV2 _entityManager;
        private readonly INewIdGenerator _newIdGenerator;

        public CloudMemosV2Controller(IMemoManagerV2 entityManager, INewIdGenerator newIdGenerator)
        {
            _entityManager = entityManager;
            _newIdGenerator = newIdGenerator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TextPieceResponceV2), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id, SortOption? sort = SortOption.None)
        {
            try
            {
                var response = await _entityManager.Get(id, sort);

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

        [HttpPut("{id}/Sort/{sort}")]
        [ProducesResponseType(typeof(TextPieceResponceV2), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Sort(string id, SortOption? sort = SortOption.None)
        {
            try
            {
                var response = await _entityManager.Sort(id, sort);

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

        [HttpGet("{id}/TextStatistics")]
        [ProducesResponseType(typeof(TextStatisticsResponce), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTextStatisticsById(string id)
        {
            try
            {
                var response = await _entityManager.GetStatistics(id);

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
        [ProducesResponseType(typeof(TextPieceResponceV2), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateMemoRequestV2 memo)
        {
            try
            {
                var response = await _entityManager.Create(memo);

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