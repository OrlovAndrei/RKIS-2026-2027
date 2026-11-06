using System;

class UpdateCommand : ICommand
{
    public TodoList? TodoList { get; set; }
    public int Index { get; set; }
    public string? Text { get; set; }

    public void Execute()
    {
        if (TodoList == null)
        {
            return;
        }

        TodoItem? item = TodoList.GetItem(Index);
        if (item != null && !string.IsNullOrWhiteSpace(Text))
        {
            item.UpdateText(Text);
            Console.WriteLine($"Задача {Index + 1} успешно обновлена!");
        }
        else
        {
            Console.WriteLine("Неверный формат или номер задачи. Используйте: update <номер> \"новый текст\"");
        }
    }
}

