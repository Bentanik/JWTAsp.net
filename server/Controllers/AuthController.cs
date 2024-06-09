using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.ViewModels;
using Repository.ViewModels.Requestes;
using Repository.ViewModels.Responses;
using Service.Interfaces;
using Service.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly RedisService _redisService;
        private readonly PasswordService _passwordService;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IUserService userService, IEmailService emailService, RedisService redisService, PasswordService passwordService, IAuthService authService)
        {
            _logger = logger;
            _userService = userService;
            _emailService = emailService;
            _redisService = redisService;
            _passwordService = passwordService;
            _authService = authService;
        }


        // Send code to register by Email
        [HttpPost("sendmailregister")]
        [ProducesResponseType(200, Type = typeof(UserResponse<Object>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Register([FromBody] [EmailAddress]string email)
        {
            var user = await _userService.GetUserByEmail(email);

            if (user != null)
                return BadRequest(new ErrorResponse(1,"This email already exists"));

            Random random = new Random();
            int code = random.Next(100000, 999999);

            EmailDto reciver = new()
            {
                To = email,
                Subject = "Register an account",
                Body = @$"<p>This is the code: {code}</p>
                        <br/>
                        The code lasts for 5 minutes
                        "
            };

            var isCheckSendMail = await _emailService.SendMailRegister(reciver);
            if (isCheckSendMail == false)
            {
                return BadRequest(new ErrorResponse(1, "Sending OTP code failed, please try again"));
            }

            await _redisService.SetStringAsync(email, code.ToString(), TimeSpan.FromMinutes(5));

            return Ok(new UserResponse<object>(0, "Please check the code in the email", null));
        }

        //Check code when send mail to register
        [HttpPost("checkcode")]
        [ProducesResponseType(200, Type = typeof(UserResponse<Object>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CheckCode([FromBody] RegisterCodeRequest request)
        {
            var code = await _redisService.GetStringAsync(request.Email);
            if(code != request.Code)
            {
                return BadRequest(new ErrorResponse(1, "Wrong code, please enter again!"));
            }
            await _redisService.DeleteKeyAsync(request.Email);
            return Ok(new UserResponse<object>(0, "Check code successfully", request.Email));
        }


        // Register user
        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(UserResponse<Object>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (registerRequest.Password != registerRequest.ConfirmPassword)
                return BadRequest(new ErrorResponse(1, "Password does not match confirm password"));

            string passwordHasher =  _passwordService.HashPassword(registerRequest.Password);
            UserDto user = new()
            {
                Id = Guid.NewGuid(),
                Email = registerRequest.Email,
                DisplayName = registerRequest.DisplayName,
                Password = passwordHasher,
                RefreshToken = "",
            };
            var createService = await _authService.CreateUser(user);
            return Ok(createService);
        }

        // Login user
        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(UserResponse<Object>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userService.GetUserByEmail(loginRequest.Email);

            if (user == null)
                return BadRequest(new ErrorResponse(1, "This email does not exist"));


            var isCheckPassword = _passwordService.VerifyPassword(
                    loginRequest?.Password,
                    user?.Password
                );

            if (!isCheckPassword)
                return BadRequest(new ErrorResponse(1, "Invalid password."));

            var loginUser = await _authService.LoginUser(user);
            Response.Cookies.Append("refreshToken", loginUser.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Path = "/",
                SameSite = SameSiteMode.Strict,
            });

            return Ok(new UserResponse<object>(0, "Login successfully", new
            {
                TokenType = "Bearer",
                AccessToken = loginUser.AccessToken,
            }));
        }

        // Refresh token
        [HttpPost("refreshtoken")]
        [ProducesResponseType(200, Type = typeof(UserResponse<Object>))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            var refreshToken = await _authService.RefreshToken(token);
            if (refreshToken == null)
                return BadRequest(new ErrorResponse(1, "Token invalid"));
            Response.Cookies.Append("refreshToken", refreshToken.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Path = "/",
                SameSite = SameSiteMode.Strict,
            });

            return Ok(new UserResponse<object>(0, "Login successfully", new
            {
                TokenType = "Bearer",
                AccessToken = refreshToken.AccessToken,
            }));
        }

        // Logout
        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout([FromQuery]Guid userId)
        {
            Response.Cookies.Delete("refreshToken");
            await _authService.DeleteRefreshToken(userId);
            return Ok(new UserResponse<Object>(0, "Logout successfully", null));
        }
    }
}
