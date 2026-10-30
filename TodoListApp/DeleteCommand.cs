// Подключаем функции консольного ввода и вывода.
using System;

// Создаём команду удаления задачи.
public class DeleteCommand : ICommand
{
    // Храним список задач приложения.
    public TodoList TodoList { get; set; } = null!;

    // Храним номер удаляемой задачи.
    public int Index { get; set; }

    // Выполняем команду удаления задачи.
    public void Execute()
    {
        // Удаляем задачу по её номеру.
        TodoList.Delete(Index);

        // Сообщаем о выполнении команды.
        Console.WriteLine("Команда удаления выполнена.");
    }
}