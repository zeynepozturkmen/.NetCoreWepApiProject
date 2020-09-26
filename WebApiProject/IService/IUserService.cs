using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Entities;
using WebApiProject.RequestModel;
using WebApiProject.ResponseModel;

namespace WebApiProject.IService
{
    public interface IUserService : IService<User>
    {
        GetTokenResponseModel Authenticate(UserLoginRequestModel model);
        Task<List<UserListResponseModel>> GetAllUserAsync();
        Task<User> CreateUserAsync(CreateUserRequestModel model);
        Task<User> UpdateUserAsync(UpdateUserRequestModel model);
        Task<User> DeleteUserAsync(ByIdRequestModel Id);

    }
}
