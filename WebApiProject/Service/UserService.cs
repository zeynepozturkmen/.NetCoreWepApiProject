using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiProject.Entities;
using WebApiProject.Helper;
using WebApiProject.Interface;
using WebApiProject.IService;
using WebApiProject.RequestModel;
using WebApiProject.ResponseModel;

namespace WebApiProject.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly AppSettings _appSettings;
        public UserService(IRepository<User> userRepository, IOptions<AppSettings> appSettings) : base(userRepository)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }
        public GetTokenResponseModel Authenticate(UserLoginRequestModel model)
        {
            var user = _userRepository.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();

            // Kullanici bulunamadıysa null döner.
            if (user == null)
                return null;

            // Authentication başarılı ise JWT token üretilir.
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString())
                }),
                Expires = DateTime.Now.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var resModel = new GetTokenResponseModel();
            resModel.Token = tokenHandler.WriteToken(token);
            resModel.ExpireTime = DateTime.Now.AddHours(6).ToString("yyyy-MM-ddTHH:mm:ssZ");

            // Sifre null olarak gonderilir.
            resModel.User.Id = user.Id;
            resModel.User.UserName = user.UserName;

            return resModel;
        }

        public async Task<List<UserListResponseModel>> GetAllUserAsync()
        {
            // Kullanicilar sifre olmadan dondurulur.

            var users = await _userRepository.GetAllAsync();

            var model = new List<UserListResponseModel>();
            foreach (var item in users)
            {
                var md = new UserListResponseModel();
                md.Id = item.Id;
                md.UserName = item.UserName;
                md.Email = item.Email;
                md.PhoneNumber = item.PhoneNumber;
                md.Address = item.Address;

                model.Add(md);
            }

            return model;
        }

        public async Task<User> CreateUserAsync(CreateUserRequestModel model)
        {
            var user = new User();
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Address = model.Address;
            user.Email = model.Email;
            user.Password = model.Password;

            await _userRepository.AddAsync(user);

            return user; ;
        }

        public async Task<User> UpdateUserAsync(UpdateUserRequestModel model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);
            if (user != null)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Address = model.Address;
                user.Email = model.Email;
                user.Password = model.Password;

                _userRepository.Update(user);

            }

            return user;
        }

        public async Task<User> DeleteUserAsync(ByIdRequestModel model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);
            if (user != null)
            {
                user.IsDeleted = true;
                _userRepository.Update(user);

            }

            return user;
        }
    }
}
