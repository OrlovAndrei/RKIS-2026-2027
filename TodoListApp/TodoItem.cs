using System; // Подключаем работу с датой и временем.

public class TodoItem // Класс одной задачи.
{
    public string Text { get; private set; } // Текст задачи: изменяется только внутри класса.
    public bool IsDone { get; private set; } // Статус выполнения задачи.
    public DateTime LastUpdate { get; private set; } // Дата создания или изменения задачи.

    public TodoItem(string text) // Конструктор получает текст новой задачи.
    {
        Text = text; // Сохраняем текст задачи.
        IsDone = false; // Новая задача ещё не выполнена.
        LastUpdate = DateTime.Now; // Запоминаем текущую дату и время.
    }

    public void MarkDone() // Метод отмечает задачу выполненной.
    {
        IsDone = true; // Меняем статус задачи.
        LastUpdate = DateTime.Now; // Обновляем дату изменения.
    }

    public void UpdateText(string newText) // Метод изменяет текст задачи.
    {
        Text = newText; // Сохраняем новый текст.
        LastUpdate = DateTime.Now; // Обновляем дату изменения.
    }

    public string GetShortInfo() // Метод возвращает краткую информацию о задаче.
    {
        string shortText = Text.Replace("\n", " "); // Заменяем переносы строк пробелами.

        if (shortText.Length > 30) // Проверяем, превышает ли текст 30 символов.
        {
            shortText = shortText.Substring(0, 30) + "..."; // Оставляем первые 30 символов.
        }

        string status = IsDone ? "сделано" : "не сделано"; // Получаем текстовый статус.

        return $"{shortText} | {status} | {LastUpdate:dd.MM.yyyy HH:mm}"; // Возвращаем краткую информацию.
    }

    public string GetFullInfo() // Метод возвращает полную информацию о задаче.
    {
        string status = IsDone ? "выполнена" : "не выполнена"; // Формируем описание статуса.

        return $"Текст: {Text}\n" + // Добавляем полный текст задачи.
               $"Статус: {status}\n" + // Добавляем статус выполнения.
               $"Последнее изменение: {LastUpdate:dd.MM.yyyy HH:mm}"; // Добавляем дату изменения.
    }
}