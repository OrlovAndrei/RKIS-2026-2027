using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Работу выполнили Кузнецов и Курбатов");

        string? firstName = ReadRequired("Введите имя: ");
        if (firstName == null) return;

        string? lastName = ReadRequired("Введите фамилию: ");
        if (lastName == null) return;

        int? birthYear = ReadBirthYear();
        if (birthYear == null) return;

        Profile profile = new Profile(firstName, lastName, birthYear.Value);
        TodoList todoList = new TodoList();

        Console.WriteLine("Добавлен пользователь " + profile.GetInfo());

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
