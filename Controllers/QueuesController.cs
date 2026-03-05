using MediatR;
using Microsoft.AspNetCore.Mvc;
using QueueProject.Application.Queues.Commands.CreateQueue;
using QueueProject.Application.Queues.Commands.DeleteQueue;
using QueueProject.Application.Queues.Commands.UpdateQueue;
using QueueProject.Application.Queues.Queries.GetQueueById;
using QueueProject.Application.Queues.Queries.GetQueuesQuery;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace QueueProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueuesController : ControllerBase
    {
        //MediatR: For handling commands and queries
        //Clean Architecture: Separation of concerns across layers

        private readonly IMediator _mediator;
        private readonly ILogger<QueuesController> _logger;
        public QueuesController(IMediator mediator, ILogger<QueuesController> logger)
        {
            _mediator = mediator;
            _logger= logger;
        }

        #region Get all Queue
        
        [HttpGet]
        public async Task<IActionResult> GetQueues()
        {
            try 
            {
                _logger.LogInformation("GetQueues operation started.");
                var query = new GetQueuesQuery { };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("GetQueues failed.",ex);
                return StatusCode(500, $"GetQueues operation. Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Get Queue by Id

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQueueById(string id)
        {
            try
            {
                _logger.LogInformation("GetQueuesById operation started.");
                var query = new GetQueueById { Id = id };
                var result = await _mediator.Send(query);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("GetQueueById failed.", ex);
                return StatusCode(500, $"GetQueueById operation. Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Create Queue
        
        [HttpPost]
        public async Task<IActionResult> CreateQueue([FromBody] CreateQueueCommand command)
        {
            try
            {
                _logger.LogInformation("CreateQueue operation started.");
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetQueueById), new { id = result }, command);
            }
            catch (System.Exception ex)
            {
                _logger.LogError("CreateQueue failed.", ex);
                return StatusCode(500, $"Insert operation. Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region Update Queue
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQueue(string id, [FromBody] UpdateQueueCommand command)
        {
            try
            {
                _logger.LogInformation("UpdateQueue operation started.");
                if (id != command.Id)
                    return BadRequest("ID mismatch");

                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch(System.Exception ex)
            {
                _logger.LogError("UpdateQueue failed.", ex);
                return StatusCode(500, $"Update operation.Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region DeleteQueue

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQueue(string id)
        {
            try
            {
                _logger.LogInformation("DeleteQueue operation started.");
                var command = new DeleteQueueCommand { Id = id };
                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError("DeleteQueue failed.", ex);
                return StatusCode(500, $"Delete operation. Internal server error: {ex.Message}");
            }

        }

        #endregion

        #region Protect only specific endpoints

        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("Anyone can access this.");
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok("You are authenticated!");
            }
        #endregion




    }
}
