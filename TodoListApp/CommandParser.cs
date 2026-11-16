using System;

namespace TodoList
{
    public static class CommandParser
    {
        public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
        {
            if (string.IsNullOrWhiteSpace(inputString)) return null;

            string[] parts = inputString.Split(' ', 2);
            string commandName = parts[0].ToLower();
            string args = parts.Length > 1 ? parts[1] : "";

            switch (commandName)
            {
                case "add":
                    var addCmd = new AddCommand { TodoList = todoList };
                    if (args.Trim() == "--multiline")
                    {
                        addCmd.Multiline = true;
                    }
                    else
                    {
                        addCmd.Text = args;
                    }
                    return addCmd;

                case "view":
                    return new ViewCommand { TodoList = todoList };

                case "done":
                    if (int.TryParse(args, out int doneIdx))
                        return new DoneCommand { TodoList = todoList, Index = doneIdx };
                    Console.WriteLine("Ошибка: укажите номер задачи.");
                    return null;

                case "update":
                    string[] updateParts = args.Split(' ', 2);
                    if (updateParts.Length == 2 && int.TryParse(updateParts[0], out int updateIdx))
                    {
                        return new UpdateCommand 
                        { 
                            TodoList = todoList, 
                            Index = updateIdx, 
                            Text = updateParts[1] 
                        };
                    }
                    Console.WriteLine("Ошибка: формат 'update <номер> <новый текст>'");
                    return null;

                case "delete":
                    if (int.TryParse(args, out int delIdx))
                        return new DeleteCommand { TodoList = todoList, Index = delIdx };
                    Console.WriteLine("Ошибка: укажите номер задачи.");
                    return null;

                case "read":
                    if (int.TryParse(args, out int readIdx))
                        return new ReadCommand { TodoList = todoList, Index = readIdx };
                    Console.WriteLine("Ошибка: укажите номер задачи.");
                    return null;

                case "profile":
                    return new ProfileCommand { Profile = profile };

                case "exit":
                    return new ExitCommand();

                case "help":
                    Console.WriteLine("Команды: add <текст>, add --multiline, done <idx>, update <idx> <текст>, delete <idx>, view, read <idx>, profile, exit");
                    return null;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help'.");
                    return null;
            }
        }
    }
}