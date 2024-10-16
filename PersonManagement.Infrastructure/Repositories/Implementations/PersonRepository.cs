namespace PersonManagement.Infrastructure.Repositories.Implementations;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    public PersonRepository(AppDbContext context) : base(context)
    {
    }
}