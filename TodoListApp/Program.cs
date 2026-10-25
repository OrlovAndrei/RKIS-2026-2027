﻿using System;
using System.Text;

namespace TodoListApp
{
    class Program
    {
        private const int InitialCapacity = 2;
        private const string AppName = "TodoListApp";

        static void Main(string[] args)
        {
            string firstName = "Виталий";
            string lastName = "Слесарев";
            int birthYear = 2004;

            string[] todos = new string[InitialCapacity];
            bool[] statuses = new bool[InitialCapacity];
            DateTime[] dates = new DateTime[InitialCapacity];
            int todoCount = 0;

            Console.WriteLine($"{AppName} запущен. Введите 'help' для списка команд.");

            bool isRunning = true;
            while (isRunning)
            {
                Console.Write("\nВведите команду: ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input)) continue;

                string[] parts = input.Split(new[] { ' ' }, 2);
                string command = parts[0].ToLower();
                string argument = parts.Length > 1 ? parts[1] : string.Empty;

                switch (command)
                {
                    case "help":
                        PrintHelp();
                        break;
                    case "profile":
                        PrintProfile(firstName, lastName, birthYear);
                        break;
                    case "add":
                        AddTask(ref todos, ref statuses, ref dates, ref todoCount, argument);
                        break;
                    case "view":
                        ViewTasks(todos, statuses, dates, todoCount, argument);
                        break;
                    case "read":
                        ReadTask(todos, statuses, dates, todoCount, argument);
                        break;
                    case "done":
                        MarkTaskDone(statuses, dates, todoCount, argument);
                        break;
                    case "update":
                        UpdateTask(todos, dates, todoCount, argument);
                        break;
                    case "delete":
                        DeleteTask(ref todos, ref statuses, ref dates, ref todoCount, argument);
                        break;
                    case "exit":
                        isRunning = false;
                        Console.WriteLine("Завершение работы программы...");
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help'.");
                        break;
                }
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("  help                               - список команд");
            Console.WriteLine("  profile                            - данные пользователя");
            Console.WriteLine("  add [--multiline | -m]             - добавить задачу");
            Console.WriteLine("  view [флаги]                       - просмотр (флаги: -i, -s, -d, -a)");
            Console.WriteLine("  read <индекс>                      - просмотреть полный текст задачи");
            Console.WriteLine("  done <индекс>                      - отметить выполненной");
            Console.WriteLine("  update <индекс> \"текст\"            - обновить текст");
            Console.WriteLine("  delete <индекс>                    - удалить задачу");
            Console.WriteLine("  exit                               - выход");
        }

        private static void PrintProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"Профиль: {firstName} {lastName}, {birthYear}");
        }

        private static void AddTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count, string argument)
        {
            bool isMultiline = argument.Contains("--multiline") || argument.Contains("-m");
            string taskText = string.Empty;

            if (isMultiline)
            {
                Console.WriteLine("Многострочный режим. Вводите строки. Для завершения введите '!end':");
                StringBuilder sb = new StringBuilder();
                bool firstLine = true;

                while (true)
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    if (line == null || line.Trim() == "!end") break;

                    if (!firstLine) sb.Append("\n");
                    sb.Append(line);
                    firstLine = false;
                }
                taskText = sb.ToString();
            }
            else
            {
                Console.Write("Введите текст задачи: ");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Ошибка: Текст задачи пуст.");
                    return;
                }
                taskText = line.Trim('"');
            }

            if (string.IsNullOrEmpty(taskText))
            {
                Console.WriteLine("Ошибка: Пустая задача не добавлена.");
                return;
            }

            if (count >= todos.Length)
            {
                ExpandArrays(ref todos, ref statuses, ref dates);
            }

            todos[count] = taskText;
            statuses[count] = false;
            dates[count] = DateTime.Now;
            count++;

            Console.WriteLine("Задача успешно добавлена!");
        }

        private static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, int count, string argument)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            bool showIndex = argument.Contains("--index") || argument.Contains("-i") || argument.Contains("--all") || argument.Contains("-a");
            bool showStatus = argument.Contains("--status") || argument.Contains("-s") || argument.Contains("--all") || argument.Contains("-a");
            bool showDate = argument.Contains("--update-date") || argument.Contains("-d") || argument.Contains("--all") || argument.Contains("-a");

            Console.WriteLine("\n--- Список задач ---");
            for (int i = 0; i < count; i++)
            {
                if (todos[i] == null) continue;

                string line = "";
                if (showIndex) line += $"[{i + 1}] ";

                string rawText = todos[i].Replace("\n", " ");
                string truncatedText = rawText.Length > 30 ? rawText.Substring(0, 27) + "..." : rawText;
                line += $"Задача: {truncatedText,-33}";

                if (showStatus)
                {
                    string statusText = statuses[i] ? "Выполнено" : "Не выполнено";
                    line += $" | Статус: {statusText,-13}";
                }

                if (showDate)
                {
                    line += $" | Дата: {dates[i]:dd.MM.yyyy HH:mm}";
                }

                Console.WriteLine(line);
            }
            Console.WriteLine("--------------------");
        }

        private static void ReadTask(string[] todos, bool[] statuses, DateTime[] dates, int count, string argument)
        {
            if (!int.TryParse(argument.Trim(), out int index) || index < 1 || index > count)
            {
                Console.WriteLine("Ошибка: Укажите корректный номер задачи. Пример: read 1");
                return;
            }

            int realIndex = index - 1;
            Console.WriteLine($"\n--- Карточка задачи #{index} ---");
            Console.WriteLine($"Полный текст:\n{todos[realIndex]}");
            Console.WriteLine($"Статус: {(statuses[realIndex] ? "выполнена" : "не выполнена")}");
            Console.WriteLine($"Дата изменения: {dates[realIndex]:dd.MM.yyyy HH:mm:ss}");
            Console.WriteLine("-----------------------------");
        }

        private static void MarkTaskDone(bool[] statuses, DateTime[] dates, int count, string argument)
        {
            if (!int.TryParse(argument.Trim(), out int index) || index < 1 || index > count)
            {
                Console.WriteLine("Ошибка: Неверный номер задачи.");
                return;
            }

            int realIndex = index - 1;
            statuses[realIndex] = true;
            dates[realIndex] = DateTime.Now;
            Console.WriteLine($"Задача {index} отмечена как выполненная!");
        }

        private static void UpdateTask(string[] todos, DateTime[] dates, int count, string argument)
        {
            string[] parts = argument.Split(new[] { ' ' }, 2);
            if (parts.Length < 2 || !int.TryParse(parts[0], out int index) || index < 1 || index > count)
            {
                Console.WriteLine("Ошибка формата. Пример: update 1 \"Новый текст\"");
                return;
            }

            int realIndex = index - 1;
            todos[realIndex] = parts[1].Trim('"');
            dates[realIndex] = DateTime.Now;
            Console.WriteLine($"Задача {index} успешно обновлена!");
        }

        private static void DeleteTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count, string argument)
        {
            if (!int.TryParse(argument.Trim(), out int index) || index < 1 || index > count)
            {
                Console.WriteLine("Ошибка: Неверный номер задачи.");
                return;
            }

            int realIndex = index - 1;

            for (int i = realIndex; i < count - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            todos[count - 1] = null;
            statuses[count - 1] = false;
            dates[count - 1] = default;
            count--;

            Console.WriteLine($"Задача {index} удалена!");
        }

        private static void ExpandArrays(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
        {
            int newSize = todos.Length * 2;
            string[] newTodos = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];

            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;
        }
    }
}