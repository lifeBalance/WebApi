using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id).AsTask();
        }

        public async Task<Comment> CreateCommentAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> UpdateCommentAsync(int id, UpdateCommentRequestDto commentDto)
        {
            var existingCommentModel = await _context.Comments.FindAsync(id);
            if (existingCommentModel == null)
            {
                return null;
            }

            existingCommentModel.Title = commentDto.Title;
            existingCommentModel.Content = commentDto.Content;

            // The model is already being tracked by the context
            await _context.SaveChangesAsync();

            return existingCommentModel;
        }
    }
}