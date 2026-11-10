// Подключаем функции консольного вывода.
using System;

// Создаём команду изменения статуса задачи.
public class StatusCommand : ICommand
{
    // Храним список задач приложения.
    public TodoList TodoList { get; set; } = null!;

    // Храним номер задачи.
    public int Index { get; set; }

    // Храним новый статус.
    public TodoStatus Status { get; set; }

    // Выполняем команду изменения статуса.
    public void Execute()
    {
        // Меняем статус; при неверном номере список сам выведет ошибку.
        if (TodoList.SetStatus(Index, Status))
        {
            Console.WriteLine("Статус задачи изменён.");
        }
    }
}