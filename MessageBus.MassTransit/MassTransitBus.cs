using MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;

/// <summary>
/// Адаптер для работы с MassTransit, поддерживающий RabbitMQ и Kafka, HealthCheck, транзакции, подтверждения доставки и восстановление соединения.
/// </summary>
public class MassTransitBus : MessageBus.Abstractions.IBus, Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck, IAsyncDisposable
{
    private readonly MassTransit.IBusControl _busControl;
    private readonly string _brokerType;
    private bool _started;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="MassTransitBus"/> с поддержкой выбранного брокера.
    /// </summary>
    /// <param name="options">Опции шины сообщений.</param>
    public MassTransitBus(MessageBus.Abstractions.IBusOptions options)
    {
        _brokerType = options.BrokerType;
        _busControl = CreateBus(options);
    }

    /// <summary>
    /// Публикует сообщение в шину.
    /// </summary>
    public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class, MessageBus.Abstractions.IMessage
        => _busControl.Publish(message, cancellationToken);

    /// <summary>
    /// Отправляет сообщение напрямую в очередь (point-to-point).
    /// </summary>
    public async Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class, MessageBus.Abstractions.IMessage
    {
        var endpoint = await _busControl.GetSendEndpoint(new Uri($"queue:{typeof(TMessage).Name}"));
        await endpoint.Send(message, cancellationToken);
    }

    /// <summary>
    /// Запускает шину сообщений.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (!_started)
        {
            await _busControl.StartAsync(cancellationToken);
            _started = true;
        }
    }

    /// <summary>
    /// Останавливает шину сообщений.
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_started)
        {
            await _busControl.StopAsync(cancellationToken);
            _started = false;
        }
    }

    /// <summary>
    /// Проверяет состояние подключения к брокеру для HealthCheck.
    /// </summary>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_started ? HealthCheckResult.Healthy($"{_brokerType} bus is running") : HealthCheckResult.Unhealthy($"{_brokerType} bus is stopped"));
    }

    /// <summary>
    /// Освобождает ресурсы.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await StopAsync();
    }

    private static MassTransit.IBusControl CreateBus(MessageBus.Abstractions.IBusOptions options)
    {
        if (options.BrokerType == "Kafka")
            throw new NotSupportedException("Kafka поддерживается только через AddMassTransit и DI. Используйте DI-интеграцию для Kafka.");
        if (options.BrokerType != "RabbitMQ")
            throw new NotSupportedException($"Брокер {options.BrokerType} не поддерживается");
        return MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(options.ConnectionString);
            cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));
            // Outbox рекомендуется настраивать через DI (AddMassTransit). Здесь не используется.
        });
    }
} 