using System;
using Microsoft.EntityFrameworkCore;
using MPara.Repositories.Entity;

namespace MPara.Repositories.Concrete
{
    public class MParaDbContext : DbContext
    {
        public MParaDbContext(DbContextOptions<MParaDbContext> options) : base(options){}

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<FastTransfer> FastTransfers { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
    }
}
