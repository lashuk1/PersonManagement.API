namespace PersonManagement.API.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection RegisterProjectServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();

        return services;
    }
}