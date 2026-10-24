using System;

// Хранит задачи и отвечает за добавление, удаление, поиск и вывод.
public class TodoList
{
    private const int InitialSize = 2;
    private const int GrowthFactor = 2;

    private TodoItem?[] _items;
    private int _count;

    public TodoList()
    {
        _items = new TodoItem?[InitialSize];
        _count = 0;
    }

    public void Add(TodoItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (_count == _items.Length)
            _items = IncreaseArray(_items, item);
        else
            _items[_count] = item;

        _count++;
    }

    // Этот метод доступен только внутри TodoList.
    private TodoItem?[] IncreaseArray(TodoItem?[] items, TodoItem item)
    {
        TodoItem?[] newItems = new TodoItem?[items.Length * GrowthFactor];
        for (int i = 0; i < _count; i++)
            newItems[i] = items[i];

        newItems[_count] = item;
        return newItems;
    }

    // Внутри программы индекс начинается с нуля.
    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= _count)
            return null;
        return _items[index];
    }

    public bool Delete(int index)
    {
        if (GetItem(index) == null)
            return false;

        for (int i = index; i < _count - 1; i++)
            _items[i] = _items[i + 1];

        _count--;
        _items[_count] = null;
        return true;
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        if (_count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }

        string header = "";
        if (showIndex)
            header += "№".PadRight(3) + " | ";
        header += "Текст задачи".PadRight(33);
        if (showDone)
            header += " | " + "Статус".PadRight(12);
        if (showDate)
            header += " | Дата изменения";

        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        for (int i = 0; i < _count; i++)
        {
            TodoItem? item = _items[i];
            if (item == null) continue;

            string row = showIndex ? (i + 1).ToString().PadRight(3) + " | " : "";
            row += showDone && showDate
                ? item.GetShortInfo()
                : item.GetShortInfo(showDone, showDate);
            Console.WriteLine(row);
        }
    }
}
