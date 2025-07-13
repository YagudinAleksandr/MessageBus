# MessageBus

**MessageBus** is a cross-platform .NET library for working with message brokers (RabbitMQ, Kafka) via MassTransit, supporting DI, IOptions, HealthCheck, transactions, delivery confirmation, connection recovery, and message serialization. Supports multiple brokers simultaneously and does not require BackgroundService for consumers.

---

## Features
- Unified abstractions for message bus
- RabbitMQ and Kafka support (via MassTransit)
- DI and IOptions integration
- HealthCheck for monitoring
- Transactions and delivery confirmation
- Connection recovery
- Flexible message serialization (JSON, XML, custom)
- Multiple brokers at once
- Modern .NET and MassTransit best practices
- Unit testing support

## Architecture
- **MessageBus.Abstractions** — interfaces (IBus, IProducer, IConsumer, IMessage, IBusOptions, IMessageSerializer)
- **MessageBus.Core** — base implementations (BusOptions, JsonMessageSerializer, DefaultProducer, DefaultConsumer)
- **MessageBus.MassTransit** — MassTransit adapter (RabbitMQ/Kafka), HealthCheck, transactions, delivery confirmation, recovery
- **MessageBus.DependencyInjection** — DI extensions, IOptions support, multi-bus registration

## Quick Start
See the main [README.md](./README.md) for detailed usage, examples, and advanced scenarios (currently in Russian).

## Internationalization
- The main documentation is in Russian, but the project is open for internationalization and English contributions.
- All code is documented with XML summary in Russian.

## License
MIT

---

**MessageBus** — a modern solution for message broker integration in .NET projects with maximum flexibility and extensibility. 