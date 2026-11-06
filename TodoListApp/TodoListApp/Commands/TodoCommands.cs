using System.Text;
using TodoListApp.Models;
using TodoListApp.Patterns;

namespace TodoListApp.Commands;

internal sealed class AddCommand(string arguments) : CommandBase
{
    private TodoItem? _item;
    private bool _applied;

    public override bool CanUndo => _applied;

    public override void Execute()
    {
        if (_item is not null)
        {
            AppInfo.Todos.Add(_item);
            _applied = true;
            Console.WriteLine($"Добавлена задача #{_item.Id.Value}.");
            return;
        }

        bool multiline = false;
        string text = arguments.Trim();
        if (text.Equals("--multiline", StringComparison.OrdinalIgnoreCase)
            || text.Equals("-m", StringComparison.OrdinalIgnoreCase))
        {
            multiline = true;
            text = string.Empty;
        }
        else if (text.StartsWith("--multiline ", StringComparison.OrdinalIgnoreCase)
            || text.StartsWith("-m ", StringComparison.OrdinalIgnoreCase))
        {
            int space = text.IndexOf(' ');
            text = text[(space + 1)..].Trim();
            multiline = true;
        }

        if (multiline)
        {
            text = ReadMultilineText();
        }
        else
        {
            text = CommandText.RemoveOptionalQuotes(text);
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Формат: add <текст> или add --multiline");
            return;
        }

        _item = AppInfo.Todos.Add(text);
        _applied = true;
        Console.WriteLine($"Добавлена задача #{_item.Id.Value}.");
    }

    public override void Unexecute()
    {
        if (_item is not null && AppInfo.Todos.TryRemoveById(_item.Id, out _, out _))
        {
            _applied = false;
        }
    }

    private static string ReadMultilineText()
    {
        Console.WriteLine("Введите строки задачи. Для завершения введите !end.");
        StringBuilder text = new();
        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line is null)
            {
                Console.WriteLine("Ввод прерван; задача не добавлена.");
                return string.Empty;
            }

            if (line == "!end")
            {
                return text.ToString();
            }

            if (text.Length > 0)
            {
                text.AppendLine();
            }

            text.Append(line);
        }
    }
}

internal sealed class ViewCommand(string arguments) : CommandBase
{
    public override void Execute()
    {
        bool showIndex = false;
        bool showStatus = false;
        bool showDate = false;

        foreach (string flag in arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (flag.Equals("--all", StringComparison.OrdinalIgnoreCase)
                || flag.Equals("-a", StringComparison.OrdinalIgnoreCase))
            {
                showIndex = showStatus = showDate = true;
                continue;
            }

            if (flag.Equals("--index", StringComparison.OrdinalIgnoreCase))
            {
                showIndex = true;
                continue;
            }

            if (flag.Equals("--status", StringComparison.OrdinalIgnoreCase))
            {
                showStatus = true;
                continue;
            }

            if (flag.Equals("--update-date", StringComparison.OrdinalIgnoreCase))
            {
                showDate = true;
                continue;
            }

            if (flag.StartsWith('-') && flag.Length > 1)
            {
                foreach (char shortFlag in flag[1..])
                {
                    switch (shortFlag)
                    {
                        case 'i': showIndex = true; break;
                        case 's': showStatus = true; break;
                        case 'd': showDate = true; break;
                        default:
                            Console.WriteLine($"Неизвестный флаг view: -{shortFlag}");
                            return;
                    }
                }

                continue;
            }

            Console.WriteLine("Формат: view [-i] [-s] [-d] или view -a");
            return;
        }

        AppInfo.Todos.View(showIndex, showStatus, showDate);
    }
}

internal sealed class ReadCommand(string arguments) : CommandBase
{
    public override void Execute()
    {
        if (!CommandText.TryParseId(arguments, out TaskId id))
        {
            return;
        }

        TodoItem? item = AppInfo.Todos.FindById(id);
        if (item is null)
        {
            Console.WriteLine($"Задача #{id.Value} не найдена.");
            return;
        }

        ITaskInfoView taskInfo = new TaskInfoAdapter(item);
        Console.WriteLine(taskInfo.Format());
    }
}

internal sealed class StatusCommand(string arguments) : CommandBase
{
    private TaskItemSnapshot? _before;
    private TaskId _id;
    private TodoStatus _newStatus;
    private bool _applied;
    private bool _parsed;

    public override bool CanUndo => _applied;

    public override void Execute()
    {
        if (!_parsed && !TryParseArguments(arguments, out _id, out _newStatus))
        {
            return;
        }

        _parsed = true;
        TodoItem? item = AppInfo.Todos.FindById(_id);
        if (item is null)
        {
            Console.WriteLine($"Задача #{_id.Value} не найдена.");
            return;
        }

        _before ??= item.Capture();
        AppInfo.Todos.SetStatus(_id, _newStatus);
        _applied = true;
        Console.WriteLine($"Статус задачи #{_id.Value}: {_newStatus}.");
    }

    public override void Unexecute()
    {
        if (_before is TaskItemSnapshot snapshot
            && AppInfo.Todos.FindById(_id) is TodoItem item)
        {
            item.Restore(snapshot);
            AppInfo.Todos.NotifyStatusChanged(item);
            _applied = false;
        }
    }

    private static bool TryParseArguments(string arguments, out TaskId id, out TodoStatus status)
    {
        id = default;
        status = default;
        string[] parts = arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 || !CommandText.TryParseId(parts[0], out id))
        {
            Console.WriteLine("Формат: status <id> <NotStarted|InProgress|Completed|Postponed|Failed>");
            return false;
        }

        if (!Enum.TryParse(parts[1], ignoreCase: true, out status) || !Enum.IsDefined(status))
        {
            Console.WriteLine("Неизвестный статус задачи.");
            return false;
        }

        return true;
    }
}

internal sealed class UpdateCommand(string arguments) : CommandBase
{
    private TaskItemSnapshot? _before;
    private TaskId _id;
    private string _text = string.Empty;
    private bool _applied;
    private bool _parsed;

    public override bool CanUndo => _applied;

    public override void Execute()
    {
        if (!_parsed && !TryParseArguments(arguments, out _id, out _text))
        {
            return;
        }

        _parsed = true;
        TodoItem? item = AppInfo.Todos.FindById(_id);
        if (item is null)
        {
            Console.WriteLine($"Задача #{_id.Value} не найдена.");
            return;
        }

        _before ??= item.Capture();
        AppInfo.Todos.Update(_id, _text);
        _applied = true;
        Console.WriteLine($"Задача #{_id.Value} изменена.");
    }

    public override void Unexecute()
    {
        if (_before is TaskItemSnapshot snapshot
            && AppInfo.Todos.FindById(_id) is TodoItem item)
        {
            item.Restore(snapshot);
            AppInfo.Todos.NotifyUpdated(item);
            _applied = false;
        }
    }

    private static bool TryParseArguments(string arguments, out TaskId id, out string text)
    {
        id = default;
        text = string.Empty;
        (string idText, string taskText) = CommandText.SplitFirst(arguments);
        if (!CommandText.TryParseId(idText, out id) || string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Формат: update <id> <новый текст>");
            return false;
        }

        text = CommandText.RemoveOptionalQuotes(taskText);
        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Текст задачи не должен быть пустым.");
            return false;
        }

        return true;
    }
}

internal sealed class DeleteCommand(string arguments) : CommandBase
{
    private TodoItem? _item;
    private TaskItemSnapshot? _snapshot;
    private int _index;
    private TaskId _id;
    private bool _applied;
    private bool _parsed;

    public override bool CanUndo => _applied;

    public override void Execute()
    {
        if (!_parsed && !CommandText.TryParseId(arguments, out _id))
        {
            Console.WriteLine("Формат: delete <id>");
            return;
        }

        _parsed = true;
        if (!AppInfo.Todos.TryRemoveById(_id, out TodoItem? deleted, out int index) || deleted is null)
        {
            Console.WriteLine($"Задача #{_id.Value} не найдена.");
            return;
        }

        _item = deleted;
        _index = index;
        _snapshot ??= deleted.Capture();
        _applied = true;
        Console.WriteLine($"Задача #{_id.Value} удалена.");
    }

    public override void Unexecute()
    {
        if (_item is null || _snapshot is not TaskItemSnapshot snapshot)
        {
            return;
        }

        _item.Restore(snapshot);
        AppInfo.Todos.InsertAt(_index, _item);
        _applied = false;
    }
}

internal static class CommandText
{
    public static bool TryParseId(string value, out TaskId id)
    {
        string normalized = value.Trim().TrimStart('#');
        if (int.TryParse(normalized, out int number) && number > 0)
        {
            id = new TaskId(number);
            return true;
        }

        id = default;
        Console.WriteLine("Укажите положительный числовой id задачи.");
        return false;
    }

    public static (string First, string Remainder) SplitFirst(string value)
    {
        string trimmed = value.Trim();
        int separator = trimmed.IndexOf(' ');
        return separator < 0
            ? (trimmed, string.Empty)
            : (trimmed[..separator], trimmed[(separator + 1)..].Trim());
    }

    public static string RemoveOptionalQuotes(string value)
    {
        string trimmed = value.Trim();
        if (trimmed.Length >= 2 && trimmed[0] == '"' && trimmed[^1] == '"')
        {
            return trimmed[1..^1]
                .Replace("\"\"", "\"", StringComparison.Ordinal)
                .Replace("\\\"", "\"", StringComparison.Ordinal);
        }

        return trimmed;
    }
}
