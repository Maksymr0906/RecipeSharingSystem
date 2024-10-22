using AutoMapper;
using RecipeSharingSystem.Business.DTOs.Comment;
using RecipeSharingSystem.Business.Services.Interfaces;
using RecipeSharingSystem.Core.Repositories;
using RecipeSharingSystem.Core.Entities;

namespace RecipeSharingSystem.Business.Services.Implementation;

public class CommentService(IUnitOfWork unitOfWork, IMapper mapper) : ICommentService
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IMapper _mapper = mapper;

	public async Task<CommentDto> CreateCommentAsync(CreateCommentRequestDto model)
	{
		var comment = _mapper.Map<Comment>(model);
		comment = await _unitOfWork.CommentRepository.CreateAsync(comment);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<CommentDto>(comment);
	}

	public async Task<ICollection<CommentDto>> GetAllCommentsAsync()
	{
		var comments = await _unitOfWork.CommentRepository.GetAllAsync();
		return _mapper.Map<ICollection<CommentDto>>(comments);
	}

	public async Task<CommentDto> GetCommentByIdAsync(Guid id)
	{
		var comment = await _unitOfWork.CommentRepository.GetByIdAsync(id);
		return _mapper.Map<CommentDto>(comment);
	}

	public async Task<CommentDto> UpdateCommentAsync(Guid id, UpdateCommentRequestDto model)
	{
		var comment = _mapper.Map<Comment>(model);
		comment.Id = id;
		comment = await _unitOfWork.CommentRepository.UpdateAsync(comment);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<CommentDto>(comment);
	}

	public async Task<CommentDto> DeleteCommentAsync(Guid id)
	{
		var comment = await _unitOfWork.CommentRepository.DeleteByIdAsync(id);
		await _unitOfWork.SaveAsync();
		return _mapper.Map<CommentDto>(comment);
	}
}
