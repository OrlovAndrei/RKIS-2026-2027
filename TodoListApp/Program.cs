using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string birthYearText = Console.ReadLine();

        int birthYear = Convert.ToInt32(birthYearText);

        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;

        Console.WriteLine();
        Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст — {age}");
        Console.WriteLine();

string[] todos = new string[2];
int todoCount = 0;

Console.WriteLine("Введите help для списка команд.");

while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    int spaceIndex = input.IndexOf(' ');
    string command = spaceIndex == -1 ? input : input.Substring(0, spaceIndex);

    switch (command)
    {
        case "help":
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help     — список всех доступных команд");
            Console.WriteLine("profile  — данные пользователя: Имя Фамилия, Год рождения");
            Console.WriteLine("add \"текст задачи\" — добавить новую задачу");
            Console.WriteLine("view     — показать все задачи");
            Console.WriteLine("exit     — завершить программу");
            break;

        case "profile":
            Console.WriteLine($"{name} {surname}, {birthYear}");
            break;

        case "add":
            string[] parts = input.Split(new[] { ' ' }, 2);

            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
            {
                Console.WriteLine("Использование: add \"текст задачи\"");
                break;
            }

            string taskText = parts[1].Trim('"');

            if (todoCount >= todos.Length)
            {
                string[] newTodos = new string[todos.Length * 2];
                for (int i = 0; i < todos.Length; i++)
                {
                    newTodos[i] = todos[i];
                }
                todos = newTodos;
                Console.WriteLine($"Массив расширен, новая длина: {todos.Length}");
            }

            todos[todoCount] = taskText;
            todoCount++;
            Console.WriteLine($"Задача \"{taskText}\" добавлена.");
            break;

        case "view":
            Console.WriteLine("Ваши задачи:");
            bool hasTasks = false;
            for (int i = 0; i < todos.Length; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    Console.WriteLine($"{i + 1}. {todos[i]}");
                    hasTasks = true;
                }
            }
            if (!hasTasks)
            {
                Console.WriteLine("Задач пока нет.");
            }
            break;

        case "exit":
            Console.WriteLine("Выход из программы...");
            return;

        default:
            Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
            break;
    }
}

        }
    }
}
