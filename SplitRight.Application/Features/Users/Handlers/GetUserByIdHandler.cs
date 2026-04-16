using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;
using SplitRight.Application.Features.Users.Commands;
using SplitRight.Domain.Contracts.Interfaces;
using SplitRight.Domain.Exceptions;
using SplitRight.Infrastructure.Contracts.Interfaces;

namespace SplitRight.Application.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdCommand, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;

        public GetUserByIdQueryHandler(IUserRepository userRepository, ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<Result<UserDTO>> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user is null) throw new NotFoundException("Usuario não encontrado.");
            if (!_currentUser.IsAdmin && user.Id != _currentUser.UserId) throw new OwnershipViolationException("Você não ver este usuário.");

            var response = new UserDTO(user.Id, user.Name, user.Email);
            return Result<UserDTO>.Success(response);
        }
    }
}
