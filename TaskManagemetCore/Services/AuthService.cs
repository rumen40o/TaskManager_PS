using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagemetCore.DTOs;
using TaskManagemetCore.Enums;
using TaskManagemetCore.Interfaces;
using TaskManagemetCore.Models;

namespace TaskManagemetCore.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public LoginResult Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    Message = "Username is required."
                };
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    Message = "Password is required."
                };
            }

            var user = _userRepository.GetByUsername(request.Username);

            if (user == null || user.PasswordHash != request.Password)
            {
                return new LoginResult
                {
                    IsSuccess = false,
                    Message = "Invalid username or password."
                };
            }

            return new LoginResult
            {
                IsSuccess = true,
                Message = "Login successful.",
                User = user
            };
        }

        public RegisterUserResult Register(RegisterUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return new RegisterUserResult
                {
                    IsSuccess = false,
                    Message = "Username is required."
                };
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return new RegisterUserResult
                {
                    IsSuccess = false,
                    Message = "Email is required."
                };
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return new RegisterUserResult
                {
                    IsSuccess = false,
                    Message = "Password is required."
                };
            }

            if (_userRepository.GetByUsername(request.Username) != null)
            {
                return new RegisterUserResult
                {
                    IsSuccess = false,
                    Message = "Username already exists."
                };
            }

            if (_userRepository.GetByEmail(request.Email) != null)
            {
                return new RegisterUserResult
                {
                    IsSuccess = false,
                    Message = "Email already exists."
                };
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.Password,
                Role = UserRole.USER,
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.Add(user);

            return new RegisterUserResult
            {
                IsSuccess = true,
                Message = "Registration successful.",
                User = user
            };
        }
    }
}
