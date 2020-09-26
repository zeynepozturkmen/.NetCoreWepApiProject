using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Entities;

namespace WebApiProject.Interface
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
