using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;
using SplitRight.Application.Features.Users.Commands;
using SplitRight.Domain.Contracts.Interfaces;
using SplitRight.Domain.Exceptions;
using SplitRight.Infrastructure.Contracts.Interfaces;

namespace SplitRight.Application.Features.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateUserHandler(IUserRepository userRepository, IPasswordHasherService passwordHasher, IUnitOfWork unitOfWork,
                                 ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.UtcNow;
            var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (existingUser is null) throw new BusinessRuleException("Usuario não encontrado.");
            if (!_currentUser.IsAdmin && existingUser.Id != _currentUser.UserId) throw new OwnershipViolationException("Você não pode alterar este usuário.");

            existingUser.UpdateProfile(request.Name, now);
            existingUser.ChangeRole(request.Role, now);

            var hash = _passwordHasher.Hash(existingUser, request.Password);
            existingUser.ChangePassword(hash, now);

            _userRepository.Update(existingUser);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new UserDTO(existingUser.Id, existingUser.Name, existingUser.Email);
            return Result<UserDTO>.Success(response);
        }
    }
}
