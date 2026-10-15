using System.Text;

// Список задач на основе массива, который расширяется при заполнении
public class TodoList
{
    private TodoItem[] _items = new TodoItem[2];
    private int _count = 0;

    public int Count => _count;

    public void Add(TodoItem item)
    {
        if (_count == _items.Length)
            _items = IncreaseArray(_items, item);
        else
            _items[_count] = item;
        _count++;
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= _count)
            return;

        // Сдвигаем задачи после удаляемой на одну позицию влево, чтобы не было «дыр» в массиве
        for (int i = index; i < _count - 1; i++)
            _items[i] = _items[i + 1];

        _count--;
        _items[_count] = null!; // убираем лишнюю ссылку, чтобы объект мог собрать GC
    }

    // Возвращает null, если задачи с таким индексом нет — проверку делает вызывающий код
    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= _count)
            return null;
        return _items[index];
    }

    public bool Contains(string text)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_items[i].Text == text)
                return true;
        }
        return false;
    }

    // Вывод задач таблицей; колонку с текстом показываем всегда, остальные — по флагам
    public void View(bool showIndex, bool showDone, bool showDate)
    {
        if (_count == 0)
        {
            Console.WriteLine("Список дел пуст.");
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine(BuildRow("№", "Задача", "Статус", "Изменена", showIndex, showDone, showDate));
        sb.AppendLine(BuildRow(new string('-', 4), new string('-', 33), new string('-', 12), new string('-', 16),
            showIndex, showDone, showDate));

        for (int i = 0; i < _count; i++)
        {
            TodoItem item = _items[i];
            sb.AppendLine(BuildRow((i + 1).ToString(), item.GetShortText(), item.StatusText,
                item.LastUpdate.ToString("dd.MM.yyyy HH:mm"), showIndex, showDone, showDate));
        }

        Console.Write(sb.ToString());
    }

    // Одна строка таблицы: отрицательная ширина в {x,-N} выравнивает по левому краю
    private static string BuildRow(string index, string text, string status, string date,
        bool showIndex, bool showDone, bool showDate)
    {
        var row = new StringBuilder();
        if (showIndex) row.Append($"{index,-4} | ");
        row.Append($"{text,-33}");
        if (showDone) row.Append($" | {status,-12}");
        if (showDate) row.Append($" | {date,-16}");
        return row.ToString().TrimEnd();
    }

    // Размер массива нельзя изменить, поэтому создаём новый в 2 раза больше,
    // копируем старые задачи и кладём новую сразу после них
    private TodoItem[] IncreaseArray(TodoItem[] items, TodoItem item)
    {
        var newItems = new TodoItem[items.Length * 2];
        for (int i = 0; i < items.Length; i++)
            newItems[i] = items[i];
        newItems[items.Length] = item;
        return newItems;
    }
}
