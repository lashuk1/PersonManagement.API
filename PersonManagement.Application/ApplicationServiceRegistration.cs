namespace PersonManagement.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPersonService, PersonService>();


        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
