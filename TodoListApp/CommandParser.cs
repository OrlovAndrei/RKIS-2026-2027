using System;

// Преобразует введённую строку в объект команды.
public static class CommandParser
{
    public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
    {
        string input = inputString.Trim();
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return new MessageCommand { Text = "Введите команду. Для справки: help" };

        switch (parts[0])
        {
            case "help":
                return parts.Length == 1
                    ? new HelpCommand()
                    : new MessageCommand { Text = "Формат: help" };
            case "profile":
                return parts.Length == 1
                    ? new ProfileCommand { Profile = profile }
                    : new MessageCommand { Text = "Формат: profile" };
            case "exit":
                return parts.Length == 1
                    ? new ExitCommand()
                    : new MessageCommand { Text = "Формат: exit" };
            case "add": return ParseAdd(input, parts, todoList);
            case "view": return ParseView(parts, todoList);
            case "read": return ParseIndexedCommand("read", parts, todoList);
            case "done": return ParseIndexedCommand("done", parts, todoList);
            case "delete": return ParseIndexedCommand("delete", parts, todoList);
            case "update": return ParseUpdate(input, todoList);
            default:
                return new MessageCommand { Text = "Неизвестная команда. Введите help." };
        }
    }

    private static ICommand ParseAdd(string input, string[] parts, TodoList todoList)
    {
        if (parts.Length == 2 &&
            (parts[1] == "-m" || parts[1] == "--multiline" || parts[1] == "--multi-line"))
            return new AddCommand { TodoList = todoList, Multiline = true };

        string[] pieces = input.Split('"');
        if (pieces.Length != 3 || pieces[0].Trim() != "add" ||
            pieces[2].Trim() != "" || string.IsNullOrWhiteSpace(pieces[1]))
            return new MessageCommand { Text = "Формат: add \"текст\" или add -m" };

        return new AddCommand { TodoList = todoList, Text = pieces[1] };
    }

    private static ICommand ParseView(string[] parts, TodoList todoList)
    {
        ViewCommand command = new ViewCommand { TodoList = todoList };
        bool all = false;

        for (int i = 1; i < parts.Length; i++)
        {
            string flag = parts[i];
            if (flag == "--index") command.ShowIndex = true;
            else if (flag == "--status") command.ShowStatus = true;
            else if (flag == "--update-date") command.ShowDate = true;
            else if (flag == "--all") all = true;
            else if (flag.StartsWith("-") && !flag.StartsWith("--") && flag.Length > 1)
            {
                foreach (char letter in flag.Substring(1))
                {
                    switch (letter)
                    {
                        case 'i': command.ShowIndex = true; break;
                        case 's': command.ShowStatus = true; break;
                        case 'd': command.ShowDate = true; break;
                        case 'a': all = true; break;
                        default:
                            return new MessageCommand { Text = "Неизвестный флаг: -" + letter };
                    }
                }
            }
            else
                return new MessageCommand { Text = "Неизвестный флаг: " + flag };
        }

        if (all)
        {
            command.ShowIndex = true;
            command.ShowStatus = true;
            command.ShowDate = true;
        }
        return command;
    }

    private static ICommand ParseIndexedCommand(string name, string[] parts, TodoList todoList)
    {
        if (parts.Length != 2)
            return new MessageCommand { Text = "Формат: " + name + " 1" };
        if (!int.TryParse(parts[1], out int index) || index < 1)
            return new MessageCommand { Text = "Задачи с таким номером нет" };

        return name switch
        {
            "read" => new ReadCommand { TodoList = todoList, Index = index },
            "done" => new DoneCommand { TodoList = todoList, Index = index },
            "delete" => new DeleteCommand { TodoList = todoList, Index = index },
            _ => new MessageCommand { Text = "Неизвестная команда" }
        };
    }

    private static ICommand ParseUpdate(string input, TodoList todoList)
    {
        string[] parts = input.Split('"');
        string[] head = parts[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3 || head.Length != 2 || head[0] != "update" ||
            parts[2].Trim() != "" || string.IsNullOrWhiteSpace(parts[1]))
            return new MessageCommand { Text = "Формат: update 1 \"новый текст\"" };
        if (!int.TryParse(head[1], out int index) || index < 1)
            return new MessageCommand { Text = "Задачи с таким номером нет" };

        return new UpdateCommand { TodoList = todoList, Index = index, Text = parts[1] };
    }
}
