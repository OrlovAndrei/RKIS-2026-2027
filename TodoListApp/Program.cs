﻿using System;
using System.IO;

namespace TodoListApp
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Работу выполнили Нечитайлов и Шишкина\n");

            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            FileManager.EnsureDataDirectory(dataDir);

            string profilePath = Path.Combine(dataDir, "profile.txt");
            string todoPath = Path.Combine(dataDir, "todo.csv");

            Profile profile = null;
            if (File.Exists(profilePath))
            {
                profile = FileManager.LoadProfile(profilePath);
            }
            
            if (profile == null)
            {
                Console.Write("Введите ваше имя: ");
                string firstName = Console.ReadLine();
                Console.Write("Введите вашу фамилию: ");
                string lastName = Console.ReadLine();
                Console.Write("Введите ваш год рождения: ");
                string birthYear = Console.ReadLine();
                profile = new Profile(firstName, lastName, birthYear);
                FileManager.SaveProfile(profile, profilePath);
            }

            TodoList todoList = new TodoList();
            if (File.Exists(todoPath))
            {
                todoList = FileManager.LoadTodos(todoPath);
            }
            else
            {
                File.WriteAllText(todoPath, "");
            }

            Console.WriteLine("\nДля списка команд введите 'help'\n");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                ICommand command = CommandParser.Parse(input, todoList, profile);
                
                if (command != null)
                {
                    command.Execute();
                    
                    if (command is AddCommand || command is DoneCommand || command is DeleteCommand || command is UpdateCommand)
                    {
                        FileManager.SaveTodos(todoList, todoPath);
                    }

                    if (command is ExitCommand) break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help'.");
                }
            }
        }
    }
}