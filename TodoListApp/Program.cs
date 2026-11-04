using System;
using System.Text;

namespace TodoList
{
    class Program
    {
        static string name1;
        static string name2;
        static int year;
        static string[] todos = new string[2];
        static int todoCount = 0;

        static bool[] statues = new bool[2];
        static DateTime[] dates = new DateTime[2];

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Сипач и Пасалиди");

            //Практическая работа №2
            Console.Write("Введите имя: ");
            name1 = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            name2 = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string yearInput = Console.ReadLine();
            year = int.Parse(yearInput);

            int curYear = DateTime.Now.Year;
            int age = curYear - year;
            Console.WriteLine($"Добавлен пользователь {name2} {name1}, возраст - {age}");
            Console.WriteLine();

            //Практическая работа №3
            todos = new string[2];
            todoCount = 0;

            while (true)
            {
                Console.Write("> ");
                string inputLine = Console.ReadLine();
                
                if (inputLine == null) continue;
                inputLine = inputLine.Trim();
                if (string.IsNullOrEmpty(inputLine)) continue;

                string[] parts = inputLine.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();
                string argsString = parts.Length > 1 ? parts[1] : "";

                if (command == "exit")
                {
                    Console.WriteLine("Завершение программы");
                    break;
                }
                 else if (command == "help") { Help(); }
                else if (command == "profile") { Profile(); }
                else if (command == "add") { Add(argsString); }
                else if (command == "view") { View(argsString); }
                else if (command == "read") { Read(argsString); } 
                else if (command == "done") { Done(argsString); }
                else if (command == "delete") { Delete(argsString); }
                else if (command == "update") { Update(argsString); }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите help");
                }
            }
        }

        static void Help()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - список команд");
            Console.WriteLine("profile - данные пользователя");
            Console.WriteLine("add \"текст\" - добавить задачу");
            Console.WriteLine("done \"номер\" - пометить задачу выполненной");
            Console.WriteLine("update \"номер\" \"текст\" - обновляет текст задачи");
            Console.WriteLine("delete \"номер\" - удалить задачу");
            Console.WriteLine("view - показывать задачи");
            Console.WriteLine("read <idx> - прочитать полный текст задачи");
            Console.WriteLine("exit - выход");
            Console.WriteLine("\nДоступные флаги:");
            Console.WriteLine("view -i / --index - показать индекс");
            Console.WriteLine("view -s / --status - показать статус");
            Console.WriteLine("view -d / --update-date - показать дату изменения");
            Console.WriteLine("view -a / --all - показать все данные");
            Console.WriteLine("add -m / --multiline - добавить задачу в несколько строк (завершение !end)");
        }

        static void Profile()
        {
            Console.WriteLine($"{name1} {name2}, {year}");
        }

        static void Add(string argsString)
        {
            bool isMultiline = argsString.Contains("-m") || argsString.Contains("--multiline");
            string task = "";

            if (isMultiline)
            {
                Console.WriteLine("Введите текст задачи (для завершения введите !end):");
                StringBuilder sb = new StringBuilder();
                
                while (true)
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    
                    if (line == null) break;
                    if (line.Trim() == "!end") break; 
                    
                    if (sb.Length > 0) sb.Append("\n"); 
                    sb.Append(line);
                }
                task = sb.ToString();
            }
            else
            {
                task = argsString.Trim('"');
            }

            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Текст задачи пуст.");
                return;
            }

            if (todoCount >= todos.Length)
            {
                Arr();
            }
            todos[todoCount] = task;
            statues[todoCount] = false;
            dates[todoCount] = DateTime.Now;
            todoCount++;
            Console.WriteLine($"Задача добавлена: {task}");
        }

        static void View(string argsString)
        {
            if (todoCount == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            bool showIndex = HasFlag(argsString, "i", "--index") || HasFlag(argsString, "a", "--all");
            bool showStatus = HasFlag(argsString, "s", "--status") || HasFlag(argsString, "a", "--all");
            bool showDate = HasFlag(argsString, "d", "--update-date") || HasFlag(argsString, "a", "--all");

            if (showIndex) Console.Write($" {"ID",-4} ");
            if (showStatus) Console.Write($" {"Статус",-12} ");
            if (showDate) Console.Write($" {"Дата изменения",-25} ");
            Console.WriteLine($" {"Текст задачи",-30} ");

            for (int i = 0; i < todoCount; i++)
            {
                if (showIndex) Console.Write($" {i + 1,-4} ");
                
                if (showStatus) 
                {
                    string st = statues[i] ? "выполнена" : "не выполнена";
                    Console.Write($" {st,-12} ");
                }

                if (showDate) 
                {
                    Console.Write($" {dates[i],-10} ");
                }

                string text = todos[i].Replace("\n", " "); 
                if (text.Length > 30)
                {
                    text = text.Substring(0, 27) + "...";
                }
                Console.WriteLine($" {text,-30} ");
            }

        }

        static void Arr()
        {
            string[] newTodos = new string[todos.Length * 2];
            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
            }
            todos = newTodos;

            bool[] newStatues = new bool[statues.Length * 2];
            for (int i = 0; i < statues.Length; i++)
            {
                newStatues[i] = statues[i];
            }
            statues = newStatues;

            DateTime[] newDates = new DateTime[dates.Length * 2];
            for (int i = 0; i < dates.Length; i++)
            {
                newDates[i] = dates[i];
            }
            dates = newDates;
            Console.WriteLine("Массивы расширены");
        }

        //Практическая работа №4
        static void Done(String argsString)
        {
            if (!TryGetIndex(argsString, out int index)) return;
            statues[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine("Задача помечена, как выполненная");
        }

        static void Delete(string argsString)
        {
            if (!TryGetIndex(argsString, out int index)) return;
            for (int i = index; i < todoCount - 1; i++)
            {
                todos[i] = todos[i+1];
                statues[i] = statues[i+1];
                dates[i] = dates[i+1];
            }
            todos[todoCount - 1] = null;
            statues[todoCount - 1] = false;
            dates[todoCount - 1] = DateTime.MinValue;
            todoCount--;
            Console.WriteLine($"Задача удалена.");
        }

        static void Update(string argsString)
        {
            string[] parts = argsString.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                Console.WriteLine("Укажите номер задачи и новый текст");
                return;
            }
            if (!TryGetIndex(parts[0], out int index)) return;

            string newText = parts[1].Trim('"');
            todos[index] = newText;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача обновлена.");
        }

        static bool TryGetIndex(string argsString, out int index)
        {
            index = -1;
            if (!int.TryParse(argsString, out int taskNumber))
            {
                Console.WriteLine("Номер задачи должен быть числом");
                return false;
            }
            index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine("Задачи с таким номером не существует");
                return false;
            }
            return true;
        }

        //Практическая работа №5
        static void Read(string argsString)
        {
            if (!TryGetIndex(argsString, out int index)) return;

            Console.WriteLine("Полный текст задачи:");
            Console.WriteLine(todos[index]);
            Console.WriteLine($"Статус: {(statues[index] ? "выполнена" : "не выполнена")}");
            Console.WriteLine($"Дата последнего изменения: {dates[index]}");
        }

        static bool HasFlag(string argsString, string shortName, string longName)
        {

            if (argsString.Contains(longName)) return true;

            string[] words = argsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                if (word.StartsWith("-") && !word.StartsWith("--"))
                {
                    if (word.Contains(shortName)) return true;
                }
            }
            return false;
        }
    }
}
