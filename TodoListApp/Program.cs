using System;
using System.IO;
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

            // === 1. НАСТРОЙКА ПУТЕЙ (Лекция 8) ===
            // Создаем папку data рядом с .exe файлом (используем Path.Combine)
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            FileManager.EnsureDataDirectory(dataDir);
            
            string profilePath = Path.Combine(dataDir, "profile.txt");
            string todoPath = Path.Combine(dataDir, "todo.csv");

            // === 2. ЗАГРУЗКА ПРОФИЛЯ ===
            // Пытаемся загрузить профиль
            Profile? userProfile = FileManager.LoadProfile(profilePath);
            
            // Если профиля нет (LoadProfile вернул null), спрашиваем данные
            if (userProfile == null)
            {
               // === ШАГ 1: Ввод профиля при старте ===
               Console.Write("Введите имя: ");
               string firstName = Console.ReadLine() ?? "";

               Console.Write("Введите фамилию: ");
               string lastName = Console.ReadLine() ?? "";

               Console.Write("Введите год рождения: ");
               int.TryParse(Console.ReadLine(), out int birthYear);

               // Создаем объект профиля с введенными данными
               userProfile = new Profile(firstName, lastName, birthYear);
               // Сразу сохраняем созданный профиль в файл
               FileManager.SaveProfile(userProfile, profilePath);
            } 
            
            Console.WriteLine($"\nДобро пожаловать, {userProfile.FirstName} {userProfile.LastName}!");

            // === 3. ЗАГРУЗКА ЗАДАЧ ===
            // Загружаем задачи из CSV-файла
            TodoList myTodoList = FileManager.LoadTodos(todoPath);

            Console.WriteLine("Введите 'help' для списка команд.\n");

            // === ШАГ 2: Главный цикл с CommandParser ===
            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                // Отдаем строку парсеру (передаем туда созданные профиль, список задач и т.д.)
                ICommand? command = CommandParser.Parse(input, myTodoList, userProfile, todoPath, profilePath);
                
                // Если команда распознана - выполняем
                if (command != null)
                {
                    command.Execute();
                }
            }
        }
    }
}