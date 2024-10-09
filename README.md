# Quest Management System

## Описание проекта

Quest Management System — это RESTful API, разработанное с использованием **.NET 8.0**, **ASP.NET Core** и **Entity Framework Core**. Система позволяет игрокам взаимодействовать с заданиями, отслеживать их прогресс и получать вознаграждения по завершении. Она основана на принципах **Clean Architecture** и **Domain-Driven Design (DDD)**. В систему входят функции, такие как принятие заданий, обновление прогресса по заданиям, завершение заданий и получение доступных квестов для игроков.

Приложение также интегрирует **MediatR** для обработки команд и паттернов запросов, что делает кодовую базу чистой и поддерживаемой. Оно использует реляционную базу данных для управления такими сущностями, как `Player`, `Quest`, `QuestReward` и `PlayerQuest`.

## Возможности

- Игроки могут принимать доступные квесты в зависимости от их уровня.
- Игроки могут обновлять свой прогресс по принятым квестам.
- Квесты могут быть завершены, и награды выдаются на основе прогресса.
- MediatR используется для разделения бизнес-логики и обработки запросов.

## Используемые технологии

- **.NET 8.0**
- **ASP.NET Core**
- **Entity Framework Core**
- **MediatR**
- **SQL Server**
- **Docker** (по желанию для контейнеризации)

## Структура проекта

Проект разделен на следующие слои:
1. **Domain Layer**: Содержит бизнес-сущности и интерфейсы.
2. **Application Layer**: Включает команды, запросы и DTO.
3. **Infrastructure Layer**: Ответственен за хранение данных (EF Core и репозитории).
4. **Presentation Layer**: Содержит контроллеры API для экспонирования функциональности.

## Начало работы

### Предварительные требования

- **.NET 8.0 SDK**
- **SQL Server** (или другая реляционная БД)
- **Docker** (по желанию, для контейнеризации)
- **Visual Studio** или **VS Code** (рекомендуется для разработки)

### Запуск проекта

#### 1. Клонирование репозитория

```bash
git clone https://github.com/yourusername/quest-management-system.git
cd quest-management-system
