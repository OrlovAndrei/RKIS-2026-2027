// Подключаем функции консольного ввода и вывода.
using System;

// Создаём команду завершения задачи.
public class DoneCommand : ICommand
{
    // Храним список задач приложения.
    public TodoList TodoList { get; set; } = null!;

    // Храним номер задачи, которую нужно завершить.
    public int Index { get; set; }

    // Выполняем команду завершения задачи.
    public void Execute()
    {
        // Получаем задачу по указанному номеру.
        TodoItem? item = TodoList.GetItem(Index);

        // Проверяем, найдена ли задача.
        if (item == null)
        {
            // Сообщаем, что задача не найдена.
            Console.WriteLine("Задача не найдена.");

            // Прекращаем выполнение команды.
            return;
        }

        // Отмечаем задачу как выполненную.
        item.MarkDone();

        // Сообщаем об успешном выполнении.
        Console.WriteLine("Задача отмечена как выполненная.");
    }
}