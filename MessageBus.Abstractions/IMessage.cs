namespace MessageBus.Abstractions
{
    /// <summary>
    /// Базовый интерфейс сообщения для шины сообщений.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Уникальный идентификатор сообщения.
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Дата и время создания сообщения (UTC).
        /// </summary>
        DateTime CreatedAt { get; }
    }
} 