using System;
using System.Text;

namespace TodoList
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настраиваем кодировку
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Работу выполнили: Осинцева и Малюкова");

            // === ШАГ 1: Ввод профиля при старте ===
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine() ?? "";

            Console.Write("Введите год рождения: ");
            int.TryParse(Console.ReadLine(), out int birthYear);

            // Создаем объект профиля с введенными данными
            Profile userProfile = new Profile(firstName, lastName, birthYear);
            Console.WriteLine($"\nДобро пожаловать, {firstName} {lastName}!");

            // Создаем объект списка задач
            TodoList myTodoList = new TodoList();

            Console.WriteLine("Введите 'help' для списка команд.\n");

            // === ШАГ 2: Главный цикл с CommandParser ===
            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                // Отдаем строку парсеру (передаем туда созданные профиль и список задач)
                ICommand command = CommandParser.Parse(input, myTodoList, userProfile);
                
                // Если команда распознана - выполняем
                if (command != null)
                {
                    command.Execute();
                }
            }
        }
    }
}