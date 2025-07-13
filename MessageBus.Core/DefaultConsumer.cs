using MessageBus.Abstractions;

/// <summary>
/// Базовая обёртка для консьюмера сообщений, делегирующая обработку через IConsumer<TMessage>.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
public class DefaultConsumer<TMessage> where TMessage : class, IMessage
{
    private readonly IConsumer<TMessage> _consumer;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="DefaultConsumer{TMessage}"/>.
    /// </summary>
    /// <param name="consumer">Экземпляр консьюмера для обработки сообщений.</param>
    public DefaultConsumer(IConsumer<TMessage> consumer)
    {
        _consumer = consumer;
    }

    /// <summary>
    /// Обрабатывает полученное сообщение.
    /// </summary>
    /// <param name="message">Сообщение для обработки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    public Task ConsumeAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        return _consumer.ConsumeAsync(message, cancellationToken);
    }
} 