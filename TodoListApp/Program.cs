﻿using System;

namespace TodoListApp
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Работу выполнили Нечитайлов и Шишкина\n");

            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите ваш год рождения: ");
            string birthYear = Console.ReadLine();

            Profile profile = new Profile(firstName, lastName, birthYear);
            Console.WriteLine(profile.GetInfo());

            Console.WriteLine("\nДля списка команд введите 'help'\n");

            TodoList todoList = new TodoList();

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                string[] parts = input.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();
                string arguments = parts.Length > 1 ? parts[1] : "";

                if (command == "help")
                {
                    ShowHelp();
                }
                else if (command == "profile")
                {
                    Console.WriteLine(profile.GetInfo());
                }
                else if (command == "add")
                {
                    string taskText = "";
                    if (arguments.Contains("-m") || arguments.Contains("--multiline"))
                    {
                        taskText = ReadMultilineTask();
                    }
                    else
                    {
                        taskText = arguments.Trim('"');
                    }

                    if (!string.IsNullOrEmpty(taskText))
                    {
                        todoList.Add(new TodoItem(taskText));
                        Console.WriteLine("Задача добавлена.");
                    }
                }
                else if (command == "view")
                {
                    bool showIndex = arguments.Contains("-i") || arguments.Contains("--index");
                    bool showStatus = arguments.Contains("-s") || arguments.Contains("--status");
                    bool showDate = arguments.Contains("-d") || arguments.Contains("--update-date");
                    bool showAll = arguments.Contains("-a") || arguments.Contains("--all");

                    if (showAll)
                    {
                        showIndex = true;
                        showStatus = true;
                        showDate = true;
                    }

                    todoList.View(showIndex, showStatus, showDate);
                }
                else if (command == "done")
                {
                    if (int.TryParse(arguments, out int index))
                    {
                        TodoItem item = todoList.GetItem(index);
                        if (item != null)
                        {
                            item.MarkDone();
                            Console.WriteLine($"Задача {index} выполнена.");
                        }
                        else Console.WriteLine("Ошибка: неверный индекс.");
                    }
                }
                else if (command == "delete")
                {
                    if (int.TryParse(arguments, out int index))
                    {
                        todoList.Delete(index);
                    }
                }
                else if (command == "update")
                {
                    string[] updateParts = arguments.Split(new char[] { ' ' }, 2);
                    if (updateParts.Length > 1 && int.TryParse(updateParts[0], out int index))
                    {
                        TodoItem item = todoList.GetItem(index);
                        if (item != null)
                        {
                            item.UpdateText(updateParts[1].Trim('"'));
                            Console.WriteLine($"Задача {index} обновлена.");
                        }
                        else Console.WriteLine("Ошибка: неверный индекс.");
                    }
                }
                else if (command == "read")
                {
                    if (int.TryParse(arguments, out int index))
                    {
                        TodoItem item = todoList.GetItem(index);
                        if (item != null)
                        {
                            Console.WriteLine(item.GetFullInfo());
                        }
                        else Console.WriteLine("Ошибка: неверный индекс.");
                    }
                }
                else if (command == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help'.");
                }
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("  help            - Список команд");
            Console.WriteLine("  profile         - Данные пользователя");
            Console.WriteLine("  add \"текст\"      - Добавить задачу");
            Console.WriteLine("  add -m          - Многострочный ввод (!end для завершения)");
            Console.WriteLine("  view            - Показать задачи (текст)");
            Console.WriteLine("  view -a         - Показать всё (индекс, текст, статус, дата)");
            Console.WriteLine("  read <idx>      - Полный текст задачи");
            Console.WriteLine("  done <idx>      - Отметить задачу выполненной");
            Console.WriteLine("  delete <idx>    - Удалить задачу");
            Console.WriteLine("  update <idx> \"т\" - Обновить текст задачи");
            Console.WriteLine("  exit            - Выход");
        }

        static string ReadMultilineTask()
        {
            Console.WriteLine("Введите строки задачи (для завершения введите !end):");
            string result = "";
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == null || line.Trim() == "!end") break;
                if (result != "") result += "\n";
                result += line;
            }
            return result;
        }
    }
}