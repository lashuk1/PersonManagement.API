namespace PersonManagement.Infrastructure.Repositories.Implementations;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;
    private IPersonRepository _personRepository;
    private IPhoneNumberRepository _phoneNumberRepository;

    public IPersonRepository PersonRepository =>
        _personRepository ??= new PersonRepository(_context);
    public IPhoneNumberRepository PhoneNumberRepository =>
            _phoneNumberRepository ??= new PhoneNumberRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}