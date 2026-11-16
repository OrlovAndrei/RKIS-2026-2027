using System;

namespace TodoList
{
    public static class CommandParser
    {
        public static ICommand? Parse(string inputString, TodoList todoList, Profile profile)
        {
            if (string.IsNullOrWhiteSpace(inputString)) return null;

            string[] parts = inputString.Split(' ', 2);
            string commandName = parts[0].ToLower();
            string args = parts.Length > 1 ? parts[1] : "";

            switch (commandName)
            {
                case "add":
                    var addCmd = new AddCommand { TodoList = todoList };
                    // проверка на -m
                    if (args.Trim() == "--multiline" || args.Trim() == "-m") 
                    {
                        addCmd.Multiline = true;
                    }
                    else
                    {
                        // Trim('"') уберет кавычки, если ввели add "Задача"
                        addCmd.Text = args.Trim('"'); 
                    }
                    return addCmd;

                case "view":
                    var viewCmd = new ViewCommand { TodoList = todoList };
                    string[] viewArgs = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var arg in viewArgs)
                    {
                        if (arg == "-a" || arg == "--all")
                        {
                            viewCmd.ShowIndex = true; 
                            viewCmd.ShowDone = true; 
                            viewCmd.ShowDate = true;
                        }
                        else if (arg == "-i" || arg == "--index") viewCmd.ShowIndex = true;
                        else if (arg == "-s" || arg == "--status") viewCmd.ShowDone = true;
                        else if (arg == "-d" || arg == "--update-date") viewCmd.ShowDate = true;
                        else if (arg.StartsWith("-") && !arg.StartsWith("--") && arg.Length > 2)
                        {
                            // Поддержка склеенных флагов типа -is
                            if (arg.Contains('i')) viewCmd.ShowIndex = true;
                            if (arg.Contains('s')) viewCmd.ShowDone = true;
                            if (arg.Contains('d')) viewCmd.ShowDate = true;
                        }
                    }
                    return viewCmd;

                case "done":
                    if (int.TryParse(args, out int doneIdx))
                        return new DoneCommand { TodoList = todoList, Index = doneIdx - 1 };
                    Console.WriteLine("Ошибка: укажите номер задачи.");
                    return null;

                case "update":
                    string[] updateParts = args.Split(' ', 2);
                    if (updateParts.Length == 2 && int.TryParse(updateParts[0], out int updateIdx))
                    {
                        return new UpdateCommand 
                        { 
                            TodoList = todoList, 
                            Index = updateIdx - 1, 
                            Text = updateParts[1].Trim('"') // Убираем кавычки, если они есть 
                        };
                    }
                    Console.WriteLine("Ошибка: формат 'update <номер> <новый текст>'");
                    return null;

                case "delete":
                    if (int.TryParse(args, out int delIdx))
                        return new DeleteCommand { TodoList = todoList, Index = delIdx - 1 };
                    Console.WriteLine("Ошибка: укажите номер задачи.");
                    return null;

                case "read":
                    if (int.TryParse(args, out int readIdx))
                        return new ReadCommand { TodoList = todoList, Index = readIdx - 1 };
                    Console.WriteLine("Ошибка: укажите номер задачи.");
                    return null;

                case "profile":
                    return new ProfileCommand { Profile = profile };

                case "exit":
                    return new ExitCommand();

                case "help":
                    return new HelpCommand();

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help'.");
                    return null;
            }
        }
    }
}