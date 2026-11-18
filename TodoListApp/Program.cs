using System;
using System.Text;

namespace TodoListApp
{
    class Program
    {
        const string CommandHelp = "help";
        const string CommandProfile = "profile";
        const string CommandAdd = "add";
        const string CommandView = "view";
        const string CommandDone = "done";
        const string CommandDelete = "delete";
        const string CommandUpdate = "update";
        const string CommandRead = "read";
        const string CommandExit = "exit";

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Катмаков и Науменко.");

            Console.Write("Введите имя: ");
            string? firstNameInput = Console.ReadLine();
            if (firstNameInput == null)
            {
                Console.WriteLine("Завершение работы программы.");
                return;
            }
            string firstName = string.IsNullOrWhiteSpace(firstNameInput) ? "Неизвестно" : firstNameInput;

            Console.Write("Введите фамилию: ");
            string? lastNameInput = Console.ReadLine();
            if (lastNameInput == null)
            {
                Console.WriteLine("Завершение работы программы.");
                return;
            }
            string lastName = string.IsNullOrWhiteSpace(lastNameInput) ? "Неизвестно" : lastNameInput;

            Console.Write("Введите год рождения: ");
            int birthYear;
            while (true)
            {
                string? yearInput = Console.ReadLine();

                if (yearInput == null)
                {
                    Console.WriteLine("Завершение работы программы.");
                    return;
                }

                if (int.TryParse(yearInput, out birthYear))
                    break;

                Console.Write("Ошибка! Введите корректный год (числом): ");
            }

            Profile userProfile = new Profile(firstName, lastName, birthYear);
            Console.WriteLine($"Добавлен пользователь {userProfile.GetInfo()}.");

            TodoList todoList = new TodoList();

            Console.WriteLine("Добро пожаловать в TodoList! Введите help для отображения списка команд.");

            while (true)
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Завершение работы программы.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] parts = input.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                    continue;

                string command = parts[0].ToLower();
                string argument = parts.Length > 1 ? parts[1] : "";

                switch (command)
                {
                    case CommandHelp:
                        HandleHelp();
                        break;
                    case CommandProfile:
                        HandleProfile(userProfile);
                        break;
                    case CommandAdd:
                        HandleAdd(todoList, argument);
                        break;
                    case CommandView:
                        HandleView(todoList, argument);
                        break;
                    case CommandDone:
                        HandleDone(todoList, argument);
                        break;
                    case CommandDelete:
                        HandleDelete(todoList, argument);
                        break;
                    case CommandUpdate:
                        HandleUpdate(todoList, argument);
                        break;
                    case CommandRead:
                        HandleRead(todoList, argument);
                        break;
                    case CommandExit:
                        Console.WriteLine("Завершение работы программы.");
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите help.");
                        break;
                }
            }
        }

        static void HandleHelp()
        {
            Console.WriteLine("help - список команд");
            Console.WriteLine("profile - данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить задачу");
            Console.WriteLine("add --multiline или add -m - многострочная задача");
            Console.WriteLine("view -i или --index - показать индекс в таблице");
            Console.WriteLine("view -s или --status - показать статус в таблице");
            Console.WriteLine("view -d или --update-date - показать дату изменения");
            Console.WriteLine("view -a или --all - показать все данные в таблице");
            Console.WriteLine("Флаги view можно комбинировать, например: view -is");
            Console.WriteLine("done <номер задачи> - отметить задачу как выполненную");
            Console.WriteLine("delete <номер задачи> - удалить задачу");
            Console.WriteLine("update <номер задачи> \"новый текст\" - обновить задачу");
            Console.WriteLine("read <номер задачи> - прочитать полный текст задачи без обрезки");
            Console.WriteLine("exit - выход");
        }

        static void HandleProfile(Profile profile)
        {
            Console.WriteLine(profile.GetInfo());
        }

        static void HandleAdd(TodoList todoList, string argument)
        {
            string trimmedArg = argument.Trim();
            bool isMultiline = trimmedArg == "-m" || trimmedArg == "--multiline";
            string task = "";

            if (isMultiline)
            {
                Console.WriteLine("Введите строки задачи (для завершения введите !end):");
                StringBuilder sb = new StringBuilder();
                bool isFirstLine = true;

                while (true)
                {
                    Console.Write("> ");
                    string? line = Console.ReadLine();

                    if (line == null)
                    {
                        Console.WriteLine("Критическая ошибка ввода. Завершение работы.");
                        Environment.Exit(0);
                    }

                    if (line == "!end")
                        break;

                    if (!isFirstLine)
                        sb.Append("\n");

                    sb.Append(line);
                    isFirstLine = false;
                }

                task = sb.ToString();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(argument))
                {
                    Console.WriteLine("Нужно ввести текст задачи");
                    return;
                }

                task = argument.Trim('"');
            }

            if (string.IsNullOrWhiteSpace(task))
                return;

            TodoItem newItem = new TodoItem(task);
            todoList.Add(newItem);
            Console.WriteLine("Задача добавлена.");
        }

        static void HandleView(TodoList todoList, string argument)
        {
            bool showAll = false;
            bool showIndex = false;
            bool showStatus = false;
            bool showDate = false;

            string[] flags = argument.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string flag in flags)
            {
                if (flag == "--index") showIndex = true;
                else if (flag == "--status") showStatus = true;
                else if (flag == "--update-date") showDate = true;
                else if (flag == "--all") showAll = true;
                else if (flag.StartsWith("-") && !flag.StartsWith("--"))
                {
                    for (int j = 1; j < flag.Length; j++)
                    {
                        if (flag[j] == 'i') showIndex = true;
                        else if (flag[j] == 's') showStatus = true;
                        else if (flag[j] == 'd') showDate = true;
                        else if (flag[j] == 'a') showAll = true;
                    }
                }
            }

            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }

            todoList.View(showIndex, showStatus, showDate);
        }

        static void HandleDone(TodoList todoList, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Введите номер задачи.");
                return;
            }

            string cleanArgument = argument.Trim('<', '>');
            if (!int.TryParse(cleanArgument, out int taskNumber))
            {
                Console.WriteLine("Некорректный номер задачи.");
                return;
            }

            TodoItem? item = todoList.GetItem(taskNumber - 1);
            if (item == null)
            {
                Console.WriteLine("Задача с таким номером не найдена.");
                return;
            }

            item.MarkDone();
            Console.WriteLine($"Задача №{taskNumber} отмечена как выполненная.");
        }

        static void HandleDelete(TodoList todoList, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Введите номер задачи.");
                return;
            }

            string cleanArgument = argument.Trim('<', '>');
            if (!int.TryParse(cleanArgument, out int taskNumber))
            {
                Console.WriteLine("Некорректный номер задачи.");
                return;
            }

            if (!todoList.Delete(taskNumber - 1))
            {
                Console.WriteLine("Задача с таким номером не найдена.");
                return;
            }
            Console.WriteLine($"Задача №{taskNumber} успешно удалена.");
        }
        
        static void HandleUpdate(TodoList todoList, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Введите номер задачи и новый текст.");
                return;
            }
            int spaceIndex = argument.IndexOf(' ');
            if (spaceIndex == -1)
            {
                Console.WriteLine("Введите новый текст задачи после её номера.");
                return;
            }
            string numberPart = argument.Substring(0, spaceIndex).Trim('<', '>');
            string textPart = argument.Substring(spaceIndex + 1);
            if (!int.TryParse(numberPart, out int taskNumber))
            {
                Console.WriteLine("Некорректный номер задачи.");
                return;
            }
            TodoItem? item = todoList.GetItem(taskNumber - 1);
            if (item == null)
            {
                Console.WriteLine("Задача с таким номером не найдена.");
                return;
            }
            string newText = textPart.Trim(' ', '"');
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Текст задачи не может быть пустым.");
                return;
            }
            item.UpdateText(newText);
            Console.WriteLine($"Задача №{taskNumber} успешно обновлена.");
        }
        
        static void HandleRead(TodoList todoList, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Введите номер задачи.");
                return;
            }
            string cleanArgument = argument.Trim('<', '>');
            if (!int.TryParse(cleanArgument, out int taskNumber))
            {
                Console.WriteLine("Некорректный номер задачи.");
                return;
            }
            TodoItem? item = todoList.GetItem(taskNumber - 1);
            if (item == null)
            {
                Console.WriteLine("Задача с таким номером не найдена.");
                return;
            }
            Console.WriteLine(item.GetFullInfo());
        }
    }
}
