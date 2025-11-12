using MediatR;
using Microsoft.AspNetCore.Mvc;
using QueueProject.Application.Queues.Commands.CreateQueue;
using QueueProject.Application.Queues.Commands.DeleteQueue;
using QueueProject.Application.Queues.Commands.UpdateQueue;
using QueueProject.Application.Queues.Queries.GetQueueById;
using QueueProject.Application.Queues.Queries.GetQueuesQuery;
using System.Threading.Tasks;

namespace QueueProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueuesController : ControllerBase
    {
        //MediatR: For handling commands and queries
        //Clean Architecture: Separation of concerns across layers

        private readonly IMediator _mediator;

        public QueuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Get all Queue
        
        [HttpGet]
        public async Task<IActionResult> GetQueues()
        {
            try 
            {
                var query = new GetQueuesQuery { };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
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
                var query = new GetQueueById { Id = id };
                var result = await _mediator.Send(query);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (System.Exception ex)
            {
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
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetQueueById), new { id = result }, command);
            }
            catch (System.Exception ex)
            {
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
                if (id != command.Id)
                    return BadRequest("ID mismatch");

                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch(System.Exception ex)
            {
                return StatusCode(500, $"Update operation.Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region DeleteQueue
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQueue(string id)
        {
            try
            {
                var command = new DeleteQueueCommand { Id = id };
                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Delete operation. Internal server error: {ex.Message}");
            }

        }
        
        #endregion
    }
}
