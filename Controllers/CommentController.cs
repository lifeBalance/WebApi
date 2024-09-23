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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = CommentMapper.ToCommentDto(comment);

            return Ok(commentDto);
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            System.Console.WriteLine("stockId: " + stockId);
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
    }
}