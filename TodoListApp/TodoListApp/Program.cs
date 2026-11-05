
using System;

Console.WriteLine(".:| TodoListApp |:.");

Console.Write("Введите имя: ");
string name = Console.ReadLine() ?? "";

Console.Write("Введите фамилию: ");
string surname = Console.ReadLine() ?? "";

Console.Write("Введите год рождения: ");
int birthYear;

while (!int.TryParse(Console.ReadLine(), out birthYear))
{
    Console.Write("Введите год рождения: ");
}

string[] todos = new string[2];
int todoCount = 0;

Console.WriteLine();
Console.WriteLine("Добро пожаловать в .:| TodoListApp |:.");
Console.WriteLine("Введите help для просмотра команд.");

bool running = true;

while (running)
{
    Console.WriteLine();
    Console.Write("> ");

    string command = Console.ReadLine() ?? "";

    string[] parts = command.Trim().Split(
        ' ',
        2,
        StringSplitOptions.RemoveEmptyEntries
    );

    if (parts.Length == 0)
    {
        continue;
    }

    string action = parts[0].ToLower();

    switch (action)
    {
        case "help":
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - список команд");
            Console.WriteLine("profile - информация о пользователе");
            Console.WriteLine("add <текст> - добавить задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("exit - выход из программы");
            break;

        case "profile":
            Console.WriteLine(
                $"{name} {surname}, {birthYear}"
            );
            break;

        case "add":
            if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
            {
                Console.WriteLine("Использование: add <текст задачи>");
                break;
            }

            string task = parts[1];

            if (todoCount >= todos.Length)
            {
                string[] newTodos = new string[todos.Length * 2];

                for (int i = 0; i < todos.Length; i++)
                {
                    newTodos[i] = todos[i];
                }

                todos = newTodos;

                Console.WriteLine("Размер массива увеличен!");
            }

            todos[todoCount] = task;
            todoCount++;

            Console.WriteLine("Задача добавлена!");
            break;

        case "view":
            Console.WriteLine("Список задач:");

            bool hasTasks = false;

            for (int i = 0; i < todos.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(todos[i]))
                {
                    Console.WriteLine($"{i + 1}. {todos[i]}");
                    hasTasks = true;
                }
            }

            if (!hasTasks)
            {
                Console.WriteLine("Список задач пуст.");
            }

            break;

        case "exit":
            running = false;
            Console.WriteLine("Программа завершена.");
            break;

        default:
            Console.WriteLine("Непонятная команда. Введите help.");
            break;
    }
}
