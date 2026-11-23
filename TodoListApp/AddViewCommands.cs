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
            SaveTasks();
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
        SaveTasks();
        Console.WriteLine("Задача добавлена");
    }

    private void SaveTasks()
    {
        try { FileManager.SaveTodos(TodoList, FileManager.TodoFilePath); }
        catch (System.IO.IOException ex) { Console.WriteLine("Ошибка сохранения: " + ex.Message); }
        catch (UnauthorizedAccessException ex) { Console.WriteLine("Нет доступа к файлу: " + ex.Message); }
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
        if (TodoList.Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }

        string header = "";
        if (ShowIndex) header += "№".PadRight(3) + " | ";
        header += "Текст задачи".PadRight(33);
        if (ShowStatus) header += " | " + "Статус".PadRight(12);
        if (ShowDate) header += " | Дата изменения";
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        int number = 1;
        // Проверяем работу IEnumerable<TodoItem> и yield return.
        foreach (TodoItem item in TodoList)
        {
            string prefix = ShowIndex ? number.ToString().PadRight(3) + " | " : "";
            Console.WriteLine(prefix + item.GetShortInfo(ShowStatus, ShowDate));
            number++;
        }
    }
}
