using System;

public class TodoList
{
    private const int InitialCapacity = 2;
    private const int GrowthFactor = 2;
    private const int TextWidth = 33;
    private const int IndexWidth = 5;
    private const int StatusWidth = 12;
    private const int DateWidth = 16;

    private TodoItem[] items;
    private int count;

    public TodoList()
    {
        items = new TodoItem[InitialCapacity];
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
        Console.WriteLine("Задача добавлена");
    }

    public void Delete(int index)
    {
        if (index < 1 || index > count)
        {
            Console.WriteLine("Ошибка. Неверный номер задачи.");
            return;
        }

        int position = index - 1;

        for (int i = position; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }

        count--;
        items[count] = null!;

        Console.WriteLine("Задача удалена");
    }

    public TodoItem? GetItem(int index)
    {
        if (index < 1 || index > count)
        {
            Console.WriteLine("Ошибка. Неверный номер задачи.");
            return null;
        }

        return items[index - 1];
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        if (count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }

        string BuildRow(
            string index,
            string text,
            string status,
            string date)
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

        Console.WriteLine(
            BuildRow("Index", "Text", "Status", "Updated"));

        string separator = new string('-', BuildRow(
            "Index", "Text", "Status", "Updated").Length);

        Console.WriteLine(separator);

        for (int i = 0; i < count; i++)
        {
            TodoItem item = items[i];

            string shortText = item.Text.Replace("\n", " ");

            if (shortText.Length > 30)
            {
                shortText = shortText.Substring(0, 30) + "...";
            }

            string status = item.IsDone
                ? "сделано"
                : "не сделано";

            string date = item.LastUpdate.ToString(
                "dd.MM.yyyy HH:mm");

            Console.WriteLine(BuildRow(
                (i + 1).ToString(),
                shortText,
                status,
                date));
        }
    }

    private TodoItem[] IncreaseArray(
        TodoItem[] items,
        TodoItem item)
    {
        int newSize = items.Length * GrowthFactor;

        TodoItem[] newItems = new TodoItem[newSize];

        for (int i = 0; i < items.Length; i++)
        {
            newItems[i] = items[i];
        }

        newItems[count] = item;

        Console.WriteLine(
            $"Массив расширен до {newSize} элементов");

        return newItems;
    }
}