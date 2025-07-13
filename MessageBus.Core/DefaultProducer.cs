using MessageBus.Abstractions;

/// <summary>
/// Базовая реализация продюсера сообщений, использующая IBus для отправки сообщений.
/// </summary>
public class DefaultProducer : IProducer
{
    private readonly IBus _bus;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultProducer"/>.
    /// </summary>
    /// <param name="bus">Экземпляр шины сообщений для отправки.</param>
    public DefaultProducer(IBus bus)
    {
        _bus = bus;
    }

    /// <summary>
    /// Отправляет сообщение в шину сообщений.
    /// </summary>
    /// <typeparam name="TMessage">Тип сообщения.</typeparam>
    /// <param name="message">Сообщение для отправки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    public Task ProduceAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class, IMessage
    {
        return _bus.PublishAsync(message, cancellationToken);
    }
} 