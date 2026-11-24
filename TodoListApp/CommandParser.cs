namespace TodoListApp;

public static class CommandParser
{
    public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
    {
        List<string> tokens = ParseCommandLine(inputString);

        if (tokens.Count == 0)
            return new UnknownCommand();

        string commandName = tokens[0].ToLower();
        string[] rest = tokens.Skip(1).ToArray();

        Dictionary<string, bool> flags = ParseFlags(rest);

        string[] positionalArgs = rest
            .Where(a => !a.StartsWith("-"))
            .ToArray();

        int index = 0;
        if (positionalArgs.Length > 0)
            int.TryParse(positionalArgs[0], out index);

        switch (commandName)
        {
            case "add":
                return new AddCommand
                {
                    Multiline = flags.ContainsKey("multiline"),
                    Text = string.Join(" ", positionalArgs),
                    TodoList = todoList
                };

            case "view":
                return new ViewCommand
                {
                    ShowIndex = flags.ContainsKey("index"),
                    ShowStatus = flags.ContainsKey("status"),
                    ShowDate = flags.ContainsKey("update-date"),
                    ShowAll = flags.ContainsKey("all"),
                    TodoList = todoList
                };

            case "read":
                return new ReadCommand { Index = index, TodoList = todoList };

            case "status":
                if (positionalArgs.Length >= 2 &&
                    int.TryParse(positionalArgs[0], out int taskIndex) &&
                    Enum.TryParse<TodoStatus>(
                        positionalArgs[1],
                        true,
                        out TodoStatus status))
                {
                    return new StatusCommand
                    {
                        Index = taskIndex,
                        Status = status,
                        TodoList = todoList
                    };
                }

                return new UnknownCommand();



            case "update":
                return new UpdateCommand
                {
                    Index = index,
                    Text = string.Join(" ", positionalArgs.Skip(1)),
                    TodoList = todoList
                };

            case "delete":
                return new DeleteCommand { Index = index, TodoList = todoList };

            case "profile":
                return new ProfileCommand { Profile = profile };

            case "help":
                return new HelpCommand();

            case "exit":
            case "quit":
                return new ExitCommand();

            default:
                return new UnknownCommand();
        }
    }

    private static List<string> ParseCommandLine(string input)
    {
        var result = new List<string>();
        bool inQuotes = false;
        string current = "";

        foreach (char c in input)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (c == ' ' && !inQuotes)
            {
                if (current != "")
                {
                    result.Add(current);
                    current = "";
                }
            }
            else
            {
                current += c;
            }
        }

        if (current != "")
            result.Add(current);

        return result;
    }

    private static Dictionary<string, bool> ParseFlags(string[] args)
    {
        var flags = new Dictionary<string, bool>();

        foreach (var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                flags[arg.Substring(2).ToLower()] = true;
            }
            else if (arg.StartsWith("-") && arg.Length > 1)
            {
                string shortFlags = arg.Substring(1).ToLower();

                foreach (char c in shortFlags)
                {
                    string flag = c switch
                    {
                        'm' => "multiline",
                        'i' => "index",
                        's' => "status",
                        'd' => "update-date",
                        'a' => "all",
                        _ => c.ToString()
                    };

                    flags[flag] = true;
                }
            }
        }

        return flags;
    }
}