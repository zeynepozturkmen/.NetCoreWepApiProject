using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.DbContexts;
using WebApiProject.Entities;
using WebApiProject.Interface;

namespace WebApiProject.Repository
{
    public class UserRepository : Repository<User>,IUserRepository
    {
        public UserRepository(UserDbContext dbContext):base(dbContext)
        {

        }
    }
}
