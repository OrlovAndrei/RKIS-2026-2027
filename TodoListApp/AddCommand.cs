namespace TodoListApp;

public class AddCommand : ICommand
{
    public bool Multiline { get; set; }
    public string Text { get; set; } = "";
    public TodoList TodoList { get; set; } = null!;

    public void Execute()
    {
        if (Multiline)
        {
            Console.WriteLine("Введите строки (для завершения введите '!end'):");

            var lines = new List<string>();
            string? line;

            while ((line = Console.ReadLine()) != null)
            {
                if (line.Trim().Equals("!end", StringComparison.OrdinalIgnoreCase))
                    break;

                lines.Add(line);
            }

            if (lines.Count > 0)
            {
                TodoList.Add(new TodoItem(string.Join("\n", lines)));
                FileManager.SaveTodos(TodoList, FileManager.TodoPath);
                Console.WriteLine("Многострочная задача добавлена.");
            }
        }
        else
        {
            string text = Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.Write("Введите текст задачи: ");
                text = Console.ReadLine() ?? "";
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                TodoList.Add(new TodoItem(text.Trim()));
                FileManager.SaveTodos(TodoList, FileManager.TodoPath);
                Console.WriteLine("Задача добавлена.");
            }
        }
    }
}