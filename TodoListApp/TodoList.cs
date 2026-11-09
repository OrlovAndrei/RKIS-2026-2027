using System; // Вывод в консоль.
using System.Collections.Generic; // List и IEnumerator.

public class TodoList // Класс для управления списком задач.
{
    private const int TextWidth = 33; // Ширина столбца текста с учётом многоточия.
    private const int IndexWidth = 5; // Ширина столбца номеров.
    private const int StatusWidth = 12; // Ширина столбца статусов.
    private const int DateWidth = 16; // Ширина столбца даты.

    private List<TodoItem> items; // Приватный список задач.

    public int Count => items.Count; // Количество задач.

    public TodoList() // Конструктор создаёт пустой список.
    {
        items = new List<TodoItem>();
    }

    // Индексатор: доступ к задаче по индексу (с нуля, как у List).
    public TodoItem this[int index]
    {
        get { return items[index]; }
    }

    // Итератор: позволяет перебирать задачи через foreach.
    public IEnumerator<TodoItem> GetEnumerator()
    {
        foreach (TodoItem item in items)
        {
            yield return item; // Отдаём задачи по одной.
        }
    }

    public void Add(TodoItem item) // Добавляет задачу в список.
    {
        items.Add(item);
        Console.WriteLine("Задача добавлена");
    }

    public void Delete(int index) // Удаляет задачу по номеру (с единицы).
    {
        if (index < 1 || index > Count)
        {
            Console.WriteLine("Ошибка. Неверный номер задачи.");
            return;
        }

        items.RemoveAt(index - 1); // List сам сдвигает элементы.
        Console.WriteLine("Задача удалена");
    }

    public TodoItem? GetItem(int index) // Возвращает задачу по номеру (с единицы).
    {
        if (index < 1 || index > Count)
        {
            Console.WriteLine("Ошибка. Неверный номер задачи.");
            return null;
        }

        return this[index - 1]; // Используем индексатор.
    }

    public void View(bool showIndex, bool showDone, bool showDate) // Выводит таблицу задач.
    {
        if (Count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }

        string BuildRow(string index, string text, string status, string date) // Формирует строку таблицы.
        {
            string row = "";

            if (showIndex)
            {
                row += index.PadRight(IndexWidth) + " | ";
            }

            row += text.PadRight(TextWidth);

            if (showDone)
            {
                row += " | " + status.PadRight(StatusWidth);
            }

            if (showDate)
            {
                row += " | " + date.PadRight(DateWidth);
            }

            return row;
        }

        string header = BuildRow("Index", "Text", "Status", "Updated");
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        int number = 1; // Номер задачи для вывода.

        foreach (TodoItem item in this) // Перебираем задачи через итератор.
        {
            string shortText = item.Text.Replace("\n", " ");

            if (shortText.Length > 30)
            {
                shortText = shortText.Substring(0, 30) + "...";
            }

            string status = item.IsDone ? "сделано" : "не сделано";
            string date = item.LastUpdate.ToString("dd.MM.yyyy HH:mm");

            Console.WriteLine(BuildRow(number.ToString(), shortText, status, date));
            number++;
        }
    }
}