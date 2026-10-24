using System;

// Одна задача: текст, статус выполнения и дата последнего изменения.
public class TodoItem
{
    public string Text { get; private set; }
    public bool IsDone { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public TodoItem(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Текст задачи не может быть пустым", nameof(text));

        Text = text;
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
        if (string.IsNullOrWhiteSpace(newText))
            throw new ArgumentException("Текст задачи не может быть пустым", nameof(newText));

        Text = newText;
        LastUpdate = DateTime.Now;
    }

    // Краткая информация: текст (до 30 символов), статус, дата.
    public string GetShortInfo() => GetShortInfo(true, true);

    // Дополнительная версия для вывода с выбранными флагами view.
    public string GetShortInfo(bool showDone, bool showDate)
    {
        string preview = Text.Replace("\r", " ").Replace("\n", " ");
        if (preview.Length > 30)
            preview = preview.Substring(0, 30) + "...";

        string info = preview.PadRight(33);
        if (showDone)
            info += " | " + (IsDone ? "сделано" : "не сделано").PadRight(12);
        if (showDate)
            info += " | " + LastUpdate.ToString("dd.MM.yyyy HH:mm:ss");
        return info;
    }

    public string GetFullInfo()
    {
        string status = IsDone ? "выполнена" : "не выполнена";
        return $"{Text}\nСтатус: {status}\nДата изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}";
    }
}
