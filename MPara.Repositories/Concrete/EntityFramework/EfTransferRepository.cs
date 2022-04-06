using System;
using Microsoft.EntityFrameworkCore;
using MPara.Repositories.Abstract;
using MPara.Repositories.Entity;

namespace MPara.Repositories.Concrete.EntityFramework
{
    public class EfTransferRepository : EfRepository<Transfer>, ITransferRepository
    {
        public EfTransferRepository(DbContext context) : base(context)
        {
        }
    }
}
