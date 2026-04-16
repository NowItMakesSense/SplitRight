using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;
using SplitRight.Application.Features.Auth.Commands;
using SplitRight.Domain.Contracts.Entities;
using SplitRight.Domain.Contracts.Interfaces;
using SplitRight.Domain.Exceptions;
using SplitRight.Infrastructure.Contracts.Interfaces;

namespace SplitRight.Application.Features.Auth.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IJwtTokenGeneratorService _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IUserRefreshTokenRepository _refreshTokenRepository;

        public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasherService passwordHasher, IUnitOfWork unitOfWork,
                                       ITokenService tokenService, IUserRefreshTokenRepository refreshTokenRepository, 
                                       IJwtTokenGeneratorService jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<LoginDTO>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null) throw new BusinessRuleException("Credenciais inválidas.");

            var isValidPassword = _passwordHasher.Verify(user, request.Password, user.PasswordHash);
            if (!isValidPassword) throw new BusinessRuleException("Credenciais inválidas.");

            var sessionId = Guid.NewGuid();

            var accessToken = _tokenService.GenerateAccessToken(user, sessionId);
            var refreshTokenValue = _tokenService.GenerateRefreshToken();

            var refreshToken = new UserRefreshToken(
                user.Id,
                sessionId,
                refreshTokenValue,
                DateTimeOffset.UtcNow.AddDays(7)
            );

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var currentUser = new UserDTO(user.Id, user.Name, user.Email);

            return Result<LoginDTO>.Success(new LoginDTO(
                accessToken,
                refreshTokenValue,
                currentUser,
                DateTimeOffset.UtcNow
            ));
        }
    }
}
