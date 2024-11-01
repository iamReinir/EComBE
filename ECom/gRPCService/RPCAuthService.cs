using ECom.Proto;
using ECom.Service;
using EComBusiness.Entity;
using EComBusiness.HelperModel;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace ECom.gRPCService
{
    public class RPCAuthService : Auth.AuthBase
    {
        EComContext _context;
        IConfiguration _config;
        public RPCAuthService(EComContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var response = new LoginResponse
            {
                IsOk = false,
                Msg = string.Empty,
                Token = string.Empty
            };
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (_config["DefaultAccount:Email"].Equals(request.Email)
               && _config["DefaultAccount:Password"].Equals(request.Password))
            {
                user = new User
                {
                    UserId = request.Email,
                    Name = "DefaultAdmin",
                    Role = UserRole.Admin,
                    Email = request.Email,
                };
            }
            else if (user == null 
                || user.Role != UserRole.Admin
                || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                response.IsOk = false;
                response.Msg = "Invalid credentials.";
                return response;
            }

            var token = PasswordHelper.GenerateJwtToken(user, _config["JWT:Key"]);
            response.IsOk = true;
            response.Token = token;
            response.Msg = "Login success.";
            return response;
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var response = new RegisterResponse
            {
                IsOk = false,
                Msg = string.Empty
            };

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                response.IsOk = false;
                response.Msg = "Email exist.";
                return response;
            }
            var hashedPassword = PasswordHelper.HashPassword(request.Password);

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Role = UserRole.Customer
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            response.IsOk = true;
            response.Msg = "Register success.";
            return response;
        }
    }
}
