namespace MessageBus.Abstractions
{
    /// <summary>
    /// Абстракция для обработчика входящих сообщений (consumer).
    /// </summary>
    public interface IConsumer<TMessage> where TMessage : class, IMessage
    {
        /// <summary>
        /// Обрабатывает входящее сообщение.
        /// </summary>
        Task ConsumeAsync(TMessage message, CancellationToken cancellationToken = default);
    }
} 