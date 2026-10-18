// Превращает введённую строку в объект команды: разбирает имя, флаги и параметры
public static class CommandParser
{
    public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
    {
        // Первое слово — имя команды, остальное — аргументы
        string[] parts = inputString.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return Error("Введите команду, help - список команд.");

        string name = parts[0].ToLower();
        string args = parts.Length > 1 ? parts[1].Trim() : "";

        switch (name)
        {
            case "help": return new HelpCommand();
            case "profile": return new ProfileCommand { Profile = profile };
            case "add": return ParseAdd(args, todoList);
            case "view": return ParseView(args, todoList);
            case "read": return ParseTaskCommand(args, new ReadCommand { TodoList = todoList });
            case "done": return ParseTaskCommand(args, new DoneCommand { TodoList = todoList });
            case "remove": return ParseTaskCommand(args, new RemoveCommand { TodoList = todoList });
            case "update": return ParseUpdate(args, todoList);
            case "exit": return new ExitCommand();
            default: return Error($"Команды {name} нет, help - список команд.");
        }
    }

    private static ICommand ParseAdd(string args, TodoList todoList)
    {
        if (args == "--multiline" || args == "-m")
            return new AddCommand { TodoList = todoList, Multiline = true };

        // Кавычки вокруг текста необязательны: add "купить хлеб" == add купить хлеб
        return new AddCommand { TodoList = todoList, Text = args.Trim('"').Trim() };
    }

    private static ICommand ParseView(string args, TodoList todoList)
    {
        var command = new ViewCommand { TodoList = todoList };

        foreach (string flag in args.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (flag.StartsWith("--"))
            {
                switch (flag)
                {
                    case "--index": command.ShowIndex = true; break;
                    case "--status": command.ShowStatus = true; break;
                    case "--update-date": command.ShowDate = true; break;
                    case "--all": command.ShowIndex = command.ShowStatus = command.ShowDate = true; break;
                    default: return Error($"Неизвестный флаг {flag}");
                }
            }
            else if (flag.StartsWith("-") && flag.Length > 1)
            {
                // Короткие флаги можно склеивать: -isd — разбираем каждую букву отдельно
                foreach (char c in flag.Substring(1))
                {
                    switch (c)
                    {
                        case 'i': command.ShowIndex = true; break;
                        case 's': command.ShowStatus = true; break;
                        case 'd': command.ShowDate = true; break;
                        case 'a': command.ShowIndex = command.ShowStatus = command.ShowDate = true; break;
                        default: return Error($"Неизвестный флаг -{c}");
                    }
                }
            }
            else
            {
                return Error($"Неизвестный аргумент {flag}");
            }
        }

        return command;
    }

    // update <номер> "текст": первое слово — номер задачи, всё остальное — новый текст
    private static ICommand ParseUpdate(string args, TodoList todoList)
    {
        string[] parts = args.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string text = parts.Length > 1 ? parts[1].Trim('"').Trim() : "";

        var command = new UpdateCommand { TodoList = todoList, Text = text };
        return ParseTaskCommand(parts.Length > 0 ? parts[0] : "", command);
    }

    // Для команд с номером задачи: проверяем, что номер — число, и переводим его в индекс с 0.
    // Существует ли задача, проверит сама команда при выполнении
    private static ICommand ParseTaskCommand(string numberText, TaskCommand command)
    {
        if (!int.TryParse(numberText, out int number))
            return Error("Номер задачи должен быть числом.");

        command.Index = number - 1;
        return command;
    }

    private static ICommand Error(string message) => new ErrorCommand { Message = message };
}
