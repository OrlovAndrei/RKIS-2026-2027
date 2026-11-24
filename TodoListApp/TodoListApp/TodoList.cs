using System;

// TodoList — список задач (Лекция 6 «Введение в ООП»)
// Теперь массив TodoItem и все операции над ним собраны в одном классе.
class TodoList
{
    private const int StartCapacity = 2; 

    // Поля. Все приватные

    private TodoItem[] _items; 
    private int _count;        

    public TodoList()
    {
        _items = new TodoItem[StartCapacity];
        _count = 0;
    }

    // Свойства

    // Сколько задач в списке
    public int Count { get { return _count; } }

    // Методы

    public void Add(TodoItem item)
    {
        if (_count == _items.Length)
        {
            _items = IncreaseArray(_items, item);
        }
        else
        {
            _items[_count] = item;
        }
        _count++;
    }

    // Получить задачу по индексу
    public TodoItem GetItem(int index)
    {
        return _items[index];
    }

    public void Delete(int index)
    {
        for (int i = index; i < _count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }
        _items[_count - 1] = null;
        _count--;
    }

    // View - таблица
    public void View(bool showIndex, bool showDone, bool showDate)
    {
        if (_count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        bool hasTable = showIndex || showDone || showDate;

        // Шапка таблицы (только из выбранных колонок)
        if (hasTable)
        {
            string header = "";

            if (showIndex)
                header += $"{"№",2}  ";
            if (showDone)
                header += $"{"Статус",-12}  ";
            if (showDate)
                header += $"{"Дата",-10}  ";
            header += "Текст";

            Console.WriteLine(header);
        }

        for (int i = 0; i < _count; i++)
        {
            TodoItem item = _items[i];
            string row = "";

            if (showIndex)
                row += $"{i + 1,2}  ";
            if (showDone)
                row += $"{(item.IsDone ? "выполнена" : "не выполнена"),-12}  ";
            if (showDate)
                row += $"{item.LastUpdate.ToString("dd.MM.yyyy"),-10}  ";
            row += item.GetShortText();

            Console.WriteLine(row);
        }
    }

    // Рост массива ×2 при переполнении
    private TodoItem[] IncreaseArray(TodoItem[] items, TodoItem item)
    {
        int newCapacity = items.Length * 2;
        Console.WriteLine($"(массивы расширены до {newCapacity} элементов)");

        TodoItem[] biggerItems = new TodoItem[newCapacity];

        for (int i = 0; i < items.Length; i++)
        {
            biggerItems[i] = items[i];
        }
        biggerItems[items.Length] = item;

        return biggerItems;
    }
}
