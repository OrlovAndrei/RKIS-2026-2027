// Подключаем возможности консольного ввода и вывода.
using System;

// Создаём команду просмотра задач.
public class ViewCommand : ICommand
{
    // Храним список задач, который нужно показать.
    public TodoList TodoList { get; set; } = null!;

    // Определяем, нужно ли показывать номера задач.
    public bool ShowIndex { get; set; }

    // Определяем, нужно ли показывать статус задач.
    public bool ShowDone { get; set; }

    // Определяем, нужно ли показывать дату изменения.
    public bool ShowDate { get; set; }

    // Выполняем команду просмотра задач.
    public void Execute()
    {
        // Передаём настройки отображения в список задач.
        TodoList.View(ShowIndex, ShowDone, ShowDate);
    }
}