using System;
using System.Text;

namespace TodoListApp;

public class AddCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public bool IsMultiline { get; set; }
    public string Text { get; set; } = "";
    public string FilePath { get; set; } = "";

    public void Execute()
    {
        string taskText = IsMultiline ? ReadMultilineText() : Text;

        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым.");
            return;
        }

        TodoList.Add(new TodoItem(taskText));
        FileManager.SaveTodos(TodoList, FilePath);
        Console.WriteLine("Задача успешно добавлена!");
    }

    private string ReadMultilineText()
    {
        Console.WriteLine("Многострочный режим (введите !end для завершения):");
        StringBuilder sb = new StringBuilder();

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null || line.Trim() == "!end") break;

            if (sb.Length > 0) sb.Append('\n');
            sb.Append(line);
        }
        return sb.ToString();
    }
}
