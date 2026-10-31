using System;

class TodoList
{
    private TodoItem?[] items;
    private int count;

    public int Count
    {
        get { return count; }
    }

    public TodoList()
    {
        items = new TodoItem?[2];
        count = 0;
    }

    public void Add(TodoItem item)
    {
        if (count == items.Length)
        {
            items = IncreaseArray(items, item);
        }
        else
        {
            items[count] = item;
        }
        count++;
    }

    public bool Delete(int index)
    {
        if (index < 0 || index >= count)
        {
            return false;
        }

        for (int i = index; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }

        items[count - 1] = null;
        count--;
        return true;
    }

    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= count)
        {
            return null;
        }
        return items[index];
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        if (count == 0)
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
        if (showDone)
        {
            header += $" | {"Статус",-12}";
        }
        if (showDate)
        {
            header += $" | {"Изменена",-19}";
        }

        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        for (int i = 0; i < count; i++)
        {
            TodoItem? item = items[i];
            if (item == null)
            {
                continue;
            }

            string row = "";
            if (showIndex)
            {
                row += $"{i + 1,-4} | ";
            }
            row += $"{item.GetShortText(),-33}";
            if (showDone)
            {
                row += $" | {item.GetStatusText(),-12}";
            }
            if (showDate)
            {
                row += $" | {item.LastUpdate:dd.MM.yyyy HH:mm:ss}";
            }
            Console.WriteLine(row);
        }
    }

    private TodoItem?[] IncreaseArray(TodoItem?[] items, TodoItem item)
    {
        TodoItem?[] newItems = new TodoItem?[items.Length * 2];
        for (int i = 0; i < items.Length; i++)
        {
            newItems[i] = items[i];
        }
        newItems[items.Length] = item;
        return newItems;
    }
}