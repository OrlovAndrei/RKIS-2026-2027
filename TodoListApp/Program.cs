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
            TodoList todoList = new TodoList();

            Console.WriteLine("\nДля списка команд введите 'help'\n");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                ICommand command = CommandParser.Parse(input, todoList, profile);
                
                if (command != null)
                {
                    command.Execute();
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