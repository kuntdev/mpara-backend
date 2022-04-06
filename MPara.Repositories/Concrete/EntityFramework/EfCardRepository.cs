using System;
using Microsoft.EntityFrameworkCore;
using MPara.Repositories.Abstract;
using MPara.Repositories.Entity;

namespace MPara.Repositories.Concrete.EntityFramework
{
    public class EfCardRepository : EfRepository<Card>, ICardRepository
    {
        public EfCardRepository(DbContext context) : base(context)
        {
        }
    }
}
