using System;
using System.Collections;
using System.Collections.Generic;

class TodoList : IEnumerable<TodoItem>
{
    private List<TodoItem> items;

    public int Count => items.Count;

    public TodoList()
    {
        items = new List<TodoItem>();
    }

    public TodoItem this[int index]
    {
        get
        {
            if (index < 0 || index >= items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            return items[index];
        }
    }

    public void Add(TodoItem item)
    {
        items.Add(item);
    }

    public bool Delete(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return false;
        }

        items.RemoveAt(index);
        return true;
    }

    public bool SetStatus(int index, TodoStatus status)
    {
        if (index < 0 || index >= items.Count)
        {
            return false;
        }

        items[index].SetStatus(status);
        return true;
    }

    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return null;
        }
        return items[index];
    }

    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("Задач пока нет.");
            return;
        }

        string header = "";
        if (showIndex)
        {
            header += $"{"№",-4} | ";
        }
        header += $"{"Задача",-33}";
        if (showStatus)
        {
            header += $" | {"Статус",-12}";
        }
        if (showDate)
        {
            header += $" | {"Изменена",-19}";
        }

        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        int index = 0;
        foreach (TodoItem item in this)
        {
            string row = "";
            if (showIndex)
            {
                row += $"{index + 1,-4} | ";
            }
            row += $"{item.GetShortText(),-33}";
            if (showStatus)
            {
                row += $" | {item.GetStatusText(),-12}";
            }
            if (showDate)
            {
                row += $" | {item.LastUpdate:dd.MM.yyyy HH:mm:ss}";
            }
            Console.WriteLine(row);
            index++;
        }
    }

    public IEnumerator<TodoItem> GetEnumerator()
    {
        for (int i = 0; i < items.Count; i++)
        {
            yield return items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
