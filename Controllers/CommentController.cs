using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;

        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
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
    }
}