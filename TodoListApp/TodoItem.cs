using System; // Работа с датой и временем.

public class TodoItem // Класс одной задачи.
{
    public string Text { get; private set; } // Текст задачи.
    public TodoStatus Status { get; private set; } // Статус задачи.
    public DateTime LastUpdate { get; private set; } // Дата создания или изменения.

    public TodoItem(string text) // Новая задача.
    {
        Text = text;
        Status = TodoStatus.NotStarted; // По умолчанию — не начато.
        LastUpdate = DateTime.Now;
    }

    // Задача с данными, восстановленными из файла.
    public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
    {
        Text = text;
        Status = status;
        LastUpdate = lastUpdate;
    }

    public void SetStatus(TodoStatus newStatus) // Меняет статус и обновляет дату.
    {
        Status = newStatus;
        LastUpdate = DateTime.Now;
    }

    public void UpdateText(string newText) // Меняет текст и обновляет дату.
    {
        Text = newText;
        LastUpdate = DateTime.Now;
    }

    public string GetStatusText() // Текстовое описание статуса.
    {
        return Status switch
        {
            TodoStatus.NotStarted => "не начато",
            TodoStatus.InProgress => "в процессе",
            TodoStatus.Completed => "выполнено",
            TodoStatus.Postponed => "отложено",
            TodoStatus.Failed => "провалено",
            _ => "неизвестно"
        };
    }

    public string GetShortInfo() // Краткая информация о задаче.
    {
        string shortText = Text.Replace("\n", " ");

        if (shortText.Length > 30)
        {
            shortText = shortText.Substring(0, 30) + "...";
        }

        return $"{shortText} | {GetStatusText()} | {LastUpdate:dd.MM.yyyy HH:mm}";
    }

    public string GetFullInfo() // Полная информация о задаче.
    {
        return $"Текст: {Text}\n" +
               $"Статус: {GetStatusText()}\n" +
               $"Последнее изменение: {LastUpdate:dd.MM.yyyy HH:mm}";
    }
}