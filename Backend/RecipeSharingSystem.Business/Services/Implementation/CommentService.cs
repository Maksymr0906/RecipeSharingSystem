using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Comment;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Data.Entities;
using RecipeSharingSystem.Data.Repositories.Interfaces;

namespace RecipeSharingSystem.Business.Services.Implementation
{
	public class CommentService : ICommentService
	{
		private readonly ICommentRepository _repository;
		private readonly IMapper _mapper;

		public CommentService(ICommentRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<CommentDto> CreateCommentAsync(CreateCommentRequestDto model)
		{
			var comment = _mapper.Map<Comment>(model);
			comment = await _repository.CreateAsync(comment);
			return _mapper.Map<CommentDto>(comment);
		}

		public async Task<ICollection<CommentDto>> GetAllCommentsAsync()
		{
			var comments = await _repository.GetAllAsync();
			return _mapper.Map<ICollection<CommentDto>>(comments);
		}

		public async Task<CommentDto> GetCommentByIdAsync(Guid id)
		{
			var comment = await _repository.GetByIdAsync(id);
			return _mapper.Map<CommentDto>(comment);
		}

		public async Task<CommentDto> UpdateCommentAsync(Guid id, UpdateCommentRequestDto model)
		{
			var comment = _mapper.Map<Comment>(model);
			comment.Id = id;
			comment = await _repository.UpdateAsync(comment);
			return _mapper.Map<CommentDto>(comment);
		}

		public async Task<CommentDto> DeleteCommentAsync(Guid id)
		{
			var comment = await _repository.DeleteByIdAsync(id);
			return _mapper.Map<CommentDto>(comment);
		}
	}
}
