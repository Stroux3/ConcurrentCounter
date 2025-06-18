# ConcurrentCounter

📌 **Цель проекта:**  
Реализовать потокобезопасный сервер со счётчиком `count`, к которому одновременно обращаются клиенты: одни читают, другие пишут. Требования:

- 🔹 Читатели читают параллельно.
- 🔹 Писатели пишут строго по одному.
- 🔹 Пока идёт запись — чтение блокируется.

---

## 🧱 Структура

- `ServerManual.cs` — реализация синхронизации через `SemaphoreSlim`.
- `ServerWithReaderWriterLockSlim.cs` — вариант с `ReaderWriterLockSlim`.
- `ConcurrentCounter.Tests` — xUnit тесты.
- `Program.cs` — демонстрация в консоли.

---

## 🧪 Тесты

Проект покрыт юнит-тестами с использованием [xUnit](https://xunit.net).  
Чтобы запустить тесты:

```bash
dotnet test
```

Проверяются:
- Одновременное чтение
- Последовательная запись
- Точность счёта после многопоточной работы

---

## 🛠️ Как запустить

1. Клонируй репозиторий:
   ```bash
   git clone https://github.com/Stroux3/ConcurrentCounter.git
   cd ConcurrentCounter
   ```

2. Построй решение:
   ```bash
   dotnet build
   ```

3. Запусти консольное приложение:
   ```bash
   dotnet run --project ConcurrentCounterApp
   ```

4. Запусти тесты:
   ```bash
   dotnet test
   ```

---

## 🔧 Используемые технологии

- .NET 8
- C# 12
- xUnit (тестирование)
- ReaderWriterLockSlim, SemaphoreSlim
- Модульность: `Core`, `App`, `Tests`

---

## 🧑‍💻 Автор

**Владимир (Stroux3)**  
📬 [GitHub Profile](https://github.com/Stroux3)