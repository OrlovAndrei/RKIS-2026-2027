// Одна задача из списка дел
public class TodoItem
{
    // Сколько символов текста показывать в кратком виде
    private const int ShortTextLength = 30;

    // Сеттеры приватные: менять задачу можно только через методы ниже,
    // поэтому дата изменения всегда обновляется вместе с данными
    public string Text { get; private set; }
    public TodoStatus Status { get; private set; }
    public DateTime LastUpdate { get; private set; }

    // Статус словами для таблицы и команды read
    public string StatusText => Status switch
    {
        TodoStatus.NotStarted => "не начата",
        TodoStatus.InProgress => "в процессе",
        TodoStatus.Completed => "выполнена",
        TodoStatus.Postponed => "отложена",
        TodoStatus.Failed => "провалена",
        _ => Status.ToString()
    };

    // Новая задача: ещё не начата, изменена только что
    public TodoItem(string text) : this(text, TodoStatus.NotStarted, DateTime.Now)
    {
    }

    // Задача, прочитанная из файла: статус и дату берём из файла, а не «сейчас»
    public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
    {
        Text = CheckText(text);
        Status = status;
        LastUpdate = lastUpdate;
    }

    public void SetStatus(TodoStatus status)
    {
        Status = status;
        LastUpdate = DateTime.Now;
    }

    public void UpdateText(string newText)
    {
        Text = CheckText(newText);
        LastUpdate = DateTime.Now;
    }

    // Текст для таблицы: в одну строку и не длиннее 30 символов
    public string GetShortText()
    {
        string oneLine = Text.Replace("\n", " ");
        if (oneLine.Length <= ShortTextLength)
            return oneLine;
        return oneLine.Substring(0, ShortTextLength) + "...";
    }

    public string GetShortInfo()
    {
        return $"{GetShortText()} | {StatusText} | {LastUpdate:dd.MM.yyyy HH:mm}";
    }

    public string GetFullInfo()
    {
        return $"{Text}\n" +
               $"Статус: {StatusText}\n" +
               $"Изменена: {LastUpdate:dd.MM.yyyy HH:mm:ss}";
    }

    // Не даём создать задачу без текста — объект всегда остаётся в корректном состоянии
    private static string CheckText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Текст задачи не может быть пустым");
        return text;
    }
}
