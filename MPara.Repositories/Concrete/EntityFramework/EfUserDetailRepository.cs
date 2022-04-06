using System;
using Microsoft.EntityFrameworkCore;
using MPara.Repositories.Abstract;
using MPara.Repositories.Entity;

namespace MPara.Repositories.Concrete.EntityFramework
{
    public class EfUserDetailRepository : EfRepository<UserDetail>, IUserDetailRepository
    {
        public EfUserDetailRepository(DbContext context) : base(context)
        {
        }
    }
}
