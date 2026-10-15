// Одна задача из списка дел
public class TodoItem
{
    // Сколько символов текста показывать в кратком виде
    private const int ShortTextLength = 30;

    // Сеттеры приватные: менять задачу можно только через методы ниже,
    // поэтому дата изменения всегда обновляется вместе с данными
    public string Text { get; private set; }
    public bool IsDone { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public string StatusText => IsDone ? "выполнена" : "не выполнена";

    public TodoItem(string text)
    {
        Text = CheckText(text);
        IsDone = false;
        LastUpdate = DateTime.Now;
    }

    public void MarkDone()
    {
        IsDone = true;
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
