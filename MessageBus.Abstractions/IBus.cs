namespace MessageBus.Abstractions
{
    /// <summary>
    /// Абстракция шины сообщений для публикации, отправки и управления жизненным циклом.
    /// </summary>
    public interface IBus
    {
        /// <summary>
        /// Публикует сообщение всем подписчикам (fanout).
        /// </summary>
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class, IMessage;
        /// <summary>
        /// Отправляет сообщение напрямую в очередь (point-to-point).
        /// </summary>
        Task SendAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class, IMessage;
        /// <summary>
        /// Запускает шину сообщений.
        /// </summary>
        Task StartAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Останавливает шину сообщений.
        /// </summary>
        Task StopAsync(CancellationToken cancellationToken = default);
    }
} 