using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;
using SplitRight.Application.Features.Users.Commands;
using SplitRight.Domain.Contracts.Interfaces;
using SplitRight.Domain.Exceptions;
using SplitRight.Infrastructure.Contracts.Interfaces;

namespace SplitRight.Application.Features.Users.Handlers
{
    public class RemoveUserHandler : IRequestHandler<RemoveUserCommand, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RemoveUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<UserDTO>> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.UtcNow;

            var existingUser = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingUser is null) throw new BusinessRuleException("Usuario nao cadastrado.");
            if (!_currentUser.IsAdmin && existingUser.Id != _currentUser.UserId) throw new OwnershipViolationException("Você não pode alterar este usuário.");

            existingUser.Delete(now);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new UserDTO(existingUser.Id, existingUser.Name, existingUser.Email);
            return Result<UserDTO>.Success(response);
        }
    }
}
