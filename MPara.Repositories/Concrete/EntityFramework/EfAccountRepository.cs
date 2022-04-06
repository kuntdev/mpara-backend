using System;
using Microsoft.EntityFrameworkCore;
using MPara.Repositories.Abstract;
using MPara.Repositories.Entity;

namespace MPara.Repositories.Concrete.EntityFramework
{
    public class EfAccountRepository : EfRepository<Account>, IAccountRepository
    {
        public EfAccountRepository(DbContext context) : base(context)
        {
        }
    }
}
