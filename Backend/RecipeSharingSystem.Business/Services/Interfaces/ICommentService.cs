using RecipeSharingSystem.Business.DTOs.Comment;

namespace RecipeSharingSystem.Business.Services.Interfaces;

public interface ICommentService
{
	Task<CommentDto> CreateCommentAsync(CreateCommentRequestDto model);
	Task<ICollection<CommentDto>> GetAllCommentsAsync();
	Task<CommentDto> GetCommentByIdAsync(Guid id);
	Task<CommentDto> UpdateCommentAsync(Guid id, UpdateCommentRequestDto model);
	Task<CommentDto> DeleteCommentAsync(Guid id);
}
