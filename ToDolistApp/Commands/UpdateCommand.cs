using System;

namespace TodoListApp;

public class UpdateCommand : IndexedCommand
{
    public string NewText { get; set; } = "";

    public override void Execute()
    {
        if (string.IsNullOrWhiteSpace(NewText))
        {
            Console.WriteLine("Ошибка: новый текст задачи не может быть пустым.");
            return;
        }
        if (!TryGetItem(out TodoItem item, out _)) return;
        item.UpdateText(NewText);
        Console.WriteLine($"Задача №{Index} успешно обновлена.");
    }
}
