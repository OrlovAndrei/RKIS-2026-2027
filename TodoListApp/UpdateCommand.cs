namespace TodoListApp;

public class UpdateCommand : ICommand
{
    public int Index { get; set; }
    public string Text { get; set; } = "";
    public TodoList TodoList { get; set; } = null!;

    public void Execute()
    {
        if (!CommandHelper.TryGetTask(TodoList, Index, out TodoItem? task))
            return;

        string newText = Text;

        if (string.IsNullOrWhiteSpace(newText))
        {
            Console.Write("Введите новый текст задачи: ");
            newText = Console.ReadLine() ?? "";
        }

        if (string.IsNullOrWhiteSpace(newText))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }

        task!.UpdateText(newText.Trim());
        Console.WriteLine($"Задача #{Index} обновлена.");
    }
}