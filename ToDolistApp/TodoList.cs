using System;
using System.Text;

namespace TodoListApp;

public class TodoList
{
    private const int InitialCapacity = 2;
    private const int MaxTableTextLength = 30;

    private TodoItem[] _items = new TodoItem[InitialCapacity];
    private int _count = 0;

    public int Count => _count;

    public void Add(TodoItem item)
    {
        if (_count >= _items.Length)
        {
            _items = IncreaseArray(_items, item);
        }
        else
        {
            _items[_count] = item;
        }
        _count++;
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= _count) return;

        for (int i = index; i < _count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }
        _count--;
        _items[_count] = null!;
    }

    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= _count) return null;
        return _items[index];
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        if (_count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        StringBuilder header = new StringBuilder();
        if (showIndex) header.Append($"| {"Индекс",-6} ");
        header.Append($"| {"Текст задачи",-MaxTableTextLength} ");
        if (showDone) header.Append($"| {"Статус",-12} ");
        if (showDate) header.Append($"| {"Дата изменения",-19} ");
        header.Append("|");

        string line = new string('-', header.Length);
        Console.WriteLine(line);
        Console.WriteLine(header);
        Console.WriteLine(line);

        for (int i = 0; i < _count; i++)
        {
            TodoItem item = _items[i];
            StringBuilder row = new StringBuilder();

            if (showIndex) row.Append($"| {i + 1,-6} ");

            string text = item.Text.Replace('\n', ' ');
            if (text.Length > MaxTableTextLength)
            {
                text = text.Substring(0, MaxTableTextLength - 3) + "...";
            }
            row.Append($"| {text,-MaxTableTextLength} ");

            if (showDone)
            {
                string status = item.IsDone ? "выполнена" : "не выполнена";
                row.Append($"| {status,-12} ");
            }

            if (showDate)
            {
                row.Append($"| {item.LastUpdate:dd.MM.yyyy HH:mm:ss} ");
            }

            row.Append("|");
            Console.WriteLine(row);
        }
        Console.WriteLine(line);
    }

    private TodoItem[] IncreaseArray(TodoItem[] items, TodoItem item)
    {
        TodoItem[] newItems = new TodoItem[items.Length * 2];
        for (int i = 0; i < items.Length; i++)
        {
            newItems[i] = items[i];
        }
        newItems[items.Length] = item;
        return newItems;
    }
}
