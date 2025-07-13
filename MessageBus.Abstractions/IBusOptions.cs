namespace MessageBus.Abstractions
{
    /// <summary>
    /// Опции конфигурации для шины сообщений.
    /// </summary>
    public interface IBusOptions
    {
        /// <summary>
        /// Тип брокера (например, RabbitMQ, Kafka).
        /// </summary>
        string BrokerType { get; }
        /// <summary>
        /// Строка подключения к брокеру.
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// Необязательный идентификатор клиента.
        /// </summary>
        string? ClientId { get; }
    }
} 