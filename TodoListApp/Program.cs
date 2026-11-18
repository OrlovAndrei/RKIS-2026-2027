using System;
using System.Text;
using TodoListApp;

namespace TodoList
{
    class Program
    {
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

            Profile user = new Profile(name1, name2, year);
            Console.WriteLine($"Добавлен пользователь {user.FirstName} {user.LastName}");
            Console.WriteLine();

            TodoList myList = new TodoList();

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
                else if (command == "profile") { Console.WriteLine(user.GetInfo()); }
                else if (command == "add") { AddTask(myList, argsString); }
                else if (command == "view") { View(myList, argsString); }
                else if (command == "read") { ReadTask(myList, argsString); } 
                else if (command == "done") { Done(myList, argsString); }
                else if (command == "delete") { Delete(myList, argsString); }
                else if (command == "update") { Update(myList, argsString); }
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
            Console.WriteLine("add -m / --multiline - добавить задачу в несколько строк (завершение !end)");
            Console.WriteLine("done \"номер\" - пометить задачу выполненной");
            Console.WriteLine("update \"номер\" \"текст\" - обновляет текст задачи");
            Console.WriteLine("delete \"номер\" - удалить задачу");
            Console.WriteLine("view [флаги] - показывать задачи");
            Console.WriteLine("  -i, --index       - показать индекс");
            Console.WriteLine("  -s, --status      - показать статус");
            Console.WriteLine("  -d, --update-date - показать дату изменения");
            Console.WriteLine("  -a, --all         - показать все данные");
            Console.WriteLine("read <idx> - прочитать полный текст задачи");
            Console.WriteLine("exit - выход");
        }

        

                static void AddTask(TodoList list, string argsString)
        {
            bool isMultiline = argsString.Contains("-m") || argsString.Contains("--multiline")
            argsString = argsString.Replace("-m", "").Replace("--multiline", "").Trim();
            string task = "";

            if (isMultiline)
            {
                Console.WriteLine("Введите текст задачи (для завершения введите !end):");
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(argsString))
                {
                    sb.Append(argsString.Trim('"'));
                }
                
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

            TodoItem newItem = new TodoItem(taskText);
            list.Add(newItem);
            Console.WriteLine($"Задача добавлена: {taskText}");
        }

         static void View(TodoList list, string argsString)
        {

            bool showIndex = HasFlag(argsString, "i", "--index") || HasFlag(argsString, "a", "--all");
            bool showStatus = HasFlag(argsString, "s", "--status") || HasFlag(argsString, "a", "--all");
            bool showDate = HasFlag(argsString, "d", "--update-date") || HasFlag(argsString, "a", "--all");

           list.View(showIndex, showStatus, showDate);
        }

        static void ReadTask(TodoList list, string argsString)
        {
           if (!TryGetIndex(argsString, list, out int index)) return;
            
            TodoItem item = list.GetItem(index);
            Console.WriteLine(item.GetFullInfo());
        }
        
        static void Done(String argsString)
        {
            if (!TryGetIndex(argsString, list, out int index)) return;
            
            TodoItem item = list.GetItem(index);
            item.MarkDone();
            Console.WriteLine("Задача помечена, как выполненная");
        }

        static void Delete(string argsString)
        {
            if (!TryGetIndex(argsString, list, out int index)) return;
            
            list.Delete(index);
            Console.WriteLine($"Задача {index + 1} удалена.");
        }

        static void Update(TodoList list, string argsString)
        {
            string[] parts = argsString.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                Console.WriteLine("Укажите номер задачи и новый текст");
                return;
            }
            if (!TryGetIndex(parts[0], list, out int index)) return;

            string newText = parts[1].Trim('"');
            
            TodoItem item = list.GetItem(index);
            item.UpdateText(newText);
            Console.WriteLine($"Задача {index + 1} обновлена.");
        }

         static bool TryGetIndex(string argsString, TodoList list, out int index)
        {
            index = -1;
            if (!int.TryParse(argsString, out int taskNumber))
            {
                Console.WriteLine("Номер задачи должен быть числом");
                return false;
            }
            index = taskNumber - 1;
            
            if (list.GetItem(index) == null)
            {
                return false;
            }
            return true;
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
