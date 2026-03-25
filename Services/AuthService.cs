using QueryBuilderApi.Data;
using QueryBuilderApi.Models;

namespace QueryBuilderApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly TokenService _tokenService;

        // Inject
        public AuthService(AppDbContext dbContext, TokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        // REGISTER
        public async Task<ApiResponse<User>> Register(RegisterDto dto)
        {
            
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (existingUser != null)
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Email already registered",
                    Data = null
                };

            
            var passwordHash = _tokenService.HashPassword(dto.Password);

           
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User registered successfully",
                Data = user
            };
        }

        // LOGIN: 
        public async Task<ApiResponse<AuthResponse>> Login(LoginDto dto)
        {
            
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
            {
                return new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Data = null
                };
            }

            
            if (!_tokenService.VerifyPassword(dto.Password, user.PasswordHash))
                return new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Data = null
                };

            
            var token = _tokenService.GenerateJwtToken(user);

            
            var response = new AuthResponse
            {
                Token = token,
                Message = "Login successful",
                User = user
            };

            return new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Login successful",
                Data = response 
            };
        }

    }
}