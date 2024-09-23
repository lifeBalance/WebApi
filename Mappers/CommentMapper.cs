using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            };
        }
    }
}