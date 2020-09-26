using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.ResponseModel
{
    public class GetTokenResponseModel
    {
        public string Token { get; set; }
        public UserResponseModel User { get; set; } = new UserResponseModel();
        public string ExpireTime { get; set; }
    }
}
