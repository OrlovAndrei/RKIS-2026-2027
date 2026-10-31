using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("Работу выполнили Кузнецов и Курбатов");

        // Папка data создаётся внутри TodoListApp, если запускать из неё.
        string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
        FileManager.EnsureDataDirectory(dataDir);
        string profilePath = Path.Combine(dataDir, "profile.txt");
        string todoPath = Path.Combine(dataDir, "todo.csv");
        FileManager.TodoFilePath = todoPath;

        Profile profile;
        if (File.Exists(profilePath))
        {
            try
            {
                profile = FileManager.LoadProfile(profilePath);
                Console.WriteLine("Профиль загружен: " + profile.GetInfo());
            }
            catch (Exception ex) when (ex is IOException || ex is ArgumentException)
            {
                Console.WriteLine("Не удалось загрузить профиль: " + ex.Message);
                return; // Не перезаписываем существующий повреждённый файл.
            }
        }
        else
        {
            string? firstName = ReadRequired("Введите имя: ");
            if (firstName == null) return;
            string? lastName = ReadRequired("Введите фамилию: ");
            if (lastName == null) return;
            int? birthYear = ReadBirthYear();
            if (birthYear == null) return;
            profile = new Profile(firstName, lastName, birthYear.Value);

            try { FileManager.SaveProfile(profile, profilePath); }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                Console.WriteLine("Не удалось сохранить профиль: " + ex.Message);
                return;
            }
            Console.WriteLine("Добавлен пользователь " + profile.GetInfo());
        }

        TodoList todoList;
        try
        {
            if (File.Exists(todoPath))
            {
                todoList = FileManager.LoadTodos(todoPath);
                Console.WriteLine("Загружено задач: " + todoList.Count);
            }
            else
            {
                todoList = new TodoList();
                FileManager.SaveTodos(todoList, todoPath);
                Console.WriteLine("Создан новый список задач");
            }
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException)
        {
            Console.WriteLine("Не удалось загрузить или создать список задач: " + ex.Message);
            return;
        }

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input == null) break;
            if (string.IsNullOrWhiteSpace(input)) continue;

            ICommand command = CommandParser.Parse(input, todoList, profile);
            command.Execute();
            if (command is ExitCommand) break;
        }
    }

    static string? ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine();
            if (value == null) return null;
            if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
            Console.WriteLine("Поле не должно быть пустым");
        }
    }

    static int? ReadBirthYear()
    {
        while (true)
        {
            Console.Write("Введите год рождения: ");
            string? text = Console.ReadLine();
            if (text == null) return null;
            if (int.TryParse(text, out int year) && year >= 1900 && year <= DateTime.Now.Year)
                return year;
            Console.WriteLine("Введите корректный год рождения числом");
        }
    }
}
