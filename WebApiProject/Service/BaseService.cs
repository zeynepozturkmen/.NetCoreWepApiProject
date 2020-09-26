using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Interface;
using WebApiProject.IService;

namespace WebApiProject.Service
{
    public  class BaseService<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;
        public BaseService(IRepository<T> repository)
        {
            _repository = repository;
        }
    }
}
