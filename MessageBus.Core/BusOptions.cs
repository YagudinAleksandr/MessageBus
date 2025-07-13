using MessageBus.Abstractions;

/// <summary>
/// Базовая реализация опций для конфигурации шины сообщений.
/// </summary>
public class BusOptions : IBusOptions
{
    /// <summary>
    /// Тип брокера (например, "RabbitMQ" или "Kafka").
    /// </summary>
    public string BrokerType { get; set; } = string.Empty;

    /// <summary>
    /// Строка подключения к брокеру сообщений.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Необязательный идентификатор клиента.
    /// </summary>
    public string? ClientId { get; set; }
} 