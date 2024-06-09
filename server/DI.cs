using Repository.Helpers;
using Repository.Interfaces;
using Repository.Repositories;
using Service.Interfaces;
using Service.Repositories;

namespace server;

public static class DI
{

    public static IServiceCollection AddWebAppDepdencyInjection(this IServiceCollection services)
    {
        //Add Auto Mapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        //Add Redis
        services.AddSingleton<RedisService>();
        //Add BcryptPassword
        services.AddSingleton<PasswordService>();
        //Add TokenGenerators
        services.AddSingleton<TokenGenerators>();

        #region ServiceDepedency
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthService>();
        #endregion

        #region RepositoryDepedency
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        #endregion
        return services;
    }
}

