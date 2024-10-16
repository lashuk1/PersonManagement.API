namespace PersonManagement.Infrastructure.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IPersonRepository PersonRepository { get; }
    IPhoneNumberRepository PhoneNumberRepository { get; }

    Task<int> SaveChangesAsync();
}