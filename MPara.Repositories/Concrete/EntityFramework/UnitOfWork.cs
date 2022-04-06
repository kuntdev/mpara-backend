using System;
using MPara.Repositories.Abstract;
using MPara.Repositories.Abstract;

namespace MPara.Repositories.Concrete.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MParaDbContext context;

        public UnitOfWork(MParaDbContext _context)
        {
            context = _context ?? throw new ArgumentNullException("dbContext can not be null");
        }

        private EfAppUserRepository _appUsers;
        private EfAccountRepository _accounts;
        private EfTransferRepository _transfers;
        private EfFastTransferRepository _fastTransfers;
        private EfCardRepository _cards;
        private EfUserDetailRepository _userDetails;


        public IAppUserRepository AppUserRepository { get { return _appUsers ?? (_appUsers = new EfAppUserRepository(context)); } }
        public IAccountRepository AccountRepository { get { return _accounts ?? (_accounts = new EfAccountRepository(context)); } }
        public ITransferRepository TransferRepository { get { return _transfers ?? (_transfers = new EfTransferRepository(context)); } }
        public IFastTransferRepository FastTransferRepository { get { return _fastTransfers ?? (_fastTransfers = new EfFastTransferRepository (context)); } }
        public ICardRepository CardRepository { get { return _cards ?? (_cards = new EfCardRepository (context)); } }
        public IUserDetailRepository UserDetailRepository { get { return _userDetails ?? (_userDetails = new EfUserDetailRepository (context)); } }



        public void Dispose()
        {
            context.Dispose();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }
    }
}
