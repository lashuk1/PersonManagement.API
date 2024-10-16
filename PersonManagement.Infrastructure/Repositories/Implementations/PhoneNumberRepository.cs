using PersonManagement.Domain.Entities;
using PersonManagement.Infrastructure.Data;
using PersonManagement.Infrastructure.Repositories.Interfaces;

namespace PersonManagement.Infrastructure.Repositories.Implementations;

public class PhoneNumberRepository : BaseRepository<PhoneNumber>, IPhoneNumberRepository
{
    public PhoneNumberRepository(AppDbContext context) : base(context)
    {
    }
}