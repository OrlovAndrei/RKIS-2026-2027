using System.Text;

namespace TodoListApp;

internal class Program
{
    static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Учебный проект TodoListApp");

        Console.Write("Имя: ");
        string? firstName = Console.ReadLine()?.Trim();
        Console.Write("Фамилия: ");
        string? lastName = Console.ReadLine()?.Trim();
        Console.Write("Год рождения: ");
        string? yearInput = Console.ReadLine();

        int currentYear = DateTime.Now.Year;
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Введите имя и фамилию.");
            return;
        }

        if (!int.TryParse(yearInput, out int birthYear) || birthYear < 1 || birthYear > currentYear)
        {
            Console.WriteLine("Введите корректный год рождения.");
            return;
        }

        int age = currentYear - birthYear;
        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

        Console.WriteLine("Введите help для списка команд.");

        string?[] todos = new string?[2];
        int todoCount = 0;

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input == null)
            {
                break;
            }

            string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                continue;
            }

            switch (parts[0])
            {
                case "help":
                    Console.WriteLine("help — список команд");
                    Console.WriteLine("profile — данные пользователя");
                    Console.WriteLine("add \"текст задачи\" — добавить задачу");
                    Console.WriteLine("view — показать задачи");
                    Console.WriteLine("exit — выход");
                    break;

                case "profile":
                    Console.WriteLine($"{firstName} {lastName}, {birthYear}");
                    break;

                case "add":
                    string text = parts.Length == 2 ? parts[1].Trim() : "";
                    if (text.Length < 2 || text[0] != '"' || text[^1] != '"')
                    {
                        Console.WriteLine("Используйте add \"текст задачи\".");
                        break;
                    }

                    text = text[1..^1].Trim();
                    if (text.Length == 0)
                    {
                        Console.WriteLine("Введите текст задачи.");
                        break;
                    }

                    if (todoCount == todos.Length)
                    {
                        var bigger = new string?[todos.Length * 2];
                        for (int i = 0; i < todos.Length; i++)
                        {
                            bigger[i] = todos[i];
                        }

                        todos = bigger;
                    }

                    todos[todoCount] = text;
                    todoCount++;
                    Console.WriteLine("Задача добавлена.");
                    break;

                case "view":
                    if (todoCount == 0)
                    {
                        Console.WriteLine("Список задач пуст.");
                    }

                    foreach (string? todo in todos)
                    {
                        if (!string.IsNullOrEmpty(todo))
                        {
                            Console.WriteLine(todo);
                        }
                    }

                    break;

                case "exit":
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Введите help.");
                    break;
            }
        }
    }
}
