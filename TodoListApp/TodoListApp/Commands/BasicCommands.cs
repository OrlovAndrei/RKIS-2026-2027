using TodoListApp.Models;

namespace TodoListApp.Commands;

internal sealed class EmptyCommand : CommandBase
{
    public override void Execute() { }
}

internal sealed class UnknownCommand(string name) : CommandBase
{
    public override void Execute() =>
        Console.WriteLine($"Неизвестная команда «{name}». Введите help.");
}

internal sealed class HelpCommand : CommandBase
{
    public override void Execute()
    {
        Console.WriteLine(
            """
            Команды:
              help                       показать справку
              profile [--out|-o]         профиль или выход из него
              add <текст> [--multiline]  добавить задачу
              view [-i|-s|-d|-a]         таблица задач с выбранными колонками
              read <id>                  прочитать задачу целиком
              status <id> <статус>       изменить статус
              update <id> <текст>        изменить текст
              delete <id>                удалить задачу
              search <текст> [-p]        найти задачу (p — только с начала текста)
              stats                      сводка по задачам
              undo / redo                отменить или повторить изменение
              types / collections        примеры struct, record и коллекций
              exit                       завершить приложение

            Статусы: NotStarted, InProgress, Completed, Postponed, Failed.
            Флаги view: -i индекс, -s статус, -d дата, -a все поля.
            """
        );
    }
}

internal sealed class ProfileCommand(string arguments) : CommandBase
{
    public override void Execute()
    {
        if (arguments is "--out" or "-o")
        {
            Console.WriteLine($"Выход из профиля {AppInfo.CurrentProfile.Login}.");
            AppInfo.SignOut();
            return;
        }

        if (!string.IsNullOrWhiteSpace(arguments))
        {
            Console.WriteLine("Использование: profile [--out|-o]");
            return;
        }

        Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
        Console.WriteLine($"Логин: {AppInfo.CurrentProfile.Login}");
    }
}

internal sealed class ExitCommand : CommandBase
{
    public override void Execute()
    {
        AppInfo.ExitRequested = true;
        Console.WriteLine("Программа завершена.");
    }
}

internal sealed class StatisticsCommand : CommandBase
{
    public override void Execute()
    {
        int[] counts = Enum.GetValues<TodoStatus>()
            .Select(status => AppInfo.Todos.Count(item => item.Status == status))
            .ToArray();

        Console.WriteLine($"Задач всего: {AppInfo.Todos.Count}");
        foreach (TodoStatus status in Enum.GetValues<TodoStatus>())
        {
            int count = counts[(int)status];
            Console.WriteLine($"  {status}: {count}");
        }
    }
}

internal sealed class SearchCommand(string arguments) : CommandBase
{
    public override void Execute()
    {
        string query = arguments;
        bool prefixOnly = false;
        if (query.EndsWith(" -p", StringComparison.OrdinalIgnoreCase)
            || query.EndsWith(" --prefix", StringComparison.OrdinalIgnoreCase))
        {
            int flagStart = query.LastIndexOf(' ');
            query = query[..flagStart].Trim();
            prefixOnly = true;
        }

        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Использование: search <текст> [-p|--prefix]");
            return;
        }

        ITaskSearchStrategy strategy = TaskSearchStrategyFactory.Create(prefixOnly);
        Predicate<TodoItem> matches = item => strategy.Matches(item, query);
        List<TodoItem> result = AppInfo.Todos.Where(item => matches(item)).ToList();
        if (result.Count == 0)
        {
            Console.WriteLine("Совпадений нет.");
            return;
        }

        foreach (TodoItem item in result)
        {
            Console.WriteLine($"#{item.Id.Value} [{item.Status}] {item.Text}");
        }
    }
}

internal sealed class TypesDemoCommand : CommandBase
{
    public override void Execute()
    {
        PersonName name = new(AppInfo.CurrentProfile.FirstName, AppInfo.CurrentProfile.LastName);
        TaskId id = new(7);
        object boxed = id;
        TaskId unboxed = (TaskId)boxed;
        TaskItemSnapshot snapshot = new(id, "Пример record struct", TodoStatus.InProgress, DateTime.Now);

        Console.WriteLine($"record struct: {name}");
        Console.WriteLine($"TaskId после boxing/unboxing: {unboxed.Value}");
        Console.WriteLine($"record snapshot: #{snapshot.Id.Value}, {snapshot.Status}");
    }
}

internal sealed class CollectionsDemoCommand : CommandBase
{
    public override void Execute()
    {
        List<TodoItem> items = AppInfo.Todos.ToList();
        if (items.Count == 0)
        {
            Console.WriteLine("Добавьте задачу, чтобы увидеть примеры коллекций.");
            return;
        }

        Stack<TodoItem> stack = new(items);
        Queue<TodoItem> queue = new(items);
        LinkedList<TodoItem> linkedList = new(items);
        Dictionary<TaskId, TodoItem> hashTable = items.ToDictionary(item => item.Id);
        (TaskId Id, string Text) tuple = (items[0].Id, items[0].Text);
        SortedSet<TodoItem> ordered = new(items);

        Console.WriteLine($"Stack: #{stack.Pop().Id.Value}; Queue: #{queue.Dequeue().Id.Value}");
        Console.WriteLine($"LinkedList: {linkedList.First?.Value.Text}");
        Console.WriteLine($"Dictionary/хэш-таблица: {hashTable.Count} задач");
        Console.WriteLine($"Tuple: #{tuple.Id.Value} — {tuple.Text}");
        Console.WriteLine($"IComparable: {string.Join(", ", ordered.Select(item => item.Id.Value))}");
    }
}

internal sealed class UndoCommand : CommandBase
{
    public override void Execute()
    {
        if (!AppInfo.UndoStack.TryPop(out ICommand? command))
        {
            Console.WriteLine("Нет изменений для отмены.");
            return;
        }

        command.Unexecute();
        AppInfo.RedoStack.Push(command);
        Console.WriteLine("Последнее изменение отменено.");
    }
}

internal sealed class RedoCommand : CommandBase
{
    public override void Execute()
    {
        if (!AppInfo.RedoStack.TryPop(out ICommand? command))
        {
            Console.WriteLine("Нет изменений для повтора.");
            return;
        }

        command.Execute();
        if (command.CanUndo)
        {
            AppInfo.UndoStack.Push(command);
            Console.WriteLine("Изменение повторено.");
        }
        else
        {
            Console.WriteLine("Изменение больше не может быть повторено.");
        }
    }
}
