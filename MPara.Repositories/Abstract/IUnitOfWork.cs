using System;

namespace MPara.Repositories.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IAppUserRepository AppUserRepository { get; }
        IAccountRepository AccountRepository { get; }
        ITransferRepository TransferRepository { get; }
        IFastTransferRepository FastTransferRepository { get; }
        ICardRepository CardRepository { get; }
        IUserDetailRepository UserDetailRepository { get; }


        int SaveChanges();
    }
}
