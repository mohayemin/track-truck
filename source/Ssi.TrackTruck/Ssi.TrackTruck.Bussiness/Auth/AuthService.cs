﻿using System.Collections.Generic;
using System.Linq;
using Ssi.TrackTruck.Bussiness.Clients;
using Ssi.TrackTruck.Bussiness.DAL;
using Ssi.TrackTruck.Bussiness.DAL.Entities;
using Ssi.TrackTruck.Bussiness.Models;

namespace Ssi.TrackTruck.Bussiness.Auth
{
    public class AuthService
    {
        private readonly IRepository _repository;
        private readonly ClientService _clientService;
        private readonly IHasher _hasher;

        public AuthService(IRepository repository, IHasher hasher, ClientService clientService)
        {
            _repository = repository;
            _hasher = hasher;
            _clientService = clientService;
        }

        public Response AuthenticateUser(SignInRequest request)
        {
            if (request.Validate())
            {
                var user = FindByUsername(request.Username);
                var valid = user != null && _hasher.Match(request.Password, user.PasswordHash);
                if (valid)
                {
                    return Response.Success(null, "Verified, redirecting...");
                }

                return Response.Error("InvalidCredentials", "Username and password does not match");
            }
            return Response.Error("Validation", "Please enter both username and password");
        }

        private DbUser FindByUsername(string username)
        {
            var usernameLower = username.ToLower();
            var user = _repository.FindOne<DbUser>(u => u.UsernameLowerCase == usernameLower);
            return user;
        }

        public Response CreateUser(AddUserRequest request)
        {
            var validation = request.Validate();
            if (validation.IsError)
            {
                return validation;
            }
            if (FindByUsername(request.Username) != null)
            {
                return Response.DuplicacyError("A user with this name is already registered");
            }
            if (request.Role == Role.BranchCustodian)
            {
                var client = _clientService.GetClient(request.ClientId);
                    
                if (client == null)
                {
                    return Response.ValidationError("The client you specified does not exist");
                }
                var branch = client.Branches.FirstOrDefault(dbBranch => dbBranch.Id == request.BranchId);
                if (branch == null)
                {
                    return Response.ValidationError("The branch you specified does not exist");
                }
            }
            var user = CreateUserObject(request.Username, request.InitialPassword, request.Role);
            _repository.Create(user);
            return Response.Success(user, "User Added");
        }

        public DbUser CreateUserObject(string username, string password, Role roles)
        {
            var user = new DbUser
            {
                Username = username,
                PasswordHash = _hasher.GenerateHash(password),
                UsernameLowerCase = username.ToLower(),
                Role = roles
            };
            return user;
        }

        public IEnumerable<UserListResponseItem> GetUserList()
        {
            return _repository.GetAll<DbUser>().Select(user => new UserListResponseItem
            {
                Username = user.Username,
                Role = user.Role
            });
        }

        public Response ChangePassword(ChangePasswordRequest request, string username)
        {
            var response = request.Validate();
            if (response.IsError)
            {
                return response;
            }

            var user = _repository.FindOne<DbUser>(u => u.UsernameLowerCase == username.ToLower());
            if (user == null)
            {
                return Response.ValidationError("User not found");
            }
            if (!IsValidCurrentPassword(request.CurrentPassword, user.PasswordHash))
            {
                return Response.ValidationError("Provided current password is invalid");
            }

            user.PasswordHash = _hasher.GenerateHash(request.NewPassword);
            _repository.Save(user);
            return Response.Success();
        }

        private bool IsValidCurrentPassword(string currentPassword, string dbPassword)
        {
            return _hasher.Match(currentPassword, dbPassword);
        }
    }
}
