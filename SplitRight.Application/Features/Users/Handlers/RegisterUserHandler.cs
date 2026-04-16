using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;
using SplitRight.Application.Features.Users.Commands;
using SplitRight.Domain.Contracts.Entities;
using SplitRight.Domain.Contracts.Enums;
using SplitRight.Domain.Contracts.Interfaces;
using SplitRight.Domain.Exceptions;
using SplitRight.Infrastructure.Contracts.Interfaces;

namespace SplitRight.Application.Features.Users.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public RegisterUserHandler(IUserRepository userRepository, IPasswordHasherService passwordHasher,
                                   IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<UserDTO>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.UtcNow;
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            if (!_currentUser.IsAdmin && request.Role == UserRole.Admin) throw new OwnershipViolationException("Você nao pode criar outro usuário Admin.");

            var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
            if (existingUser is not null) throw new BusinessRuleException("Email já cadastrado.");

            var passwordHash = _passwordHasher.Hash(existingUser!, request.Password);
            var user = new User(request.Name, normalizedEmail, passwordHash, UserRole.User, now);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new UserDTO(user.Id, user.Name, user.Email);
            return Result<UserDTO>.Success(response);
        }
    }
}
