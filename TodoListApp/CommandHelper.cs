namespace TodoListApp;

public static class CommandHelper
{
    public static bool TryGetTask(TodoList todoList, int index, out TodoItem? task)
    {
        task = null;

        if (index < 1)
        {
            Console.WriteLine("Укажите корректный номер задачи.");
            return false;
        }

        task = todoList.GetItem(index - 1);

        if (task == null)
        {
            Console.WriteLine("Задачи с таким номером нет.");
            return false;
        }

        return true;
    }
}