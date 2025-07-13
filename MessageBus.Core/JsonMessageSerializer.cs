using System.Text.Json;
using MessageBus.Abstractions;

/// <summary>
/// Реализация сериализатора сообщений с использованием формата JSON.
/// Позволяет сериализовать и десериализовать сообщения, реализующие интерфейс IMessage.
/// </summary>
public class JsonMessageSerializer : IMessageSerializer
{
    /// <summary>
    /// Возвращает тип контента сериализации ("application/json").
    /// </summary>
    public string ContentType => "application/json";

    /// <summary>
    /// Сериализует сообщение в массив байт в формате JSON.
    /// </summary>
    /// <typeparam name="T">Тип сообщения, реализующий IMessage.</typeparam>
    /// <param name="message">Сообщение для сериализации.</param>
    /// <returns>Массив байт, представляющий сериализованное сообщение.</returns>
    public byte[] Serialize<T>(T message) where T : class, IMessage
    {
        return JsonSerializer.SerializeToUtf8Bytes(message, message.GetType());
    }

    /// <summary>
    /// Десериализует сообщение из массива байт в формате JSON.
    /// </summary>
    /// <typeparam name="T">Тип сообщения, реализующий IMessage.</typeparam>
    /// <param name="data">Массив байт, содержащий сериализованное сообщение.</param>
    /// <returns>Десериализованное сообщение типа T.</returns>
    public T Deserialize<T>(byte[] data) where T : class, IMessage
    {
        return JsonSerializer.Deserialize<T>(data)!;
    }
} 