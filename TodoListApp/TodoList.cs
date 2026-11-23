using System.Text;

namespace TodoListApp;

public sealed class TodoList
{
    private TodoItem?[] _items = new TodoItem?[2];

    public void Add(TodoItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] is null)
            {
                _items[i] = item;
                return;
            }
        }

        _items = IncreaseArray(_items, item);
    }

    public void Delete(int index)
    {
        if (GetItem(index) is null)
            throw new ArgumentOutOfRangeException(nameof(index), "Задачи с таким индексом не существует.");

        for (int i = index; i < _items.Length - 1; i++)
            _items[i] = _items[i + 1];

        _items[^1] = null;
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        List<string> headers = new();
        if (showIndex)
            headers.Add("Индекс");

        headers.Add("Текст задачи");

        if (showDone)
            headers.Add("Статус");

        if (showDate)
            headers.Add("Дата изменения");

        List<string[]> rows = new();
        for (int i = 0; i < _items.Length; i++)
        {
            TodoItem? item = _items[i];
            if (item is null)
                continue;

            List<string> cells = new();
            if (showIndex)
                cells.Add(i.ToString());

            cells.Add(item.GetTextPreview());

            if (showDone)
                cells.Add(item.IsDone ? "выполнена" : "не выполнена");

            if (showDate)
                cells.Add(item.LastUpdate.ToString("dd.MM.yyyy HH:mm"));

            rows.Add(cells.ToArray());
        }

        if (rows.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        int[] widths = new int[headers.Count];
        for (int i = 0; i < headers.Count; i++)
            widths[i] = headers[i].Length;

        foreach (string[] row in rows)
        {
            for (int i = 0; i < row.Length; i++)
                widths[i] = Math.Max(widths[i], row[i].Length);
        }

        Console.WriteLine(BuildTableBorder(widths));
        Console.WriteLine(BuildTableRow(headers, widths));
        Console.WriteLine(BuildTableBorder(widths));
        foreach (string[] row in rows)
            Console.WriteLine(BuildTableRow(row, widths));

        Console.WriteLine(BuildTableBorder(widths));
    }

    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= _items.Length)
            return null;

        return _items[index];
    }

    private static TodoItem?[] IncreaseArray(TodoItem?[] items, TodoItem item)
    {
        TodoItem?[] biggerItems = new TodoItem?[items.Length * 2];
        for (int i = 0; i < items.Length; i++)
            biggerItems[i] = items[i];

        biggerItems[items.Length] = item;
        return biggerItems;
    }

    private static string BuildTableBorder(int[] widths)
    {
        StringBuilder border = new();
        foreach (int width in widths)
            border.Append('+').Append('-', width + 2);

        return border.Append('+').ToString();
    }

    private static string BuildTableRow(IReadOnlyList<string> cells, int[] widths)
    {
        StringBuilder row = new();
        for (int i = 0; i < cells.Count; i++)
            row.Append("| ").Append(cells[i].PadRight(widths[i])).Append(' ');

        return row.Append('|').ToString();
    }
}
