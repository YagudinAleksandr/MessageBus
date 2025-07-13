namespace MessageBus.Abstractions
{
    /// <summary>
    /// Абстракция для сериализации и десериализации сообщений.
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// Сериализует сообщение в массив байт.
        /// </summary>
        byte[] Serialize<T>(T message) where T : class, IMessage;
        /// <summary>
        /// Десериализует сообщение из массива байт.
        /// </summary>
        T Deserialize<T>(byte[] data) where T : class, IMessage;
        /// <summary>
        /// Тип содержимого сериализатора (например, "application/json").
        /// </summary>
        string ContentType { get; }
    }
} 