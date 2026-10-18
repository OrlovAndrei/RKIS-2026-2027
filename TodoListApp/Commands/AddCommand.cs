using System.Text;

public class AddCommand : ICommand
{
    public required TodoList TodoList { get; set; }
    public string Text { get; set; } = "";

    // Флаг --multiline / -m: текст задачи вводится построчно уже при выполнении команды
    public bool Multiline { get; set; }

    public void Execute()
    {
        string text = Multiline ? ReadMultiline() : Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }
        if (TodoList.Contains(text))
        {
            Console.WriteLine("Такая задача уже есть в списке.");
            return;
        }

        TodoList.Add(new TodoItem(text));
        Console.WriteLine($"Задача добавлена под номером {TodoList.Count}.");
    }

    // Читаем строки до !end и склеиваем их через \n в один текст задачи
    private static string ReadMultiline()
    {
        Console.WriteLine("Вводите строки задачи, для завершения введите !end");
        var sb = new StringBuilder();
        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line is null || line.Trim() == "!end")
                break;

            if (sb.Length > 0)
                sb.Append('\n');
            sb.Append(line);
        }
        return sb.ToString().Trim();
    }
}
