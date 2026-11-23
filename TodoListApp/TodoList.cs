using System;
using System.Collections;
using System.Collections.Generic;

// Вместо самодельного массива используем приватный List<TodoItem>.
public class TodoList : IEnumerable<TodoItem>
{
    private readonly List<TodoItem> _items = new List<TodoItem>();

    public int Count => _items.Count;

    // Индексатор: например, todoList[0] — первая задача.
    public TodoItem this[int index] => _items[index];

    public void Add(TodoItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _items.Add(item);
    }

    // Для старых команд удобно возвращать null при неверном номере.
    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= Count) return null;
        return this[index];
    }

    public bool Delete(int index)
    {
        if (GetItem(index) == null) return false;
        _items.RemoveAt(index);
        return true;
    }

    // Новая установка статуса вместо старого Done.
    public bool SetStatus(int index, TodoStatus status)
    {
        TodoItem? item = GetItem(index);
        if (item == null) return false;
        item.SetStatus(status);
        return true;
    }

    // Метод-итератор через yield return: TodoList можно использовать в foreach.
    public IEnumerator<TodoItem> GetEnumerator()
    {
        for (int i = 0; i < _items.Count; i++)
            yield return _items[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Вывод с флагами view (позже эту логику вызовет ViewCommand).
    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        if (Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }

        string header = "";
        if (showIndex) header += "№".PadRight(3) + " | ";
        header += "Текст задачи".PadRight(33);
        if (showStatus) header += " | " + "Статус".PadRight(12);
        if (showDate) header += " | Дата изменения";

        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        int number = 1;
        foreach (TodoItem item in this)
        {
            string row = showIndex ? number.ToString().PadRight(3) + " | " : "";
            row += item.GetShortInfo(showStatus, showDate);
            Console.WriteLine(row);
            number++;
        }
    }
}
