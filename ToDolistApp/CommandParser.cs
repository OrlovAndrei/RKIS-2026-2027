using System;

namespace TodoListApp;

public static class CommandParser
{
    public static ICommand Parse(string inputString, TodoList todoList, Profile profile, string todoFilePath)
    {
        if (string.IsNullOrWhiteSpace(inputString))
        {
            return new UnknownCommand { Message = "" };
        }

        string trimmed = inputString.Trim();
        string commandName = trimmed.Split(' ')[0].ToLower();
        
        switch (commandName)
        {
            case "help":
                return new HelpCommand();

            case "profile":
                return new ProfileCommand { Profile = profile };

            case "exit":
                return new ExitCommand();

            case "add":
                bool isMultiline = HasFlag(trimmed, "--multiline", 'm');
                string text = (!isMultiline && trimmed.Length > 4) 
                    ? trimmed.Substring(4).Trim('"') 
                    : "";
                return new AddCommand
                {
                    TodoList = todoList,
                    IsMultiline = isMultiline,
                    Text = text,
                    FilePath = todoFilePath
                };

            case "view":
                bool showAll = HasFlag(trimmed, "--all", 'a');
                return new ViewCommand
                {
                    TodoList = todoList,
                    ShowIndex = showAll || HasFlag(trimmed, "--index", 'i'),
                    ShowStatus = showAll || HasFlag(trimmed, "--status", 's'),
                    ShowDate = showAll || HasFlag(trimmed, "--update-date", 'd')
                };

            case "status":
                string[] statusParts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (statusParts.Length < 3 || !int.TryParse(statusParts[1], out int stIdx) ||
                    !Enum.TryParse<TodoStatus>(statusParts[2], true, out TodoStatus parsedStatus))
                {
                    return new UnknownCommand
                    {
                        Message = "Ошибка: формат: status <индекс> <статус> (notstarted, inprogress, completed, postponed, failed)"
                    };
                }
                return new StatusCommand
                {
                    TodoList = todoList,
                    Index = stIdx,
                    Status = parsedStatus,
                    FilePath = todoFilePath
                };


            case "delete":
                if (!TryParseIndex(trimmed, out int delIdx))
                    return new UnknownCommand { Message = "Ошибка: укажите числовой индекс. Пример: delete 1" };
                return new DeleteCommand { TodoList = todoList, Index = delIdx, FilePath = todoFilePath  };

            case "read":
                if (!TryParseIndex(trimmed, out int readIdx))
                    return new UnknownCommand { Message = "Ошибка: укажите числовой индекс. Пример: read 1" };
                return new ReadCommand { TodoList = todoList, Index = readIdx };

            case "update":
                string[] parts = trimmed.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 3 || !int.TryParse(parts[1], out int updIdx))
                    return new UnknownCommand { Message = "Ошибка: формат команды: update <idx> \"new_text\"" };
                return new UpdateCommand
                {
                    TodoList = todoList,
                    Index = updIdx,
                    NewText = parts[2].Trim('"'),
                    FilePath = todoFilePath 
                };

            default:
                return new UnknownCommand();
        }
    }

    private static bool TryParseIndex(string input, out int index)
    {
        index = 0;
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2 && int.TryParse(parts[1], out index);
    }

    private static bool HasFlag(string input, string longFlag, char shortFlag)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < parts.Length; i++)
        {
            if (parts[i] == longFlag) return true;
            if (parts[i].StartsWith("-") && !parts[i].StartsWith("--") && parts[i].Contains(shortFlag))
                return true;
        }
        return false;
    }
}
