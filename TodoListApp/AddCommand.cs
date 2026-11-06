using System;
using System.Text;

class AddCommand : ICommand
{
    public TodoList? TodoList { get; set; }
    public string? Text { get; set; }
    public bool IsMultiline { get; set; }

    public void Execute()
    {
        if (TodoList == null)
        {
            return;
        }

        string? task = Text;

        if (IsMultiline)
        {
            task = ReadMultilineText();
        }

        if (string.IsNullOrWhiteSpace(task))
        {
            Console.WriteLine("Неверный формат. Используйте: add \"текст задачи\" или add --multiline");
            return;
        }

        TodoList.Add(new TodoItem(task));
        Console.WriteLine("Задача успешно добавлена!");
    }

    private string ReadMultilineText()
    {
        Console.WriteLine("Введите текст задачи построчно, для завершения введите !end");
        StringBuilder sb = new StringBuilder();

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();

            if (line == null || line.Trim() == "!end")
            {
                break;
            }

            if (sb.Length > 0)
            {
                sb.Append('\n');
            }
            sb.Append(line);
        }

        return sb.ToString();
    }
}

