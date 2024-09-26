using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.Interfaces;
using WebApi.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepo.GetAllCommentsAsync();

            var commentDto = comments
                .Select(c => CommentMapper.ToCommentDto(c));

            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCommentById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = CommentMapper.ToCommentDto(comment);

            return Ok(commentDto);
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModelExists = await _stockRepo.StockExistsAsync(stockId);
            if (!stockModelExists)
            {
                return BadRequest("Stock not found");
            }
            var commentModel = commentDto.ToCommentFromCreate(stockId);
            var createdComment = await _commentRepo.CreateCommentAsync(commentModel);

            var commentResponse = CommentMapper.ToCommentDto(createdComment);

            return CreatedAtAction(nameof(GetCommentById), new { id = createdComment.Id }, commentResponse);
        }

        [HttpPut("{stockId:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int stockId, [FromBody] UpdateCommentRequestDto updatedCommentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            System.Console.WriteLine("stockId: " + stockId);
            var comment = await _commentRepo.UpdateCommentAsync(stockId, updatedCommentDto);
            if (comment == null)
            {
                return BadRequest("Comment not found");
            }

            return Ok(CommentMapper.ToCommentDto(comment));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await _commentRepo.DeleteCommentAsync(id);
            if (commentModel == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(CommentMapper.ToCommentDto(commentModel));
        }
    }
}