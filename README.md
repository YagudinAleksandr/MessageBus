# MessageBus

**MessageBus** — кросс-платформенная библиотека для работы с брокерами сообщений (RabbitMQ, Kafka) через MassTransit с поддержкой DI, IOptions, HealthCheck, транзакций, подтверждений доставки, восстановления соединения и сериализации сообщений. Позволяет одновременно использовать несколько брокеров без BackgroundService для consumer'ов.

---

## Оглавление
- [Возможности](#возможности)
- [Архитектура](#архитектура)
- [Быстрый старт](#быстрый-старт)
- [Сценарии использования](#сценарии-использования)
  - [RabbitMQ](#rabbitmq)
  - [Kafka](#kafka)
  - [Producer/Consumer](#producerconsumer)
  - [HealthCheck](#healthcheck)
  - [Транзакции и подтверждения доставки](#транзакции-и-подтверждения-доставки)
  - [Кастомная сериализация](#кастомная-сериализация)
  - [Graceful shutdown и восстановление](#graceful-shutdown-и-восстановление)
  - [Несколько брокеров и множественная регистрация bus](#несколько-брокеров-и-множественная-регистрация-bus)
- [Расширенные примеры](#расширенные-примеры)
- [Best practices и советы](#best-practices-и-советы)
- [FAQ и troubleshooting](#faq-и-troubleshooting)
- [Ссылки и документация](#ссылки-и-документация)
- [Лицензия](#лицензия)
- [Контакты](#контакты)

---

## Возможности
- Унифицированные абстракции для работы с шиной сообщений
- Поддержка RabbitMQ и Kafka (через MassTransit)
- Интеграция с DI и IOptions
- HealthCheck для мониторинга состояния
- Транзакции и подтверждения доставки
- Восстановление соединения
- Гибкая сериализация сообщений (JSON, XML, кастомные)
- Одновременная работа с несколькими брокерами
- Современные best practices MassTransit и .NET
- Поддержка unit-тестирования

## Архитектура
- **MessageBus.Abstractions** — интерфейсы (IBus, IProducer, IConsumer, IMessage, IBusOptions, IMessageSerializer)
- **MessageBus.Core** — базовые реализации (BusOptions, JsonMessageSerializer, DefaultProducer, DefaultConsumer)
- **MessageBus.MassTransit** — адаптер для MassTransit (RabbitMQ/Kafka), HealthCheck, транзакции, подтверждения, восстановление
- **MessageBus.DependencyInjection** — расширения для DI, поддержка IOptions, регистрация нескольких bus

### Принципы работы
- Все взаимодействия асинхронные (`Task`/`Task<T>`)
- Producer и Consumer регистрируются через DI
- Для RabbitMQ используется собственный адаптер, для Kafka — MassTransit Rider
- Не используются BackgroundService для consumer'ов — обработка сообщений интегрируется напрямую через DI
- Все интерфейсы снабжены XML summary на русском языке

---

## Быстрый старт

### 1. Установка NuGet-пакетов
Добавьте ссылки на проекты/пакеты:
- MessageBus.Abstractions
- MessageBus.Core
- MessageBus.MassTransit
- MessageBus.DependencyInjection
- MassTransit, MassTransit.RabbitMQ, MassTransit.Kafka (для Kafka)

### 2. Конфигурация RabbitMQ через DI/IOptions
```csharp
// appsettings.json
"RabbitMQ": {
  "BrokerType": "RabbitMQ",
  "ConnectionString": "amqp://guest:guest@localhost:5672/"
}
```
```csharp
// Program.cs
builder.Services.AddRabbitMqBus<BusOptions>(builder.Configuration);
```

### 3. Конфигурация Kafka через DI/IOptions
> **Внимание:** Для Kafka используйте AddMassTransitRider и AddRider. Пример:
```csharp
// appsettings.json
"Kafka": {
  "BrokerType": "Kafka",
  "ConnectionString": "PLAINTEXT://localhost:9092",
  "ClientId": "my-app"
}
```
```csharp
// Program.cs
builder.Services.AddMassTransit(x =>
{
    x.AddRider(rider =>
    {
        // Конфигурируйте Kafka endpoints
    });
});
```

---

## Сценарии использования

### RabbitMQ
- Используйте `AddRabbitMqBus<TOptions>` для регистрации bus и всех зависимостей через DI.
- Поддерживаются HealthCheck, транзакции, подтверждения доставки, восстановление соединения.

### Kafka
- Используйте `AddMassTransitRider` и `AddRider` для интеграции с Kafka.
- Для сериализации сообщений используйте стандартные или кастомные сериализаторы.

### Producer/Consumer
- Определите сообщение и consumer:
```csharp
public class MyMessage : IMessage { ... }
public class MyConsumer : IConsumer<MyMessage> { ... }
```
- Зарегистрируйте consumer через DI:
```csharp
services.AddSingleton<IConsumer<MyMessage>, MyConsumer>();
```
- Запустите bus и отправьте сообщение:
```csharp
await bus.StartAsync();
await bus.PublishAsync(new MyMessage { ... });
```

### HealthCheck
- Включите HealthChecks для мониторинга состояния bus:
```csharp
builder.Services.AddHealthChecks().AddCheck<MassTransitBus>("bus");
```
- Можно реализовать кастомный HealthCheck для расширенной диагностики.

### Транзакции и подтверждения доставки
- Используйте Outbox через MassTransit для гарантии доставки (exactly-once):
```csharp
cfg.UseInMemoryOutbox(context);
```

### Кастомная сериализация
- Реализуйте свой сериализатор, например, для XML:
```csharp
public class XmlMessageSerializer : IMessageSerializer { ... }
services.AddSingleton<IMessageSerializer, XmlMessageSerializer>();
```

### Graceful shutdown и восстановление
- Корректно завершайте работу через `DisposeAsync` или `StopAsync`:
```csharp
await bus.DisposeAsync();
```
- Восстановление соединения происходит автоматически при сбоях (RabbitMQ).

### Несколько брокеров и множественная регистрация bus
- Зарегистрируйте несколько bus с разными настройками:
```csharp
builder.Services.AddRabbitMqBus<RabbitOptions>(builder.Configuration, "RabbitMQ");
builder.Services.AddMassTransit(x => { ... });
```

---

## Расширенные примеры

### Полный пример: регистрация и использование Consumer (RabbitMQ)
```csharp
// 1. Определяем сообщение
public class MyMessage : IMessage { ... }
// 2. Определяем consumer
public class MyConsumer : IConsumer<MyMessage> { ... }
// 3. Регистрация в DI и запуск
var services = new ServiceCollection();
var options = new BusOptions { ... };
services.AddSingleton<IBusOptions>(options);
services.AddRabbitMqBus<BusOptions>(new ConfigurationBuilder().Build());
var consumer = new MyConsumer();
services.AddSingleton<IConsumer<MyMessage>>(consumer);
var provider = services.BuildServiceProvider();
var bus = provider.GetRequiredService<IBus>();
await bus.StartAsync();
// 4. Отправка сообщения
await bus.PublishAsync(new MyMessage { Payload = "Привет, Consumer!" });
await Task.WhenAny(consumer.Received.Task, Task.Delay(5000));
await bus.StopAsync();
```

### Полный пример: регистрация и использование Consumer (Kafka через MassTransit Rider)
```csharp
// 1. Определяем сообщение
public class KafkaMessage : IMessage { ... }
// 2. Определяем consumer
public class KafkaConsumer : IConsumer<KafkaMessage> { ... }
// 3. Регистрация через MassTransit Rider
var services = new ServiceCollection();
services.AddMassTransit(x =>
{
    x.AddConsumer<KafkaConsumer>();
    x.AddRider(rider =>
    {
        rider.AddConsumer<KafkaConsumer>();
        rider.UsingKafka((context, k) =>
        {
            k.Host("localhost:9092");
            k.TopicEndpoint<KafkaMessage>("sample-topic", "sample-group", e =>
            {
                e.ConfigureConsumer<KafkaConsumer>(context);
            });
        });
    });
});
var provider = services.BuildServiceProvider();
var busControl = provider.GetRequiredService<IBusControl>();
await busControl.StartAsync();
// 4. Отправка сообщения
await busControl.Publish(new KafkaMessage { Payload = "Привет, Kafka Consumer!" });
var kafkaConsumer = provider.GetRequiredService<KafkaConsumer>();
await Task.WhenAny(kafkaConsumer.Received.Task, Task.Delay(5000));
await busControl.StopAsync();
```

### Транзакции и подтверждения доставки (RabbitMQ)
```csharp
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672/");
        cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(10)));
        cfg.UseInMemoryOutbox(context); // Гарантия доставки (exactly-once)
    });
});
```

### Кастомный HealthCheck
```csharp
public class CustomBusHealthCheck : IHealthCheck { ... }
builder.Services.AddHealthChecks().AddCheck<CustomBusHealthCheck>("custom-bus");
```

### Кастомная сериализация сообщений
```csharp
public class XmlMessageSerializer : IMessageSerializer { ... }
builder.Services.AddSingleton<IMessageSerializer, XmlMessageSerializer>();
```

### Graceful shutdown и восстановление соединения
```csharp
await bus.DisposeAsync(); // Корректное завершение работы
```

### Регистрация нескольких bus с разными настройками
```csharp
builder.Services.AddRabbitMqBus<RabbitOptions>(builder.Configuration, "RabbitMQ");
builder.Services.AddMassTransit(x => { ... });
```

---

## Best practices и советы
- Используйте асинхронные методы для всех операций с bus и сообщениями
- Для продакшена обязательно настройте retry, outbox и HealthCheck
- Для Kafka всегда используйте AddMassTransitRider
- Для unit-тестирования используйте InMemory bus
- Не храните чувствительные данные в сообщениях без шифрования
- Для сложных сценариев используйте отдельные топики/очереди для разных типов сообщений
- Следите за актуальностью NuGet-пакетов MassTransit и брокеров

---

## FAQ и troubleshooting

**Q: Почему consumer не получает сообщения?**
- Проверьте регистрацию consumer в DI
- Убедитесь, что bus запущен (`StartAsync`)
- Проверьте настройки брокера и подключения

**Q: Как добавить несколько consumer для одного типа сообщения?**
- Зарегистрируйте несколько реализаций IConsumer<T> через DI

**Q: Как реализовать dead-letter queue?**
- Используйте стандартные механизмы MassTransit для DLQ

**Q: Как тестировать обработку сообщений?**
- Используйте InMemory bus и TaskCompletionSource для ожидания сообщений

**Q: Как реализовать отложенную доставку (delay)?**
- Используйте механизмы отложенных сообщений MassTransit

---

## Ссылки и документация
- [Документация MassTransit](https://masstransit.io/documentation/)
- [Документация RabbitMQ](https://www.rabbitmq.com/documentation.html)
- [Документация Apache Kafka](https://kafka.apache.org/documentation/)
- [Документация .NET DI](https://learn.microsoft.com/ru-ru/dotnet/core/extensions/dependency-injection)

---

## Лицензия
MIT

---

## Контакты
- Вопросы и предложения: issues или pull request в репозитории
- Автор: [Ваше имя или команда]

---

**MessageBus** — современное решение для интеграции с брокерами сообщений в .NET-проектах с максимальной гибкостью и расширяемостью. 