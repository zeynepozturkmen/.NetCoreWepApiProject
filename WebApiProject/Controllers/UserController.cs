using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.Entities;
using WebApiProject.IService;
using WebApiProject.RequestModel;
using WebApiProject.ResponseModel;

namespace WebApiProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] UserLoginRequestModel userParam)
        {

            var user = _userService.Authenticate(userParam);

            if (user == null)
                return BadRequest(new { message = "Kullanici veya şifre hatalı!" });

            return Ok(user);
        }


        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users =await _userService.GetAllUserAsync();

            if (users == null)
                return BadRequest(new { message = "Kullanicilar yok!" });

            return Ok(users);
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel model)
        {
            var user =await _userService.CreateUserAsync(model);
            if (user == null)
                return BadRequest(new { message = "Kullanici kaydedilirken hata oluştu!" });

            var resModel = new ByNameResponseModel()
            {
                UserName = user.UserName
            };

            return Ok(resModel);
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestModel model)
        {
            var user = await _userService.UpdateUserAsync(model);
            if (user == null)
                return BadRequest(new { message = "Kullanici güncellenirken hata oluştu!" });

            var resModel = new ByNameResponseModel()
            {
                UserName = user.UserName
            };

            return Ok(resModel);
        }

        [HttpPost("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] ByIdRequestModel model)
        {
            if (model.Id==0)
            {
                return BadRequest(new { message = "Geçersiz id!" });
            }
            var user = await _userService.DeleteUserAsync(model);
            if (user == null)
                return BadRequest(new { message = "Kullanici silinirken hata oluştu!" });

            var resModel = new ByNameResponseModel()
            {
                UserName = user.UserName
            };

            return Ok(resModel);
        }

    }
}
