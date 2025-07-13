namespace MessageBus.Abstractions
{
    /// <summary>
    /// Абстракция для отправителя сообщений (producer).
    /// </summary>
    public interface IProducer
    {
        /// <summary>
        /// Отправляет сообщение в шину.
        /// </summary>
        Task ProduceAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class, IMessage;
    }
} 