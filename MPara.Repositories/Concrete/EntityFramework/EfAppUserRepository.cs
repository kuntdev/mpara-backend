using System;
using Microsoft.EntityFrameworkCore;
using MPara.Repositories.Abstract;
using MPara.Repositories.Entity;

namespace MPara.Repositories.Concrete.EntityFramework
{
    public class EfAppUserRepository : EfRepository<AppUser>, IAppUserRepository
    {
        public EfAppUserRepository(DbContext context) : base(context)
        {
        }
    }
}
