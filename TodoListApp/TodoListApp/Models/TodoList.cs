using System.Collections;
using System.Text;

namespace TodoListApp.Models;

internal sealed class TodoList : IEnumerable<TodoItem>
{
    private readonly List<TodoItem> _items = new();
    private int _nextId = 1;

    public event Action<TodoItem>? OnTodoAdded;
    public event Action<TodoItem>? OnTodoDeleted;
    public event Action<TodoItem>? OnTodoUpdated;
    public event Action<TodoItem>? OnStatusChanged;

    public int Count => _items.Count;

    public TodoItem this[int index] => _items[index];

    public TodoItem Add(string text)
    {
        TodoItem item = new(new TaskId(_nextId++), text.Trim());
        Add(item);
        return item;
    }

    public void Add(TodoItem item)
    {
        _items.Add(item);
        _nextId = Math.Max(_nextId, item.Id.Value + 1);
        OnTodoAdded?.Invoke(item);
    }

    public void InsertAt(int index, TodoItem item)
    {
        _items.Insert(Math.Clamp(index, 0, _items.Count), item);
        _nextId = Math.Max(_nextId, item.Id.Value + 1);
        OnTodoAdded?.Invoke(item);
    }

    public bool Delete(int index)
    {
        if (index < 0 || index >= _items.Count)
        {
            return false;
        }

        TodoItem item = _items[index];
        _items.RemoveAt(index);
        OnTodoDeleted?.Invoke(item);
        return true;
    }

    public bool TryRemoveById(TaskId id, out TodoItem? item, out int index)
    {
        index = IndexOf(id);
        if (index < 0)
        {
            item = null;
            return false;
        }

        item = _items[index];
        return Delete(index);
    }

    public TodoItem GetItem(int index) => _items[index];

    public TodoItem? FindById(TaskId id)
    {
        int index = IndexOf(id);
        return index < 0 ? null : _items[index];
    }

    public bool SetStatus(TaskId id, TodoStatus status)
    {
        TodoItem? item = FindById(id);
        if (item is null)
        {
            return false;
        }

        item.SetStatus(status);
        OnStatusChanged?.Invoke(item);
        return true;
    }

    public bool Update(TaskId id, string newText)
    {
        TodoItem? item = FindById(id);
        if (item is null)
        {
            return false;
        }

        item.UpdateText(newText.Trim());
        OnTodoUpdated?.Invoke(item);
        return true;
    }

    public void NotifyUpdated(TodoItem item) => OnTodoUpdated?.Invoke(item);

    public void NotifyStatusChanged(TodoItem item) => OnStatusChanged?.Invoke(item);

    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        List<(string Header, Func<TodoItem, string> Value)> columns = new();
        if (showIndex)
        {
            columns.Add(("ID", item => item.Id.Value.ToString()));
        }

        columns.Add(("Задача", item => Shorten(item.Text)));
        if (showStatus)
        {
            columns.Add(("Статус", item => item.Status.ToString()));
        }

        if (showDate)
        {
            columns.Add(("Изменена", item => item.LastUpdate.ToString("dd.MM.yyyy HH:mm")));
        }

        if (_items.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        int[] widths = columns
            .Select(column => Math.Max(
                column.Header.Length,
                _items.Max(item => column.Value(item).Length)))
            .ToArray();

        PrintRow(columns.Select(column => column.Header).ToArray(), widths);
        Console.WriteLine(string.Join("-+-", widths.Select(width => new string('-', width))));
        foreach (TodoItem item in _items)
        {
            PrintRow(columns.Select(column => column.Value(item)).ToArray(), widths);
        }
    }

    public int IndexOf(TaskId id)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Id == id)
            {
                return i;
            }
        }

        return -1;
    }

    public IEnumerator<TodoItem> GetEnumerator()
    {
        foreach (TodoItem item in _items)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static string Shorten(string text)
    {
        string value = text.Replace("\r", " ").Replace("\n", " ↵ ");
        return value.Length > 30 ? value[..30] + "..." : value;
    }

    private static void PrintRow(IReadOnlyList<string> values, IReadOnlyList<int> widths)
    {
        StringBuilder row = new();
        for (int i = 0; i < values.Count; i++)
        {
            if (i > 0)
            {
                row.Append(" | ");
            }

            row.Append(values[i].PadRight(widths[i]));
        }

        Console.WriteLine(row.ToString());
    }
}
