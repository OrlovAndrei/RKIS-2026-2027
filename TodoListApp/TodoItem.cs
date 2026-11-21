using System;

namespace TodoListApp;

public class TodoItem
{
    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime LastModified { get; set; } = DateTime.Now;

    // Новый метод для красивого вывода (Лекция 7)
    public virtual string GetDisplayText()
    {
        return $"[{(IsCompleted ? "x" : " ")}] {Text} (изменено: {LastModified:dd.MM.yyyy HH:mm})";
    }
}

// НОВЫЕ КЛАССЫ-НАСЛЕДНИКИ (Лекция 7)
public class UrgentTodoItem : TodoItem
{
    public DateTime Deadline { get; set; }

    public override string GetDisplayText()
    {
        return $"[!!! СРОЧНО] {base.GetDisplayText()} (дедлайн: {Deadline:dd.MM.yyyy})";
    }
}

public class RecurringTodoItem : TodoItem
{
    public int IntervalDays { get; set; }

    public override string GetDisplayText()
    {
        return $"[↻ ПОВТОР] {base.GetDisplayText()} (каждые {IntervalDays} дн.)";
    }
}