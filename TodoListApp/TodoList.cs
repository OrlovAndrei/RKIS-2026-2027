namespace TodoListApp;

public class TodoList
{
    private TodoItem[] items = new TodoItem[2];
    private int count = 0;

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

    public void Delete(int index)
    {
        if (index < 0 || index >= count)
        {
            Console.WriteLine("Задачи с таким индексом нет.");
            return;
        }

        for (int i = index; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }

        count--;
        items[count] = null!;
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
            Console.WriteLine("Список задач пуст.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            string result = "";

            if (showIndex)
            {
                result += $"{i} ";
            }

            result += items[i].Text;

            if (showDone)
            {
                string status = items[i].IsCompleted
                    ? "done"
                    : "not done";

                result += $" | {status}";
            }

            if (showDate)
            {
                result += $" | {items[i].LastModified}";
            }

            Console.WriteLine(result);
        }
    }

    private TodoItem[] IncreaseArray(TodoItem[] currentItems, TodoItem item)
    {
        TodoItem[] newItems = new TodoItem[currentItems.Length * 2];

        Array.Copy(currentItems, newItems, count);

        newItems[count] = item;

        return newItems;
    }
}