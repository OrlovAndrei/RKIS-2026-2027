using System.Collections;

namespace TodoListApp;

public class TodoList : IEnumerable<TodoItem>
{
    private readonly List<TodoItem> items = new List<TodoItem>();

    public int Count => items.Count;

    public void Add(TodoItem item)
    {
        items.Add(item);
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            Console.WriteLine("Задачи с таким индексом нет.");
            return;
        }

        items.RemoveAt(index);
    }

    public TodoItem this[int index]
    {
        get
        {
            if (index < 0 || index >= items.Count)
                throw new ArgumentOutOfRangeException();

            return items[index];
        }
    }

    public TodoItem? GetItem(int index)
    {
        if (index < 0 || index >= items.Count)
            return null;

        return items[index];
    }

    public void SetStatus(int index, TodoStatus status)
    {
        if (index < 0 || index >= items.Count)
        {
            Console.WriteLine("Задачи с таким индексом нет.");
            return;
        }

        items[index].SetStatus(status);
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            string result = "";

            if (showIndex)
                result += $"{i + 1} ";

            result += items[i].Text;

            if (showDone)
                result += $" | {items[i].Status}";

            if (showDate)
                result += $" | {items[i].LastModified}";

            Console.WriteLine(result);
        }
    }

    public IEnumerator<TodoItem> GetEnumerator()
    {
        foreach (TodoItem item in items)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}