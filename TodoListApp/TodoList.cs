using System.Collections;
using System.Text;

// Список задач. Внутри обычный List<T>, который сам увеличивает размер, поэтому свой IncreaseArray больше не нужен.
// IEnumerable<TodoItem> позволяет перебирать задачи через foreach
public class TodoList : IEnumerable<TodoItem>
{
    private readonly List<TodoItem> _items = new();

    public int Count => _items.Count;

    // Индексатор: todos[0] вместо todos.GetItem(0). При неверном индексе бросает исключение, как и массив
    public TodoItem this[int index]
    {
        get
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _items[index];
        }
    }

    // «Безопасная» версия индексатора: возвращает null, если задачи с таким индексом нет
    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= _items.Count)
            return null;
        return _items[index];
    }

    public void Add(TodoItem item)
    {
        _items.Add(item);
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= _items.Count)
            return;
        _items.RemoveAt(index);
    }

    public void Update(int index, string newText)
    {
        this[index].UpdateText(newText);
    }

    public void SetStatus(int index, TodoStatus status)
    {
        this[index].SetStatus(status);
    }

    public bool Contains(string text)
    {
        foreach (TodoItem item in _items)
        {
            if (item.Text == text)
                return true;
        }
        return false;
    }

    // Метод-итератор: yield return отдаёт задачи по одной, а компилятор сам строит перечислитель
    public IEnumerator<TodoItem> GetEnumerator()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            yield return _items[i];
        }
    }

    // Старый необобщённый интерфейс тоже надо реализовать, но он просто вызывает обобщённый
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Вывод задач таблицей; колонку с текстом показываем всегда, остальные — по флагам
    public void View(bool showIndex, bool showStatus, bool showDate)
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("Список дел пуст.");
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine(BuildRow("№", "Задача", "Статус", "Изменена", showIndex, showStatus, showDate));
        sb.AppendLine(BuildRow(new string('-', 4), new string('-', 33), new string('-', 10), new string('-', 16),
            showIndex, showStatus, showDate));

        int number = 1;
        foreach (TodoItem item in this)
        {
            sb.AppendLine(BuildRow(number.ToString(), item.GetShortText(), item.StatusText,
                item.LastUpdate.ToString("dd.MM.yyyy HH:mm"), showIndex, showStatus, showDate));
            number++;
        }

        Console.Write(sb.ToString());
    }

    // Одна строка таблицы: отрицательная ширина в {x,-N} выравнивает по левому краю
    private static string BuildRow(string index, string text, string status, string date,
        bool showIndex, bool showStatus, bool showDate)
    {
        var row = new StringBuilder();
        if (showIndex) row.Append($"{index,-4} | ");
        row.Append($"{text,-33}");
        if (showStatus) row.Append($" | {status,-10}");
        if (showDate) row.Append($" | {date,-16}");
        return row.ToString().TrimEnd();
    }
}
