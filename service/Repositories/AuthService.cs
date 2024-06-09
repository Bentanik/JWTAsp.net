using AutoMapper;
using Repository.Helpers;
using Repository.Interfaces;
using Repository.Models;
using Repository.ViewModels;
using Repository.ViewModels.Responses;
using Service.Interfaces;
using System.Security.Claims;
namespace Service.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;
        private readonly TokenGenerators _tokenGenerators;

        public AuthService(IMapper mapper, IAuthRepository authRepository, TokenGenerators tokenGenerators)
        {
            _mapper = mapper;
            _authRepository = authRepository;
            _tokenGenerators = tokenGenerators;
        }

        public async Task<UserResponse<object>> CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var isCheckCreate = await _authRepository.CreateUser(user);
            return new UserResponse<object>(isCheckCreate ? 0 : 1, isCheckCreate ? "Create user succssfully" : "Create user fail", null);
        }

        public async Task<bool> DeleteRefreshToken(Guid userId)
        {
            return await _authRepository.DeleteRefreshToken(userId);
        }

        public async Task<AuthenticationUserResponse> LoginUser(User user)
        {
            List<Claim> claims = new() {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.RoleCode.RoleName),
            };

            var (accessToken, refreshToken) = _tokenGenerators.GenerateTokens(claims);

            await _authRepository.UpdateRefreshToken(user.Id, refreshToken);

            return new AuthenticationUserResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<AuthenticationUserResponse> RefreshToken(string token)
        {
            //Check token have valid?
            var isValidRefreshToken = _tokenGenerators.ValidateRefreshToken(token);

            if (!isValidRefreshToken) return null;

            //Get refreshToken in database
            var user = await _authRepository.GetRefreshToken(token);

            if (user == null) return null;

            List<Claim> claims = new() {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.RoleCode.RoleName),
            };

            var (accessToken, refreshToken) = _tokenGenerators.GenerateTokens(claims);

            await _authRepository.UpdateRefreshToken(user.Id, refreshToken);

            return new AuthenticationUserResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}
