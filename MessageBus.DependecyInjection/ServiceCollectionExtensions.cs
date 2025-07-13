using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MessageBus.Abstractions;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Расширения для регистрации MessageBus с поддержкой нескольких брокеров, DI и IOptions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует RabbitMQ-шину сообщений и связанные сервисы.
    /// </summary>
    /// <typeparam name="TOptions">Тип опций, реализующий IBusOptions.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="optionsName">Имя конфигурации опций (по умолчанию - "RabbitMQ").</param>
    /// <returns>IServiceCollection для fluent-интерфейса.</returns>
    public static IServiceCollection AddRabbitMqBus<TOptions>(this IServiceCollection services, IConfiguration configuration, string optionsName = "RabbitMQ") where TOptions : class, IBusOptions
    {
        services.Configure<TOptions>(configuration.GetSection(optionsName));
        services.AddSingleton<MessageBus.Abstractions.IBus>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<TOptions>>().Value;
            return new MassTransitBus(opts);
        });
        return services;
    }

    /// <summary>
    /// Регистрирует Kafka-шину сообщений и связанные сервисы через MassTransit.
    /// </summary>
    /// <typeparam name="TOptions">Тип опций, реализующий IBusOptions.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="optionsName">Имя конфигурации опций (по умолчанию - "Kafka").</param>
    /// <param name="configure">Делегат для дополнительной настройки MassTransit.</param>
    /// <returns>IServiceCollection для fluent-интерфейса.</returns>
    public static IServiceCollection AddKafkaBus<TOptions>(this IServiceCollection services, IConfiguration configuration, string optionsName = "Kafka", Action<IBusRegistrationConfigurator>? configure = null) where TOptions : class, IBusOptions
    {
        services.Configure<TOptions>(configuration.GetSection(optionsName));
        // Для Kafka используйте AddMassTransitRider и AddRider, пример см. в README.md
        throw new NotSupportedException("Для Kafka используйте AddMassTransitRider и AddRider. Пример в README.md.");
    }
} 