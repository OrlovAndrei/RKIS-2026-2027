using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace TodoListApp;

public class TodoList : IEnumerable<TodoItem>
{
    private const int MaxTableTextLength = 30;

    private List<TodoItem> _items = new List<TodoItem>();
    
    public int Count => _items.Count;

    public TodoItem this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    public void Add(TodoItem item)
    {
        _items.Add(item);
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= _items.Count) return;
        _items.RemoveAt(index);
    }

    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= _items.Count) return null;
        return _items[index];
    }
    public void SetStatus(int index, TodoStatus status)
    {
        if (index < 0 || index >= _items.Count) return;
        _items[index].SetStatus(status);
    }
    public IEnumerator<TodoItem> GetEnumerator()
    {
        foreach (var item in _items)
        {
            yield return item;
        }
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        StringBuilder header = new StringBuilder();
        if (showIndex) header.Append($"| {"Индекс",-6} ");
        header.Append($"| {"Текст задачи",-MaxTableTextLength} ");
        if (showStatus) header.Append($"| {"Статус",-12} ");
        if (showDate) header.Append($"| {"Дата изменения",-19} ");
        header.Append("|");

        string line = new string('-', header.Length);
        Console.WriteLine(line);
        Console.WriteLine(header);
        Console.WriteLine(line);

        for (int i = 0; i < _items.Count; i++)
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

            if (showStatus)
            {
                string status = TodoItem.GetStatusText(item.Status);
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

}
