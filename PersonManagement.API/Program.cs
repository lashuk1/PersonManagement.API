var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.RegisterApplicationServices();
    builder.Services.RegisterProjectServices();

    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new[] { "en-US", "ka-GE" };
        options.DefaultRequestCulture = new RequestCulture("en-US");
        options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
        options.SupportedUICultures = options.SupportedCultures;
    });

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();

#pragma warning disable CS0618 
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidationActionFilter>(); 
    })
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<PersonCreateDtoValidator>();
    });
#pragma warning restore CS0618 

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionHandlingMiddleware>(); 
    app.UseMiddleware<LocalizationMiddleware>();    

    app.UseHttpsRedirection();
    app.UseAuthorization();

    app.MapControllers();
}
