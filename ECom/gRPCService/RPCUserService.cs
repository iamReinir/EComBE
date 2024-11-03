using ECom.Service;
using EComBusiness.HelperModel;
using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using UsermanagementGrpc;

namespace ECom.gRPCService
{
    public class RPCUserService : UserManagement.UserManagementBase
    {

        EComContext _context;
        IConfiguration _config;
        public RPCUserService(EComContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<ListUsersResponse> Index(ListUsersRequest request, ServerCallContext context)
        {
            int count = await _context.AppUsers
                .NotDeleted()
                .CountAsync();
            var users = await _context.AppUsers
                .NotDeleted()
                .Select(user => new UsermanagementGrpc.User
                {
                    Id = user.UserId,
                    Email = user.Email,
                    Name = user.Name,
                    Password = user.PasswordHash,
                    Phonenumber = user.PhoneNumber,
                    Address = user.Address,
                    Role = Role.Map(user.Role),
                })
                .ToListAsync();
            var resp = new ListUsersResponse
            {
                TotalCount = count,
                Users = { users }
            };
            return resp;
        }

        // RPCUserService/Details/5
        public async Task<UserResponse> Details(GetUserRequest request, ServerCallContext context)
        {
            var cur = await _context.AppUsers
                .NotDeleted()
                .SingleOrDefaultAsync(u
                => u.UserId.Equals(request.Id)) ?? new EComBusiness.Entity.User();
            var found = !string.IsNullOrEmpty(cur.UserId);

            return new UserResponse
            {
                User = new User
                {
                    Id = cur.UserId,
                    Address = cur.Address,
                    Email = cur.Email,
                    Name = cur.Name,
                    Password = cur.PasswordHash,
                    Phonenumber = cur.PhoneNumber,
                    Role = Role.Map(cur.Role),
                },
                IsOk = found,
                Msg = found ? string.Empty : "User not found"
            };
        }

        // GET: RPCUserService/Edit/5
        public async Task<UserResponse> Update(UpdateUserRequest request, ServerCallContext context)
        {
            var user = await _context.AppUsers
                .NotDeleted()
                .FirstOrDefaultAsync(u => u.Email == request.Email) ?? new EComBusiness.Entity.User();
            var found = !string.IsNullOrEmpty(user.UserId);

            var hashedPassword = PasswordHelper.HashPassword(request.Password);
            if(!string.IsNullOrEmpty(user.UserId))
            {    
                user.Name = request.Name;
                user.PasswordHash = hashedPassword;
                user.Address = request.Address;
                user.PhoneNumber = request.Phonenumber;
                user.Role = Role.Map(request.Role);
                await _context.SaveChangesAsync();
            }

            return new UserResponse
            {
                User = new User
                {
                    Id = user.UserId,
                    Email = user.Email,
                    Name = user.Name,
                    Phonenumber = user.PhoneNumber,
                    Address = user.Address,
                    Password = hashedPassword,
                    Role = Role.Map(user.Role)
                },
                IsOk = found,
                Msg = found ? string.Empty : "User not found"
            };
        }

        public async Task<UserResponse> Delete(DeleteUserRequest request, ServerCallContext context)
        {
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u => u.UserId == request.Id) ?? new EComBusiness.Entity.User();
            var found = !string.IsNullOrEmpty(user.UserId);
            if (!string.IsNullOrEmpty(user.UserId))
            {
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return new UserResponse
            {
                User = new User
                {
                    Id = user.UserId,
                    Email = user.Email,
                    Name = user.Name,
                    Phonenumber = user.PhoneNumber,
                    Address = user.Address,
                    Password = user.PasswordHash,
                    Role = Role.Map(user.Role)
                },
                IsOk = found,
                Msg = found ? string.Empty : "User not found"
            };
        }
    }
}
