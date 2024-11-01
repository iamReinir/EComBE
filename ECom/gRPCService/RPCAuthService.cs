using ECom.Proto;
using Grpc.Core;

namespace ECom.gRPCService
{
    public class RPCAuthService : Auth.AuthBase
    {
        EComContext _context;
        public RPCAuthService(EComContext context)
        {
            _context = context;
        }

        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            return base.Login(request, context);
        }

        public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            return base.Register(request, context);
        }
    }
}
