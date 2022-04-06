using System;
using Microsoft.EntityFrameworkCore;
using MPara.Repositories.Abstract;
using MPara.Repositories.Entity;

namespace MPara.Repositories.Concrete.EntityFramework
{
    public class EfFastTransferRepository : EfRepository<FastTransfer>, IFastTransferRepository
    {
        public EfFastTransferRepository(DbContext context) : base(context)
        {
        }
    }
}
