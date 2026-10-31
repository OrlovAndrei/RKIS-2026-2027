using System;
using System.Text;

public class AddCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public string Text { get; set; } = "";
    public bool Multiline { get; set; }

    public void Execute()
    {
        if (!Multiline)
        {
            TodoList.Add(new TodoItem(Text));
            Console.WriteLine("Задача добавлена");
            return;
        }

        Console.WriteLine("Введите строки задачи. Для завершения напишите !end");
        StringBuilder result = new StringBuilder();
        bool firstLine = true;

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Ввод прерван, задача не добавлена");
                return;
            }
            if (line.Trim() == "!end") break;

            if (!firstLine) result.Append('\n');
            result.Append(line);
            firstLine = false;
        }

        string fullText = result.ToString();
        if (string.IsNullOrWhiteSpace(fullText))
        {
            Console.WriteLine("Нельзя добавить пустую задачу");
            return;
        }

        TodoList.Add(new TodoItem(fullText));
        Console.WriteLine("Задача добавлена");
    }
}

public class ViewCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public bool ShowIndex { get; set; }
    public bool ShowStatus { get; set; }
    public bool ShowDate { get; set; }

    public void Execute()
    {
        TodoList.View(ShowIndex, ShowStatus, ShowDate);
    }
}
